using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using ORTChatWP8.SignalR;
using Microsoft.Phone.Controls;
using System.Threading.Tasks;

namespace ORTChatWP8.Views
{
    public partial class Chat : PhoneApplicationPage
    {
        ChatClient phoneToChat = new ChatClient();
        bool isTyping = false;

        BackgroundWorker ChatBackgroundDataWorker = new BackgroundWorker();
        BackgroundWorker DisconnectBackgroundDataWorker = new BackgroundWorker();

        public Chat()
        {
            InitializeComponent();

            //Seteamos los BackgroundWorkers para chatear y desconectarse en segundo plano
            ChatBackgroundDataWorker.WorkerSupportsCancellation = false;
            ChatBackgroundDataWorker.WorkerReportsProgress = false;
            ChatBackgroundDataWorker.DoWork += new DoWorkEventHandler(ChatBackgroundDataWorker_DoWork);

            DisconnectBackgroundDataWorker.WorkerSupportsCancellation = false;
            DisconnectBackgroundDataWorker.WorkerReportsProgress = false;
            DisconnectBackgroundDataWorker.DoWork += new DoWorkEventHandler(DisconnectBackgroundDataWorker_DoWork);
        }

        private void sendChatBtn_Click(object sender, EventArgs e)
        {
            if (chatTextbox.Text.Trim() != string.Empty)
            {
                //Guardamos el mensaje en el objeto a enviar al servidor y limpiamos el Textbox
                phoneToChat.ChatMessage = chatTextbox.Text.Trim();
                chatTextbox.Text = string.Empty;

                //Disparamos el Worker de manera asincrónica para postear el mensaje al servidor
                ChatBackgroundDataWorker.RunWorkerAsync();
            }
        }

        private void ChatBackgroundDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Llamamos al servidor para postear el mensaje
            App.Current.SignalRHub.Chat(phoneToChat);
        }

        private void DisconnectBackgroundDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Llamamos al servidor para salir de la sala de chat
            App.Current.SignalRHub.LeaveChat(phoneToChat);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //Al iniciar la página, guardamos el ID del dispositivo y el nombre de usuario en el objeto a enviar al servidor
            phoneToChat.PhoneClientId = App.Current.DeviceID;
            phoneToChat.ChatUserName = App.Current.ChatUserName;

            //Instanciamos el Hub para poder interactuar
            App.Current.SignalRHub = new SignalRMessagingHub();

            //Nos attacheamos a los eventos del servidor, para escuchar cuando se produce alguno de los mismos
            App.Current.SignalRHub.SignalRServerNotification += new SignalRServerHandler(SignalRHub_SignalRServerNotification);
            App.Current.SignalRHub.SignalRSomeoneIsTyping += new SignalRServerTypingHandler(SignalRHub_SignalRSomeoneIsTyping);
            App.Current.SignalRHub.SignalRHideIsTyping += new SignalRServerTypingHandler(SignalRHub_SignalRHideIsTyping);

            try
            {
                //Llamamos al servidor para unirnos a la sala
                App.Current.SignalRHub.JoinChat(phoneToChat);
            }
            catch (Exception)
            {
                MessageBox.Show("Hubo un error al intentar conectar con el servidor de chat, por favor intente nuevamente.");
                this.NavigationService.GoBack();
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            //Disparamos el Worker de manera asincrónica para dejar la sala
            DisconnectBackgroundDataWorker.RunWorkerAsync();

            //Nos dettacheamos de los eventos del servidor
            App.Current.SignalRHub.SignalRServerNotification -= new SignalRServerHandler(SignalRHub_SignalRServerNotification);
            App.Current.SignalRHub.SignalRSomeoneIsTyping -= new SignalRServerTypingHandler(SignalRHub_SignalRSomeoneIsTyping);
            App.Current.SignalRHub.SignalRHideIsTyping -= new SignalRServerTypingHandler(SignalRHub_SignalRHideIsTyping);
        }

        protected void SignalRHub_SignalRServerNotification(object sender, SignalREventArgs e)
        {
            //Al enviarse un nuevo mensaje desde el servidor, lo agregamos a la conversación local
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                chatDialog.Text += "\r\n" + e.ChatMessageFromServer;
                scrollViewer.ScrollToVerticalOffset(scrollViewer.ExtentHeight + 100);
            }));
        }

        protected void SignalRHub_SignalRSomeoneIsTyping(object sender, SignalRTypingArgs e)
        {
            //Al comenzar un usuario a escribir, el servidor nos notifica y lo mostramos en pantalla
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                TextBox txtName = new TextBox();
                txtName.Name = e.ConnectionId;
                txtName.Text = e.Name + " is typing...";

                if (this.FindName(txtName.Name) == null)
                {
                    stackPanel.Children.Add(txtName);
                }
            }));
        }

        protected void SignalRHub_SignalRHideIsTyping(object sender, SignalRTypingArgs e)
        {
            //Cuando un usuario deja de escribir, el servidor nos notifica y lo mostramos en pantalla
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                stackPanel.Children.Remove((UIElement)this.FindName(e.ConnectionId));
            }));
        }
    }
}