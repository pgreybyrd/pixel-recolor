using PixelRecolor.Core;
using PixelRecolor.Wpf;
using System.IO;
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

        var appearanceJson =
            LoadResourceText(
                "Assets/Rat/Appearances/sir_rattington.json");

        var appearance =
            CreatureAppearanceLoader.Load(
                appearanceJson);

        var paletteJson =
            LoadResourceText(
                $"Assets/Rat/Palettes/{appearance.Palette}.json");

        var palette =
            RegionPaletteLoader.Load(
                paletteJson);

        var result =
            CreatureAppearanceRenderer.Build(
                _source,
                _regionMask,
                regions,
                palette,
                appearance,

                patternId =>
                    LoadBitmap(
                        $"Assets/Rat/Patterns/{patternId}.png"),

                accessoryId =>
                    LoadBitmap(
                        $"Assets/Rat/Accessories/{accessoryId}.png"),

                effectId =>
                    LoadBitmap(
                        $"Assets/Rat/Effects/{effectId}.png"));

        RecoloredImage.Source =
            result;

        SetPixelPerfectSize(
            RecoloredImage,
            result,
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

    private static string LoadResourceText(
        string resourcePath)
    {
        var uri =
            new Uri(
                $"pack://application:,,,/{resourcePath}",
                UriKind.Absolute);

        var resource =
            Application.GetResourceStream(uri)
            ?? throw new InvalidOperationException(
                $"Could not find resource: {resourcePath}");

        using var reader =
            new StreamReader(resource.Stream);

        return reader.ReadToEnd();
    }

    private static BitmapSource LoadBitmap(
        string resourcePath)
    {
        var bitmap =
            new BitmapImage(
                new Uri(
                    $"pack://application:,,,/{resourcePath}",
                    UriKind.Absolute));

        bitmap.Freeze();

        return bitmap;
    }
}