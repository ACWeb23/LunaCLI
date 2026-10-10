using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Graphics;
using Windows.UI;
using Zenless.Ui;

namespace Zenless
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            App.Host = this;
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(TitleBar);
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            AppWindow.TitleBar.ButtonBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            AppWindow.TitleBar.ButtonInactiveBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            AppWindow.TitleBar.ButtonForegroundColor = Color.FromArgb(255, 244, 241, 234);
            AppWindow.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(32, 255, 255, 255);
            AppWindow.TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(20, 255, 255, 255);

            if (AppWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.PreferredMinimumWidth = 1024;
                presenter.PreferredMinimumHeight = 700;
            }

            AppWindow.Resize(new SizeInt32(1360, 900));
            PlayerName.Text = App.Player.Name;
            Nav.SelectedItem = Nav.MenuItems[0];
        }

        private void Nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is not NavigationViewItem item || item.Tag is not string tag)
                return;

            Type page = tag switch
            {
                "weapons" => typeof(WeaponsPage),
                "disks" => typeof(DisksPage),
                _ => typeof(CharactersPage)
            };

            if (ContentFrame.CurrentSourcePageType != page)
                ContentFrame.Navigate(page);
        }
    }
}
