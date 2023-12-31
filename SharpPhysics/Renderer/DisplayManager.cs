using GLFW;
using SharpPhysics.Utilities.MISC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static OpenGL.GL;

namespace SharpPhysics.Renderer
{
    internal static class DisplayManager
    {
        public static Window Window { get; set; }
        public static Vector2 WindowSize { get; set; }

        internal static void CreateWindow(int width, int height, string title)
        {
            WindowSize = new Vector2(width, height);

            Glfw.Init();

            // using opengl 3.3

            // -> 3 <-.3
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);

            // 3.-> 3 <-
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);

            // using core profile

            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);

            //Rectangle screen = Glfw.PrimaryMonitor.WorkArea;
            //int x = (screen.Width - width) / 2;
            //int y = (screen.Height - height) / 2;

            //Glfw.SetWindowPosition(Window, x, y);

            // focusing the window

            Glfw.WindowHint(Hint.Focused, true);

            Window = Glfw.CreateWindow(width, height, title, GLFW.Monitor.None, Window.None);

            if (Window == Window.None) ErrorHandler.ThrowError("Error, Internal/External error, GLFW graphics init failed.", true);

            Glfw.MakeContextCurrent(Window);

            Import(Glfw.GetProcAddress);

            glViewport(0, 0, width, height);

            Glfw.SwapInterval(0); // VSync is off
        }
        internal static void CloseWindow()
        {
            Glfw.Terminate();
        }
    }
}
