var ws = null;

window.onbeforeunload = function () {
    if (ws) {
        ws.close();
    }
}

window.onload = function () {
    if ("WebSocket" in window) {
        // Let us open a web socket
        ws = new WebSocket(getWebAppURL());
        
        ws.onopen = function() {
            // Web Socket is connected, send data using send()
            ws.send(JSON.stringify({"msgType" : "hello"}));
        };
        
        ws.onmessage = function (evt) { 
            var received_msg = JSON.parse(evt.data);
            $("#log").append("<br/>");
            $("#log").append(received_msg.msgType + ": " + received_msg.stringValue);
        };
        
        ws.onclose = function() { 
            // websocket is closed.
        };
    } else {
        // The browser doesn't support WebSocket
        alert("WebSocket NOT supported by your Browser!");
    }
}

function WebSocketTest() {
    ws.send(JSON.stringify({"msgType" : "hello"}));
}


// the next two functions were taken from jWebSocket
function getServerURL( aSchema, aHost, aPort, aContext, aServlet ) {
    var lURL =
    aSchema + "://"
    + aHost 
    + ( aPort ? ":" + aPort : "" );
    if( aContext && aContext.length > 0 ) {
        lURL += aContext;
        if( aServlet && aServlet.length > 0 ) {
            lURL += aServlet;
        }
    }
    return lURL;
}
	
function getWebAppURL(aContext, aServlet) {
    var lContext = aContext || self.location.pathname;
    var lServlet = aServlet || "/websocket.jws";
    return getServerURL(
                "https" === self.location.protocol ? "wss" : "ws",
                self.location.hostname,
                self.location.port,
                lContext,
                lServlet
                );
}
