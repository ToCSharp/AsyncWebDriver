// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Zu.WebBrowser.Commands;

namespace Zu.WebBrowser.Communication {
    public class DebuggerClientMarionette : IDebuggerClient {

        public DebuggerConnection Connection => _connection as DebuggerConnection;

        public event EventHandler<JObject> EventMess;
        public event EventHandler<JToken> ResponseMess;

        private readonly IDebuggerConnection _connection;

        private ConcurrentDictionary<int, TaskCompletionSource<JToken>> _messages =
            new ConcurrentDictionary<int, TaskCompletionSource<JToken>>();

        private readonly static Newtonsoft.Json.JsonSerializerSettings jsonSettings = new Newtonsoft.Json.JsonSerializerSettings() {
            DateParseHandling = Newtonsoft.Json.DateParseHandling.None
        };

        public DebuggerClientMarionette(IDebuggerConnection connection) {

            _connection = connection;
            _connection.OutputMessage += OnOutputMessage;
            _connection.ConnectionClosed += OnConnectionClosed;
        }

        /// <summary>
        /// Send a command to debugger.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task SendRequestAsync(DebuggerCommand command, CancellationToken cancellationToken = new CancellationToken()) {
            cancellationToken.ThrowIfCancellationRequested();
            if (!_connection.Connected) throw new Exception("DebuggerConnectionMarionette not connected");
            try {
                TaskCompletionSource<JToken> promise = _messages.GetOrAdd(command.Id, i => new TaskCompletionSource<JToken>());
                _connection.SendMessage(command.ToString());
                cancellationToken.ThrowIfCancellationRequested();

                cancellationToken.Register(() => promise.TrySetCanceled(), false);

                JToken response = await promise.Task.ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

                command.ProcessResponse(response);
            } finally {
                TaskCompletionSource<JToken> promise;
                _messages.TryRemove(command.Id, out promise);
            }
        }


        /// <summary>
        /// Execudes the provided code, and catches any expected exceptions that may arise from direct or indirect use of <see cref="DebuggerClient.SendRequestAsync"/>.
        /// (in particular, when the connection is shut down, or is forcibly dropped from the other end).
        /// </summary>
        /// <remarks>
        /// This is intended to be used primarily with fire-and-forget async void methods that run on threadpool threads and cannot leak those exceptions
        /// without crashing the process.
        /// </remarks>
        public static async void RunWithRequestExceptionsHandled(Func<Task> action) {
            try {
                await action().ConfigureAwait(false);
            } catch (IOException) {
            } catch (OperationCanceledException) {
            }
        }

        /// <summary>
        /// Handles disconnect from debugger.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnConnectionClosed(object sender, EventArgs e) {
            ConcurrentDictionary<int, TaskCompletionSource<JToken>> messages = Interlocked.Exchange(ref _messages, new ConcurrentDictionary<int, TaskCompletionSource<JToken>>());
            foreach (var kv in messages) {
                var exception = new IOException("Resources.DebuggerConnectionClosed");
                try
                {
                    kv.Value.SetException(exception);
                }
                catch(Exception ex)
                {

                }
            }

            messages.Clear();
        }

        /// <summary>
        /// Process message from debugger connection.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Event arguments.</param>
        private void OnOutputMessage(object sender, MessageEventArgs args) {
            //if (string.IsNullOrWhiteSpace(args.Message)) return;
            JArray message;
            try
            {
                message = Newtonsoft.Json.JsonConvert.DeserializeObject(args.Message) as JArray; //, jsonSettings);
            }
            catch(Exception ex)
            {
                return;
            }
            if (message == null || message.Count < 2) return;
            var messageType = (string)message[0]; // "type"];

            switch (messageType) {
                case "2":
                    HandleEventMessage(message);
                    break;

                case "1":
                    HandleResponseMessage(message);
                    break;

                default:
                    Debug.Fail(string.Format(CultureInfo.CurrentCulture, "Unrecognized type '{0}' in message: {1}", messageType, message));
                    break;
            }
        }

        /// <summary>
        /// Handles event message.
        /// </summary>
        /// <param name="message">Message.</param>
        private void HandleEventMessage(JArray message) //JObject
        {
            EventMess?.Invoke(this, message[3] as JObject);
        }

        /// <summary>
        /// Handles response message.
        /// </summary>
        /// <param name="message">Message.</param>
        private void HandleResponseMessage(JArray message) {
            TaskCompletionSource<JToken> promise;
            var messageId = (int)message[1]; // "request_seq"];

            if (_messages.TryGetValue(messageId, out promise)) {
                promise.SetResult(message[3] as JToken);
            } else {
                //Debug.Fail(string.Format(CultureInfo.CurrentCulture, "Invalid response identifier '{0}'", messageId));
            }
            ResponseMess?.Invoke(this, message[3] as JToken);
        }
    }
}