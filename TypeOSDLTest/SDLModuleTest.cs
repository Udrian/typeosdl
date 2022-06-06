using System.Linq;
using TypeOEngine.Typedeaf.Core;
using TypeOEngine.Typedeaf.Core.Common;
using TypeOEngine.Typedeaf.Core.Engine;
using TypeOEngine.Typedeaf.Core.Engine.Interfaces;
using TypeOEngine.Typedeaf.Desktop;
using TypeOEngine.Typedeaf.Desktop.Engine.Services;
using TypeOEngine.Typedeaf.SDL;
using TypeOEngine.Typedeaf.SDL.Engine.Contents;
using TypeOEngine.Typedeaf.SDL.Engine.Graphics;
using TypeOEngine.Typedeaf.SDL.Engine.Hardwares;
using Xunit;

namespace Test
{
    public class SDLModuleTest
    {
        public string GameName { get; set; } = "test";

        public class TestGame : Game
        {
            public ILogger Logger { get; set; }
            public WindowService WindowService { get; set; }
            public KeyboardInputService KeyboardInputService { get; set; }
            public SceneList Scenes { get; set; }

            public override void Initialize()
            {
                Scenes = CreateSceneHandler();
                Scenes.Window = WindowService.CreateWindow("test", new Vec2(100, 100), new Vec2(100, 100));
                Scenes.Canvas = WindowService.CreateCanvas(Scenes.Window);
                Scenes.ContentLoader = WindowService.CreateContentLoader(Scenes.Canvas);
            }

            public override void Update(double dt)
            {
                Exit();
            }

            public override void Draw()
            {
            }

            public override void Cleanup()
            {
            }
        }

        public class TestScene1 : Scene
        {
            public ILogger Logger { get; set; }
            public WindowService WindowService { get; set; }
            public KeyboardInputService KeyboardInputService { get; set; }

            public override void Draw()
            {
            }

            public override void Initialize()
            {
            }

            public override void OnEnter(Scene from)
            {
            }

            public override void OnExit(Scene to)
            {
            }

            public override void Update(double dt)
            {
            }
        }

        public class TestScene2 : Scene
        {
            public override void Draw()
            {
            }

            public override void Initialize()
            {
            }

            public override void OnEnter(Scene from)
            {
            }

            public override void OnExit(Scene to)
            {
            }

            public override void Update(double dt)
            {
            }
        }

        [Fact]
        public void LoadSDLModule()
        {
            var typeO = TypeO.Create<TestGame>(GameName)
                             .LoadModule<SDLModule>() as TypeO;
            var module = typeO.Context.Modules.FirstOrDefault(m => m.GetType() == typeof(SDLModule)) as SDLModule;
            Assert.NotNull(module);
            Assert.IsType<SDLModule>(module);
            Assert.NotEmpty(typeO.Context.Modules);
        }


        [Fact]
        public void LoadDefaults()
        {
            var typeO = TypeO.Create<TestGame>(GameName) as TypeO;
            typeO.LoadModule<DesktopModule>()
                .LoadModule<SDLModule>()
                .Start();

            Assert.NotEmpty(typeO.Context.ContentBinding);
            Assert.NotEmpty(typeO.Context.Hardwares);
            Assert.NotEmpty(typeO.Context.Services);

            var testGame = (typeO.Context.Game as TestGame);

            Assert.NotNull(testGame.Logger);
            Assert.IsType<DefaultLogger>(testGame.Logger);

            Assert.NotNull(testGame.WindowService);
            Assert.NotNull(testGame.KeyboardInputService);

            Assert.NotNull(testGame.Scenes);

            Assert.NotNull(testGame.Scenes.Window);
            Assert.IsType<SDLWindow>(testGame.Scenes.Window);

            Assert.NotNull(testGame.Scenes.Canvas);
            Assert.IsType<SDLCanvas>(testGame.Scenes.Canvas);

            Assert.NotNull(testGame.Scenes.ContentLoader);
            Assert.IsType<SDLContentLoader>(testGame.Scenes.ContentLoader);
        }

        [Fact]
        public void SwitchScene()
        {
            var typeO = TypeO.Create<TestGame>(GameName) as TypeO;
            typeO.LoadModule<DesktopModule>()
                .LoadModule<SDLModule>()
                .Start();

            var testGame = (typeO.Context.Game as TestGame);
            testGame.Scenes.SetScene<TestScene1>();

            Assert.NotNull(testGame.Scenes.CurrentScene);
            Assert.IsType<TestScene1>(testGame.Scenes.CurrentScene);

            var testScene = testGame.Scenes.CurrentScene as TestScene1;

            Assert.NotNull(testScene.Logger);
            Assert.IsType<DefaultLogger>(testScene.Logger);

            Assert.NotNull(testScene.WindowService);

            Assert.NotNull(testScene.KeyboardInputService);

            Assert.NotNull(testScene.Scenes);

            Assert.NotNull(testScene.Scenes.Window);
            Assert.IsType<SDLWindow>(testScene.Scenes.Window);

            Assert.NotNull(testScene.Scenes.Canvas);
            Assert.IsType<SDLCanvas>(testScene.Scenes.Canvas);

            Assert.NotNull(testScene.Scenes.ContentLoader);
            Assert.IsType<SDLContentLoader>(testScene.Scenes.ContentLoader);

            testGame.Scenes.SetScene<TestScene2>();
            Assert.NotNull(testGame.Scenes.CurrentScene);
            Assert.IsType<TestScene2>(testGame.Scenes.CurrentScene);
        }
    }
}
