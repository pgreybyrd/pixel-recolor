using PixelRecolor.Core;
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
                    "pack://application:,,,/Assets/test_sprite_channels.png",
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
            RecoloredImage is null)
        {
            return;
        }

        var red =
            new RecolorSettings(
                RedHueSlider.Value,
                RedSaturationSlider.Value);

        var green =
            new RecolorSettings(
                GreenHueSlider.Value,
                GreenSaturationSlider.Value);

        var blue =
            new RecolorSettings(
                BlueHueSlider.Value,
                BlueSaturationSlider.Value);

        RecoloredImage.Source =
            BitmapRecolorer.RecolorChannels(
                _source,
                _mask,
                red,
                green,
                blue);
    }
}