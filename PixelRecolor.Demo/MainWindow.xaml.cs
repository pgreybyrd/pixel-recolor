using PixelRecolor.Core;
using PixelRecolor.Wpf;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixelRecolor.Demo;

public partial class MainWindow : Window
{
    private BitmapSource? _source;
    private BitmapSource? _regionMask;

    public MainWindow()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            _source =
                new BitmapImage(
                    new Uri(
                        "pack://application:,,,/Assets/Rat/rat.png",
                        UriKind.Absolute));

            _regionMask =
                new BitmapImage(
                    new Uri(
                        "pack://application:,,,/Assets/Rat/rat-regions.png",
                        UriKind.Absolute));

            OriginalImage.Source = _source;

            SetPixelPerfectSize(
                OriginalImage,
                _source,
                2);

            UpdateRecolor();
        };
    }

    private void ColorSlider_ValueChanged(
        object sender,
        RoutedPropertyChangedEventArgs<double> e)
    {
        // Sliders aren't part of the region test yet.
    }

    private void UpdateRecolor()
    {
        if (_source is null ||
            _regionMask is null ||
            RecoloredImage is null)
        {
            return;
        }

        var regions =
            new List<RegionDefinition>
            {
                new(
                    new RegionId("nose"),
                    new RgbColor(255, 0, 0)),

                new(
                    new RegionId("ears"),
                    new RgbColor(255, 132, 0)),

                new(
                    new RegionId("eyes"),
                    new RgbColor(255, 251, 0)),

                new(
                    new RegionId("paws"),
                    new RgbColor(114, 255, 0)),

                new(
                    new RegionId("feet"),
                    new RgbColor(0, 255, 202)),

                new(
                    new RegionId("head"),
                    new RgbColor(141, 0, 255)),

                new(
                    new RegionId("body"),
                    new RgbColor(0, 255, 123)),

                new(
                    new RegionId("belly"),
                    new RgbColor(255, 0, 224)),

                new(
                    new RegionId("tail"),
                    new RgbColor(2, 6, 247))
            };

        var palette =
            new RegionPalette();

        // Nose - soft dusty pink
        palette.Set(
            new RegionId("nose"),
            new RecolorSettings(350, 0.30));

        // Ears - warm pink
        palette.Set(
            new RegionId("ears"),
            new RecolorSettings(350, 0.22));

        // Eyes - near-black neutral
        palette.Set(
            new RegionId("eyes"),
            new RecolorSettings(0, 0.0));

        // Front paws - pale pink
        palette.Set(
            new RegionId("paws"),
            new RecolorSettings(350, 0.16));

        // Feet - pale pink, slightly warmer
        palette.Set(
            new RegionId("feet"),
            new RecolorSettings(355, 0.14));

        // Head - neutral grey
        palette.Set(
            new RegionId("head"),
            new RecolorSettings(215, 0.08));

        // Body - cool grey
        palette.Set(
            new RegionId("body"),
            new RecolorSettings(215, 0.10));

        // Belly - softer/warmer grey
        palette.Set(
            new RegionId("belly"),
            new RecolorSettings(25, 0.06));

        // Tail - muted dusty pink
        palette.Set(
            new RegionId("tail"),
            new RecolorSettings(350, 0.20));

        var recolored =
            BitmapRecolorer.RecolorRegions(
                _source,
                _regionMask,
                regions,
                palette);

        RecoloredImage.Source =
            recolored;

        SetPixelPerfectSize(
            RecoloredImage,
            recolored,
            2);
    }

    private static void SetPixelPerfectSize(
        System.Windows.Controls.Image image,
        BitmapSource source,
        int scale)
    {
        var dpi =
            VisualTreeHelper.GetDpi(image);

        image.Width =
            source.PixelWidth *
            scale /
            dpi.DpiScaleX;

        image.Height =
            source.PixelHeight *
            scale /
            dpi.DpiScaleY;

        RenderOptions.SetBitmapScalingMode(
            image,
            BitmapScalingMode.NearestNeighbor);

        image.SnapsToDevicePixels = true;
    }
}

//using PixelRecolor.Core;
//using PixelRecolor.Wpf;
//using System.Windows;
//using System.Windows.Media.Imaging;

//namespace PixelRecolor.Demo;

//public partial class MainWindow : Window
//{
//    private BitmapSource? _source;
//    private BitmapSource? _mask;

//    public MainWindow()
//    {
//        InitializeComponent();

//        Loaded += (_, _) =>
//        {
//            _source = new BitmapImage(
//                new Uri(
//                    "pack://application:,,,/Assets/test_sprite.png",
//                    UriKind.Absolute));

//            _mask = new BitmapImage(
//                new Uri(
//                    "pack://application:,,,/Assets/test_sprite_channels.png",
//                    UriKind.Absolute));

//            OriginalImage.Source =
//                _source;

//            UpdateRecolor();
//        };
//    }

//    private void ColorSlider_ValueChanged(
//        object sender,
//        RoutedPropertyChangedEventArgs<double> e)
//    {
//        UpdateRecolor();
//    }

//    private void UpdateRecolor()
//    {
//        if (_source is null ||
//            _mask is null ||
//            RecoloredImage is null)
//        {
//            return;
//        }

//        var red =
//            new RecolorSettings(
//                RedHueSlider.Value,
//                RedSaturationSlider.Value);

//        var green =
//            new RecolorSettings(
//                GreenHueSlider.Value,
//                GreenSaturationSlider.Value);

//        var blue =
//            new RecolorSettings(
//                BlueHueSlider.Value,
//                BlueSaturationSlider.Value);

//        RecoloredImage.Source =
//            BitmapRecolorer.RecolorChannels(
//                _source,
//                _mask,
//                red,
//                green,
//                blue);
//    }
//}