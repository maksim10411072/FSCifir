namespace FSCifir

module Format =
    type CifirImage(width: int, height: int, pixels: int[][]) = class
        member this.width = width
        member this.height = height
        member this.rawPixels = pixels
        member this.pixelAt x y =
            try pixels[y][x]
            with e ->
                printfn "x: %i y: %i" x y
                reraise()
        end