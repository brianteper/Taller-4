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
using Newtonsoft.Json;

namespace ORTChatWP8.SignalR
{
    public class SignalRMessagingHub : ISignalRHub
    {
        IHubProxy SignalRChatHub;

        // Use the specific port# for local server or URI if hosted.        
        HubConnection chatConnection = new HubConnection("http://10.0.1.25:17991/");

        public event SignalRServerHandler SignalRServerNotification;
        public event SignalRServerTypingHandler SignalRSomeoneIsTyping;
        public event SignalRServerTypingHandler SignalRHideIsTyping;

        public SignalRMessagingHub()
        {
            // Reference to SignalR Server Hub & Proxy.                      
            SignalRChatHub = chatConnection.CreateHubProxy("ChatHub");
        }

        public virtual void JoinChat(ChatClient phoneChatMessage)
        {
            // Fire up SignalR Connection & join chatroom.  
            chatConnection.Start().ContinueWith(task =>
            {
                if (!task.IsFaulted)
                {
                    // Join the Server's list of Chatroom clients.
                    //SignalRChatHub.Invoke("JoinFromPhone", phoneChatMessage.ChatUserName).Wait();
                }
                else
                {
                    // Oopsie, do some error handling.
                }
            });

            // Listen to chat events on SignalR Server & wire them up appropriately.
            SignalRChatHub.On<string>("addChatMessage", message =>
            {
                SignalREventArgs chatArgs = new SignalREventArgs();
                var serverMsg = JsonConvert.DeserializeObject<ChatServer>(message);
                chatArgs.ChatMessageFromServer = serverMsg.Name + ": " + serverMsg.Message;

                // Raise custom event & let it bubble up.
                OnSignalRServerNotificationReceived(chatArgs);
            });

            SignalRChatHub.On<string>("showIsTyping", info =>
            {
                SignalRTypingArgs typingArgs = JsonConvert.DeserializeObject<SignalRTypingArgs>(info);

                // Raise custom event & let it bubble up.
                OnSignalRSomeoneIsTyping(typingArgs);
            });

            SignalRChatHub.On<string>("hideIsTyping", info =>
            {
                SignalRTypingArgs typingArgs = JsonConvert.DeserializeObject<SignalRTypingArgs>(info);

                // Raise custom event & let it bubble up.
                OnSignalRHideIsTyping(typingArgs);
            });
        }

        public virtual void Chat(ChatClient phoneChatMessage)
        {
            // Post message to Server Chatroom.
            SignalRChatHub.Invoke("PushMessageFromPhone", phoneChatMessage.ChatUserName, phoneChatMessage.ChatMessage).Wait();
        }

        public virtual void SomeoneIsTyping(ChatClient phoneChatMessage)
        {
            // Post message to Server Chatroom.
            SignalRChatHub.Invoke("SomeoneIsTyping", phoneChatMessage.ChatUserName).Wait();
        }

        public virtual void HideIsTyping(ChatClient phoneChatMessage)
        {
            // Post message to Server Chatroom.
            SignalRChatHub.Invoke("FinishTyping").Wait();
        }

        public virtual void LeaveChat(ChatClient phoneChatMessage)
        {
            // Leave the Server's Chatroom.
            SignalRChatHub.Invoke("DisconnectFromPhone", phoneChatMessage.ChatUserName).Wait();
        }

        public virtual void OnSignalRServerNotificationReceived(SignalREventArgs e)
        {
            if (SignalRServerNotification != null)
            {
                SignalRServerNotification(this, e);
            }
        }

        public virtual void OnSignalRSomeoneIsTyping(SignalRTypingArgs e)
        {
            if (SignalRSomeoneIsTyping != null)
            {
                SignalRSomeoneIsTyping(this, e);
            }
        }

        public virtual void OnSignalRHideIsTyping(SignalRTypingArgs e)
        {
            if (SignalRHideIsTyping != null)
            {
                SignalRHideIsTyping(this, e);
            }
        }

    }
}
