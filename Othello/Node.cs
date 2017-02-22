using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class Node
    {

        public int evaluation { get; set; }
        public int[] position { get; set; }

        private List<Node> children = new List<Node>();


        private static int counter = 0;


        public Node(int[] position)
        {
            this.position = position;
            this.evaluation = 0;
        }

        public void addChild(Node child)
        {
            this.children.Add(child);
        }

        public Boolean isLeaf()
        {
            return this.children.Count == 0;
        }

        

    }
}
