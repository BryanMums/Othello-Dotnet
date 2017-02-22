using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{



    class Matrix
    {

        int[,] matrix ;
        int i, j, player;


        public Matrix()
        {
            this.matrix = new int[8, 8];

            // Ce sont les coins
            this.matrix[0, 0] = this.matrix[0, 7] = this.matrix[7, 0] = this.matrix[7, 7] = 20;

            // Les valeurs à côté des coins
            this.matrix[1, 1] = this.matrix[1, 6] = this.matrix[6, 1] = this.matrix[6, 6] = -7;
            this.matrix[0, 1] = this.matrix[1, 0] = this.matrix[7, 6] = this.matrix[6, 7] = -3;
            this.matrix[0, 6] = this.matrix[1, 7] = this.matrix[6, 0] = this.matrix[7, 1] = -3;

            // Les valeurs au bord
            this.matrix[0, 2] = this.matrix[2, 0] = this.matrix[0, 5] = this.matrix[5, 0] = 11;
            this.matrix[5, 2] = this.matrix[2, 5] = this.matrix[7, 5] = this.matrix[5, 7] = 11;
            this.matrix[0, 3] = this.matrix[0, 4] = this.matrix[7, 3] = this.matrix[7, 4] = 8;
            this.matrix[3, 0] = this.matrix[4, 0] = this.matrix[3, 7] = this.matrix[4, 7] = 8;

            // Les valeurs au centre
            this.matrix[3, 3] = this.matrix[3, 4] = this.matrix[4, 3] = this.matrix[4, 4] = -3;
            this.matrix[2, 3] = this.matrix[2, 4] = this.matrix[5, 3] = this.matrix[5, 4] = 2;
            this.matrix[3, 2] = this.matrix[3, 5] = this.matrix[4, 2] = this.matrix[4, 5] = 2;
            this.matrix[2, 2] = this.matrix[5, 2] = this.matrix[2, 5] = this.matrix[5, 5] = 2;
        }

        public int getValue(int i, int j)
        {
            return this.matrix[i, j];
        }
    }
}
