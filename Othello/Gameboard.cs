using System;
using System.Timers;

namespace Othello
{
    public class Gameboard : IPlayable
    {
        private Case[,] board;
        private int size;
        private int whiteScore, blackScore = 0;
        private Timer whiteTimer, blackTimer;
        public bool activePlayer = false;
        

        public Gameboard(int size = 8)
        {
            this.size = size;
            board = new Case[this.size, this.size];
            // On va remplir notre board
            for(int i = 0; i< this.size; i++)
            {
                for(int j = 0; j < this.size; j++)
                {
                    board[i, j] = new Othello.Case((char) (97 + i), j);
                }
            }
            board[3, 3].setState(1);
            board[4, 4].setState(1);
            board[3, 4].setState(0);
            board[4, 3].setState(0);

            // Les timers
            whiteTimer = new Timer(1000);
            blackTimer = new Timer(1000);

            whiteTimer.Elapsed += new ElapsedEventHandler(UpdatePlayerTime);
            blackTimer.Elapsed += new ElapsedEventHandler(UpdatePlayerTime);

            // On démarre le timer du premier joueur
            blackTimer.Start();
            
        }

        public static void UpdatePlayerTime(object source, ElapsedEventArgs e)
        {       
            Console.Write("\r{0}", DateTime.Now);
            

        }

    public bool isPlayable(int column, int line, bool isWhite)
        {
            // On vérifie déjà si la case est vide ou non
            if (board[line, column].getState() != -1)
                return false;


            for (int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                   if (checkThisDirection(column, line, i, j, isWhite))
                        return true;
                }
            }

            return false;
        }

        public bool checkThisDirection(int column, int line, int directionX, int directionY, bool isWhite)
        {
            int nb = 0;
            if (directionX == 0 && directionY == 0)
                return false;
            int c = column + directionY; int l = line + directionX;
            if (c > 7 || c < 0 || l > 7 || l < 0)
                return false;
            //Check first neighbour
            if (!(board[l, c].getState() == (isWhite ? 1 : 0)))
                return false;
            while ((board[l, c].getState() == (isWhite ? 1 : 0)))
            {
                c += directionY;
                l += directionX;
                nb += 1;
                if(c > 7 || l > 7 || c < 0 || l < 0)
                {
                    return false;
                }
            }
            if(board[l, c].getState() == -1 || nb == 0)
            {
                return false;
            }
            return true;
        }

        public bool playMove(int column, int line, bool isWhite)
        {

            Console.WriteLine("Point joué : (" + column + ", " + line + ")");
            // Est-ce que le coup peut être joué ? 
            if (!isPlayable(column, line, isWhite))
                return false;

            // On va tester pour toutes les directions
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int nb = 0;
                    
                    // Si différent que la case où on est
                    if (!(i == 0 && j == 0))
                    {
                        bool end = false; // Variable de fin.
                        int c = column + j; int l = line + i; // c & l sont les coordonnées du voisin.
                        if (c < 0 || c > 7 || l < 0 || l > 7)
                            continue;
                        // Tant qu'on a une valeur différente que la notre
                        while ((board[l, c].getState() == (isWhite ? 1 : 0)))
                        {
                            c += j; l += i; //On va aller au prochain voisin dans la direction.
                            nb += 1; 

                            // On test si le prochain voisin est en dehors du board.
                            if (c > 7 || l > 7 || c < 0 || l < 0)
                            {
                                end = true; // Si c'est le cas on arrête.
                                break; // On sort de la while
                            }
                        }

                        // Donc si on est pas sorti du board
                        if (!end)
                        {
                            // On teste si sa valeur est vide ou qu'on n'a pas eu de voisin directement possible.
                            if(board[l, c].getState() == -1 || nb == 0){
                                end = true;  // On arrête
                            }
                            if (!end)
                            {
                                // A ce moment-là, on a les coordonnées du point qui est de la même couleur que nous. (c, l)
                                // On va remplir entre le ce point là et le point de départ.
                                int incrementX = (line < l ? 1 : -1);
                                int incrementY = (column < c ? -1 : 1);

                                if(line - l != 0 && column - c != 0) // Si on a une diagonale (Changement dans les lignes et colonnes)
                                {
                                    //diagonale
                                    Console.WriteLine("Diagonale");
                                    int startIndex = (line < l ? line : l);
                                    int endIndex = (line < l ? l : line);
                                    int indexC = 0;
                                    int incrementC = 0;

                                    if (startIndex == line) // Si on part de line, on va partir de column dans l'indexC
                                    {
                                        incrementC = (column < c ? 1 : -1);
                                        indexC = column;
                                    }
                                    else if(startIndex == l) // Si on part de l, on va partir de c dans l'indexC
                                    {
                                        incrementC = (column < c ? -1 : 1);
                                        indexC = c;
                                    }
                                    for (int index = startIndex; index <= endIndex; index+=1, indexC+=incrementC)
                                    {
                                        board[index, indexC].setState((isWhite ? 0 : 1));
                                    }
                                }
                                else if (line - l != 0 && column - c == 0) // Si on a une colonne, il y a qu'un changement dans les lignes.
                                {
                                    //colonne
                                    Console.WriteLine("Colonne");
                                    int startIndex = (line < l ? line : l);
                                    int endIndex = (line < l ? l : line);
                                    for (int index = startIndex;  index <= endIndex ; index += 1)
                                    {
                                        board[index, column].setState((isWhite ? 0 : 1));
                                    }
                                    
                                }
                                else if(column - c != 0 && line - l == 0) // Si on a une ligne, il y a qu'un changement dans les colonnes.
                                {
                                    //ligne
                                    Console.WriteLine("Ligne");
                                    int startIndex = (column < c ? column : c);
                                    int endIndex = (column < c ? c : column);
                                    for (int index = startIndex; index <= endIndex; index += 1)
                                    {
                                        board[line, index].setState((isWhite ? 0 : 1));
                                    }
                                }
                            }
                            
                        }
                    }
                        
                }
            }
            return true;
        }

        public Tuple<char, int> getNextMove(int[,] game, int level, bool whiteTurn)
        {
            return new Tuple<char, int>('a', 1);
        }

        public int getWhiteScore()
        {
            return whiteScore;
        }

        public int getBlackScore()
        {
            return blackScore;
        }

        public Case[,] getBoard()
        {
            return this.board;
        }

        public bool getActivePlayer()
        {
            return this.activePlayer;
        }

        public void setActivePlayer(bool b)
        {
            this.activePlayer = b;
        }

        public void drawBoard()
        {
            int rowLength = board.GetLength(0);
            int colLength = board.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0} ", board[i, j].getState()));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.ReadLine();
        }

        public int nbPossibilities(bool player)
        {
            int nb = 0;
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (isPlayable(i, j, player))
                        nb++;
                }
            }
            return nb;
        }

        public int getScore(bool player)
        {
            int score = 0;
            int rowLength = board.GetLength(0);
            int colLength = board.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    if(board[i, j].getState() == (player ? 0 : 1))
                        score++;
                }
            }
            return score;
        }
    }
}
