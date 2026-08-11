using PixelRecolor.Wpf;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PixelRecolor.Demo;

public partial class MainWindow : Window
{
    private BitmapSource? _source;
    private BitmapSource? _mask;

    public MainWindow()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            _source = new BitmapImage(
                new Uri(
                    "pack://application:,,,/Assets/test_sprite.png",
                    UriKind.Absolute));

            _mask = new BitmapImage(
                new Uri(
                    "pack://application:,,,/Assets/test_sprite_mask.png",
                    UriKind.Absolute));

            OriginalImage.Source =
                _source;

            UpdateRecolor();
        };
    }

    private void ColorSlider_ValueChanged(
        object sender,
        RoutedPropertyChangedEventArgs<double> e)
    {
        UpdateRecolor();
    }

    private void UpdateRecolor()
    {
        if (_source is null ||
            _mask is null ||
            RecoloredImage is null ||
            HueSlider is null ||
            SaturationSlider is null)
        {
            return;
        }

        RecoloredImage.Source =
            BitmapRecolorer.RecolorGrayscale(
                _source,
                _mask,
                HueSlider.Value,
                SaturationSlider.Value);
    }
}