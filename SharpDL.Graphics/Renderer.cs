using SDL2;
using SharpDL.Shared;
using System;
using System.Collections.Generic;

namespace SharpDL.Graphics
{
    public class Renderer : IDisposable
    {
        #region Feilds and Constructor

        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<RendererFlags> flags = new List<RendererFlags>();

        public Window Window { get; private set; }

        public int Index { get; private set; }

        public IEnumerable<RendererFlags> Flags { get { return flags; } }

        public IntPtr Handle { get; private set; }

        public Renderer(Window window, int index, RendererFlags flags)
        {
            if (window == null)
            {
                throw new ArgumentNullException(Errors.E_WINDOW_NULL);
            }

            Window = window;
            Index = index;

            var copyFlags = new List<RendererFlags>();
            foreach (RendererFlags flag in Enum.GetValues(typeof(RendererFlags)))
            {
                if (flags.HasFlag(flag))
                {
                    this.flags.Add(flag);
                }
            }

            Handle = SDL.SDL_CreateRenderer(Window.Handle, Index, (uint)flags);
            if (Handle == IntPtr.Zero)
            {
                throw new InvalidOperationException(Utilities.GetErrorMessage("SDL_CreateRenderer"));
            }
        }

        #endregion

        #region Render Functions

        public void ClearScreen()
        {
            ThrowExceptionIfRendererIsNull();

            int result = SDL.SDL_RenderClear(Handle);
            if (Utilities.IsError(result))
            {
                throw new InvalidOperationException(Utilities.GetErrorMessage("SDL_RenderClear"));
            }
        }



