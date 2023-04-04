using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;

namespace Temtris
{
    // Handles the creation of polyminos for a game of Temtris
    internal abstract class MinoFactory
    {
        protected Random rand = new Random();
        public abstract List<Mino> Next();

        public abstract double NextFallRate(double fallRate);

        protected virtual Color NextColor()
        {
            var colorProp = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var minoColors = colorProp.Select(prop => (Color)prop.GetValue(null, null));
            Color minoColor = minoColors.ElementAt(rand.Next() % minoColors.Count());
            if (minoColor.Equals(Colors.Black) || minoColor.Equals(Colors.Transparent))
            {
                minoColor = Colors.Pink;
            }
            return minoColor;
        }
    }

    internal class Menu_MinoFactory : MinoFactory
    {
        public override List<Mino> Next()
        {
            List<Mino> tetra = new List<Mino>();
            Color minoColor = NextColor();

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
        public override List<Mino> Next()
        {
            List<Mino> tetra = new List<Mino>();
            Color minoColor = NextColor();

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
                return fallRate -= 5.0;
            else
                return fallRate;
        }
    }

    internal class Hard_MinoFactory : MinoFactory
    {
        enum shapes
        {
            L,
            J,
            T,
            O,
            I,
            S,
            Z,
        };

        public override List<Mino> Next()
        {
            List<Mino> tetra = new List<Mino>();
            Color minoColor = NextColor();
            shapes minoShape = (shapes)Enum.GetValues(typeof(shapes)).GetValue(rand.Next() % 7);

            Mino one = new Mino();
            one.x = 4;
            one.y = 0;
            one.color = minoColor;
            Mino two = new Mino(one);
            Mino three = new Mino(one);
            Mino four = new Mino(one);

            switch (minoShape)
            {
                case shapes.L:
                    {
                        two.x = 5;
                        three.x = 3;
                        four.x = 3;
                        four.y = 1;
                        break;
                    }
                case shapes.J:
                    {
                        two.x = 5;
                        three.x = 3;
                        four.x = 5;
                        four.y = 1;
                        break;
                    }
                case shapes.T:
                    {
                        two.x = 5;
                        three.x = 3;
                        four.y = 1;
                        break;
                    }
                case shapes.O:
                    {
                        two.x = 5;
                        three.y = 1;
                        four.x = 5;
                        four.y = 1;
                        break;
                    }
                case shapes.S:
                    {
                        two.x = 5;
                        three.y = 1;
                        four.x = 3;
                        four.y = 1;
                        break;
                    }
                case shapes.I:
                    {
                        two.x = 5;
                        three.x = 6;
                        four.x = 3;
                        break;
                    }
                case shapes.Z:
                    {
                        two.x = 3;
                        three.y = 1;
                        four.x = 5;
                        four.y = 1;
                        break;
                    }
            }

            tetra.Add(one);
            tetra.Add(two);
            tetra.Add(three);
            tetra.Add(four);

            return tetra;
        }
        public override double NextFallRate(double fallRate)
        {
            if (fallRate > 150.0)
                return fallRate -= 10.0;
            else
                return fallRate;
        }
    }
}
