namespace ORTChatWP8.SignalR
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ChatClient
    {
        public string PhoneClientId { get; set; }

        public string ChatUserName { get; set; }

        public string ChatMessage { get; set; }
    }

    public class ChatServer
    {
        public string Name { get; set; }

        public string Message { get; set; }
    }
}
