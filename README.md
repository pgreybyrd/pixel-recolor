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
- Framework-independent core library
- WPF `BitmapSource` support
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

## Planned Features

- RGB multi-channel masks
- Multiple independently recolorable regions
- Per-channel hue, saturation, and value controls
- MonoGame `Texture2D` adapter
- Recolored sprite caching
- Additional recolor modes
- Expanded demo controls

### Multi-Channel Masks

A future RGB mask will allow a single mask image to define multiple independently recolorable regions.

For example:

```text
Red channel   → main fabric
Green channel → trim
Blue channel  → accents
```

This could allow one clothing sprite to use:

```text
Main fabric → purple
Trim        → gold
Accents     → teal
```

without requiring separate artwork for every color combination.

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

Grayscale recoloring and grayscale mask support are currently implemented and working in the WPF demo. MonoGame support and multi-channel recoloring are planned.