using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Zenless.Ui
{
    public sealed partial class DisksPage : Page
    {
        private readonly List<FrameworkElement> _cards = new();

        public DisksPage()
        {
            InitializeComponent();
            SizeChanged += (_, e) => ViewHelpers.Relayout(CardHost, _cards, e.NewSize.Width - 32, 280);
            Loaded += (_, _) => LoadCards();
        }

        private void LoadCards()
        {
            _cards.Clear();
            Player player = App.Player;
            var disks = player.DiskDrives
                .OrderBy(disk => ViewHelpers.DiskHolder(player, disk) == null)
                .ThenBy(disk => disk.Drive_Num)
                .ThenBy(disk => disk.DiskID)
                .ToList();

            Subtitle.Text = disks.Count == 0
                ? "No disk drives yet. Create one to add it to the inventory."
                : $"{disks.Count} owned. Create a drive, or equip one onto an agent.";

            foreach (Disk_Drive disk in disks)
                _cards.Add(DiskCard(player, disk));

            ViewHelpers.Relayout(CardHost, _cards, ActualWidth - 32, 280);
        }

        private Border DiskCard(Player player, Disk_Drive disk)
        {
            Character? holder = ViewHelpers.DiskHolder(player, disk);
            var image = new ImageField { ImageHeight = 150, Fit = Stretch.Uniform };
            image.Bind("Disk drive", disk.ImagePath, path => disk.ImagePath = path, $"Slot {disk.Drive_Num}");

            var stack = new StackPanel { Spacing = 4 };
            stack.Children.Add(image);
            stack.Children.Add(new TextBlock
            {
                Text = ViewHelpers.Pretty(disk.DriveSet),
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                TextWrapping = TextWrapping.Wrap
            });
            stack.Children.Add(ViewHelpers.Muted($"ID {disk.DiskID}  ·  Slot {disk.Drive_Num}"));
            stack.Children.Add(ViewHelpers.Muted(
                $"Main  {ViewHelpers.Pretty(disk.MainStat)}  {ViewHelpers.FormatStat(disk.MainStat, disk.MainStatValue)}"));

            foreach (var sub in disk.GetSubstats())
            {
                double value = Disk_Drive.SubstatRollValue(sub.Stat) * sub.NumRoll;
                string rolls = sub.NumRoll == 1 ? "roll" : "rolls";
                stack.Children.Add(ViewHelpers.Muted(
                    $"{ViewHelpers.Pretty(sub.Stat)}  {ViewHelpers.FormatStat(sub.Stat, value)}  ·  {sub.NumRoll} {rolls}"));
            }

            stack.Children.Add(new TextBlock
            {
                Text = holder != null ? $"Equipped on {holder.getName()}" : "In inventory",
                Foreground = holder != null ? ViewHelpers.Brush("GoldBrush") : ViewHelpers.Brush("MutedBrush"),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 6, 0, 0)
            });

            if (holder == null)
            {
                var equip = new Button
                {
                    Content = "Equip",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(0, 8, 0, 0)
                };
                Disk_Drive bound = disk;
                equip.Click += async (_, _) => await EquipFromInventory(bound);
                stack.Children.Add(equip);
            }

            return ViewHelpers.Card(stack);
        }

        private async void NewDisk_Click(object sender, RoutedEventArgs e)
        {
            var setBox = ChoiceBox(Enum.GetValues<driveSet>().OrderBy(Disk_Drive.SetName), Disk_Drive.SetName);
            var slotBox = new ComboBox { HorizontalAlignment = HorizontalAlignment.Stretch };
            for (int slot = 1; slot <= 6; slot++)
                slotBox.Items.Add(new ComboBoxItem { Content = $"Slot {slot}", Tag = slot });
            slotBox.SelectedIndex = 0;

            var mainBox = new ComboBox { HorizontalAlignment = HorizontalAlignment.Stretch };
            var mainValue = new NumberBox
            {
                SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline,
                SmallChange = 0.01,
                LargeChange = 0.05,
                Value = 0.24
            };
            var mainPanel = new StackPanel { Spacing = 6 };
            mainPanel.Children.Add(ViewHelpers.Muted("Main stat"));
            mainPanel.Children.Add(mainBox);
            mainPanel.Children.Add(ViewHelpers.Muted("Main stat value. Percents are fractions, so 0.24 is 24%."));
            mainPanel.Children.Add(mainValue);

            var subBoxes = new ComboBox[4];
            var rollBoxes = new NumberBox[4];
            advancedStats[] defaults =
            {
                advancedStats.ATK_Percent,
                advancedStats.Crit_Rate,
                advancedStats.Crit_Damage,
                advancedStats.Anomaly_Proficiency
            };

            var form = new StackPanel { Spacing = 10, MinWidth = 420 };
            form.Children.Add(ViewHelpers.Muted("Drive set"));
            form.Children.Add(setBox);
            form.Children.Add(ViewHelpers.Muted("Slot"));
            form.Children.Add(slotBox);
            form.Children.Add(mainPanel);

            for (int i = 0; i < 4; i++)
            {
                subBoxes[i] = ChoiceBox(Disk_Drive.SubstatOptions, stat => ViewHelpers.Pretty(stat));
                Select(subBoxes[i], defaults[i]);
                rollBoxes[i] = new NumberBox
                {
                    Minimum = 1,
                    Maximum = 6,
                    Value = 2,
                    SmallChange = 1,
                    SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline
                };
                var row = new Grid { ColumnSpacing = 8 };
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(96) });
                row.Children.Add(subBoxes[i]);
                Grid.SetColumn(rollBoxes[i], 1);
                row.Children.Add(rollBoxes[i]);
                form.Children.Add(ViewHelpers.Muted($"Substat {i + 1} and rolls"));
                form.Children.Add(row);
            }

            var error = new TextBlock { Foreground = ViewHelpers.Brush("GoldBrush"), TextWrapping = TextWrapping.Wrap };
            form.Children.Add(error);

            void RefreshMain()
            {
                int slot = Selected<int>(slotBox);
                mainPanel.Visibility = slot >= 4 ? Visibility.Visible : Visibility.Collapsed;
                if (slot < 4)
                    return;

                advancedStats previous = mainBox.SelectedItem is ComboBoxItem item && item.Tag is advancedStats chosen
                    ? chosen
                    : Disk_Drive.MainStatOptions(slot)[0];
                mainBox.Items.Clear();
                foreach (advancedStats option in Disk_Drive.MainStatOptions(slot))
                    mainBox.Items.Add(new ComboBoxItem { Content = ViewHelpers.Pretty(option), Tag = option });
                Select(mainBox, previous);
            }

            slotBox.SelectionChanged += (_, _) => RefreshMain();
            mainBox.SelectionChanged += (_, _) =>
            {
                if (mainBox.SelectedItem is ComboBoxItem item && item.Tag is advancedStats stat)
                    mainValue.Value = Disk_Drive.SuggestedMainValue(stat);
            };
            RefreshMain();

            var dialog = ViewHelpers.Dialog("New disk drive", new ScrollViewer { Content = form, MaxHeight = 520 }, XamlRoot, "Create");
            dialog.PrimaryButtonClick += (_, args) =>
            {
                try
                {
                    var subs = new Dictionary<advancedStats, int>();
                    for (int i = 0; i < 4; i++)
                    {
                        advancedStats stat = Selected<advancedStats>(subBoxes[i]);
                        if (!subs.TryAdd(stat, (int)Math.Round(rollBoxes[i].Value)))
                            throw new ArgumentException("Choose four different substats.");
                    }

                    int slot = Selected<int>(slotBox);
                    advancedStats main = slot >= 4 ? Selected<advancedStats>(mainBox) : advancedStats.nill;
                    double value = slot >= 4 ? mainValue.Value : 0;
                    App.Player.CreateDiskDrive(Selected<driveSet>(setBox), subs, slot, main, value);
                }
                catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                {
                    error.Text = ex.Message;
                    args.Cancel = true;
                }
            };

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                LoadCards();
        }

        private async Task EquipFromInventory(Disk_Drive disk)
        {
            List<Character> roster = App.Player.Characters.OrderBy(character => character.getName()).ToList();
            var combo = new ComboBox { HorizontalAlignment = HorizontalAlignment.Stretch, MinWidth = 320 };
            foreach (Character character in roster)
            {
                Disk_Drive? occupied = character.GetDiskInSlot(disk.Drive_Num);
                string suffix = occupied == null ? "empty slot" : $"replaces {ViewHelpers.Pretty(occupied.DriveSet)}";
                combo.Items.Add(new ComboBoxItem
                {
                    Content = $"{character.getName()}  ·  slot {disk.Drive_Num} {suffix}",
                    Tag = character
                });
            }
            combo.SelectedIndex = 0;

            var dialog = ViewHelpers.Dialog("Equip disk drive", combo, XamlRoot, "Equip");
            if (await dialog.ShowAsync() != ContentDialogResult.Primary
                || combo.SelectedItem is not ComboBoxItem item
                || item.Tag is not Character target)
                return;

            App.Player.EquipDisk(target, disk);
            LoadCards();
        }

        private static ComboBox ChoiceBox<T>(IEnumerable<T> values, Func<T, string> label) where T : notnull
        {
            var box = new ComboBox { HorizontalAlignment = HorizontalAlignment.Stretch };
            foreach (T value in values)
                box.Items.Add(new ComboBoxItem { Content = label(value), Tag = value });
            if (box.Items.Count > 0)
                box.SelectedIndex = 0;
            return box;
        }

        private static void Select(ComboBox box, advancedStats stat)
        {
            foreach (ComboBoxItem item in box.Items)
            {
                if (item.Tag is advancedStats candidate && candidate == stat)
                {
                    box.SelectedItem = item;
                    return;
                }
            }
            if (box.Items.Count > 0)
                box.SelectedIndex = 0;
        }

        private static T Selected<T>(ComboBox box)
        {
            if (box.SelectedItem is ComboBoxItem item && item.Tag is T value)
                return value;
            throw new ArgumentException("Choose a value for every field.");
        }
    }
}
