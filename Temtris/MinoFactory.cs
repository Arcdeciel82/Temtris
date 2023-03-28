using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Temtris
{
    // Handles the creation of polyminos for a game of Temtris
    internal abstract class MinoFactory
    {
        public abstract List<Mino> Next();
        
        public abstract double NextFallRate(double fallRate);
    }

    internal class Menu_MinoFactory : MinoFactory
    {
        Random rand = new Random();
        public override List<Mino> Next()
        {
            List<Mino> tetra = new List<Mino>();

            Mino first = new Mino();
            first.x = rand.Next() % 9;
            first.y = 0;
            first.color = Colors.Blue;
            tetra.Add(first);

            Mino second = new Mino();
            second.x = first.x + 1;
            second.y = 0;
            second.color = Colors.Green;
            tetra.Add(second);


            return tetra;
        }

        public override double NextFallRate(double fallRate)
        {
            return 250.0;   
        }
    }

    internal class Easy_MinoFactory : MinoFactory
    {
        public override List<Mino> Next()
        {
            throw new NotImplementedException();
        }

        public override double NextFallRate(double fallRate)
        {
            throw new NotImplementedException();
        }
    }

    internal class Hard_MinoFactory : MinoFactory
    {
        public override List<Mino> Next()
        {
            throw new NotImplementedException();
        }
        public override double NextFallRate(double fallRate)
        {
            throw new NotImplementedException();
        }
    }
}
