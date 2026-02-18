using System.Windows;

namespace DetlaLauncher.Windows
{
    public partial class ConfirmDialog : Window
    {
        public bool Result { get; private set; }

        public ConfirmDialog(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }
    }
}