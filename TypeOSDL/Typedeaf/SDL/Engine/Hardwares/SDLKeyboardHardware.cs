using System.Collections.Generic;
using TypeOEngine.Typedeaf.Core.Engine.Hardwares;
using TypeOEngine.Typedeaf.Desktop.Engine.Hardwares.Interfaces;
using TypeOEngine.Typedeaf.SDL.Engine.Hardwares.Interfaces;

namespace TypeOEngine.Typedeaf.SDL
{
    namespace Engine.Hardwares
    {
        public class SDLKeyboardHardware : Hardware, IKeyboardHardware, ISDLEvents
        {
            private Dictionary<SDL2.SDL.SDL_Keycode, bool> OldState { get; set; }
            private Dictionary<SDL2.SDL.SDL_Keycode, bool> NewState { get; set; }

            public override void Initialize()
            {
                OldState = new Dictionary<SDL2.SDL.SDL_Keycode, bool>();
                NewState = new Dictionary<SDL2.SDL.SDL_Keycode, bool>();
            }

            public override void Cleanup()
            {
            }

            public void UpdateEvents(List<SDL2.SDL.SDL_Event> events)
            {
                OldState = new Dictionary<SDL2.SDL.SDL_Keycode, bool>(NewState);
                foreach(var e in events)
                {
                    if(!(e.type == SDL2.SDL.SDL_EventType.SDL_KEYDOWN || e.type == SDL2.SDL.SDL_EventType.SDL_KEYUP))
                    {
                        continue;
                    }

                    bool? state = null;
                    if(e.type == SDL2.SDL.SDL_EventType.SDL_KEYDOWN && e.key.repeat == 0)
                    {
                        state = true;
                    }
                    else if(e.type == SDL2.SDL.SDL_EventType.SDL_KEYUP)
                    {
                        state = false;
                    }

                    if(state.HasValue)
                    {
                        if(!NewState.ContainsKey(e.key.keysym.sym))
                        {
                            NewState.Add(e.key.keysym.sym, state.Value);
                        }
                        else
                        {
                            NewState[e.key.keysym.sym] = state.Value;
                        }
                    }
                }
            }

            public bool CurrentKeyDownEvent(object key)
            {
                if(!NewState.ContainsKey((SDL2.SDL.SDL_Keycode)key)) return false;
                return NewState[(SDL2.SDL.SDL_Keycode)key];
            }

            public bool CurrentKeyUpEvent(object key)
            {
                if(!NewState.ContainsKey((SDL2.SDL.SDL_Keycode)key)) return true;
                return !NewState[(SDL2.SDL.SDL_Keycode)key];
            }

            public bool OldKeyDownEvent(object key)
            {
                if(!OldState.ContainsKey((SDL2.SDL.SDL_Keycode)key)) return false;
                return OldState[(SDL2.SDL.SDL_Keycode)key];
            }

            public bool OldKeyUpEvent(object key)
            {
                if(!OldState.ContainsKey((SDL2.SDL.SDL_Keycode)key)) return true;
                return !OldState[(SDL2.SDL.SDL_Keycode)key];
            }
        }
    }
}
