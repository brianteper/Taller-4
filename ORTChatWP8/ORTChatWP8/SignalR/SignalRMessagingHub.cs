using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace ORTChatWP8.SignalR
{
    public class SignalRMessagingHub : ISignalRHub
    {
        #region "Members"

        IHubProxy SignalRChatHub;

        // Use the specific port# for local server or URI if hosted.        
        HubConnection chatConnection = new HubConnection("http://10.0.1.17:17991/");

        public event SignalRServerHandler SignalRServerNotification;

        #endregion

        #region "Constructor"

        public SignalRMessagingHub()
        {
            // Reference to SignalR Server Hub & Proxy.                      
            SignalRChatHub = chatConnection.CreateHubProxy("ChatHub");
        }

        #endregion

        #region "Implementation"

        public virtual void JoinChat(ChatClient phoneChatMessage)
        {
            // Fire up SignalR Connection & join chatroom.  
            chatConnection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // Oopsie, do some error handling.
                }

                // Join the Server's list of Chatroom clients.
                SignalRChatHub.Invoke("JoinFromPhone", phoneChatMessage.PhoneClientId, phoneChatMessage.ChatUserName).Wait();
                SignalRChatHub.Invoke("PushMessageToClients", phoneChatMessage.ChatUserName + " just joined!").Wait();
            });

            // Listen to chat events on SignalR Server & wire them up appropriately.
            SignalRChatHub.On<string>("addChatMessage", message =>
            {
                SignalREventArgs chatArgs = new SignalREventArgs();
                chatArgs.ChatMessageFromServer = message;

                // Raise custom event & let it bubble up.
                OnSignalRServerNotificationReceived(chatArgs);
            });
        }

        public virtual void Chat(ChatClient phoneChatMessage)
        {
            // Post message to Server Chatroom.
            SignalRChatHub.Invoke("PushMessageToClients", phoneChatMessage.ChatMessage).Wait();
        }

        public virtual void LeaveChat(ChatClient phoneChatMessage)
        {
            // Leave the Server's Chatroom.
            SignalRChatHub.Invoke("Disconnect", phoneChatMessage.PhoneClientId, phoneChatMessage.ChatUserName).Wait();
            SignalRChatHub.Invoke("PushMessageToClients", phoneChatMessage.ChatUserName + " just left!").Wait();
        }

        #endregion

        #region "Methods"

        public virtual void OnSignalRServerNotificationReceived(SignalREventArgs e)
        {
            if (SignalRServerNotification != null)
            {
                SignalRServerNotification(this, e);
            }
        }

        #endregion
    }
}
