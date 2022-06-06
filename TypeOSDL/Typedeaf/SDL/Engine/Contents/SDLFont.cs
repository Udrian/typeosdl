using SDL2;
using TypeOEngine.Typedeaf.Core.Common;
using TypeOEngine.Typedeaf.Core.Engine;
using TypeOEngine.Typedeaf.Core.Engine.Contents;
using TypeOEngine.Typedeaf.Core.Engine.Interfaces;
using TypeOEngine.Typedeaf.SDL.Engine.Graphics;
using SDL_Font = System.IntPtr;

namespace TypeOEngine.Typedeaf.SDL
{
    namespace Engine.Contents
    {
        public class SDLFont : Font
        {
            private ILogger Logger { get; set; }

            public SDLCanvas Canvas { get; private set; }
            public SDL_Font SDL_Font { get; private set; }

            public SDLFont() : base() { }

            public override Vec2 MeasureString(string text)
            {
                SDL_ttf.TTF_SizeText(SDL_Font, text, out int w, out int h);
                return new Vec2(w, h);
            }

            public override void Load(string path, ContentLoader contentLoader)
            {
                Canvas = contentLoader.Canvas as SDLCanvas;
                FilePath = path;

                SDL_Font = SDL_ttf.TTF_OpenFont(FilePath, FontSize);
                FontSize = FontSize;
                if(SDL_Font == SDL_Font.Zero)
                {
                    Logger.Log(LogLevel.Error, $"Error loading SDLFont '{path}' with error : {SDL2.SDL.SDL_GetError()}");
                }
            }

            public override void Cleanup()
            {
                SDL_ttf.TTF_CloseFont(SDL_Font);
            }

            public override int FontSize {
                get => base.FontSize;
                set {
                    base.FontSize = value;

                    SDL_Font = SDL_ttf.TTF_OpenFont(FilePath, FontSize);
                }
            }
        }
    }
}