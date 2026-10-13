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
            Closed += (_, _) => App.Save();
            PlayerName.Text = App.Player.Name;
            Nav.SelectedItem = Nav.MenuItems[0];
        }

        private int _view;

        /// <summary>Shows the open user's name and rebuilds the current page from that save.</summary>
        internal void ShowPlayer()
        {
            PlayerName.Text = App.Player.Name;
            Type page = ContentFrame.CurrentSourcePageType ?? typeof(CharactersPage);
            ContentFrame.Navigate(page, _view++);
        }

        private async void Users_Click(object sender, RoutedEventArgs e)
        {
            if (Content is not FrameworkElement content || content.XamlRoot == null)
                return;

            List<string> names = UserStore.ListNames();
            var list = new ListView { SelectionMode = ListViewSelectionMode.Single, MaxHeight = 240, MinWidth = 420 };
            for (int i = 0; i < names.Count; i++)
            {
                list.Items.Add(names[i]);
                if (string.Equals(names[i], App.Player.Name, StringComparison.OrdinalIgnoreCase))
                    list.SelectedIndex = i;
            }

            var nameBox = new TextBox
            {
                Header = "New user name",
                PlaceholderText = "Name"
            };
            var error = new TextBlock
            {
                Foreground = ViewHelpers.Brush("GoldBrush"),
                TextWrapping = TextWrapping.Wrap
            };
            var panel = new StackPanel { Spacing = 12, MinWidth = 420 };
            panel.Children.Add(ViewHelpers.Muted("Open a saved user, or create one. A new user starts with an empty roster."));
            if (names.Count > 0)
                panel.Children.Add(list);
            panel.Children.Add(nameBox);
            panel.Children.Add(error);

            var dialog = ViewHelpers.Dialog("Users", panel, content.XamlRoot, "Create");
            if (names.Count > 0)
                dialog.SecondaryButtonText = "Open";

            Player? pending = null;
            dialog.PrimaryButtonClick += (_, args) =>
            {
                try
                {
                    pending = UserStore.Create(nameBox.Text);
                }
                catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or IOException)
                {
                    pending = null;
                    error.Text = ex.Message;
                    args.Cancel = true;
                }
            };
            dialog.SecondaryButtonClick += (_, args) =>
            {
                if (list.SelectedItem is not string selected)
                {
                    error.Text = "Select a user to open.";
                    args.Cancel = true;
                    return;
                }

                try
                {
                    pending = UserStore.Load(selected);
                }
                catch (Exception ex) when (ex is InvalidDataException or IOException or ArgumentException or InvalidOperationException)
                {
                    pending = null;
                    error.Text = ex.Message;
                    args.Cancel = true;
                }
            };

            ContentDialogResult result = await dialog.ShowAsync();
            if (pending != null && result is ContentDialogResult.Primary or ContentDialogResult.Secondary)
                App.SwitchTo(pending);
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
