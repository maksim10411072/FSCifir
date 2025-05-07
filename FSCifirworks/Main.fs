namespace FSCifirworks
open System
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open FSCifirConversion
open FSCifir

module Main =
    [<EntryPoint>]
    let main args =
        match args with
        | [|"-c"; cifirFile; pngFile|] 
            -> printfn "Reading %s..." cifirFile
               use cifir:IO.StreamReader = new IO.StreamReader(cifirFile)
               let ctxt = cifir.ReadToEnd()
               printfn "Readed %s. Converting..." cifirFile
               let pc = Deserialization.parseCifir ctxt
               printfn "Converted. Saving as %s..." pngFile
               use p:Image<Rgb24> = Conversion.cifir2png pc
               p.SaveAsPng(pngFile)
               printfn "Saved as %s." pngFile

        | [|"-p"; pngFile; cifirFile|] 
            -> printfn "Loading %s..." pngFile
               use pimg:Image<Rgb24> = Image.Load<Rgb24>(pngFile)
               printfn "Loading %s. Converting..." pngFile
               let cc = Conversion.png2cifir pimg
               printfn "Converted. Serializing..."
               let ptxt = Serialization.serializeCifir cc
               printfn "Serialized. Saving as %s..." cifirFile
               use cifir:IO.StreamWriter = new IO.StreamWriter(cifirFile)
               cifir.Write(ptxt)
               printfn "Saved as %s." cifirFile
        | [|"-i"; cifirImage|]
            -> printfn "Parsing %s..." cifirImage
               use cifir:IO.StreamReader = new IO.StreamReader(cifirImage)
               let ctxt = cifir.ReadToEnd()
               let pc = Deserialization.parseCifir ctxt
               printfn "%ix%i" pc.width pc.height
        | args when Array.Exists(args, fun x -> x = "-h" || x = "--help") 
            -> printfn "Usage:\n\t-h | --help\n\t-c <cifirfile> <pngfile>\n\t-p <pngfile> <cifirfile>"
        | _ -> printfn "What? I don't understand you, try running me with -h or --help"

        0
