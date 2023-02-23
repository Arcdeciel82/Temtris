using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Temtris
{
    enum Color
    {
        Red, Green, Blue, Pink;
    }

    struct mino
    {
        
        public int x,y;
        public Color color;
    }

    internal class Matrix
    {
        private List<mino> minos_active;
        private List<mino> minos_inactive;

        public void Update(float ElapsedTime)
        {

        }
    }
}
