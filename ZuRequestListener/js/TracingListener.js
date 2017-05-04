
if (typeof (Services) == 'undefined')
    Cu.import('resource://gre/modules/Services.jsm');


this.fileNameFromUrl = function (url) {
    if (!url) return "noname";
    var ps = url.split('#').shift().split('?').shift().split('/');
    var res = ps.pop();
    if (res === "") res = ps.pop().replace(/\./, '');
    return res.split('#').shift().split('?').shift();
    //return url?url.split('/').pop().split('#').shift().split('?').shift():"noname";
}

this.extFromFileName = function (name) {
    if (!name) return "";
    var ps = name.split('.');
    if (ps.length == 1) return "";
    return ps.pop();
}

this.readBinaryFromFile = function (filename) {
    var fileIO = require("sdk/io/file");
    var text = null;
    if (fileIO.exists(filename)) {
        var bReader = fileIO.open(filename, "b");
        if (!bReader.closed) {
            text = bReader.read();
            bReader.close();
        }
    }
    return text;
}

this.writeBinaryToFile = function (text, filename) {
    var fileIO = require("sdk/io/file");
    var TextWriter = fileIO.open(filename, "bw");
    if (!TextWriter.closed) {
        TextWriter.write(text);
        TextWriter.close();
    }
}
String.prototype.hashCode = function () {
    var hash = 0, i, char;
    if (this.length == 0) return hash;
    for (i = 0; i < this.length; i++) {
        char = this.charCodeAt(i);
        hash = ((hash << 5) - hash) + char;
        hash = hash & hash; // Convert to 32bit integer
    }
    return hash;
};


var CC = this.Components.Constructor;
var BinaryInputStream = CC('@mozilla.org/binaryinputstream;1', 'nsIBinaryInputStream', 'setInputStream');
var BinaryOutputStream = CC('@mozilla.org/binaryoutputstream;1', 'nsIBinaryOutputStream', 'setOutputStream');
var StorageStream = CC('@mozilla.org/storagestream;1', 'nsIStorageStream', 'init');

this.TracingListener = function () {
    this.receivedChunks = []; // array for incoming data. holds chunks as they come, onStopRequest we join these junks to get the full source
    this.responseBody; // we'll set this to the 
    this.responseBody64; // we'll set this to the 
    this.code = "";
    this.error = "";
    //this.responseStatusCode;
    this.url;
    this.loadGroupUrl;
    this.responseStatus;

    this.pathToSave = "C:\\temp\\";
    this.filesToSave = [];
    this.filesExtToSave = [];
    this.filesExtNotRecord = [];
    this.contentTypesNotRecord = [];

    this.doReplaceFile = false;
    this.doSaveFile = false;
    this.doRecordFile = true;
    this.doSendBinary = false;

    this.topic = "";

    this.deferredDone = {
        promise: null,
        resolve: null,
        reject: null
    };
    this.deferredDone.promise = new Promise(function (resolve, reject) {
        this.resolve = resolve;
        this.reject = reject;
    }.bind(this.deferredDone));
    Object.freeze(this.deferredDone);
    this.promiseDone = this.deferredDone.promise;
}
this.TracingListener.prototype = {
    onDataAvailable: function (aRequest, aContext, aInputStream, aOffset, aCount) {

        if (this.doRecordFile) {
            var iStream = new BinaryInputStream(aInputStream) // binaryaInputStream
            var sStream = new StorageStream(8192, aCount, null); // storageStream // not sure why its 8192 but thats how eveyrone is doing it, we should ask why
            var oStream = new BinaryOutputStream(sStream.getOutputStream(0)); // binaryOutputStream

            // Copy received data as they come.
            var data = iStream.readBytes(aCount);
            this.receivedChunks.push(data);

            oStream.writeBytes(data, aCount);

            this.originalListener.onDataAvailable(aRequest, aContext, sStream.newInputStream(0), aOffset, aCount);
        } else {
            this.originalListener.onDataAvailable(aRequest, aContext, aInputStream, aOffset, aCount);
        }

    },
    onStartRequest: function (aRequest, aContext) {
        this.url = aRequest.name;
        if (top.zuSendEvent) {
            top.zuSendEvent({ "to": "RequestListener", "fname": "RequestListener.onStartRequest", "url": this.url });
        }
        var ext = extFromFileName(fileNameFromUrl(this.url));
        for (var i = 0; i < this.filesExtNotRecord.length; ++i) {
            if (this.filesExtNotRecord[i] === ext) {
                this.doRecordFile = false;
                //added = true;
                break;
            }
        }
        var ct = aRequest.contentType;
        if (this.contentTypesNotRecord.indexOf(ct) !== -1) {
            this.doRecordFile = false;
        }

        this.originalListener.onStartRequest(aRequest, aContext);
    },
    onStopRequest: function (aRequest, aContext, aStatusCode) {
        if (top.zuSendEvent) {
            top.zuSendEvent({ "to": "RequestListener", "fname": "RequestListener.onStopRequest", "url": this.url });
        }
        if (this.doRecordFile) {
            this.responseBody = this.receivedChunks.join("");
            delete this.receivedChunks;

            if (this.responseBody !== "") {

                try {
                    var converter = Cc["@mozilla.org/intl/scriptableunicodeconverter"]
                        .createInstance(Ci.nsIScriptableUnicodeConverter);
                    converter.charset = "UTF-8";

                    var res = converter.ConvertToUnicode(this.responseBody);
                    this.code = "unicode";
                    this.responseBody = res;
                } catch (e) {
                    this.error = e.toString();
                    if (this.doSendBinary && encode64 instanceof Function) {
                        try {

                            this.responseBody64 = encode64(this.responseBody);
                            this.code = "base64";
                        } catch (e2) {
                            this.code = "error";
                            this.responseBody = e2.toString();
                        }

                    } else {
                        this.responseBody = "";
                    }


                }
            }



            this.responseStatus = aStatusCode;

        }

        this.originalListener.onStopRequest(aRequest, aContext, aStatusCode);

        this.deferredDone.resolve();
    },
    QueryInterface: function (aIID) {
        if (aIID.equals(Ci.nsIStreamListener) || aIID.equals(Ci.nsISupports)) {
            return this;
        }
        throw Cr.NS_NOINTERFACE;
    }
};

