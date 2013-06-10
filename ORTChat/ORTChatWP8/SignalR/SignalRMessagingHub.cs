namespace ORTChatWP8.SignalR
{
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

    public class SignalRMessagingHub : ISignalRHub
    {
        public SignalRMessagingHub()
        {
            // Instanciamos la conexión con el hub en la url especificada
            this.ChatConnection = new HubConnection("http://10.0.1.25:17991/");

            // Referenciamos al Hub y Proxy de SignalR
            this.SignalRChatHub = this.ChatConnection.CreateHubProxy("ChatHub");
        }

        public event SignalRServerHandler SignalRServerNotification;

        public event SignalRServerTypingHandler SignalRSomeoneIsTyping;

        public event SignalRServerTypingHandler SignalRHideIsTyping;

        private IHubProxy SignalRChatHub
        {
            get;
            set;
        }

        private HubConnection ChatConnection
        {
            get;
            set;
        }

        public virtual void JoinChat(ChatClient phoneChatMessage)
        {
            // Disparamos la conexión y nos unimos a la sala
            this.ChatConnection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // Manejar el error ante un fallo en la conexión
                }
            });

            // Escuchamos los eventos del servidor y los disparamos
            this.SignalRChatHub.On<string>("addChatMessage", message =>
            {
                SignalREventArgs chatArgs = new SignalREventArgs();
                var serverMsg = JsonConvert.DeserializeObject<ChatServer>(message);
                chatArgs.ChatMessageFromServer = serverMsg.Name + ": " + serverMsg.Message;

                // Disparamos nuestro evento local
                OnSignalRServerNotificationReceived(chatArgs);
            });

            this.SignalRChatHub.On<string>("showIsTyping", info =>
            {
                SignalRTypingArgs typingArgs = JsonConvert.DeserializeObject<SignalRTypingArgs>(info);

                OnSignalRSomeoneIsTyping(typingArgs);
            });

            this.SignalRChatHub.On<string>("hideIsTyping", info =>
            {
                SignalRTypingArgs typingArgs = JsonConvert.DeserializeObject<SignalRTypingArgs>(info);

                OnSignalRHideIsTyping(typingArgs);
            });
        }

        public virtual void Chat(ChatClient phoneChatMessage)
        {
            // Posteamos el mensaje al server
            this.SignalRChatHub.Invoke("PushMessageFromPhone", phoneChatMessage.ChatUserName, phoneChatMessage.ChatMessage).Wait();
        }

        public virtual void SomeoneIsTyping(ChatClient phoneChatMessage)
        {
            // Posteamos al server que alguien comenzó a escribir
            this.SignalRChatHub.Invoke("SomeoneIsTyping", phoneChatMessage.ChatUserName).Wait();
        }

        public virtual void HideIsTyping(ChatClient phoneChatMessage)
        {
            // Posteamos al server que se dejó de escribir
            this.SignalRChatHub.Invoke("FinishTyping").Wait();
        }

        public virtual void LeaveChat(ChatClient phoneChatMessage)
        {
            // Dejamos la sala de chat
            this.SignalRChatHub.Invoke("DisconnectFromPhone", phoneChatMessage.ChatUserName).Wait();
        }

        public virtual void OnSignalRServerNotificationReceived(SignalREventArgs e)
        {
            if (this.SignalRServerNotification != null)
            {
                this.SignalRServerNotification(this, e);
            }
        }

        public virtual void OnSignalRSomeoneIsTyping(SignalRTypingArgs e)
        {
            if (this.SignalRSomeoneIsTyping != null)
            {
                this.SignalRSomeoneIsTyping(this, e);
            }
        }

        public virtual void OnSignalRHideIsTyping(SignalRTypingArgs e)
        {
            if (this.SignalRHideIsTyping != null)
            {
                this.SignalRHideIsTyping(this, e);
            }
        }
    }
}
