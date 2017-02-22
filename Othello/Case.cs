using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
   
    public class Case
    {
        public int column { get; set; }
        public int row { get; set; }
        private int state = -1; //-1->vide, 0->blanc, 1->noir
        public Case(int column, int row)
        {
            this.column = column;
            this.row = row;
        }

        public int getState()
        {
            return state;
        }

        public void setState(int state)
        {
            this.state = state;
        }
    }
}
