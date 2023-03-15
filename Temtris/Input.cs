using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Update()
        {
            foreach (KeyValuePair<Key, KeyState> entry in keyMap)
            {
                bool pressed = Keyboard.IsKeyDown(entry.Key);
                KeyState state = entry.Value;
                if (!pressed && state.isHeld)
                {
                    state.isHeld = false;
                    state.isReleased = true;
                }
                else if (pressed && !state.isHeld)
                {
                    state.isPressed = true;
                    state.isHeld = true;
                }
                else if (state.isPressed)
                {
                    state.isPressed = false;
                }
                else if (state.isReleased)
                {
                    state.isReleased = false;
                }
            }
        }

        public KeyState GetKey(Key key)
        {
            return keyMap[key];
        }

    }
}
