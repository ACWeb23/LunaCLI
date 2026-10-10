using Microsoft.UI.Xaml;

namespace Zenless
{
    /// <summary>
    /// WinUI application. Started by default, and also by <c>--ui</c>. The terminal menu never constructs this type.
    /// </summary>
    public partial class App : Application
    {
        private static Player? _player;

        internal static Player Player => _player ?? throw new InvalidOperationException("No player is loaded.");

        internal static MainWindow? Host { get; set; }

        internal static void UsePlayer(Player player)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
        }

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var window = new MainWindow();
            Host = window;
            window.Activate();
        }
    }
}
