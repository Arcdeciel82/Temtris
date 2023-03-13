using System.Collections.Generic;

namespace Temtris
{
    enum Color
    {
        Red,
        Green,
        Blue,
        Pink,
    }

    struct Mino
    {
        public int x, y;
        public Color color;
    }

    // Basically where all of the game data is stored.
    struct Matrix
    {
        private List<Mino> minos_active;
        private List<Mino> minos_inactive;
        private int score;
    }
}
