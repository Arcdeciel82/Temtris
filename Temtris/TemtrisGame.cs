using System.Collections;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Temtris
{
    enum Direction
    {
        Up, Down, Left, Right
    }

    internal class TemtrisGame : GameEngine
    {
        Matrix matrix;
        MinoFactory factory;
        double elapsedTime = 0.0;
        double fallRate = 500.0;
        bool isRunning = false;

        protected override void OnStart()
        {
            isRunning = true;
            matrix = new Matrix();
            factory = new MinoFactory();
            matrix.active_Tetra = factory.Next();
        }

        protected override bool OnUpdate(double elapsedTimeMs)
        {
            elapsedTime += elapsedTimeMs;
            if (elapsedTime > fallRate)
            {
                elapsedTime -= fallRate;

                // perform timed updates
                DropTetra();
                RowClear();
            }

            // act on user input / non timed updates

            //ProcessUserInput(); Apparently needs to be on main thread.... that complicates things...

            return isRunning;
        }

        public Matrix GetMatrix()
        {
            return new Matrix(matrix);
        }

        private void ProcessUserInput()
        {
            if ((Keyboard.GetKeyStates(Key.W) & KeyStates.Down) > 0)
            {
                RotateTetrimino();
            }
            if ((Keyboard.GetKeyStates(Key.A) & KeyStates.Down) > 0)
            {
                TranslateTetrimino(Direction.Left);
            }
            if ((Keyboard.GetKeyStates(Key.D) & KeyStates.Down) > 0)
            {
                TranslateTetrimino(Direction.Right);
            }
            if ((Keyboard.GetKeyStates(Key.S) & KeyStates.Down) > 0)
            {
                TranslateTetrimino(Direction.Down);
            }
        }

        private void RotateTetrimino()
        {
            //TODO: Implement
        }

        private bool TranslateTetrimino(Direction d)
        {
            int x = 0, y = 0;
            switch (d)
            {
                case Direction.Left:
                    x = -1;
                    break;
                case Direction.Right:
                    x = 1; 
                    break;
                case Direction.Up:
                    y = -1;
                    break;
                case Direction.Down:
                    y = 1;
                    break;
            }

            // Create backup List in case of collision.
            List<Mino> oldTetra = new List<Mino>();

            foreach (Mino m in matrix.active_Tetra)
            {
                oldTetra.Add(new Mino(m));
            }

            foreach (Mino m in matrix.active_Tetra)
            {
                m.x += x;
                m.y += y;
                if (m.x < 0 || m.x > 9 || MinoCollision())
                {
                    matrix.active_Tetra = oldTetra;
                    return false;
                }
            }

            return true;
        }

        private bool MinoCollision()
        {
            // This is a slow solution, but with so few minos it's probably fine.
            foreach (Mino m in matrix.active_Tetra)
            {
                if (m.y > 19)
                {
                    return true;
                }
                foreach (Mino mi in matrix.inactive_Minos)
                {
                    if (mi.x == m.x && mi.y == m.y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void DropTetra()
        {
            if (!TranslateTetrimino(Direction.Down))
            {
                matrix.inactive_Minos.AddRange(matrix.active_Tetra);
                matrix.active_Tetra = factory.Next();
            }
        }

        private void RowClear()
        {
            //TODO: Implement
        }
    }
}
