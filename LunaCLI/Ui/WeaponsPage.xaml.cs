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
                ? "No weapons yet. Add a W-Engine to put it in the inventory."
                : $"{weapons.Count} owned. Add another copy, delete one, or replace a portrait.";

            foreach (Weapon weapon in weapons)
                _cards.Add(WeaponCard(player, weapon));

            ViewHelpers.Relayout(CardHost, _cards, ActualWidth - 32, 280);
        }

        private Border WeaponCard(Player player, Weapon weapon)
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

            var delete = new Button
            {
                Content = "Delete",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 8, 0, 0)
            };
            Weapon bound = weapon;
            Character? equippedOn = holder;
            delete.Click += async (_, _) => await DeleteWeapon(bound, equippedOn);
            stack.Children.Add(delete);

            return ViewHelpers.Card(stack);
        }

        private async Task DeleteWeapon(Weapon weapon, Character? holder)
        {
            string detail = holder == null
                ? $"Delete {weapon.get_Name()} from the inventory?"
                : $"{weapon.get_Name()} is equipped on {holder.getName()}. Delete it from the inventory?";
            var dialog = ViewHelpers.Dialog("Delete weapon", detail, XamlRoot, "Delete");
            if (await dialog.ShowAsync() != ContentDialogResult.Primary)
                return;

            App.Player.RemoveWeapon(weapon);
            LoadCards();
        }

        private async void AddWeapon_Click(object sender, RoutedEventArgs e)
        {
            List<Weapon> catalog = WeaponRoster.CreateAll()
                .OrderBy(weapon => weapon.get_Name())
                .ToList();
            if (catalog.Count == 0)
            {
                await ViewHelpers.Dialog("Add weapon", "No W-Engines are available.", XamlRoot).ShowAsync();
                return;
            }

            var list = new ListView { SelectionMode = ListViewSelectionMode.Single, MaxHeight = 360, MinWidth = 420 };
            foreach (Weapon weapon in catalog)
            {
                var (baseValue, advanced) = weapon.get_Base_Stats();
                string stats = string.Join("    ", advanced.Select(stat =>
                    $"{ViewHelpers.Pretty(stat.Key)} {ViewHelpers.FormatStat(stat.Key, stat.Value)}"));
                list.Items.Add(new ListViewItem
                {
                    Tag = weapon.WeaponID,
                    Content = $"{weapon.get_Name()}    {weapon.BaseStatLabel} {baseValue:N0}    {stats}"
                });
            }
            list.SelectedIndex = 0;

            var dialog = ViewHelpers.Dialog("Add weapon", list, XamlRoot, "Add");
            if (await dialog.ShowAsync() != ContentDialogResult.Primary
                || list.SelectedItem is not ListViewItem item
                || item.Tag is not int id)
                return;

            App.Player.AddWeapon(WeaponRoster.Create(id));
            LoadCards();
        }
    }
}
