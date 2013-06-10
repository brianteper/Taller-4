namespace ORTChatWP8.SignalR
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SignalREventArgs : EventArgs
    {
        public string ChatMessageFromServer
        {
            get;
            set;
        }
    }
}
