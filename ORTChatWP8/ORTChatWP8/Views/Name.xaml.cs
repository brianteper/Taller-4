﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ORTChatWP8.Views
{
    public partial class Name : PhoneApplicationPage
    {
        public Name()
        {
            InitializeComponent();

            goButton.IsEnabled = false;
        }

        private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (nameTextBox.Text.Trim().Length > 0)
                goButton.IsEnabled = true;
            else
                goButton.IsEnabled = false;
        }

        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.ChatUserName = nameTextBox.Text.Trim();
            this.NavigationService.Navigate(new Uri("/Views/Chat.xaml", UriKind.Relative));
        }
    }
}