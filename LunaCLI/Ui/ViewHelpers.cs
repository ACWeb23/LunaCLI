using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace Zenless.Ui
{
    internal static class ViewHelpers
    {
        public static Brush Brush(string key) => (Brush)Application.Current.Resources[key];

        public static string Pretty(Enum value) => value.ToString().Replace('_', ' ');

        public static string FormatStat(advancedStats stat, double value)
        {
            return Character.IsRatioStat(stat) ? value.ToString("P1") : value.ToString("N0");
        }

        public static string Initials(string name)
        {
            string[] parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return "?";
            if (parts.Length == 1)
                return parts[0].Length <= 2 ? parts[0].ToUpperInvariant() : parts[0][..2].ToUpperInvariant();
            return string.Concat(char.ToUpperInvariant(parts[0][0]), char.ToUpperInvariant(parts[^1][0]));
        }

        public static Color ElementColor(atkElement element)
        {
            return element switch
            {
                atkElement.Physical => Color.FromArgb(255, 196, 190, 178),
                atkElement.Fire => Color.FromArgb(255, 232, 93, 76),
                atkElement.Ice => Color.FromArgb(255, 126, 200, 227),
                atkElement.Electric => Color.FromArgb(255, 240, 180, 41),
                atkElement.Wind => Color.FromArgb(255, 125, 206, 160),
                atkElement.Ether => Color.FromArgb(255, 195, 155, 211),
                atkElement.Lumiflux => Color.FromArgb(255, 242, 211, 138),
                _ => Color.FromArgb(255, 168, 162, 150)
            };
        }

        public static Border Card(UIElement content)
        {
            return new Border
            {
                Margin = new Thickness(8),
                Padding = new Thickness(14),
                CornerRadius = new CornerRadius(18),
                Background = Brush("CardBrush"),
                BorderBrush = Brush("LineBrush"),
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Child = content
            };
        }

        public static TextBlock SectionTitle(string text)
        {
            return new TextBlock
            {
                Text = text,
                FontSize = 20,
                FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
                Margin = new Thickness(0, 8, 0, 0)
            };
        }

        public static TextBlock Muted(string text)
        {
            return new TextBlock
            {
                Text = text,
                Foreground = Brush("MutedBrush"),
                TextWrapping = TextWrapping.Wrap
            };
        }

        /// <summary>
        /// Lays cards into a wrapping grid. Each card keeps at least <paramref name="minCardWidth"/>.
        /// </summary>
        public static void Relayout(Grid host, IReadOnlyList<FrameworkElement> cards, double width, double minCardWidth)
        {
            host.Children.Clear();
            host.ColumnDefinitions.Clear();
            host.RowDefinitions.Clear();
            if (cards.Count == 0 || width <= 0)
                return;

            int columns = Math.Max(1, (int)Math.Floor(width / minCardWidth));
            for (int column = 0; column < columns; column++)
                host.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            int rows = (int)Math.Ceiling(cards.Count / (double)columns);
            for (int row = 0; row < rows; row++)
                host.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            for (int index = 0; index < cards.Count; index++)
            {
                FrameworkElement card = cards[index];
                Grid.SetRow(card, index / columns);
                Grid.SetColumn(card, index % columns);
                host.Children.Add(card);
            }
        }

        public static Character? WeaponHolder(Player player, Weapon weapon)
        {
            return player.Characters.FirstOrDefault(character => player.GetEquippedWeapon(character) == weapon);
        }

        public static Character? DiskHolder(Player player, Disk_Drive disk)
        {
            return player.Characters.FirstOrDefault(character => player.GetEquippedDisks(character).Contains(disk));
        }
    }
}
