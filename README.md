# Pixel Recolor
**Created by Paulina Greybyrd**

A reusable C# library for runtime recoloring of pixel-art sprites while preserving the original artwork's shading and detail.

PixelRecolor separates framework-independent color processing from rendering-specific implementations, allowing the same recoloring system to be used with WPF, MonoGame, and future rendering frameworks.

## Features

- Runtime grayscale sprite recoloring
- Preserves original pixel brightness and shading
- Hue and saturation control
- Alpha transparency preservation
- Selective recoloring with grayscale masks
- Partial recoloring using grayscale mask strength
- RGB multi-channel recolor masks
- Three independently recolorable regions from a single mask
- Independent hue and saturation controls per channel
- Framework-independent core library
- WPF `BitmapSource` support
- Interactive WPF demo
- MonoGame adapter planned

## Projects

### PixelRecolor.Core

Framework-independent recoloring logic.

Contains no WPF or MonoGame dependencies and can be shared by different rendering implementations.

### PixelRecolor.Wpf

WPF adapter for PixelRecolor.

Provides recoloring support for `BitmapSource` images and converts WPF bitmap data to and from the framework-independent color representation used by Core.

### PixelRecolor.MonoGame

MonoGame adapter for PixelRecolor.

Currently a project skeleton. It will eventually provide `Texture2D` support using the same recoloring logic provided by `PixelRecolor.Core`.

### PixelRecolor.Demo

Small WPF application used to visually test and demonstrate recoloring behavior.

## Grayscale Recoloring

PixelRecolor uses the original sprite's brightness as the value component of the new color.

This means a grayscale sprite such as:

```text
dark gray   → shadow
medium gray → body
light gray  → highlight
```

can be recolored while retaining that structure:

```text
dark gray   → dark purple
medium gray → purple
light gray  → bright purple
```

The artwork's original shading therefore remains visible instead of being replaced by a flat tint.

## Basic WPF Usage

```csharp
BitmapSource recolored =
    BitmapRecolorer.RecolorGrayscale(
        source,
        hue: 285,
        saturation: 0.8);
```

Hue is expressed in degrees from `0` to `360`.

Saturation ranges from `0.0` to `1.0`.

## Recolor Masks

A separate grayscale PNG can control which portions of a sprite are recolored.

The source image and mask must have matching dimensions.

Mask brightness determines recolor strength:

```text
White      → 100% recolored
Gray       → partially recolored
Black      → unchanged
```

This allows artwork to contain elements that remain untouched while another portion changes color.

For example, a food bowl could recolor its ceramic body while preserving:

- food
- outlines
- decorative details
- shadows
- other intentionally fixed colors

### Masked WPF Usage

```csharp
BitmapSource recolored =
    BitmapRecolorer.RecolorGrayscale(
        source,
        mask,
        hue: 210,
        saturation: 0.8);
```

## Architecture

```text
                    PixelRecolor.Core
                    /               \
                   /                 \
                  ▼                   ▼
        PixelRecolor.Wpf      PixelRecolor.MonoGame
                │
                ▼
        PixelRecolor.Demo
```

`PixelRecolor.Core` contains the color-processing logic and does not depend on either rendering framework.

Framework-specific projects translate their native image representations into data that Core can process.

This keeps recoloring behavior consistent between applications while allowing each renderer to handle its own image types.

## Multi-Channel Masks

PixelRecolor supports RGB channel masks that define three independently recolorable regions within a single mask image.

Each color channel represents a separate recolor region:

```text
Red channel   → region 1
Green channel → region 2
Blue channel  → region 3
Black         → unchanged
```

For example, a clothing sprite could define:

```text
Red   → main fabric
Green → trim
Blue  → accents
```

Each region receives its own `RecolorSettings`, allowing independent hue and saturation control while preserving the brightness and shading of the original grayscale artwork.

```csharp
var red =
    new RecolorSettings(
        hue: 285,
        saturation: 0.8);

var green =
    new RecolorSettings(
        hue: 45,
        saturation: 0.8);

var blue =
    new RecolorSettings(
        hue: 180,
        saturation: 0.8);

BitmapSource recolored =
    BitmapRecolorer.RecolorChannels(
        source,
        channelMask,
        red,
        green,
        blue);
```

A single grayscale sprite can therefore produce many color combinations without requiring separate artwork for every variation.

### Channel Mask Format

For clearly separated regions, mask pixels can use pure RGB values:

```text
#FF0000 → red channel
#00FF00 → green channel
#0000FF → blue channel
#000000 → unchanged
```

Channel intensity represents the strength of that channel's influence.

The current implementation is designed primarily for masks where each recolorable pixel belongs to one channel. Behavior for overlapping channel values will be refined as the library develops.

## Planned Features

- Define blending behavior for overlapping RGB mask channels
- Per-channel value/brightness adjustment
- MonoGame `Texture2D` adapter
- Recolored sprite caching
- Additional recolor modes
- Expanded demo controls

## Intended Uses

PixelRecolor is designed for reusable runtime customization of pixel-art assets such as:

- clothing
- furniture
- bowls and containers
- toys
- habitat objects
- character accessories
- UI elements
- other customizable sprites

## Status

Early development.

Grayscale recoloring, grayscale strength masks, and RGB multi-channel recoloring are implemented and working in the WPF demo.

MonoGame support, caching, and additional recoloring controls are planned.