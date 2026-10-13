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
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (ReferenceEquals(_player, player))
                return;

            _player = player;
            player.Changed += () => UserStore.Save(player);
        }

        /// <summary>Writes the open user, then makes <paramref name="player"/> the one on screen.</summary>
        internal static void SwitchTo(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (_player != null)
                UserStore.Save(_player);
            UsePlayer(player);
            UserStore.SetActive(player.Name);
            Host?.ShowPlayer();
        }

        /// <summary>Writes the open user. Image edits do not go through <see cref="Player"/>, so the window calls this too.</summary>
        internal static void Save()
        {
            if (_player != null)
                UserStore.Save(_player);
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
