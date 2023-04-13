using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Temtris
{
    enum Direction
    {
        Up, Down, Left, Right
    }

    enum Difficulty
    {
        Menu,
        Easy,
        Hard,
    };

    // Handles all of the game logic and updates related to a game of Temtris.
    internal class TemtrisGame : GameEngine
    {
        private Matrix matrix;
        private MinoFactory factory;
        private Input keyboard;
        private Difficulty difficulty;
        private double elapsedTime = 0.0;
        private double fallRate = 500.0;
        private bool isRunning = false;
        private double fallRateMod = 1.0;
        public bool IsRunning { get => isRunning; }

        // Runs once when the game is started
        protected override void OnStart(Difficulty d)
        {
            isRunning = true;
            matrix = new Matrix();
            keyboard = new Input();
            difficulty = d;
            switch (d)
            {
                case Difficulty.Menu:
                    factory = new Menu_MinoFactory();
                    break;
                case Difficulty.Easy:
                    factory = new Easy_MinoFactory();
                    break;
                case Difficulty.Hard:
                    factory = new Hard_MinoFactory();
                    break;
            }
            matrix.active_Tetra = factory.Next();
            matrix.preview_Tetra = factory.Next();
            fallRate = factory.NextFallRate(fallRate);
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
                isRunning = !MinoCollision();
            }
            matrix.score += elapsedTimeMs;
            // act on user input / non timed updates
            ProcessUserInput();
            return IsRunning;
        }

        // Returns a copy of the Matrix
        public Matrix GetMatrix()
        {
            return new Matrix(matrix);
        }

        public Difficulty GetDifficulty()
        {
            return difficulty;
        }

        // Gets input updates and responds to them.
        private void ProcessUserInput()
        {
            // Tell the main thread to do the keyboard update, because apparently that's neccesary.
            Application.Current.Dispatcher.Invoke(() =>
            {
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
            List<Mino> backup = new List<Mino>();
            foreach (Mino m in matrix.active_Tetra)
            {
                backup.Add(new Mino(m));
            }

            // Center of rotation
            double xC = matrix.active_Tetra[0].x;
            double yC = matrix.active_Tetra[0].y;

            foreach (Mino m in matrix.active_Tetra)
            {
                // original location
                int xO = m.x;
                int yO = m.y;

                m.x = (int)Math.Floor(yC - yO + xC);
                m.y = (int)Math.Floor(xO - xC + yC);
            }
            if (MinoCollision())
            {
                matrix.active_Tetra = backup;
            }
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
                foreach (Mino mi in matrix.inactive_Tetra)
                {
                    if (mi.x == m.x && mi.y == m.y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Lowers active_Tetra or gets the next Tetra from factory. Performs row clears and updates fallrate accordingly.
        private void DropTetra()
        {
            if (!TranslateTetrimino(Direction.Down))
            {
                matrix.inactive_Tetra.AddRange(matrix.active_Tetra);
                matrix.active_Tetra = matrix.preview_Tetra;
                matrix.preview_Tetra = factory.Next();
                fallRate = factory.NextFallRate(fallRate);
                RowClear();
            }
        }

        // Checks for and performs row clears. Updates score accordingly.
        private void RowClear()
        {
            int clearCount = 0;
            int[] tally = new int[20];
            foreach (Mino m in matrix.inactive_Tetra)
            {
                if (m.y >= 0)
                    tally[m.y]++;
            }
            for(int i = 0; i < tally.Length; i++)
            {
                if (tally[i] == 10)
                {
                    clearCount++;
                    RemoveRowAndShift(i);
                }
            }
            matrix.score += clearCount * 10000;
            if (clearCount == 4)
            {
                matrix.score += 35000;
            }

        }

        // Removes the given row of minos and moves the 
        private void RemoveRowAndShift(int row)
        {
            matrix.inactive_Tetra.RemoveAll(mino => mino.y == row);
            foreach (Mino m in matrix.inactive_Tetra)
            {
                if (m.y < row)
                {
                    m.y++;
                }
            }
        }
    }
}
