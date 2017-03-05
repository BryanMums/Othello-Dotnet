using System;

using System.Collections.Generic;
using System.Timers;

namespace Othello
{
    public class GameBoard : IPlayable.IPlayable, ICloneable
    {
        private Case[,] board;
        private int size;
        private int whiteScore, blackScore = 0;
        public int whiteTime, blackTime;
        private int gameOpeningstate, gameEndState;
        
        private Boolean noAvailableMove { get; set; }

        public bool activePlayer = false;


        public GameBoard()
        {
            int size = 8;

            this.size = size;
            board = new Case[this.size, this.size];
            // On va remplir notre board
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    board[i, j] = new Othello.Case(i, j);
                }
            }
            // Initialisation des cases de départ
            board[3, 3].setState(0);
            board[4, 4].setState(0);
            board[3, 4].setState(1);
            board[4, 3].setState(1);
            // Mise à jour des scores
            majScores();
        }

        public GameBoard(GameBoard b)
        {
            this.size = b.size;
            board = b.board;
            majScores();
        }


        public Object Clone() {
            return new GameBoard(this);
        }
        

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            // On vérifie déjà si la case est vide ou non
            if (board[column, line].getState() != -1)
                return false;


            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
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
            if (!(board[c, l].getState() == (isWhite ? 1 : 0)))
                return false;
            while ((board[c, l].getState() == (isWhite ? 1 : 0)))
            {
                c += directionY;
                l += directionX;
                nb += 1;
                if (c > 7 || l > 7 || c < 0 || l < 0)
                {
                    return false;
                }
            }
            if (board[c, l].getState() == -1 || nb == 0)
            {
                return false;
            }
            return true;
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            // Est-ce que le coup peut être joué ? 
            if (!IsPlayable(column, line, isWhite))
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
                        while ((board[c, l].getState() == (isWhite ? 1 : 0)))
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
                            if (board[c, l].getState() == -1 || nb == 0)
                            {
                                end = true;  // On arrête
                            }
                            if (!end)
                            {
                                // A ce moment-là, on a les coordonnées du point qui est de la même couleur que nous. (c, l)
                                // On va remplir entre le ce point là et le point de départ.
                                int incrementX = (line < l ? 1 : -1);
                                int incrementY = (column < c ? -1 : 1);

                                if (line - l != 0 && column - c != 0) // Si on a une diagonale (Changement dans les lignes et colonnes)
                                {
                                    //diagonale
                                    int startIndex = (line < l ? line : l);
                                    int endIndex = (line < l ? l : line);
                                    int indexC = 0;
                                    int incrementC = 0;

                                    if (startIndex == line) // Si on part de line, on va partir de column dans l'indexC
                                    {
                                        incrementC = (column < c ? 1 : -1);
                                        indexC = column;
                                    }
                                    else if (startIndex == l) // Si on part de l, on va partir de c dans l'indexC
                                    {
                                        incrementC = (column < c ? -1 : 1);
                                        indexC = c;
                                    }
                                    for (int index = startIndex; index <= endIndex; index += 1, indexC += incrementC)
                                    {
                                        board[indexC, index].setState((isWhite ? 0 : 1));
                                    }
                                }
                                else if (line - l != 0 && column - c == 0) // Si on a une colonne, il y a qu'un changement dans les lignes.
                                {
                                    //colonne
                                    int startIndex = (line < l ? line : l);
                                    int endIndex = (line < l ? l : line);
                                    for (int index = startIndex; index <= endIndex; index += 1)
                                    {
                                        board[column, index].setState((isWhite ? 0 : 1));
                                    }

                                }
                                else if (column - c != 0 && line - l == 0) // Si on a une ligne, il y a qu'un changement dans les colonnes.
                                {
                                    //ligne
                                    int startIndex = (column < c ? column : c);
                                    int endIndex = (column < c ? c : column);
                                    for (int index = startIndex; index <= endIndex; index += 1)
                                    {
                                        board[index, line].setState((isWhite ? 0 : 1));
                                    }
                                }
                            }

                        }
                    }

                }
            }
            majScores();
            return true;
        }

        public void majScores()
        {
            // Mis à jour les scores.
            whiteScore = 0;
            blackScore = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j].getState() == 0)
                    {
                        whiteScore++;
                    }
                    else if (board[i, j].getState() == 1)
                    {
                        blackScore++;
                    }
                }
            }
        }


        // Liste des coups disponibles
        private List<Case> getAvailableMoves(Boolean player)
        {

            List<Case> availableMoves = new List<Case>();

            foreach(Case c in this.board){
                if (this.IsPlayable(c.column, c.row, player)){
                    availableMoves.Add(c);
                }
                
            }

            return availableMoves;
        }

        // Permet d'obtenir le meilleur coup selon notre IA
        public Tuple<int, int> GetNextMove(int[,] game, int depth, bool whiteTurn)
        {
            this.gameOpeningstate = 12; 
            this.gameEndState = 60 -  depth;
            this.activePlayer = whiteTurn;
            GameBoard l_gameboard = new GameBoard();

            // On va remplir le board de notre nouveau gameboard par rapport à nos valeurs.
            for (int i = 0; i < game.GetLength(0); i++) {
                for (int j = 0; j < game.GetLength(1); j++)
                {
                    l_gameboard.board[j, i].setState(game[j, i]);
                }
            }
            int[] result = alphabeta(l_gameboard, new Node(new int[] {0,0}), depth, 1, Double.NegativeInfinity);
            
            // Si on n'a pas de résultat possible.
            if (result[1] == -1)
            {
                // On retourne (-1,-1) pour spécifier qu'il n'y pas de coordonnées possibles.
                return new Tuple<int, int>(-1, -1);
            }
            // On retourne notre coordonnées 
            return new Tuple<int, int>(result[1], result[2]);
        }


        // Algo alphabeta 
        private int[] alphabeta(GameBoard board, Node root, int depth, int minormax, double parentValue)
        {
            
            // Si minormax = 1 -> Maximisation et si minormax = 0 -> Minimisation

            // Le joueur à ce niveau
            Boolean l_player = minormax == 1 ? this.activePlayer : !this.activePlayer;

            // Les coups jouables
            List<Case> possibleMoves = this.getAvailableMoves(l_player);

            // Evaluation
            int score = eval(root, possibleMoves.Count, l_player, board);
            // Attribue son évalutation au  noeud racine
            root.evaluation = score;

            // En cas de situation de fin
            if(depth==0 || possibleMoves.Count == 0)
            {
                return new int[] {root.evaluation, -1, -1}; 
            }


            this.noAvailableMove = false;

            // Valeurs par défaut pour la première itération
            int bestValue = (int)(minormax * Double.NegativeInfinity);
            int[] bestMove = { -1,-1};


            foreach(Case c in possibleMoves)
            {
                // On va faire un clone de notre gameboard
                GameBoard l_gameboard = (GameBoard)board.Clone();

                // On crée notre nouvelle élément Node selon les coordonnées du coup possible
                Node newElem = new Node(new int[] { c.column, c.row });

                // On l'ajoute à notre root
                root.addChild(newElem);

                // On joue le coup.
                l_gameboard.PlayMove(c.column, c.row, l_player);

                // On calcule l'alphabeta pour la prochaine couche
                int[] val = alphabeta(l_gameboard, newElem, depth - 1, -minormax, bestValue);

                // On regarde si la valeur retournée est meilleure que celle que l'on a déjà.
                if (val[0] * minormax > bestValue * minormax)
                 {
                     // On change nos valeurs car on en a des meilleurs
                     bestValue = val[0];
                     bestMove = new int[] { c.column, c.row };

                    // On regarde si on peut arrêter de chercher dans les fils du noeud courant 
                     if (bestValue * minormax > parentValue * minormax)
                     {
                         break;
                     }
                 }
                 
            }
            // On retourne un tableau de int avec le meilleur résultat et les coordonnées.
            return new int[]{ bestValue, bestMove[0], bestMove[1]};
        }

        // Permet de récupérer le nombre de coins d'un joueur.
        public int getCornerPoints(Boolean playerId)
        {

            int n = 0;

            int player = (playerId == true) ? 1 : 0;

            if(this.board[0, 0].getState() == player)
            {
                n++;
            }
            if(this.board[this.size-1, 0].getState() == player)
            {
                n++;
            }
            if(this.board[0, this.size-1].getState() == player)
            {
                n++;
            }
            if(this.board[this.size-1, this.size-1].getState() == player)
            {
                n++;
            }

            return n;

        }

        // Méthode d'évalutation
        private int eval(Node node, int positions, Boolean playerId, GameBoard board)
        {
            
            int l_turn = board.getScore(playerId) + board.getScore(!playerId) - 4; // Nombre de tours (Nb de pions totaux - 4, ceux de départ)


            Matrix evalMatrix = new Matrix(); // On crée notre matrix

            // Permet d'obtenir la différence de mouvements possibles entre les 2 joueurs
            int diffAvailableMoves = board.getAvailableMoves(playerId).Count - board.getAvailableMoves(!playerId).Count;
            // Permet d'obtenir la différence de score entre les 2 joueurs
            int diffScore = board.getScore(playerId) - board.getScore(!playerId);
            // Permet d'obtenir la différence de pièces dans les coins entre les 2 joueurs
            int diffCornerCoins = board.getCornerPoints(playerId) - board.getCornerPoints(!playerId);

            // Si on est au début de la partie
            if (l_turn <= gameOpeningstate)
            {
                // Au début de la partie, on va donner plus d'importance aux mouvements possibles pour avoir un plus vaste choix.
                return (int)2 * diffAvailableMoves + diffScore + evalMatrix.getValue(node.position[0], node.position[1]);
            }
            // Si on est au milieu de la partie
            else if (l_turn > this.gameOpeningstate && l_turn < this.gameEndState)
            {
                // En milieu de partie, on va donner plus d'importance aux positions sur les bords ou dans les coins.
                evalMatrix.setMiddleGameValues();
                return (int)0.5 * diffAvailableMoves + diffScore + 3 * diffCornerCoins + 6 * evalMatrix.getValue(node.position[0], node.position[1]);
            }
            // Si on est à la fin de la partie
            else
            {
                // En fin de partie, on va surtout choisir par rapport aux nombre de pièces.
                return (int)0.1 * diffAvailableMoves + 3 * diffScore + 3 * evalMatrix.getValue(node.position[0], node.position[1]);
            }
        }

        public int[,] GetBoard()
        {
            int[,] gboard = new int[8, 8];
            foreach(Case c in this.board)
            {
                gboard[c.column, c.row] = c.getState();
            }

            return gboard;
        }

        public string GetName()
        {
            return "Groell_Ducommun_Muhmenthaler";
        }

        public int GetWhiteScore()
        {
            return whiteScore;
        }

        public bool getActivePlayer()
        {
            return this.activePlayer;
        }

        public void setActivePlayer(bool b)
        {
            this.activePlayer = b;
        }

        public int GetBlackScore()
        {
            return blackScore;
        }

        public int getWhiteTime()
        {
            return this.whiteTime;
        }

        public void setWhiteTime(int t)
        {
            this.whiteTime = t;
        }

        public int getBlackTime()
        {
            return this.blackTime;
        }

        public void setBlackTime(int t)
        {
            this.blackTime =  t;
        }

        public Case[,] getBoard()
        {
            return this.board;
        }

        public void setBoard(Case[,] board)
        {
            this.board = board;
        }



        public int nbPossibilities(bool player)
        {
            int nb = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (IsPlayable(i, j, player))
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
                    if (board[i, j].getState() == (player ? 0 : 1))
                        score++;
                }
            }
            return score;
        }
    }
}