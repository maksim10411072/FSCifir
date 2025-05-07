// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open Game
open FSCifir

[<EntryPoint>]
let main argv =
    use g = new Fscifirgetreal()
#if DEBUG
    let flnm = "raytest.cif"
#else
    let flnm = argv[0]
#endif
    use f = new System.IO.StreamReader(flnm)
    let txt = f.ReadToEnd()
    let img = Deserialization.parseCifir(txt)
    g.loadImage(img)
    g.Run()
    0