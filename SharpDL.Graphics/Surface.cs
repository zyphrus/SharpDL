﻿using SDL2;
using SharpDL.Shared;
using System;
using System.Runtime.InteropServices;

namespace SharpDL.Graphics
{
    public class Surface : IDisposable
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string FilePath { get; private set; }

        public IntPtr Handle { get; private set; }

        public SurfaceType Type { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        //public IntPtr Pixels { get; set; }
        //public int Pitch { get; set; }

        public Surface(string filePath, SurfaceType surfaceType)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath", Errors.E_SURFACE_PATH_MISSING);
            }

            FilePath = filePath;
            Type = surfaceType;
            if (surfaceType == SurfaceType.BMP)
            {
                Handle = SDL.SDL_LoadBMP(FilePath);
            }
            else if (surfaceType == SurfaceType.PNG)
            {
                Handle = SDL_image.IMG_Load(FilePath);
            }

            if (Handle == IntPtr.Zero)
            {
                throw new InvalidOperationException(String.Format("Error while loading image surface: {0}", SDL.SDL_GetError()));
            }

            GetSurfaceMetaData();
        }

        public Surface(Font font, string text, Color color)
            : this(font, text, color, 0)
        {
        }

        public Surface(Font font, string text, Color color, int wrapLength)
        {
            if (font == null)
            {
                throw new ArgumentNullException("font", Errors.E_FONT_NULL);
            }

            if (wrapLength < 0)
                throw new ArgumentOutOfRangeException("wrapLength", "Wrap length must be greater than 0.");

            Type = SurfaceType.Text;
            SDL.SDL_Color rawColor = new SDL.SDL_Color() { r = color.R, g = color.G, b = color.B };

            if (wrapLength > 0)
            {
                Handle = SDL_ttf.TTF_RenderText_Blended_Wrapped(font.Handle, text, rawColor, (uint)wrapLength);
            }
            else
            {
                Handle = SDL_ttf.TTF_RenderText_Blended(font.Handle, text, rawColor);
            }

            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException(String.Format("Error while loading text surface: {0}", SDL.SDL_GetError()));

            GetSurfaceMetaData();
        }

        private void GetSurfaceMetaData()
        {
            if (Handle == IntPtr.Zero)
            {
                throw new InvalidOperationException(Errors.E_FONT_NULL);
            }

            SDL.SDL_Surface rawSurface = (SDL.SDL_Surface)Marshal.PtrToStructure(Handle, typeof(SDL.SDL_Surface));
            Width = rawSurface.w;
            Height = rawSurface.h;
            //Pixels = rawSurface.pixels;
            //Pitch = rawSurface.pitch;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Surface()
        {
            //log.Debug("A surface resource has leaked. Did you forget to dispose the object?");
        }

        private void Dispose(bool disposing)
        {
            if (Handle != IntPtr.Zero)
            {
                SDL.SDL_FreeSurface(Handle);
                Handle = IntPtr.Zero;
            }
        }
    }
}