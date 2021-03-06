﻿// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Zu.WebBrowser.Logging;

namespace Zu.WebBrowser.Communication {
    public class DebuggerConnectionMarionette : IDebuggerConnection {
        private static readonly Encoding _encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        private readonly AsyncProducerConsumerCollection<byte[]> _packetsToSend = new AsyncProducerConsumerCollection<byte[]>();
        private readonly INetworkClientFactory _networkClientFactory;
        private INetworkClient _networkClient;
        private readonly object _networkClientLock = new object();
        private bool _isClosed = false;


        public DebuggerConnectionMarionette(INetworkClientFactory networkClientFactory) {
            _networkClientFactory = networkClientFactory;
        }

        public void Dispose() {
            Close();
        }

        /// <summary>
        /// Close connection.
        /// </summary>
        public void Close() {
            _isClosed = true;

            lock (_networkClientLock) {
                if (_networkClient != null) {
                    _networkClient.Dispose();
                    _networkClient = null;
                }
            }
        }

        object lock1 = new object();
        /// <summary>
        /// Send a message.
        /// </summary>
        /// <param name="message">Message.</param>
        public void SendMessage(string message) {
            if (!Connected) {
                ConnectionClosed?.Invoke(this, EventArgs.Empty);
                return;
            }

            LiveLogger.WriteLine("Request: " + message, typeof(DebuggerConnection));

            var messageBody = _encoding.GetBytes(message);
            var messageHeader = _encoding.GetBytes(messageBody.Length + ":");//  string.Format(CultureInfo.InvariantCulture, "Content-Length: {0}\r\n\r\n", messageBody.Length));

            lock (lock1)
            {
                _packetsToSend.Add(messageHeader);
                _packetsToSend.Add(messageBody);
            }
        }

        /// <summary>
        /// Fired when received inbound message.
        /// </summary>
        public event EventHandler<MessageEventArgs> OutputMessage;

        /// <summary>
        /// Fired when connection was closed.
        /// </summary>
        public event EventHandler<EventArgs> ConnectionClosed;

        /// <summary>
        /// Gets a value indicating whether connection established.
        /// </summary>
        public bool Connected {
            get {
                lock (_networkClientLock) {
                    return _networkClient != null && _networkClient.Connected;
                }
            }
        }


        /// <summary>
        /// Connect to specified debugger endpoint.
        /// </summary>
        /// <param name="uri">URI identifying the endpoint to connect to.</param>
        public void Connect(Uri uri) {
            //Utilities.ArgumentNotNull("uri", uri);
            //LiveLogger.WriteLine("Debugger connecting to URI: {0}", uri);

            Close();
            _isClosed = false;
            lock (_networkClientLock) {
                int connection_attempts = 0;
                const int MAX_ATTEMPTS = 10;
                while (true) {
                    connection_attempts++;
                    try {
                        // TODO: This currently results in a call to the synchronous TcpClient
                        // constructor, which is a blocking call, and can take a couple of seconds
                        // to connect (with timeouts and retries). This code is running on the UI
                        // thread. Ideally this should be connecting async, or moved off the UI thread.
                        _networkClient = _networkClientFactory.CreateNetworkClient(uri);

                        // Unclear if the above can succeed and not be connected, but check for safety.
                        // The code needs to either break out the while loop, or hit the retry logic
                        // in the exception handler.
                        if (_networkClient.Connected) {
                            LiveLogger.WriteLine("Debugger connected successfully");
                            break;
                        }
                        else {
                            throw new SocketException();
                        }
                    }
                    catch (Exception ex) {
                        //if (ex.IsCriticalException()) {
                        //    throw;
                        //}
                        LiveLogger.WriteLine("Connection attempt {0} failed with: {1}", connection_attempts, ex);
                        if (_isClosed || connection_attempts >= MAX_ATTEMPTS) {
                            throw;
                        }
                        else {
                            // See above TODO. This should be moved off the UI thread or posted to retry
                            // without blocking in the meantime. For now, this seems the lesser of two
                            // evils. (The other being the debugger failing to attach on launch if the
                            // debuggee socket wasn't open quickly enough).
                            System.Threading.Thread.Sleep(400);
                        }
                    }
                }
            }

            Task.Factory.StartNew(ReceiveAndDispatchMessagesWorker);
            Task.Factory.StartNew(SendPacketsWorker);
        }

