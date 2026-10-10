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
                ? "No disk drives in the inventory."
                : $"{disks.Count} owned. Each drive has an image field — choose a file or drop one on it.";

            foreach (Disk_Drive disk in disks)
                _cards.Add(DiskCard(player, disk));

            ViewHelpers.Relayout(CardHost, _cards, ActualWidth - 32, 280);
        }

        private static Border DiskCard(Player player, Disk_Drive disk)
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

            return ViewHelpers.Card(stack);
        }
    }
}
