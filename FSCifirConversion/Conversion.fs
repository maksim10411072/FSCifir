namespace FSCifirConversion
open FSCifir
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats

module Conversion =
    let decomposeRgb (composedRgb: int): byte * byte * byte =
        let r = byte <| (composedRgb &&& 0xFF0000) / 0x10000
        let g = byte <| (composedRgb &&& 0x00FF00) / 0x100
        let b = byte <| (composedRgb &&& 0x0000FF)
        (r, g, b)

    let composeRgb (r: byte) (g: byte) (b: byte): int =
        int b ||| (int g <<< 8) ||| (int r <<< 16)

    let cifir2png (cifirImage: Format.CifirImage): Image<Rgb24> =
        let image = new Image<Rgb24>(cifirImage.width, cifirImage.height)
        for y in [0 .. cifirImage.height - 1] do
            for x in [0 .. cifirImage.width - 1] do
                let r, g, b = decomposeRgb <| cifirImage.pixelAt x y
                image[x, y] <- Rgb24 (r, g, b)
        image

    let png2cifir (pngImage: Image<Rgb24>): Format.CifirImage =
        let width = pngImage.Width
        let height = pngImage.Height
        let pixels = Array.init height (
            fun y -> Array.init width (
                fun x ->
                    let pixel = pngImage[x, y]
                    composeRgb pixel.R pixel.G pixel.B
            )
        )
        Format.CifirImage (width, height, pixels)