using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temtris
{
    // Handles the creation of polyminos for a game of Temtris
    internal class MinoFactory
    {
        // Placeholder returns the same 1x2 Tetra.
        public List<Mino> Next()
        {
            List<Mino> tetra = new List<Mino>();

            Mino first = new Mino();
            first.x = 5;
            first.y = 0;
            tetra.Add(first);

            Mino second = new Mino();
            second.x = 6;
            second.y = 0;
            tetra.Add(second);


            return tetra;
        }
    }
}
