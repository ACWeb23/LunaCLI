using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Zenless.Ui
{
    public sealed partial class WeaponsPage : Page
    {
        private readonly List<FrameworkElement> _cards = new();

        public WeaponsPage()
        {
            InitializeComponent();
            SizeChanged += (_, e) => ViewHelpers.Relayout(CardHost, _cards, e.NewSize.Width - 32, 280);
            Loaded += (_, _) => LoadCards();
        }

        private void LoadCards()
        {
            _cards.Clear();
            Player player = App.Player;
            var weapons = player.Weapons
                .OrderBy(weapon => ViewHelpers.WeaponHolder(player, weapon) == null)
                .ThenBy(weapon => weapon.get_Name())
                .ToList();

            Subtitle.Text = weapons.Count == 0
                ? "No weapons in the inventory."
                : $"{weapons.Count} owned. Portraits come with the app. You can still replace one.";

            foreach (Weapon weapon in weapons)
                _cards.Add(WeaponCard(player, weapon));

            ViewHelpers.Relayout(CardHost, _cards, ActualWidth - 32, 280);
        }

        private static Border WeaponCard(Player player, Weapon weapon)
        {
            var (atk, advanced) = weapon.get_Base_Stats();
            Character? holder = ViewHelpers.WeaponHolder(player, weapon);

            var image = new ImageField { ImageHeight = 180, Fit = Stretch.Uniform };
            image.Bind("Weapon", weapon.ImagePath, path => weapon.ImagePath = path, "W-Engine");

            var stack = new StackPanel { Spacing = 6 };
            stack.Children.Add(image);
            stack.Children.Add(new TextBlock
            {
                Text = weapon.get_Name(),
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                TextWrapping = TextWrapping.Wrap
            });
            stack.Children.Add(ViewHelpers.Muted($"{weapon.BaseStatLabel}  {atk:N0}"));
            foreach (var stat in advanced)
                stack.Children.Add(ViewHelpers.Muted($"{ViewHelpers.Pretty(stat.Key)}  {ViewHelpers.FormatStat(stat.Key, stat.Value)}"));
            stack.Children.Add(new TextBlock
            {
                Text = holder != null ? $"Equipped on {holder.getName()}" : "In inventory",
                Foreground = holder != null ? ViewHelpers.Brush("GoldBrush") : ViewHelpers.Brush("MutedBrush"),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 4, 0, 0)
            });

            return ViewHelpers.Card(stack);
        }
    }
}
