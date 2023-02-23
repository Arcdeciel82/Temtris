using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Temtris
{
    abstract class GameEngine
    {
        public void Start()
        {

        }

        private void GameLoop()
        {

        }

        abstract protected void OnStart();

        abstract protected void OnUpdate(float ElapsedTime);

        
    }
}
