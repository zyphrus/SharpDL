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

        public static byte[] getState()
        {
            int size = 0;
            var ptr = SDL.SDL_GetKeyboardState(out size);
            var keystate = new byte[size];
            int index = 0;
            while (index < size)
            {
                int block = ptr.ToInt32();
                keystate[index++] = (byte)(block >> 24 % 256);
                if (index < size)
                    keystate[index++] = (byte)(block >> 16 % 256);
                if (index < size)
                    keystate[index++] = (byte)(block >> 8 % 256);
                if (index < size)
                    keystate[index++] = (byte)(block >> 0 % 256);
                ptr += 1;
            }
            return keystate;
        }
    }
}

