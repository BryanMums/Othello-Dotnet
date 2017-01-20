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

        public void saveInFile(String path)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            System.IO.File.WriteAllText(@""+path, json);

        }

        public StateGame loadFromFile(String path)
        {
            using (StreamReader r = new StreamReader(@""+path))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<StateGame>(json);
            }
        }
        



    }
}
