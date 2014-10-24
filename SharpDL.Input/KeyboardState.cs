using System;
using System.Collections.Generic;
using SDL2;

namespace SharpDL.Input
{
    public struct KeyboardState
    {
        public List<KeyState> Keys;
    }

    public struct KeyState
    {
        public SDL.SDL_Scancode Scancode{ get; private set; }

        public byte State { get; private set; }

        public KeyState(SDL.SDL_Scancode scancode, byte state)
            : this()
        {
            Scancode = scancode;
            State = state;
        }
    }
}

