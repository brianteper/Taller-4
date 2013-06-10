namespace ORTChatWP8.SignalR
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    // Custom delegate.
    public delegate void SignalRServerHandler(object sender, SignalREventArgs e);

    public delegate void SignalRServerTypingHandler(object sender, SignalRTypingArgs e);

    public interface ISignalRHub
    {
        // Custom event to act when something happens on SignalR Server.
        event SignalRServerHandler SignalRServerNotification;

        event SignalRServerTypingHandler SignalRSomeoneIsTyping;

        event SignalRServerTypingHandler SignalRHideIsTyping;

        void JoinChat(ChatClient phoneChatMessage);

        void Chat(ChatClient phoneChatMessage);

        void LeaveChat(ChatClient phoneChatMessage);

        void SomeoneIsTyping(ChatClient phoneChatMessage);

        void HideIsTyping(ChatClient phoneChatMessage);
    }
}
