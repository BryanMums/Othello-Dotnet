using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
   
    public class Case
    {
        private char column;
        private int row;
        private int state = -1; //-1->vide, 0->blanc, 1->noir
        public Case(char column, int row)
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
