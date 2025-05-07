namespace FSCifir
open System

module Serialization =
    let serializeHeader (width: int) (height: int): string =
        sprintf "<%ix%i>" width height

    let serializePixelLine (cols: int array): string =
        Array.map (sprintf "#%06X") cols
        |> String.concat " "

    let serializeCifir (image: Format.CifirImage): string =
        let header = serializeHeader image.width image.height
        let lines = Array.toList <|
                    Array.map serializePixelLine image.rawPixels
        String.concat "\n" (header::lines)