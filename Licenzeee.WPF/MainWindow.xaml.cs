using MahApps.Metro.Controls;

namespace Fateblade.Licenzeee.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HamburgerMenu_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs args)
        {
            if (HamburgerMenuControl.IsPaneOpen)
            {
                HamburgerMenuControl.IsPaneOpen = false;
            }
        }
    }
}
