using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Text;

namespace Zenless.Ui
{
    public sealed partial class CharactersPage : Page
    {
        private readonly Dictionary<Character, Image> _thumbs = new();
        private readonly Dictionary<Character, FrameworkElement> _thumbHolders = new();

        public CharactersPage()
        {
            InitializeComponent();
            Loaded += (_, _) => LoadRoster();
        }

        private void LoadRoster()
        {
            RosterList.Items.Clear();
            _thumbs.Clear();
            _thumbHolders.Clear();

            foreach (Character character in App.Player.Characters)
            {
                RosterList.Items.Add(new ListViewItem
                {
                    Tag = character,
                    Content = RosterRow(character)
                });
            }

            if (RosterList.Items.Count > 0)
                RosterList.SelectedIndex = 0;
            else
                Detail.Children.Add(ViewHelpers.Muted("No characters in the roster."));
        }

        private Grid RosterRow(Character character)
        {
            var row = new Grid { ColumnSpacing = 12 };
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(72) });
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var placeholder = new Border
            {
                Background = new SolidColorBrush(ViewHelpers.ElementColor(character.atkElement)),
                Opacity = 0.85,
                Child = new TextBlock
                {
                    Text = ViewHelpers.Initials(character.getName()),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = ViewHelpers.Brush("InkBrush")
                }
            };
            var photo = new Image { Stretch = Stretch.UniformToFill };
            var frame = new Border
            {
                Width = 72,
                Height = 96,
                CornerRadius = new CornerRadius(12),
                Child = new Grid
                {
                    Children = { placeholder, photo }
                }
            };
            ApplyThumb(photo, placeholder, character.ImagePath);
            _thumbs[character] = photo;
            _thumbHolders[character] = placeholder;
            row.Children.Add(frame);

