using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var colorProp = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var minoColors = colorProp.Select(prop => (Color)prop.GetValue(null, null));
            Color minoColor = minoColors.ElementAt(rand.Next() % minoColors.Count());
            if (minoColor == Colors.Black)
            {
                minoColor = Colors.Pink;
            }

            Mino first = new Mino();
            first.x = rand.Next() % 9;
            first.y = 0;
            first.color = minoColor;
            tetra.Add(first);

            Mino second = new Mino();
            second.x = first.x + 1;
            second.y = 0;
            second.color = minoColor;
            tetra.Add(second);


            return tetra;
        }

        public override double NextFallRate(double fallRate)
        {
            return 25.0;   
        }
    }

    internal class Easy_MinoFactory : MinoFactory
    {
        Random rand = new Random();

        public override List<Mino> Next()
        {
            List<Mino> tetra = new List<Mino>();
            var colorProp = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var minoColors = colorProp.Select(prop => (Color)prop.GetValue(null, null));
            Color minoColor = minoColors.ElementAt(rand.Next() % minoColors.Count());
            if (minoColor == Colors.Black)
            {
                minoColor = Colors.Pink;
            }

            Mino first = new Mino();
            first.x = rand.Next() % 9;
            first.y = 0;
            first.color = minoColor;
            tetra.Add(first);

            Mino second = new Mino();
            second.x = first.x + 1;
            second.y = 0;
            second.color = minoColor;
            tetra.Add(second);


            return tetra;
        }

        public override double NextFallRate(double fallRate)
        {
            if (fallRate > 250.0)
                return fallRate * 0.95f;
            else
                return fallRate;
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
