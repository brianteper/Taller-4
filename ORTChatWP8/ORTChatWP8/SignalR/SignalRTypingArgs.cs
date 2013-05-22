using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTChatWP8.SignalR
{
    public class SignalRTypingArgs : EventArgs
    {
        public string ConnectionId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
