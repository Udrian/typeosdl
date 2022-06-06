using SDL2;
using System;
using System.Collections.Generic;
using TypeOEngine.Typedeaf.Core.Engine;
using TypeOEngine.Typedeaf.Core.Engine.Contents;
using TypeOEngine.Typedeaf.Core.Engine.Interfaces;
using TypeOEngine.Typedeaf.Core.Interfaces;
using TypeOEngine.Typedeaf.Desktop.Engine.Hardwares.Interfaces;
using TypeOEngine.Typedeaf.SDL.Engine.Contents;
using TypeOEngine.Typedeaf.SDL.Engine.Hardwares;
using TypeOEngine.Typedeaf.SDL.Engine.Hardwares.Interfaces;
using TypeOEngine.Typedeaf.SDL.Engine.Services;

namespace TypeOEngine.Typedeaf.SDL
{
    public class SDLModule : Module<SDLModuleOption>, IUpdatable
    {
        private ILogger Logger { get; set; }
        public bool Pause { get; set; }

        public SDLModule() : base()
        {
        }

        public override void Initialize()
        {
            TypeO.AddService<SDLService>();
            TypeO.Context.GetService<SDLService>().Option = Option;

            //Initial SDL
            foreach(var hint in Option.Hints)
            {
                SDL2.SDL.SDL_SetHint(hint.Key, hint.Value);
            }

            foreach(var eventState in Option.EventStates)
            {
                SDL2.SDL.SDL_EventState(eventState.Key, eventState.Value);
            }

            if(SDL2.SDL.SDL_Init(Option.SDLInitFlags) != 0)
            {
                var message = $"SDL_Init Error: {SDL2.SDL.SDL_GetError()}";
                Logger.Log(LogLevel.Fatal, message);
                TypeO.Context.Exit();
                throw new ApplicationException(message);
            }

            if(SDL_image.IMG_Init(Option.IMGInitFlags) == 0)
            {
                var message = $"IMG_Init Error: {SDL2.SDL.SDL_GetError()}";
                Logger.Log(LogLevel.Fatal, message);
                TypeO.Context.Exit();
                throw new ApplicationException(message);
            }

            if(SDL_ttf.TTF_Init() != 0)
            {
                var message = $"TTF_Init Error: {SDL2.SDL.SDL_GetError()}";
                Logger.Log(LogLevel.Fatal, message);
                TypeO.Context.Exit();
                throw new ApplicationException(message);
            }
        }

        public override void Cleanup()
        {
            SDL_ttf.TTF_Quit();
            SDL_image.IMG_Quit();
            SDL2.SDL.SDL_Quit();
        }

        public void Update(double dt)
        {
            var events = new List<SDL2.SDL.SDL_Event>();
            while(SDL2.SDL.SDL_PollEvent(out SDL2.SDL.SDL_Event e) > 0)
            {
                events.Add(e);
                if(e.type == SDL2.SDL.SDL_EventType.SDL_QUIT)
                {
                    TypeO.Context.Exit();
                }
            }

            foreach(var hardware in TypeO.Context.Hardwares.Values)
            {
                if(hardware is ISDLEvents sdlEvents)
                {
                    sdlEvents.UpdateEvents(events);
                }
            }
        }

        public override void LoadExtensions()
        {
            TypeO.AddHardware<IWindowHardware, SDLWindowHardware>();
            TypeO.AddHardware<IKeyboardHardware, SDLKeyboardHardware>();
            TypeO.AddHardware<IMouseHardware, SDLMouseHardware>();
            TypeO.BindContent<Texture, SDLTexture>();
            TypeO.BindContent<Font, SDLFont>();
        }
    }
}