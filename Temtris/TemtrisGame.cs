using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Temtris
{
    enum Direction
    {
        Up, Down, Left, Right
    }

    // Handles all of the game logic and updates related to a game of Temtris.
    internal class TemtrisGame : GameEngine
    {
        Matrix matrix;
        MinoFactory factory;
        Input keyboard;
        double elapsedTime = 0.0;
        double fallRate = 500.0;
        bool isRunning = false;
        double fallRateMod = 1.0;

        // Runs once when the game is started
        protected override void OnStart()
        {
            isRunning = true;
            matrix = new Matrix();
            factory = new MinoFactory();
            keyboard = new Input();
            matrix.active_Tetra = factory.Next();
        }

        // Runs on every loop of the game engine.
        protected override bool OnUpdate(double elapsedTimeMs)
        {
            elapsedTime += elapsedTimeMs;
            if (elapsedTime > fallRate * fallRateMod)
            {
                elapsedTime -= fallRate * fallRateMod;

                // perform timed updates
                DropTetra();
                RowClear();
            }

            // act on user input / non timed updates
            ProcessUserInput();
            return isRunning;
        }

        // Returns a copy of the Matrix
        public Matrix GetMatrix()
        {
            return new Matrix(matrix);
        }

        // Gets input updates and responds to them.
        private void ProcessUserInput()
        {
            // Tell the main thread to do the keyboard update, because apparently that's neccesary.
            Application.Current.Dispatcher.Invoke(() => {
                keyboard.Update();
            });

            if (keyboard.GetKey(Key.W).isPressed)
            {
                RotateTetrimino();
            }
            if (keyboard.GetKey(Key.A).isPressed)
            {
                TranslateTetrimino(Direction.Left);
            }
            if (keyboard.GetKey(Key.D).isPressed)
            {
                TranslateTetrimino(Direction.Right);
            }
            if (keyboard.GetKey(Key.S).isPressed)
            {
                elapsedTime = 0.0;
                fallRateMod = 0.3;
            }
            if (keyboard.GetKey(Key.S).isReleased)
            {
                fallRateMod = 1.0;
            }
        }

        // Rotates matrix.active_Tetra 90 degrees clockwise.
        private void RotateTetrimino()
        {
            //TODO: Implement
        }

        // Translates matrix.active_Tetra 1 unit in the given dirction or returns false if this would cause a collision.
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
                if (MinoCollision())
                {
                    matrix.active_Tetra = oldTetra;
                    return false;
                }
            }

            return true;
        }

        // Returns true if active_Tetra is colliding with any minos, the left/right walls, or the floor.
        private bool MinoCollision()
        {
            // This is a /slow/ solution, but with so few minos it's probably fine.
            foreach (Mino m in matrix.active_Tetra)
            {
                if (m.y > 19 || m.x < 0 || m.x > 9)
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

        // Lowers active_Tetra or gets the next Tetra from factory.
        private void DropTetra()
        {
            if (!TranslateTetrimino(Direction.Down))
            {
                matrix.inactive_Minos.AddRange(matrix.active_Tetra);
                matrix.active_Tetra = factory.Next();
            }
        }

        // Checks for and performs row clears. Updates fallrate accordingly.
        private void RowClear()
        {
            //TODO: Implement
        }
    }
}
