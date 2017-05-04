//"use strict";

var {Constructor: CC, classes: Cc, interfaces: Ci, utils: Cu} = Components;

var loader = Cc["@mozilla.org/moz/jssubscript-loader;1"].getService(Ci.mozIJSSubScriptLoader);
const ServerSocket = CC("@mozilla.org/network/server-socket;1", "nsIServerSocket", "initSpecialConnection");

Cu.import("resource://gre/modules/Log.jsm");
Cu.import("resource://gre/modules/Preferences.jsm");
Cu.import("resource://gre/modules/Services.jsm");


loader.loadSubScript("resource://devtools/shared/transport/transport.js");

var myTop = this;

this.MConn = function (aConnId, aConn, aServ) {
    this.connId = aConnId;
    this.server = aServ;
    this.conn = aConn;
    this.conn.hooks = this;

    this.doSerializeFunctionsToString = true;
}

this.MConn.prototype = {

    onClosed: function (aStatus) {
        this.server._connectionClosed(this);
    },

    sendEvent: function (packet) {
        try {
            this.conn.send({ "event": this.serialize(packet) });
        } catch (e) {
            result = e.message;
            this.conn.send({ "error": true, "event": result });
        }
    },

    serialize: function serialize(obj, doNotSerializeProperties) {
        try {
            var type = typeof (obj);
            var serialized = {
                "type": type,
                "value": ""
            };
            if (type === "object" || type === "function") {
                if (obj === null) {
                    serialized["value"] = "null";
                } else if (type === "function") {
                    if (this.doSerializeFunctionsToString === true) serialized["value"] = obj.toString();
                } else if (obj instanceof Array) {
                    var arr = [];
                    for (var i = 0; i < obj.length; i++) {
                        arr.push(this.serialize(obj[i], true));
                    }
                    serialized["value"] = arr;
                } else {
                    if (doNotSerializeProperties) {
                        //empty
                    }
                    else serialized["value"] = this._serializeProperties(obj);
                }
            } else if (type === "number" && (
                isNaN(obj)
                || obj === Infinity
                || obj === -Infinity)) {
                serialized["value"] = obj.toString();
            } else if (type === "string") {
                serialized["value"] = obj;
            } else {
                serialized["value"] = obj;
            }
            return serialized;
        } catch (e) {
            return { "serializeError": this._serializeProperties(e) };
        }
    },

    _serializeProperties: function _serializeProperties(obj) {
        var o = {};
        if (obj instanceof Error) {
            if (obj.message) o['message'] = this.serialize(obj.message, true);
            if (obj.stack) o['stack'] = this.serialize(obj.stack, true);
            if (obj.name) o['name'] = this.serialize(obj.name, true);
            if (obj.columnNumber) o['columnNumber'] = this.serialize(obj.columnNumber, true);
            if (obj.lineNumber) o['lineNumber'] = this.serialize(obj.lineNumber, true);
            if (obj.fileName) o['fileName'] = this.serialize(obj.fileName, true);
            return o;
        }

        var properties = [];
        Object.getOwnPropertyNames(obj).forEach(function (key) {
            properties.push(key);
        });
        //var o = {};
        for (var index = 0; index < properties.length; index++) {
            var pName = properties[index];
            try {
                var prop = obj[pName];

                o[pName] = this.serialize(prop, true);
            } catch (x) {
                o[pName] = this.serialize(x.message, true);
            }
        }
        if (!o.constructor && obj.constructor && obj.constructor !== obj) {
            o["constructor"] = this.serialize(obj.constructor, true);
        }
        if (!o.proto && obj.prototype && obj.prototype !== obj) {
            o["proto"] = this.serialize(obj.prototype, true);
        }
        if (!o.__proto__ && obj.__proto__ && obj.__proto__ !== obj) {
            o["__proto__"] = this.serialize(obj.__proto__, true);
        }
        return o;
    }
};

this.MarionetteServer2 = function (port, forceLocal) {
    this.port = port;
    this.forceLocal = forceLocal;
    this.conns = {};
    this.nextConnId = 0;
    this.alive = false;
};

MarionetteServer2.prototype.start = function () {
    if (this.alive) {
        return;
    }
    let flags = Ci.nsIServerSocket.KeepWhenOffline;
    if (this.forceLocal) {
        flags |= Ci.nsIServerSocket.LoopbackOnly;
    }
    this.listener = new ServerSocket(this.port, flags, 1);
    this.listener.asyncListen(this);
    this.alive = true;
};

MarionetteServer2.prototype.stop = function () {
    if (!this.alive) {
        return;
    }
    this.closeListener();
    this.alive = false;
};

MarionetteServer2.prototype.closeListener = function () {
    this.listener.close();
    this.listener = null;
};

MarionetteServer2.prototype.onSocketAccepted = function (
    serverSocket, clientSocket) {
    let input = clientSocket.openInputStream(0, 0, 0);
    let output = clientSocket.openOutputStream(0, 0, 0);
    let transport = new DebuggerTransport(input, output);
    let connId = "conn" + this.nextConnId++;

    let mconn = new MConn(connId, transport, this);
    this.conns[connId] = mconn;

    transport.ready();
};

MarionetteServer2.prototype.onConnectionClosed = function (conn) {
    let id = conn.connId;
    delete this.conns[id];
};
MarionetteServer2.prototype.sendEvent = function (packet) {
    for (let c in this.conns) {
        this.conns[c].sendEvent(packet);
        // this.conns[c].conn.send({"event": packet});
    }
}

top.MarionetteServer2 = MarionetteServer2;
top.MConn = MConn;