this.httpResponseObserver = {
    QueryInterface: function (aIID) {
        if (aIID.equals(Ci.nsIObserver)) {
            return this;
        }
        throw Cr.NS_NOINTERFACE;
    },
    //QueryInterface: XPCOMUtils.generateQI([Ci.nsIObserver, Ci.nsISupportsWeakReference, Ci.nsIWeakReference]),

    //QueryReferent: function (iid) { return this.QueryInterface(iid) },

    listeners: [],
    //  multiplexStream: null, 
    //baseLoadPath: "D:\\Temp\\Loads\\",

    pathToSave: "C:\\temp\\",
    filesToSave: [],
    filesExtToSave: [],
    onStopRequestUrls: [],
    filesExtNotRecord: [],
    contentTypesNotRecord: [],
    doSaveAll: false,
    doSendBinary: false,

    errors: [],
    requests: [],

    addListener: function (listener) {
        if (this.listeners.indexOf(listener) < 0) {
            this.listeners.push(listener);
        }
    },

    removeListener: function (listener) {
        var lIndex = this.listeners.indexOf(listener);
        if (lIndex > 0) {
            return this.listeners.splice(lIndex, 1);
        }
    },

    addFileToSave: function (url) {
        this.filesToSave.push(url);
    },
    addFileExtToSave: function (ext) {
        this.filesExtToSave.push(ext);
    },

    addOnStopRequestUrl: function (url) {
        this.onStopRequestUrls.push(url);
    },

    observe: function (aSubject, aTopic, aData) {
        var newListener = new TracingListener();
        if (this.doSaveAll == true) {
            newListener.doSaveFile = true;
        }
        if (this.doSendBinary == true) {
            newListener.doSendBinary = true;
        }
        newListener.filesExtNotRecord = this.filesExtNotRecord;
        newListener.contentTypesNotRecord = this.contentTypesNotRecord;
        var self = this;

        aSubject.QueryInterface(Ci.nsIHttpChannel);

        aSubject.QueryInterface(Ci.nsITraceableChannel);
        newListener.originalListener = aSubject.setNewListener(newListener);

        newListener.promiseDone.then(
            function () {
                if (newListener.doRecordFile) {
                    if (newListener.doSaveFile) {
                        var hashC = newListener.responseBody.hashCode();
                        var fName = fileNameFromUrl(newListener.url)
                        var ext = extFromFileName(fName);
                        if (ext != '') ext = '.' + ext;
                        var p = self.pathToSave + fName + '_' + hashC + ext;
                        writeBinaryToFile(newListener.responseBody, p);
                        for (var i = 0; i < self.listeners.length; ++i) {
                            if (typeof (self.listeners[i].onFileSaved) === "function") {
                                try {
                                    self.listeners[i].onFileSaved(newListener.url, p, hashC);
                                } catch (e) {
                                }
                            }
                        }
                    }
                    for (var i = 0; i < self.listeners.length; ++i) {
                        if (typeof (self.listeners[i].onFileLoaded) == "function") {
                            try {
                                if (newListener.responseBody64) {
                                    self.listeners[i].onFileLoaded(newListener.url, newListener.responseBody64, newListener.code);
                                } else {
                                    self.listeners[i].onFileLoaded(newListener.url, newListener.responseBody, newListener.code);
                                }
                            } catch (e) {
                            }
                        }
                    }
                }

            },
            function (aReason) {
                self.errors.push(aReason);
                // promise was rejected, right now i didnt set up rejection, but i should listen to on abort or bade status code then reject maybe
            }
        ).catch(
            function (aCatch) {
                self.errors.push(aCatch);
                //console.error('something went wrong, a typo by dev probably:', aCatch);
            }
            );
    }
};


Services.obs.addObserver(httpResponseObserver, 'http-on-examine-response', false);
Services.obs.addObserver(httpResponseObserver, 'http-on-examine-cached-response', false);

return 'ok';
