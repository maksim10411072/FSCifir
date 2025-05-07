module Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Input
open FSCifir
open System

type Fscifirgetreal () as x =
    inherit Game()
 
    do x.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(x)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let mutable lastMouse: MouseState = Unchecked.defaultof<MouseState>
    let mutable targetCif: Option<Format.CifirImage> = None
    let mutable cifirTex: Texture2D = null
    let mutable texCenter = Vector2.Zero
    let mutable texSize = Vector2.Zero
    let mutable texScale = 1f
    let mutable texScaleSmoothed = texScale
    let mutable texPosition = Vector2.Zero
    let calcTexScale (delta: int, insensivity: float32): float32 =
        let deltan = float32 delta / insensivity
        MathF.Max(
            match deltan with
            | deltan when deltan > 0f -> texScale/deltan
            | deltan when deltan < 0f -> texScale*(-deltan)
            | _ -> texScale
            , 0.001f
        )
        

    let decomposeRgb (composedRgb: int): int * int * int =
        let r = (composedRgb &&& 0xFF0000) / 0x10000
        let g = (composedRgb &&& 0x00FF00) / 0x100
        let b = (composedRgb &&& 0x0000FF)
        (r, g, b)

    let mutable color = Color.Crimson
    member public this.loadImage(image: Format.CifirImage) =
        targetCif <- Some image

    override x.Initialize() =
    
        spriteBatch <- new SpriteBatch(x.GraphicsDevice)
        if targetCif.IsSome then
            let image = targetCif.Value
            let width = image.width
            let height = image.height
            let texture = new Texture2D(x.GraphicsDevice, width, height, false, SurfaceFormat.Color)
            let pixels1dized = Array.map (fun col -> 
                let r, g, b = decomposeRgb col
                new Color(r, g, b)) (Array.concat image.rawPixels)
            texture.SetData<Color>(pixels1dized, 0, pixels1dized.Length)
            color <- Color.Black
            cifirTex <- texture
            texSize <- Vector2(float32 width, float32 height)
            graphics.PreferredBackBufferHeight <- height
            graphics.PreferredBackBufferWidth <- width
            graphics.ApplyChanges()
            texCenter <- Vector2(float32 width / 2f, float32 height / 2f)
            //texPosition <- x.Window.ClientBounds.Size.ToVector2() / 2f - texCenter
        x.Window.AllowUserResizing <- true
        x.IsMouseVisible <- true
        base.Initialize()
         
         // TODO: Add your initialization logic here

        ()

    override this.LoadContent() =
        
         // TODO: use x.Content to load your game content here   
         // On Windows you can load any PNG file directly as Texture2D

         // Read more about MonoGame's Content Pipeline: https://docs.monogame.net/articles/tools/mgcb_editor.html
         // or install it with package manager console: [dotnet tool install -g dotnet-mgcb-editor]
        
        ()
 
    override this.Update (gameTime) =
        let mouseState = Mouse.GetState()
        if mouseState.LeftButton = ButtonState.Pressed then
            let delta = mouseState.Position - lastMouse.Position
            texPosition <- texPosition + Vector2(float32 delta.X, float32 delta.Y)
        texScale <- calcTexScale (lastMouse.ScrollWheelValue - mouseState.ScrollWheelValue, 100f)
#if DEBUG
        if lastMouse.ScrollWheelValue <> mouseState.ScrollWheelValue then
            printfn "%f" texScale
#endif
        lastMouse <- mouseState
        texScaleSmoothed <- texScaleSmoothed * 0.9f + texScale * 0.1f
        base.Update(gameTime)
        
 
    override this.Draw (gameTime) =

        x.GraphicsDevice.Clear color

        if cifirTex <> null then
            spriteBatch.Begin()
            spriteBatch.Draw(
                cifirTex, 
                new Rectangle(
                    (texPosition + x.Window.ClientBounds.Size.ToVector2() / 2f - texCenter*texScaleSmoothed).ToPoint(), 
                    (texScaleSmoothed*texSize).ToPoint()
                ),
                Color.White)// texPosition + x.Window.ClientBounds.Size.ToVector2() / 2f - texCenter, Color.White)
            spriteBatch.End()
        // TODO: Add your drawing code here

        ()