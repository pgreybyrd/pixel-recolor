using PixelRecolor.Core;
using PixelRecolor.Wpf;
using System.Windows;
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

            OriginalImage.Source =
                _source;

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

        palette.Set(
            new RegionId("nose"),
            new RecolorSettings(0, 1.0));

        palette.Set(
            new RegionId("ears"),
            new RecolorSettings(30, 1.0));

        palette.Set(
            new RegionId("eyes"),
            new RecolorSettings(60, 1.0));

        palette.Set(
            new RegionId("paws"),
            new RecolorSettings(120, 1.0));

        palette.Set(
            new RegionId("feet"),
            new RecolorSettings(170, 1.0));

        palette.Set(
            new RegionId("head"),
            new RecolorSettings(275, 1.0));

        palette.Set(
            new RegionId("body"),
            new RecolorSettings(145, 1.0));

        palette.Set(
            new RegionId("belly"),
            new RecolorSettings(310, 1.0));

        palette.Set(
            new RegionId("tail"),
            new RecolorSettings(240, 1.0));

        RecoloredImage.Source =
            BitmapRecolorer.RecolorRegions(
                _source,
                _regionMask,
                regions,
                palette);
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