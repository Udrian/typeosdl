using System;
using System.Collections.Generic;
using TypeOEngine.Typedeaf.Core.Engine.Contents;
using TypeOEngine.Typedeaf.SDL.Engine.Graphics;

namespace TypeOEngine.Typedeaf.SDL
{
    namespace Engine.Contents
    {
        public class SDLContentLoader : ContentLoader
        {
            public new SDLCanvas Canvas { get; set; }

            public SDLContentLoader(SDLCanvas canvas, Dictionary<Type, Type> contentBinding) : base(canvas, contentBinding) { }
        }
    }
}