﻿using SDL2;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using SharpDL.Shared;

namespace SharpDL.Graphics
{
    public class Window : IDisposable
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Title
        {
            get
            {
                if (Handle != IntPtr.Zero)
                {
                    return SDL.SDL_GetWindowTitle(Handle);
                }
                throw new InvalidOperationException(Errors.E_WINDOW_NULL);
            }
            set
            {
                if (Handle != IntPtr.Zero)
                    SDL.SDL_SetWindowTitle(Handle, value);
                else
                    throw new InvalidOperationException(Errors.E_WINDOW_NULL);
            }
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public IEnumerable<WindowFlags> Flags { get; private set; }

        public IntPtr Handle { get; private set; }

        public Window(string title, int x, int y, int width, int height, WindowFlags flags)
        {
            if (String.IsNullOrEmpty(title))
            {
                title = "SharpDL Window";
            }

            X = x;
            Y = y;
            Width = width;
            Height = height;

            List<WindowFlags> copyFlags = new List<WindowFlags>();
            foreach (WindowFlags flag in Enum.GetValues(typeof(WindowFlags)))
                if (flags.HasFlag(flag))
                    copyFlags.Add(flag);

            Flags = copyFlags;

            Handle = SDL.SDL_CreateWindow(title, X, Y, Width, Height, (SDL.SDL_WindowFlags)flags);
            if (Handle == IntPtr.Zero)
            {
                throw new InvalidOperationException(String.Format("SDL_CreateWindow: {0}", SDL.SDL_GetError()));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Window()
        {
            //log.Debug("A window resource has leaked. Did you forget to dispose the object?");
        }

        private void Dispose(bool isDisposing)
        {
            if (Handle != IntPtr.Zero)
            {
                SDL.SDL_DestroyWindow(Handle);
                Handle = IntPtr.Zero;
            }
        }
    }
}