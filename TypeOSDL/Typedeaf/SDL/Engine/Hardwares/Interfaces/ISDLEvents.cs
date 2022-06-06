using System.Collections.Generic;

namespace TypeOEngine.Typedeaf.SDL
{
    namespace Engine.Hardwares.Interfaces
    {
        public interface ISDLEvents
        {
            void UpdateEvents(List<SDL2.SDL.SDL_Event> events);
        }
    }
}