        internal void RenderTexture(IntPtr textureHandle, float positionX, float positionY, int sourceWidth, int sourceHeight, double angle, Vector center)
        {
            if (textureHandle == IntPtr.Zero)
            {
                throw new ArgumentNullException("textureHandle", Errors.E_TEXTURE_NULL);
            }

            // SDL only accepts integer positions (x,y) in the rendering Rect
            var destinationRectangle = new SDL.SDL_Rect() { x = (int)positionX, y = (int)positionY, w = sourceWidth, h = sourceHeight };
            var sourceRectangle = new SDL.SDL_Rect() { x = 0, y = 0, w = sourceWidth, h = sourceHeight };
            var centerPoint = new SDL.SDL_Point() { x = (int)center.X, y = (int)center.Y };

            int result = SDL.SDL_RenderCopyEx(Handle, textureHandle, ref sourceRectangle, ref destinationRectangle, angle, ref centerPoint, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            if (Utilities.IsError(result))
            {
                throw new InvalidOperationException(Utilities.GetErrorMessage("SDL_RenderCopyEx"));
            }
        }

        internal void RenderTexture(IntPtr textureHandle, float positionX, float positionY, int sourceWidth, int sourceHeight)
        {
            var source = new Rectangle(0, 0, sourceWidth, sourceHeight);
            RenderTexture(textureHandle, positionX, positionY, source);
        }

        internal void RenderTexture(IntPtr textureHandle, float positionX, float positionY, Rectangle source)
        {
            if (textureHandle == IntPtr.Zero)
            {
                throw new ArgumentNullException("textureHandle", Errors.E_TEXTURE_NULL);
            }

            int width = source.Width;
            int height = source.Height;

            // SDL only accepts integer positions (x,y) in the rendering Rect
            var destinationRectangle = new SDL.SDL_Rect() { x = (int)positionX, y = (int)positionY, w = width, h = height };
            var sourceRectangle = new SDL.SDL_Rect() { x = source.X, y = source.Y, w = width, h = height };

            int result = SDL.SDL_RenderCopy(Handle, textureHandle, ref sourceRectangle, ref destinationRectangle);
            if (Utilities.IsError(result))
            {
                throw new InvalidOperationException(Utilities.GetErrorMessage("SDL_RenderCopy"));
            }
        }

        public void RenderTexture(Texture texture, Vector position, Rectangle? source, double angle, Vector center)
        {
            RenderTexture(texture.Handle, position.X, position.Y, (source != null ? source.Value.Width : texture.Width)
                , (source != null ? source.Value.Height : texture.Height), angle, center);
        }

        public void RenderTexture(Texture texture, Point position, Rectangle? source, double angle, Vector center)
        {


            RenderTexture(texture.Handle, position.X, position.Y, (source != null ? source.Value.Width : texture.Width)
                , (source != null ? source.Value.Height : texture.Height), angle, center);
        }

        public void RenderPoint(Point point, Color color)
        {
            RenderPoint(point.X, point.Y, color);
        }

        public void RenderPoint(int x, int y, Color color)
        {
            ThrowExceptionIfRendererIsNull();

            var oldColor = GetDrawColor();
            SetDrawColor(color);
            int result = SDL.SDL_RenderDrawPoint(Handle, x, y);
            SetDrawColor(oldColor);
            if (Utilities.IsError(result))
            {
                throw new InvalidOperationException(); //TODO: Proper error
            }
        }

        public void RenderRect(Rectangle rect, Color color)
        {
            ThrowExceptionIfRendererIsNull();

            var sdlRect = rect.ToSDLRect();
            var oldColor = GetDrawColor();
            SetDrawColor(color);
            int result = SDL.SDL_RenderDrawRect(Handle, ref sdlRect);
            SetDrawColor(oldColor);
            if (Utilities.IsError(result))
            {
                throw new InvalidOperationException(); //TODO: Proper error
            }
        }

        public void RenderRect(int x, int y, int w, int h, Color color)
        {
            RenderRect(new Rectangle(x, y, w, h), color);
        }

        public void RenderLine(Point a, Point b, Color color)
        {
            RenderLine(a.X, a.Y, b.X, b.Y, color);
        }

        public void RenderLine(Vector a, Vector b, Color color)
        {
            RenderLine((int)a.X, (int)a.Y, (int)b.X, (int)b.Y, color);
        }

        /// <summary>
        /// Renders the line.
        /// </summary>
        /// <param name="x1">The first x value.</param>
        /// <param name="y1">The first y value.</param>
        /// <param name="x2">The second x value.</param>
        /// <param name="y2">The second y value.</param>
        /// <param name="color">Color to be rendered</param>
        /// <remarks>Renderer's color only changes during draw, returns to old color after</remarks>
        public void RenderLine(int x1, int y1, int x2, int y2, Color color)
        {
            ThrowExceptionIfRendererIsNull();
            var oldColor = GetDrawColor();
            SetDrawColor(color);
            int result = SDL.SDL_RenderDrawLine(Handle, x1, y1, x2, y2);
            SetDrawColor(oldColor);
            if (Utilities.IsError(result))
            {
                throw new InvalidOperationException(); //TODO: Proper error
            }

        }

        public void RenderPresent()
        {
            ThrowExceptionIfRendererIsNull();

            SDL.SDL_RenderPresent(Handle);
        }

        #endregion

        #region Settings

        public Color GetDrawColor()
        {
            byte r, g, b, a;
            SDL.SDL_GetRenderDrawColor(Handle, out r, out g, out b, out a);
            return new Color(r, g, b, a);
        }

        public void ResetRenderTarget()
        {
            ThrowExceptionIfRendererIsNull();

            int result = SDL2.SDL.SDL_SetRenderTarget(Handle, IntPtr.Zero);
            if (Utilities.IsError(result))
            {
                throw new InvalidOperationException(Utilities.GetErrorMessage("SDL_SetRenderTarget"));
            }
        }

        public void SetRenderTarget(RenderTarget renderTarget)
        {
            ThrowExceptionIfRendererIsNull();

            if (!flags.Contains(RendererFlags.SupportRenderTargets))
            {
                throw new InvalidOperationException(Errors.E_RENDERER_NO_RENDER_TARGET_SUPPORT);
            }

            int result = SDL.SDL_SetRenderTarget(Handle, renderTarget.Handle);
            if (Utilities.IsError(result))
            {
                throw new InvalidOperationException(Utilities.GetErrorMessage("SDL_SetRenderTarget"));
            }
        }

        public void SetBlendMode(BlendMode blendMode)
        {
            int result = SDL.SDL_SetRenderDrawBlendMode(Handle, (SDL.SDL_BlendMode)blendMode);
            if (Utilities.IsError(result))
            {
                throw new InvalidOperationException(Utilities.GetErrorMessage("SDL_SetDrawBlendMode"));
            }
        }

        public void SetDrawColor(Color color)
        {
            SetDrawColor(color.R, color.G, color.B, 255);
        }

        public void SetDrawColor(Color color, byte a)
        {
            SetDrawColor(color.R, color.G, color.B, a);
        }

        public void SetDrawColor(byte r, byte g, byte b, byte a)
        {
            ThrowExceptionIfRendererIsNull();

            int result = SDL.SDL_SetRenderDrawColor(Handle, r, g, b, a);

            if (Utilities.IsError(result))
            {
                throw new InvalidOperationException(Utilities.GetErrorMessage("SDL_SetRenderDrawColor"));
            }
        }

        public void SetRenderLogicalSize(int width, int height)
        {
            ThrowExceptionIfRendererIsNull();

            int result = SDL.SDL_RenderSetLogicalSize(Handle, width, height);
            if (Utilities.IsError(result))
            {
                throw new InvalidOperationException(Utilities.GetErrorMessage("SDL_RenderSetLogicalSize"));
            }
        }

        #endregion

        #region Dispose & Other

        private void ThrowExceptionIfRendererIsNull()
        {
            if (Handle == IntPtr.Zero)
            {
                throw new InvalidOperationException(Errors.E_RENDERER_NULL);
            }
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Renderer()
        {
            //log.Debug("A renderer resource has leaked. Did you forget to dispose the object?");
        }

        private void Dispose(bool disposing)
        {
            if (Handle != IntPtr.Zero)
            {
                SDL.SDL_DestroyRenderer(Handle);
                Handle = IntPtr.Zero;
            }
        }

        #endregion
    }
}