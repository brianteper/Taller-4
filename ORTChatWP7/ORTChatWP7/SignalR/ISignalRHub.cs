using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTChatWP7.SignalR
{
    // Custom delegate.
    public delegate void SignalRServerHandler(object sender, SignalREventArgs e);

    public interface ISignalRHub
    {
        // Custom event to act when something happens on SignalR Server.
        event SignalRServerHandler SignalRServerNotification;

        void JoinChat(ChatClient phoneChatMessage);
        void Chat(ChatClient phoneChatMessage);
        void LeaveChat(ChatClient phoneChatMessage);
    }
}
