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
using ORTChatWP7.SignalR;
using Microsoft.Phone.Controls;

namespace ORTChatWP7.Views
{
    public partial class Chat : PhoneApplicationPage
    {
        ChatClient phoneToChat = new ChatClient();

        BackgroundWorker ChatBackgroundDataWorker = new BackgroundWorker();
        BackgroundWorker DisconnectBackgroundDataWorker = new BackgroundWorker();

        public Chat()
        {
            InitializeComponent();

            // Set up the background workers for Chat & Disconnect.
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
                phoneToChat.PhoneClientId = App.Current.DeviceID;
                phoneToChat.ChatUserName = App.Current.ChatUserName;
                phoneToChat.ChatMessage = chatTextbox.Text.Trim();
                chatTextbox.Text = string.Empty;

                // Fire off background thread to post message to server Chatroom.               
                ChatBackgroundDataWorker.RunWorkerAsync();
            }
        }

        private void ChatBackgroundDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Send to server for adding message to Chatroom.
            App.Current.SignalRHub.Chat(phoneToChat);
        }

        private void DisconnectBackgroundDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Leave Chatroom.
            App.Current.SignalRHub.LeaveChat(phoneToChat);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ChatClient phoneToChat = new ChatClient();
            phoneToChat.PhoneClientId = App.Current.DeviceID;
            phoneToChat.ChatUserName = App.Current.ChatUserName;

            // Instantiate with default or any other implementation.
            App.Current.SignalRHub = new SignalRMessagingHub();

            // Wire-up to listen to custom event from SignalR Hub.
            App.Current.SignalRHub.SignalRServerNotification += new SignalRServerHandler(SignalRHub_SignalRServerNotification);

            // Send to server for joining Chatroom.
            App.Current.SignalRHub.JoinChat(phoneToChat);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            phoneToChat.PhoneClientId = App.Current.DeviceID;
            phoneToChat.ChatUserName = App.Current.ChatUserName;

            // Unwire.
            App.Current.SignalRHub.SignalRServerNotification -= new SignalRServerHandler(SignalRHub_SignalRServerNotification);

            // Fire off background thread to leave server Chatroom.               
            DisconnectBackgroundDataWorker.RunWorkerAsync();
        }

        protected void SignalRHub_SignalRServerNotification(object sender, SignalREventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                // Add to local ChatRoom.
                chatDialog.Text += "\r\n" + e.ChatMessageFromServer;
            }));
        }
    }
}