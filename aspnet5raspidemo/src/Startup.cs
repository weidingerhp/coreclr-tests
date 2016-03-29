using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.WebSockets;
using System.Net.WebSockets;
using System.Collections.Concurrent;
using RaspiAspSample.SocketHandling;

namespace RaspiAspSample
{   
    public class Startup
    {
        
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole()
                .AddDebug();

            app.UseWebSockets();
            app.Use(async (http, next) => { await WebSocketAcceptor(http, next); });
            
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
        
        
        async private Task WebSocketAcceptor(HttpContext http, Func<Task> next) {
            if (http.WebSockets.IsWebSocketRequest) {
                    var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                    if (webSocket != null && webSocket.State == WebSocketState.Open) {
                        var socketHandler = new SocketHandler(webSocket);
                        // do something here ... e.g. game-engine, add them to somewhere
                        await socketHandler.HandleLifeCycle();
                    }
                } else {
                    // Nothing to do here, pass downstream.  
                    await next();
                }
        }
    }
}