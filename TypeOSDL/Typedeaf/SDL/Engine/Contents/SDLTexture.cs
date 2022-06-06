using SDL2;
using TypeOEngine.Typedeaf.Core.Common;
using TypeOEngine.Typedeaf.Core.Engine;
using TypeOEngine.Typedeaf.Core.Engine.Contents;
using TypeOEngine.Typedeaf.Core.Engine.Interfaces;
using TypeOEngine.Typedeaf.SDL.Engine.Graphics;
using SDL_Image = System.IntPtr;

namespace TypeOEngine.Typedeaf.SDL
{
    namespace Engine.Contents
    {
        public class SDLTexture : Texture
        {
            private ILogger Logger { get; set; }
            public SDL_Image SDL_Image { get; set; }

            public SDLTexture() : base() { }

            public override void Load(string path, ContentLoader contentLoader)
            {
                var sdlCanvas = contentLoader.Canvas as SDLCanvas;
                FilePath = path;

                SDL_Image = SDL_image.IMG_LoadTexture(sdlCanvas.SDLRenderer, FilePath);
                if(SDL_Image == SDL_Image.Zero)
                {
                    Logger.Log(LogLevel.Error, $"Error loading SDLTexture '{path}' with error : {SDL2.SDL.SDL_GetError()}");
                }

                SDL2.SDL.SDL_QueryTexture(SDL_Image, out _, out _, out int w, out int h);
                Size = new Vec2(w, h);
            }

            public override void Cleanup()
            {
                Logger.Log(LogLevel.Debug, $"Unloading Texture with path: '{FilePath}'");
                SDL2.SDL.SDL_DestroyTexture(SDL_Image);
            }
        }
    }
}