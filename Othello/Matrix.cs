using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class Matrix
    {

        int[,] matrix;

        public Matrix()
        {
            this.matrix = new int[8, 8];

            // On met une valeur haute pour les coins   
            this.matrix[0, 0] = this.matrix[0, 7] = this.matrix[7, 0] = this.matrix[7, 7] = 20;

            // Les valeurs autour du coin sont dangereuses, donc on leur met des valeurs basses
            this.matrix[1, 1] = this.matrix[1, 6] = this.matrix[6, 1] = this.matrix[6, 6] = -7;
            this.matrix[0, 1] = this.matrix[1, 0] = this.matrix[7, 6] = this.matrix[6, 7] = -3;
            this.matrix[0, 6] = this.matrix[1, 7] = this.matrix[6, 0] = this.matrix[7, 1] = -3;

            // Les bords sont très important, donc ils auront des valeurs plus grandes
            this.matrix[0, 2] = this.matrix[2, 0] = this.matrix[0, 5] = this.matrix[5, 0] = 11;
            this.matrix[5, 2] = this.matrix[2, 5] = this.matrix[7, 5] = this.matrix[5, 7] = 11;
            this.matrix[0, 3] = this.matrix[0, 4] = this.matrix[7, 3] = this.matrix[7, 4] = 8;
            this.matrix[3, 0] = this.matrix[4, 0] = this.matrix[3, 7] = this.matrix[4, 7] = 8;

            // Les valeurs au centre n'ont pas beaucoup de valeurs
            this.matrix[3, 3] = this.matrix[3, 4] = this.matrix[4, 3] = this.matrix[4, 4] = -3;
            this.matrix[2, 3] = this.matrix[2, 4] = this.matrix[5, 3] = this.matrix[5, 4] = 2;
            this.matrix[3, 2] = this.matrix[3, 5] = this.matrix[4, 2] = this.matrix[4, 5] = 2;
            this.matrix[2, 2] = this.matrix[5, 2] = this.matrix[2, 5] = this.matrix[5, 5] = 2;

            // Les valeurs à côté des bords n'ont pas beaucoup de valeurs car elles donnent potentiellement accès aux bords.
            this.matrix[1, 2] = this.matrix[2, 1] = this.matrix[1, 5] = this.matrix[5, 1] = 1;
            this.matrix[6, 2] = this.matrix[2, 6] = this.matrix[6, 5] = this.matrix[5, 6] = 1;
            this.matrix[1, 3] = this.matrix[1, 4] = this.matrix[3, 1] = this.matrix[4, 1] = 1;
            this.matrix[3, 6] = this.matrix[4, 6] = this.matrix[6, 3] = this.matrix[6, 4] = 1;
        }

        public void setMiddleGameValues()
        {
            // Première et dernière colonne
            for (int j = 0; j <= 7; j++)
            {
                matrix[0,j] += 250;
                matrix[7,j] += 250;
            }

            // Première et dernière ligne
            for (int i = 1; i <= 6; i++)
            {
                matrix[i,0] += 250;
                matrix[i,7] += 250;
            }
        }

        public int getValue(int i, int j)
        {
            return this.matrix[i, j];
        }
    }
}
