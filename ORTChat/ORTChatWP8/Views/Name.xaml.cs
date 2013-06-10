namespace ORTChatWP8.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;

    public partial class Name : PhoneApplicationPage
    {
        public Name()
        {
            this.InitializeComponent();

            GoButton.IsEnabled = false;
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Habilitamos el botón para pasar a la siguiente pantalla según se haya ingresado texto o no
            if (NameTextBox.Text.Trim().Length > 0)
            {
                GoButton.IsEnabled = true;
            }
            else
            {
                GoButton.IsEnabled = false;
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            // Guardamos el nombre de usuario ingresado
            App.Current.ChatUserName = NameTextBox.Text.Trim();
            this.NavigationService.Navigate(new Uri("/Views/Chat.xaml", UriKind.Relative));
        }
    }
}