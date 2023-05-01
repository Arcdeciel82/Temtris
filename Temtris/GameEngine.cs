// Curt Lynch
// CSCI 352
// 04/25/2023
// game engine template

using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Temtris
{
    // Generic game engine template
    abstract internal class GameEngine
    {
        Stopwatch time = new Stopwatch();
        BackgroundWorker worker;
        double timeSinceUIUpdate = 0.0;
        Difficulty difficulty;

        public bool IsCancelled { get; set; } = false;

        // Starts the game Engine (on this thread)
        public object Start(Difficulty d)
        {
            time.Start();
            OnStart(d);
            double ElapsedTime = time.Elapsed.TotalMilliseconds;
            bool isRunning = true;
            while (isRunning)
            {
                isRunning = OnUpdate(ElapsedTime);

                // game updates should probably at least be 1ms apart
                if (ElapsedTime < 1.0)
                    Thread.Sleep(1);

                ElapsedTime = time.Elapsed.TotalMilliseconds;
                time.Restart();
            }
            return OnStop();
        }

        private object AsyncLoop(Difficulty d)
        {
            time.Start();
            OnStart(d);
            double ElapsedTime = time.Elapsed.TotalMilliseconds;
            bool isRunning = true;
            while (isRunning && !IsCancelled)
            {
                isRunning = OnUpdate(ElapsedTime);
                timeSinceUIUpdate += ElapsedTime;

                // game updates should probably at least be 1ms apart
                if (ElapsedTime < 1.0)
                    Thread.Sleep(1);

                // Ensure some time has passed since last UI update. 5ms/Update = 200fps max should be fine.
                if (timeSinceUIUpdate > 5.0)
                {
                    timeSinceUIUpdate = 0.0;
                    worker.ReportProgress(0, this);
                }

                ElapsedTime = time.Elapsed.TotalMilliseconds;
                time.Restart();
            }

            if (IsCancelled)
            {
                worker.CancelAsync();
            }

            return OnStop();
        }

        public void StartAsync(Difficulty d, ProgressChangedEventHandler update, RunWorkerCompletedEventHandler completed)
        {
            difficulty = d;

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += WorkerDoWork;
            worker.ProgressChanged += update;
            worker.RunWorkerCompleted += completed;

            worker.RunWorkerAsync();
        }

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            worker = (BackgroundWorker)sender;
            e.Result = AsyncLoop(difficulty);
        }

        // Runs once after Start()
        protected abstract void OnStart(Difficulty d);

        // Runs on each game loop. Game loop exits if OnUpdate returns false.
        protected abstract bool OnUpdate(double elapsedTimeMs);

        // Runs once after the Game loop exits.
        protected virtual object OnStop() { return this; }
    }
}
