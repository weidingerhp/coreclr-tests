using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RaspiAspSample.SocketHandling {
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum MsgType {
        hello,
        login,
        logout,
        setitem,
        gamefinished
    }
    
    internal class MessageContent {
        [JsonProperty("msgType")]
        public MsgType MsgType { get; set; }
        
        [JsonProperty("value")]
        public object Value { get; set; }
    }
}