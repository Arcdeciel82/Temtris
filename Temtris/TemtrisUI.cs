using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temtris
{
    abstract class TemtrisUI
    {
        abstract public void Start();

        abstract public void Update(List<Mino> minos);

        abstract public int Menu();
    }
}
