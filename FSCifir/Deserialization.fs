namespace FSCifir
open FSCifir.Format
open System

module Deserialization =
    let parseHeader str: int array =
        let nxn = String.filter (fun c -> not <| List.contains c ['<'; '>'; ' ']) str
        in Array.map (int) (nxn.Split("x"))

    let parsePixelLine (inputString: string): int array =
        inputString.Split(" ")
        |> Array.filter (fun s -> s.Length > 0)
        |> Array.map (fun hs -> 
           Int32.Parse(hs.TrimStart('#'), Globalization.NumberStyles.HexNumber))

    let parseCifir (str: string) = 
        let comregex = Text.RegularExpressions.Regex("<--.*-->")
        let lines = (comregex.Replace(str, "")).Split("\n")
        let size = parseHeader lines[0]
        let pixels = lines[1..] |> Array.filter (fun l -> l.Length > 0) |> Array.map parsePixelLine
        CifirImage (size[0], size[1], pixels)