        /// <summary>
        /// Sends packets queued by <see cref="SendMessage"/>.
        /// </summary>
        private async void SendPacketsWorker() {
            INetworkClient networkClient;
            lock (_networkClientLock) {
                networkClient = _networkClient;
            }
            if (networkClient == null) {
                return;
            }

            try {
                var stream = networkClient.GetStream();
                while (Connected) {
                    byte[] packet = await _packetsToSend.TakeAsync().ConfigureAwait(false);
                    await stream.WriteAsync(packet, 0, packet.Length).ConfigureAwait(false);
                    await stream.FlushAsync().ConfigureAwait(false);
                }
            } catch (SocketException) {
            }
            catch (ObjectDisposedException) {
            }
            catch (InvalidOperationException) {
            }
            catch (IOException) {
            }
            catch (Exception e) {
                LiveLogger.WriteLine(string.Format(CultureInfo.CurrentCulture, "Failed to write message {0}.", e), typeof(DebuggerConnection));
            }
            finally
            {
                ConnectionClosed?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Receives messages from debugger, parses them to extract the body, and dispatches them to <see cref="OutputMessage"/> listeners.
        /// </summary>
        private async void ReceiveAndDispatchMessagesWorker() {
            LiveLogger.WriteLine("Established connection.", typeof(DebuggerConnection));

            INetworkClient networkClient;
            lock (_networkClientLock) {
                networkClient = _networkClient;
            }
            if (networkClient == null) {
                return;
            }

            try {
                var stream = networkClient.GetStream();

                // Use a single read buffer and a single StringBuilder (periodically cleared) across loop iterations,
                // to avoid costly repeated allocations.
                var buffer = new byte[0x1000];
                var sb = new StringBuilder();

                // Read and process incoming messages until disconnected.
                while (true) {
                    // Read the header of this message.
                    int contentLength = 0;
                    while (true) {
                        // Read a single header field.
                        string field;
                        sb.Clear();
                        while (true) {
                            int bytesRead = await stream.ReadAsync(buffer, 0, 1).ConfigureAwait(false);
                            if (bytesRead < 1) {
                                // End of stream - we are disconnected from debuggee.
                                throw new EndOfStreamException();
                            }

                            var ch = (char)buffer[0];
                            if (ch == ':')
                            {
                                field = sb.ToString();
                                break;
                            }
                            sb.Append(ch);

                        }

                        // Blank line terminates the header.
                        if (string.IsNullOrEmpty(field)) {
                            break;
                        }

                        int.TryParse(field, out contentLength);
                        break;
                    }

                    if (contentLength == 0) {
                        continue;
                    }

                    // Read the body of this message.

                    // If our preallocated buffer is large enough, use it - this should be true for vast majority of messages.
                    // If not, allocate a buffer that is large enough and use that, then throw it away - don't replace the original
                    // buffer with it, so that we don't hold onto a huge chunk of memory for the rest of the debugging session just
                    // because of a single long message.
                    var bodyBuffer = buffer.Length >= contentLength ? buffer : new byte[contentLength];

                    for (int i = 0; i < contentLength; ) {
                        i += await stream.ReadAsync(bodyBuffer, i, contentLength - i).ConfigureAwait(false);
                    }

                    string message = _encoding.GetString(bodyBuffer, 0, contentLength);
                    LiveLogger.WriteLine("Response: " + message, typeof(DebuggerConnection));

                    // Notify subscribers.
                    OutputMessage?.Invoke(this, new MessageEventArgs(message));
                }
            } catch (SocketException) {
            }
            catch (IOException) {
            }
            catch (ObjectDisposedException) {
            }
            catch (InvalidOperationException) {
            }
            catch (DecoderFallbackException ex) {
                LiveLogger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Error decoding response body: {0}", ex), typeof(DebuggerConnection));
            }
            catch (JsonReaderException ex) {
                LiveLogger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Error parsing JSON response: {0}", ex), typeof(DebuggerConnection));
            }
            catch (Exception ex) {
                LiveLogger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Message processing failed: {0}", ex), typeof(DebuggerConnection));
                //throw;
            }
            finally {
                LiveLogger.WriteLine("Connection was closed.", typeof(DebuggerConnection));

                ConnectionClosed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}