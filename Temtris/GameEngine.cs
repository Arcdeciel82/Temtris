using System;
using System.Collections.Generic;
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

        public void Start()
        {
            Stopwatch.StartNew();
            OnStart();
            double ElapsedTime = time.Elapsed.TotalMilliseconds;
            bool isRunning = true;
            while (isRunning)
            {
                time.Restart();
                isRunning = OnUpdate(ElapsedTime);
                ElapsedTime = time.Elapsed.TotalMilliseconds;
            }
        }

        protected abstract void OnStart();
        protected abstract bool OnUpdate(double elapsedTimeMs);
        protected virtual void OnStop()
        {

        }
    }
}
