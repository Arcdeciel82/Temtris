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

    // Represents a single block in the matrix
    class Mino
    {
        public int x, y;
        public Color color;

        public Mino()
        {

        }

        public Mino(Mino copy)
        {
            x = copy.x;
            y = copy.y;
            color = copy.color; // shallow copy , but that's fine
        }
    }

    // Basically where all of the game data is stored.
    class Matrix
    {
        public List<Mino> active_Tetra = new List<Mino>();
        public List<Mino> inactive_Minos = new List<Mino>();
        public int score;

        public Matrix() { }
        public Matrix(Matrix copy)
        {
            active_Tetra.AddRange(copy.active_Tetra);
            inactive_Minos.AddRange(copy.inactive_Minos);
            score = copy.score;
        }
    }
}
