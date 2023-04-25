// Curt Lynch
// CSCI 352
// 04/25/2023
// game data container

using System.Collections.Generic;
using System.Windows.Media;

namespace Temtris
{
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
            color = copy.color;
        }
    }

    // Basically where all of the game data is stored.
    class Matrix
    {
        public List<Mino> preview_Tetra = new List<Mino>();
        public List<Mino> active_Tetra = new List<Mino>();
        public List<Mino> inactive_Tetra = new List<Mino>();
        public double score;

        public Matrix() { }
        public Matrix(Matrix copy)
        {
            // There's a race condition here where if copy's lists change size during AddRange there may not be enough room in this list
            // I thought the copy would help prevent that, but it probably made things worse.
            active_Tetra.AddRange(copy.active_Tetra);
            inactive_Tetra.AddRange(copy.inactive_Tetra);
            preview_Tetra.AddRange(copy.preview_Tetra);
            score = copy.score;
        }
    }
}
