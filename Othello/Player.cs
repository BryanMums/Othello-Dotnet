using System;

namespace Othello
{
    public class Player
    {
        private int playerID;
        private String name;

        public Player(int depth, int playerID, String name = "Player")
        {
            this.playerID = playerID;
            this.name = name;
        }
    }
}