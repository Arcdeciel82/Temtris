using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Temtris
{
    // isPressed is only true for the first update after when the key is pressed
    // isHeld is true the entire time the key is pressed
    // isReleased is only true for the first update after when the key is released
    class KeyState
    {
        public bool isPressed = false;
        public bool isHeld = false;
        public bool isReleased = false;
    }

    // Keeps track of the KeyState for every System.Input.Windows.Key.
    internal class Input
    {
        Dictionary<Key, KeyState> keyMap = new Dictionary<Key, KeyState>();

        public Input()
        {
            // Kinda regret doing it this way...
            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                if (!keyMap.ContainsKey(key) && key != Key.None)
                {
                    KeyState state = new KeyState();
                    keyMap.Add(key, state);
                }
            }
        }

        // Updates the internal KeyStates
        public void Update()
        {
            foreach (KeyValuePair<Key, KeyState> entry in keyMap)
            {
                bool pressed = Keyboard.IsKeyDown(entry.Key);
                KeyState state = entry.Value;
                if (pressed)
                {
                    if (state.isHeld)
                    {
                        state.isPressed = false;
                    }
                    else
                    {
                        state.isPressed = true;
                    }
                    state.isHeld = true;
                    state.isReleased = false;
                }
                if (!pressed)
                {
                    if (state.isHeld)
                    {
                        state.isReleased = true;
                    }
                    else
                    {
                        state.isReleased = false;
                    }
                    state.isHeld = false;
                    state.isPressed = false;
                }
            }
        }

        // Returns the KeyState of the given key.
        public KeyState GetKey(Key key)
        {
            return keyMap[key];
        }

    }
}
