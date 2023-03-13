using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Temtris
{
    abstract internal class GameEngine
    {
        Stopwatch time = new Stopwatch();
        BackgroundWorker worker;
        double timeSinceUIUpdate = 0.0;

        public GameEngine Start(BackgroundWorker w)
        {
            worker = w;
            time.Start();
            OnStart();
            double ElapsedTime = time.Elapsed.TotalMilliseconds;
            bool isRunning = true;
            while (isRunning)
            {
                //TotalTime = time.Elapsed.TotalMilliseconds;
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

        protected abstract void OnStart();
        protected abstract bool OnUpdate(double elapsedTimeMs);
        protected virtual void OnStop() {}
    }
}
