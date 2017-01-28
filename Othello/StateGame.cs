using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class StateGame
    {
        private int[,] board;
        private int timeBlack;
        private int timeWhite;
        private bool activePlayer;

        public int[,] Board
        {
            get
            {
                return board;
            }

            set
            {
                board = value;
            }
        }

        public int TimeBlack
        {
            get
            {
                return timeBlack;
            }

            set
            {
                timeBlack = value;
            }
        }

        public int TimeWhite
        {
            get
            {
                return timeWhite;
            }

            set
            {
                timeWhite = value;
            }
        }

        public bool ActivePlayer
        {
            get
            {
                return activePlayer;
            }

            set
            {
                activePlayer = value;
            }
        }
        
        public StateGame()
        {
            this.board = new int[8,8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    this.board[i, j] = -1;
                }
            }
            this.TimeBlack = 0;
            this.TimeWhite = 0;
            this.ActivePlayer = false;
        }
        public StateGame(Case[,] _board, int timeBlack, int timeWhite, bool activePlayer)
        {
            this.board = new int[_board.GetLength(0), _board.GetLength(1)];
            for(int i = 0; i < _board.GetLength(0); i++)
            {
                for(int j = 0; j < _board.GetLength(1); j++)
                {
                    this.board[i, j] = _board[i,j].getState();
                }
            }
            this.TimeBlack = timeBlack;
            this.TimeWhite = timeWhite;
            this.ActivePlayer = activePlayer;
        }

        public StateGame(int[,] _board, int timeBlack, int timeWhite, bool activePlayer)
        {
            this.board = new int[_board.GetLength(0), _board.GetLength(1)];
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    this.board[i, j] = _board[i, j];
                }
            }
            this.TimeBlack = timeBlack;
            this.TimeWhite = timeWhite;
            this.ActivePlayer = activePlayer;
        }

        public Case[,] getCaseBoard()
        {
            Case[,] board = new Case[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = new Othello.Case((char)(97 + i), j);
                    board[i, j].setState(this.board[i, j]);
                }
            }
            return board;
        }

        public string getJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
