using System.Dynamic;

namespace Temtris
{
    internal class TemtrisGame : GameEngine
    {
        Matrix matrix;
        // MinoFactory ???
        // Input class ???
        double elapsedTime = 0.0;
        double fallRate = 10000.0;
        bool isRunning = false;

        protected override void OnStart()
        {
            isRunning = true;
            matrix = new Matrix();
        }

        protected override bool OnUpdate(double elapsedTimeMs)
        {
            elapsedTime += elapsedTimeMs;
            if (elapsedTime > fallRate)
            {
                elapsedTime -= fallRate;
                // perform timed updates

                // for testing game exits after first loop. (10 secs)
                isRunning = false;
            }
            // act on user input / non timed updates

            return isRunning;
        }

        public Matrix GetMatrix()
        {
            return matrix;
        }
    }
}
