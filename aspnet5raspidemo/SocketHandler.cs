using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.WebSockets;
using System.Net.WebSockets;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace RaspiAspSample
{
	class SocketHandler {
		WebSocket webSocket;
        private ConcurrentBag<WebSocket> clients = new ConcurrentBag<WebSocket>();
		
        public SocketHandler(WebSocket webSocket) {
                this.webSocket = webSocket;
        }
        
        public async Task HandleLifeCycle() {
            try {
                clients.Add(webSocket);
                Console.Out.WriteLine("Handling new Socket Connection!!");
                while (webSocket.State == WebSocketState.Open) { 
                    var token = System.Threading.CancellationToken.None; 
                    var rcvBuffer = new ArraySegment<byte>(new byte[2048]);
                    
                    // Below will wait for a request message.
                    var received = await webSocket.ReceiveAsync(rcvBuffer, token);
                    switch (received.MessageType) {
                        case WebSocketMessageType.Text:
                            var request = Encoding.UTF8.GetString(rcvBuffer.Array, 
                                                                    rcvBuffer.Offset, 
                                                                    received.Count);
                            await HandleStringMessage(request);
                        break;
                        case WebSocketMessageType.Binary:
                        break;
                        case WebSocketMessageType.Close:
                        // connection closing
                        return;
                    }
                 }
            }
            catch (Exception ex) {
                // wanted to catch Microsoft.Net.WebSockets.WebSocketException - but ms does only want to use it in a sealed manner
                // looks like the socket has died ....
                // nothing left to do
            }
        }
        
        public bool IsConnected() {
             return (webSocket != null) && (webSocket.State == WebSocketState.Open);
        }

        async Task HandleStringMessage(string request) {
             var msgContent = JsonConvert.DeserializeObject<MessageContent>(request);
             Console.Out.WriteLine("Received Content: " + msgContent.MsgType);
             var resp = new MessageContent();
             resp.MsgType = "Welcome";
             resp.StringValue = "A little present for you";
             await SendTextMessage(JsonConvert.SerializeObject(resp));
        }
        
        
        async Task SendTextMessage(string text) {
            await webSocket.SendAsync(
                      new ArraySegment<Byte>(Encoding.UTF8.GetBytes(text)),
                      WebSocketMessageType.Text,
                      true,
                      System.Threading.CancellationToken.None);            
        }
        
        async Task BroadcastTextMessage(string text) {
            await Task.WhenAll(clients.Where(s => s.State == WebSocketState.Open)
                      .Select(async s => await SendTextMessage(text)));                            
        }
	}
}