            var text = new StackPanel { VerticalAlignment = VerticalAlignment.Center, Spacing = 2 };
            text.Children.Add(new TextBlock
            {
                Text = character.getName(),
                FontWeight = FontWeights.SemiBold,
                TextWrapping = TextWrapping.Wrap
            });
            text.Children.Add(ViewHelpers.Muted($"{character.rank}-Rank  ·  {ViewHelpers.Pretty(character.specialty)}"));
            text.Children.Add(ViewHelpers.Muted(ViewHelpers.Pretty(character.atkElement)));
            Grid.SetColumn(text, 1);
            row.Children.Add(text);
            return row;
        }

        private void RosterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RosterList.SelectedItem is ListViewItem item && item.Tag is Character character)
                ShowDetail(character);
        }

        private void ShowDetail(Character character)
        {
            Detail.Children.Clear();
            CombatStats stats = character.GetCombatStats();

            var header = new Grid { ColumnSpacing = 24 };
            header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var portrait = new ImageField { Width = 280, ImageHeight = 340, VerticalAlignment = VerticalAlignment.Top };
            portrait.SetAccent(ViewHelpers.ElementColor(character.atkElement));
            portrait.Bind(
                "Character",
                character.ImagePath,
                path => character.ImagePath = path,
                ViewHelpers.Initials(character.getName()));
            portrait.ImageChanged += (_, _) =>
            {
                if (_thumbs.TryGetValue(character, out Image? photo) && _thumbHolders.TryGetValue(character, out FrameworkElement? holder))
                    ApplyThumb(photo, holder, character.ImagePath);
            };
            header.Children.Add(portrait);

            var info = new StackPanel { Spacing = 12, VerticalAlignment = VerticalAlignment.Center };
            info.Children.Add(new TextBlock
            {
                Text = character.getName(),
                FontSize = 36,
                FontWeight = FontWeights.SemiBold,
                TextWrapping = TextWrapping.Wrap
            });
            info.Children.Add(RankBadge(character));
            info.Children.Add(ViewHelpers.Muted(
                $"{ViewHelpers.Pretty(character.specialty)}   ·   {ViewHelpers.Pretty(character.attackType)}   ·   {ViewHelpers.Pretty(character.atkElement)}"));
            info.Children.Add(StatBlock(stats));
            Grid.SetColumn(info, 1);
            header.Children.Add(info);
            Detail.Children.Add(header);

            Detail.Children.Add(ViewHelpers.SectionTitle("W-Engine"));
            Weapon? weapon = character.GetEquippedWeapon();
            if (weapon == null)
                Detail.Children.Add(ViewHelpers.Muted("No W-Engine equipped."));
            else
                Detail.Children.Add(WeaponBlock(weapon));

            Detail.Children.Add(ViewHelpers.SectionTitle("Disk drives"));
            Detail.Children.Add(DiskGrid(character));

            Detail.Children.Add(ViewHelpers.SectionTitle("Set bonuses"));
            var lines = character.FormatSetBonuses();
            if (lines.Count == 0)
            {
                Detail.Children.Add(ViewHelpers.Muted("No set bonus yet."));
            }
            else
            {
                foreach (string line in lines)
                {
                    bool nested = line.StartsWith("      ", StringComparison.Ordinal);
                    Detail.Children.Add(new TextBlock
                    {
                        Text = line.Trim().Replace('_', ' '),
                        Opacity = nested ? 0.75 : 0.95,
                        Margin = new Thickness(nested ? 16 : 0, 0, 0, 0),
                        TextWrapping = TextWrapping.Wrap
                    });
                }
            }
        }

        private static Border RankBadge(Character character)
        {
            return new Border
            {
                Background = ViewHelpers.Brush("GoldBrush"),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(10, 3, 10, 3),
                HorizontalAlignment = HorizontalAlignment.Left,
                Child = new TextBlock
                {
                    Text = $"{character.rank}-Rank",
                    FontWeight = FontWeights.SemiBold,
                    Foreground = ViewHelpers.Brush("InkBrush")
                }
            };
        }

        private static Grid StatBlock(CombatStats stats)
        {
            (string Label, string Value)[] lines =
            {
                ("HP", stats.HP.ToString("N0")),
                ("Crit Rate", stats.CritRate.ToString("P1")),
                ("ATK", stats.ATK.ToString("N0")),
                ("Crit DMG", stats.CritDamage.ToString("P1")),
                ("DEF", stats.DEF.ToString("N0")),
                ("Anomaly Proficiency", stats.AnomalyProficiency.ToString("N0")),
                ("Impact", stats.Impact.ToString("N1")),
                ("Anomaly Mastery", stats.AnomalyMastery.ToString("N1")),
                ("PEN Ratio", stats.PenRatio.ToString("P1")),
                ("Energy Regen", stats.EnergyRegen.ToString("N2")),
                ("DMG Bonus", stats.DamageBonus.ToString("P1"))
            };

            var grid = new Grid { ColumnSpacing = 28, RowSpacing = 8, MaxWidth = 640 };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < lines.Length; i++)
            {
                int row = i / 2;
                if (grid.RowDefinitions.Count <= row)
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var pair = new Grid();
                pair.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                pair.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                pair.Children.Add(new TextBlock { Text = lines[i].Label, Opacity = 0.65, Margin = new Thickness(0, 0, 12, 0) });
                var value = new TextBlock { Text = lines[i].Value, FontWeight = FontWeights.SemiBold };
                Grid.SetColumn(value, 1);
                pair.Children.Add(value);
                Grid.SetRow(pair, row);
                Grid.SetColumn(pair, i % 2);
                grid.Children.Add(pair);
            }

            return grid;
        }

        private static UIElement WeaponBlock(Weapon weapon)
        {
            var (atk, advanced) = weapon.get_Base_Stats();
            var row = new Grid { ColumnSpacing = 16 };
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var image = new ImageField { Width = 200, ImageHeight = 180, Fit = Stretch.Uniform, VerticalAlignment = VerticalAlignment.Top };
            image.Bind("Weapon", weapon.ImagePath, path => weapon.ImagePath = path, "W-Engine");
            row.Children.Add(image);

            var text = new StackPanel { Spacing = 4, VerticalAlignment = VerticalAlignment.Center };
            text.Children.Add(new TextBlock
            {
                Text = weapon.get_Name(),
                FontSize = 22,
                FontWeight = FontWeights.SemiBold,
                TextWrapping = TextWrapping.Wrap
            });
            text.Children.Add(ViewHelpers.Muted($"{weapon.BaseStatLabel}  {atk:N0}"));
            foreach (var stat in advanced)
                text.Children.Add(ViewHelpers.Muted($"{ViewHelpers.Pretty(stat.Key)}  {ViewHelpers.FormatStat(stat.Key, stat.Value)}"));
            Grid.SetColumn(text, 1);
            row.Children.Add(text);
            return row;
        }

        private Grid DiskGrid(Character character)
        {
            var grid = new Grid { ColumnSpacing = 12, RowSpacing = 12 };
            for (int column = 0; column < 3; column++)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            for (int slot = 1; slot <= 6; slot++)
            {
                int captured = slot;
                var stack = new StackPanel { Spacing = 6 };
                Disk_Drive? disk = character.GetDiskInSlot(slot);
                var image = new ImageField { ImageHeight = 132, Fit = Stretch.Uniform };
                if (disk == null)
                {
                    image.ShowEmpty("Disk drive", $"Slot {slot}");
                    stack.Children.Add(image);
                    stack.Children.Add(ViewHelpers.Muted("Empty"));
                }
                else
                {
                    Disk_Drive bound = disk;
                    image.Bind("Disk drive", bound.ImagePath, path => bound.ImagePath = path, $"Slot {slot}");
                    stack.Children.Add(image);
                    stack.Children.Add(new TextBlock
                    {
                        Text = ViewHelpers.Pretty(bound.DriveSet),
                        FontWeight = FontWeights.SemiBold,
                        TextWrapping = TextWrapping.Wrap
                    });
                    stack.Children.Add(ViewHelpers.Muted(
                        $"Slot {bound.Drive_Num}  ·  {ViewHelpers.Pretty(bound.MainStat)}  {ViewHelpers.FormatStat(bound.MainStat, bound.MainStatValue)}"));
                }

                var actions = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
                var equip = new Button { Content = disk == null ? "Equip" : "Change" };
                equip.Click += async (_, _) => await PickDisk(character, captured);
                actions.Children.Add(equip);
                if (disk != null)
                {
                    var clear = new Button { Content = "Unequip" };
                    clear.Click += (_, _) =>
                    {
                        App.Player.UnequipDisk(character, captured);
                        ShowDetail(character);
                    };
                    actions.Children.Add(clear);
                }
                stack.Children.Add(actions);

                var card = ViewHelpers.Card(stack);
                Grid.SetRow(card, (slot - 1) / 3);
                Grid.SetColumn(card, (slot - 1) % 3);
                grid.Children.Add(card);
            }

            return grid;
        }

        private async void AddCharacter_Click(object sender, RoutedEventArgs e)
        {
            List<Character> missing = CharacterRoster.CreateAll()
                .Where(agent => App.Player.GetCharacter(agent.characterID) == null)
                .OrderBy(agent => agent.getName())
                .ToList();
            if (missing.Count == 0)
            {
                await ViewHelpers.Dialog("Add character", "Every agent is already in the roster.", XamlRoot).ShowAsync();
                return;
            }

            var list = new ListView { SelectionMode = ListViewSelectionMode.Single, MaxHeight = 360 };
            foreach (Character agent in missing)
            {
                list.Items.Add(new ListViewItem
                {
                    Tag = agent.characterID,
                    Content = $"{agent.getName()}    {agent.rank}-Rank    {ViewHelpers.Pretty(agent.specialty)} / {ViewHelpers.Pretty(agent.atkElement)}"
                });
            }
            list.SelectedIndex = 0;

            var minds = new NumberBox
            {
                Header = "Mindscapes",
                Minimum = 0,
                Maximum = 6,
                Value = 0,
                SmallChange = 1,
                SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline
            };
            var panel = new StackPanel { Spacing = 10, MinWidth = 420 };
            panel.Children.Add(minds);
            panel.Children.Add(list);

            var dialog = ViewHelpers.Dialog("Add character", panel, XamlRoot, "Add");
            if (await dialog.ShowAsync() != ContentDialogResult.Primary
                || list.SelectedItem is not ListViewItem item
                || item.Tag is not int id)
                return;

            Character created = CharacterRoster.Create(id, (int)Math.Clamp(minds.Value, 0, 6));
            App.Player.AddCharacter(created);
            LoadRoster();
        }

        private async Task PickDisk(Character character, int slot)
        {
            List<Disk_Drive> options = App.Player.UnequippedDiskDrives
                .Where(disk => disk.Drive_Num == slot)
                .OrderBy(disk => disk.DiskID)
                .ToList();
            if (options.Count == 0)
            {
                await ViewHelpers.Dialog("Equip disk drive", $"No unequipped slot {slot} disks are in the inventory.", XamlRoot).ShowAsync();
                return;
            }

            var list = new ListView { SelectionMode = ListViewSelectionMode.Single, MaxHeight = 360, MinWidth = 420 };
            foreach (Disk_Drive disk in options)
            {
                list.Items.Add(new ListViewItem
                {
                    Tag = disk,
                    Content = $"{ViewHelpers.Pretty(disk.DriveSet)}    {ViewHelpers.Pretty(disk.MainStat)} {ViewHelpers.FormatStat(disk.MainStat, disk.MainStatValue)}"
                });
            }
            list.SelectedIndex = 0;

            var dialog = ViewHelpers.Dialog($"Equip slot {slot}", list, XamlRoot, "Equip");
            if (await dialog.ShowAsync() != ContentDialogResult.Primary
                || list.SelectedItem is not ListViewItem item
                || item.Tag is not Disk_Drive chosen)
                return;

            App.Player.EquipDisk(character, chosen);
            ShowDetail(character);
        }

        private static void ApplyThumb(Image photo, FrameworkElement placeholder, string? path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                photo.Source = null;
                photo.Visibility = Visibility.Collapsed;
                placeholder.Visibility = Visibility.Visible;
                return;
            }

            var bitmap = new BitmapImage { DecodePixelWidth = 200 };
            bitmap.UriSource = new Uri(Path.GetFullPath(path));
            photo.Source = bitmap;
            photo.Visibility = Visibility.Visible;
            placeholder.Visibility = Visibility.Collapsed;
        }
    }
}
