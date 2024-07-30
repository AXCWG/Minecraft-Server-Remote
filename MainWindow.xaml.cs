using RconSharp;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        bool authenticated;
        RconClient? client;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            
            if (addr.Text.IndexOf(":") == -1)
            {
                
                client = RconClient.Create(addr.Text, 25575);

            }
            else
            {
                client = RconClient.Create(addr.Text, Convert.ToInt32(addr.Text.Substring(addr.Text.IndexOf(":"))));


            }
            await client.ConnectAsync();
            this.authenticated = await client.AuthenticateAsync(pw.Password);
            if (this.authenticated)
            {

                var testrun = await client.ExecuteCommandAsync("list", false);

                Output.Text += "Success. " + "\n";
                Controls.IsEnabled = false;
                commandbox.IsEnabled = true;
                disconnect.IsEnabled = true;
            }
            else
            {
                Output.Text += "Try another password. \n";
            }

        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Output.Text += "Disconnected. \n";
            client?.Disconnect();
            Controls.IsEnabled = true;
            commandbox.IsEnabled = false;
            disconnect.IsEnabled = false;
        }

        
        
        private async void commandbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                if (commandbox.Text == "disconnect")
                {
                    Button_Click_2(null, null);
                    commandbox.Text = null;

                }
                else
                {
                    Output.Text += await send_n_return(commandbox.Text) + "\n";
                    commandbox.Text = null;


                }
            }
        }
        private async Task<string> send_n_return(string command)
        {
            
             
            
            return await client?.ExecuteCommandAsync(command, false);
        }
    }
}