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

        //Instanciamos la conexión con el hub en la url especificada
        HubConnection chatConnection = new HubConnection("http://10.0.1.25:17991/");

        public event SignalRServerHandler SignalRServerNotification;
        public event SignalRServerTypingHandler SignalRSomeoneIsTyping;
        public event SignalRServerTypingHandler SignalRHideIsTyping;

        public SignalRMessagingHub()
        {
            //Referenciamos al Hub y Proxy de SignalR
            SignalRChatHub = chatConnection.CreateHubProxy("ChatHub");
        }

        public virtual void JoinChat(ChatClient phoneChatMessage)
        {
            //Disparamos la conexión y nos unimos a la sala
            chatConnection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    //Manejar el error ante un fallo en la conexión
                }
            });

            //Escuchamos los eventos del servidor y los disparamos
            SignalRChatHub.On<string>("addChatMessage", message =>
            {
                SignalREventArgs chatArgs = new SignalREventArgs();
                var serverMsg = JsonConvert.DeserializeObject<ChatServer>(message);
                chatArgs.ChatMessageFromServer = serverMsg.Name + ": " + serverMsg.Message;

                //Disparamos nuestro evento local
                OnSignalRServerNotificationReceived(chatArgs);
            });

            SignalRChatHub.On<string>("showIsTyping", info =>
            {
                SignalRTypingArgs typingArgs = JsonConvert.DeserializeObject<SignalRTypingArgs>(info);

                OnSignalRSomeoneIsTyping(typingArgs);
            });

            SignalRChatHub.On<string>("hideIsTyping", info =>
            {
                SignalRTypingArgs typingArgs = JsonConvert.DeserializeObject<SignalRTypingArgs>(info);

                OnSignalRHideIsTyping(typingArgs);
            });
        }

        public virtual void Chat(ChatClient phoneChatMessage)
        {
            //Posteamos el mensaje al server
            SignalRChatHub.Invoke("PushMessageFromPhone", phoneChatMessage.ChatUserName, phoneChatMessage.ChatMessage).Wait();
        }

        public virtual void SomeoneIsTyping(ChatClient phoneChatMessage)
        {
            //Posteamos al server que alguien comenzó a escribir
            SignalRChatHub.Invoke("SomeoneIsTyping", phoneChatMessage.ChatUserName).Wait();
        }

        public virtual void HideIsTyping(ChatClient phoneChatMessage)
        {
            //Posteamos al server que se dejó de escribir
            SignalRChatHub.Invoke("FinishTyping").Wait();
        }

        public virtual void LeaveChat(ChatClient phoneChatMessage)
        {
            //Dejamos la sala de chat
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
