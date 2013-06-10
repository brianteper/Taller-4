namespace ORTChatWP8.Views
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using Microsoft.Phone.Controls;
    using ORTChatWP8.SignalR;

    public partial class Chat : PhoneApplicationPage
    {
        public Chat()
        {
            this.InitializeComponent();

            this.PhoneToChat = new ChatClient();
            this.ChatBackgroundDataWorker = new BackgroundWorker();
            this.DisconnectBackgroundDataWorker = new BackgroundWorker();

            // Seteamos los BackgroundWorkers para chatear y desconectarse en segundo plano
            this.ChatBackgroundDataWorker.WorkerSupportsCancellation = false;
            this.ChatBackgroundDataWorker.WorkerReportsProgress = false;
            this.ChatBackgroundDataWorker.DoWork += new DoWorkEventHandler(this.ChatBackgroundDataWorker_DoWork);

            this.DisconnectBackgroundDataWorker.WorkerSupportsCancellation = false;
            this.DisconnectBackgroundDataWorker.WorkerReportsProgress = false;
            this.DisconnectBackgroundDataWorker.DoWork += new DoWorkEventHandler(this.DisconnectBackgroundDataWorker_DoWork);
        }

        private ChatClient PhoneToChat
        {
            get;
            set;
        }

        private BackgroundWorker ChatBackgroundDataWorker
        {
            get;
            set;
        }

        private BackgroundWorker DisconnectBackgroundDataWorker
        {
            get;
            set;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Al iniciar la página, guardamos el ID del dispositivo y el nombre de usuario en el objeto a enviar al servidor
            this.PhoneToChat.PhoneClientId = App.Current.DeviceID;
            this.PhoneToChat.ChatUserName = App.Current.ChatUserName;

            // Instanciamos el Hub para poder interactuar
            App.Current.SignalRHub = new SignalRMessagingHub();

            // Nos attacheamos a los eventos del servidor, para escuchar cuando se produce alguno de los mismos
            App.Current.SignalRHub.SignalRServerNotification += new SignalRServerHandler(this.SignalRHub_SignalRServerNotification);
            App.Current.SignalRHub.SignalRSomeoneIsTyping += new SignalRServerTypingHandler(this.SignalRHub_SignalRSomeoneIsTyping);
            App.Current.SignalRHub.SignalRHideIsTyping += new SignalRServerTypingHandler(this.SignalRHub_SignalRHideIsTyping);

            try
            {
                // Llamamos al servidor para unirnos a la sala
                App.Current.SignalRHub.JoinChat(this.PhoneToChat);
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

            // Disparamos el Worker de manera asincrónica para dejar la sala
            this.DisconnectBackgroundDataWorker.RunWorkerAsync();

            // Nos dettacheamos de los eventos del servidor
            App.Current.SignalRHub.SignalRServerNotification -= new SignalRServerHandler(this.SignalRHub_SignalRServerNotification);
            App.Current.SignalRHub.SignalRSomeoneIsTyping -= new SignalRServerTypingHandler(this.SignalRHub_SignalRSomeoneIsTyping);
            App.Current.SignalRHub.SignalRHideIsTyping -= new SignalRServerTypingHandler(this.SignalRHub_SignalRHideIsTyping);
        }

        protected void SignalRHub_SignalRServerNotification(object sender, SignalREventArgs e)
        {
            // Al enviarse un nuevo mensaje desde el servidor, lo agregamos a la conversación local
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                chatDialog.Text += "\r\n" + e.ChatMessageFromServer;
                scrollViewer.ScrollToVerticalOffset(scrollViewer.ExtentHeight + 100);
            }));
        }

        protected void SignalRHub_SignalRSomeoneIsTyping(object sender, SignalRTypingArgs e)
        {
            // Al comenzar un usuario a escribir, el servidor nos notifica y lo mostramos en pantalla
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
            // Cuando un usuario deja de escribir, el servidor nos notifica y lo mostramos en pantalla
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                stackPanel.Children.Remove((UIElement)this.FindName(e.ConnectionId));
            }));
        }

        private void SendChatBtn_Click(object sender, EventArgs e)
        {
            if (chatTextbox.Text.Trim() != string.Empty)
            {
                // Guardamos el mensaje en el objeto a enviar al servidor y limpiamos el Textbox
                this.PhoneToChat.ChatMessage = chatTextbox.Text.Trim();
                chatTextbox.Text = string.Empty;

                // Disparamos el Worker de manera asincrónica para postear el mensaje al servidor
                this.ChatBackgroundDataWorker.RunWorkerAsync();
            }
        }

        private void ChatBackgroundDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Llamamos al servidor para postear el mensaje
            App.Current.SignalRHub.Chat(this.PhoneToChat);
        }

        private void DisconnectBackgroundDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Llamamos al servidor para salir de la sala de chat
            App.Current.SignalRHub.LeaveChat(this.PhoneToChat);
        }
    }
}