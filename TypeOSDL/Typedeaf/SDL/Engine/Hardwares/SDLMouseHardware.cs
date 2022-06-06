using System.Collections.Generic;
using TypeOEngine.Typedeaf.Core.Common;
using TypeOEngine.Typedeaf.Core.Engine.Hardwares;
using TypeOEngine.Typedeaf.Desktop.Engine.Hardwares.Interfaces;
using TypeOEngine.Typedeaf.SDL.Engine.Hardwares.Interfaces;

namespace TypeOEngine.Typedeaf.SDL
{
    namespace Engine.Hardwares
    {
        public class SDLMouseHardware : Hardware, IMouseHardware, ISDLEvents
        {
            private Dictionary<uint, bool> OldState { get; set; }
            private Dictionary<uint, bool> NewState { get; set; }

            public Vec2 CurrentMousePosition { get; set; }
            public Vec2 OldMousePosition { get; set; }
            public Vec2 CurrentWheelPosition { get; set; }
            public Vec2 OldWheelPosition { get; set; }

            public override void Initialize()
            {
                OldState = new Dictionary<uint, bool>();
                NewState = new Dictionary<uint, bool>();

                CurrentMousePosition = new Vec2();
                OldMousePosition = new Vec2();
                CurrentWheelPosition = new Vec2();
                OldWheelPosition = new Vec2();
            }

            public override void Cleanup()
            {
            }

            public void UpdateEvents(List<SDL2.SDL.SDL_Event> events)
            {
                OldMousePosition = new Vec2(CurrentMousePosition);
                OldWheelPosition = new Vec2(CurrentWheelPosition);
                OldState = new Dictionary<uint, bool>(NewState);
                foreach(var e in events)
                {
                    if(e.type == SDL2.SDL.SDL_EventType.SDL_MOUSEWHEEL)
                    {
                        CurrentWheelPosition += new Vec2(e.wheel.x, e.wheel.y);
                    }
                    else if(e.type == SDL2.SDL.SDL_EventType.SDL_MOUSEMOTION)
                    {
                        CurrentMousePosition = new Vec2(e.motion.x, e.motion.y);
                    }

                    bool? state = null;
                    if(e.type == SDL2.SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN && e.key.repeat == 0)
                    {
                        state = true;
                    }
                    else if(e.type == SDL2.SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
                    {
                        state = false;
                    }

                    if(state.HasValue)
                    {
                        if(!NewState.ContainsKey(e.button.button))
                        {
                            NewState.Add(e.button.button, state.Value);
                        }
                        else
                        {
                            NewState[e.button.button] = state.Value;
                        }
                    }
                }
            }

            public bool CurrentButtonDownEvent(object key)
            {
                if(!NewState.ContainsKey((uint)key)) return false;
                return NewState[(uint)key];
            }

            public bool CurrentButtonUpEvent(object key)
            {
                if(!NewState.ContainsKey((uint)key)) return true;
                return !NewState[(uint)key];
            }

            public bool OldButtonDownEvent(object key)
            {
                if(!OldState.ContainsKey((uint)key)) return false;
                return OldState[(uint)key];
            }

            public bool OldButtonUpEvent(object key)
            {
                if(!OldState.ContainsKey((uint)key)) return true;
                return !OldState[(uint)key];
            }
        }
    }
}
