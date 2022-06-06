using TypeOEngine.Typedeaf.Core.Engine.Contents;
using TypeOEngine.Typedeaf.Core.Engine.Graphics;
using TypeOEngine.Typedeaf.Core.Engine.Hardwares;
using TypeOEngine.Typedeaf.Desktop.Engine.Graphics;
using TypeOEngine.Typedeaf.Desktop.Engine.Hardwares.Interfaces;
using TypeOEngine.Typedeaf.SDL.Engine.Contents;
using TypeOEngine.Typedeaf.SDL.Engine.Graphics;

namespace TypeOEngine.Typedeaf.SDL
{
    namespace Engine.Hardwares
    {
        public class SDLWindowHardware : Hardware, IWindowHardware
        {
            public override void Initialize() { }

            DesktopWindow IWindowHardware.CreateWindow()
            {
                return CreateWindow();
            }

            public SDLWindow CreateWindow()
            {
                return new SDLWindow();
            }

            public Canvas CreateCanvas(Window desktopWindow)
            {
                var canvas = new SDLCanvas
                {
                    Window = desktopWindow
                };
                return canvas;
            }

            public ContentLoader CreateContentLoader(Canvas canvas)
            {
                return new SDLContentLoader((SDLCanvas)canvas, Context.ContentBinding);
            }

            public override void Cleanup()
            {
            }
        }
    }
}
