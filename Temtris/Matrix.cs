﻿using System;
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

    struct Mino
    {
        
        public int x,y;
        public Color color;
    }

    internal class Matrix
    {
        private List<Mino> minos_active;
        private List<Mino> minos_inactive;

        public void Update(float ElapsedTime)
        {

        }
    }
}
