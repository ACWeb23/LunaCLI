using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Zenless.Ui
{
    public sealed partial class ImageField : UserControl
    {
        private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tif", ".tiff", ".webp", ".jfif"
        };

        private Action<string?>? _commit;

        public ImageField()
        {
            InitializeComponent();
            Picture.ImageFailed += OnImageFailed;
        }

        public double ImageHeight
        {
            get => Chrome.Height;
            set => Chrome.Height = value;
        }

        public Stretch Fit { get; set; } = Stretch.UniformToFill;

        public event EventHandler? ImageChanged;

        public void Bind(string label, string? path, Action<string?> commit, string placeholder)
        {
            FieldLabel.Text = label;
            PlaceholderText.Text = placeholder;
            _commit = commit;
            AllowDrop = true;
            ChooseButton.Visibility = Visibility.Visible;
            Show(path);
        }

        /// <summary>An empty equipment slot. The well is visible, and no file can be chosen.</summary>
        public void ShowEmpty(string label, string message)
        {
            FieldLabel.Text = label;
            PlaceholderText.Text = message;
            _commit = null;
            AllowDrop = false;
            Picture.Source = null;
            Picture.Visibility = Visibility.Collapsed;
            PlaceholderHost.Visibility = Visibility.Visible;
            ChooseButton.Content = "Empty";
            ChooseButton.IsEnabled = false;
            RemoveButton.Visibility = Visibility.Collapsed;
        }

        public void SetAccent(Windows.UI.Color color)
        {
            Chrome.BorderBrush = new SolidColorBrush(color);
            Chrome.BorderThickness = new Thickness(1.5);
        }

        private void Show(string? path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                Picture.Source = null;
                Picture.Visibility = Visibility.Collapsed;
                PlaceholderHost.Visibility = Visibility.Visible;
                ChooseButton.Content = "Choose image";
                RemoveButton.Visibility = Visibility.Collapsed;
                return;
            }

            var bitmap = new BitmapImage { DecodePixelWidth = 720 };
            Picture.Stretch = Fit;
            bitmap.UriSource = new Uri(Path.GetFullPath(path));
            Picture.Source = bitmap;
            Picture.Visibility = Visibility.Visible;
            PlaceholderHost.Visibility = Visibility.Collapsed;
            ChooseButton.Content = "Change image";
            RemoveButton.Visibility = Visibility.Visible;
        }

        private void Commit(string? path)
        {
            _commit?.Invoke(path);
            Show(path);
            ImageChanged?.Invoke(this, EventArgs.Empty);
        }

        private async void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_commit == null || App.Host == null)
                return;

            try
            {
                var picker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.Thumbnail
                };
                try
                {
                    picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                }
                catch (Exception)
                {
                    // Unpackaged apps can open the picker without a pictures-library capability.
                }
                picker.FileTypeFilter.Add(".png");
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".bmp");
                picker.FileTypeFilter.Add(".gif");
                picker.FileTypeFilter.Add(".webp");

                IntPtr hwnd = WindowNative.GetWindowHandle(App.Host);
                InitializeWithWindow.Initialize(picker, hwnd);

                StorageFile file = await picker.PickSingleFileAsync();
                if (file == null)
                    return;
                Commit(file.Path);
            }
            catch (Exception ex)
            {
                PlaceholderText.Text = ex.Message;
                PlaceholderHost.Visibility = Visibility.Visible;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_commit == null)
                return;
            Commit(null);
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (_commit == null)
            {
                e.AcceptedOperation = DataPackageOperation.None;
                return;
            }

            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "Set image";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsGlyphVisible = false;
        }

        private async void OnDrop(object sender, DragEventArgs e)
        {
            if (_commit == null || !e.DataView.Contains(StandardDataFormats.StorageItems))
                return;

            var items = await e.DataView.GetStorageItemsAsync();
            if (items.Count == 0 || items[0] is not StorageFile file)
                return;
            if (!ImageExtensions.Contains(Path.GetExtension(file.Path)))
            {
                PlaceholderText.Text = "Use a PNG, JPG, BMP, GIF, or WEBP file";
                return;
            }

            Commit(file.Path);
        }

        private void OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Picture.Source = null;
            Picture.Visibility = Visibility.Collapsed;
            PlaceholderHost.Visibility = Visibility.Visible;
            PlaceholderText.Text = "Could not open that image";
            ChooseButton.Content = "Choose image";
            RemoveButton.Visibility = Visibility.Visible;
        }
    }
}
