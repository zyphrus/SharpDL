using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using SDL2;

namespace SharpDL.Input
{
    public static class Keyboard
    {
        public static void StartTextInput()
        {
            SDL.SDL_StartTextInput();
        }

        public static void StopTextInput()
        {
            SDL.SDL_StopTextInput();
        }

        public static KeyboardState GetState()
        {
            var state = new KeyboardState();
            var keys = new List<KeyState>();
           

            int size = 0;
            SDL.SDL_PumpEvents();
            var ptr = (SDL.SDL_GetKeyboardState(out size));
            unsafe
            {
                var bptr = (byte*)(ptr.ToPointer());
                for (int i = 0; i < size; i++)
                {
                    keys.Add(new KeyState((SDL.SDL_Scancode)(i), bptr[i]));
                }
            }
            state.Keys = keys;
            return state;
        }
    }
}

