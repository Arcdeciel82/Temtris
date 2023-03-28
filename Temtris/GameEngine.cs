using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Temtris
{
    // Generic gameengine intended to run as a BackgroundWorker
    abstract internal class GameEngine
    {
        Stopwatch time = new Stopwatch();
        BackgroundWorker worker;
        double timeSinceUIUpdate = 0.0;

        // Starts the game Engine
        public GameEngine Start(BackgroundWorker w, Difficulty d = Difficulty.Easy)
        {
            worker = w;
            time.Start();
            OnStart(d);
            double ElapsedTime = time.Elapsed.TotalMilliseconds;
            bool isRunning = true;
            while (isRunning && !w.CancellationPending)
            {
                isRunning = OnUpdate(ElapsedTime);
                timeSinceUIUpdate += ElapsedTime;

                Thread.Sleep(1); // gameloop is a bit too fast atm.

                // Ensure some time has passed since last UI update. 5ms/Update = 200fps max should be fine.
                if (timeSinceUIUpdate > 5.0)
                {
                    timeSinceUIUpdate = 0.0;
                    worker.ReportProgress(0, this);
                }

                ElapsedTime = time.Elapsed.TotalMilliseconds;
                time.Restart();
            }
            OnStop();
            return this;
        }

        // Runs once after Start()
        protected abstract void OnStart(Difficulty d);

        // Runs on each game loop. Game loop exits if OnUpdate returns false.
        protected abstract bool OnUpdate(double elapsedTimeMs);

        // Runs once after the Game loop exits.
        protected virtual void OnStop() { }
    }
}
