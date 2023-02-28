using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temtris
{
    class TemtrisUI
    {
        // Starts the UI with Main Menu
        GameEngine Temptris;

        public void Start()
        {
            Temptris= new TemtrisGame();
        }

        private bool Game_Update()
        {

        }
    }
}
