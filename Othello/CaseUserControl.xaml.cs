using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello
{
    /// <summary>
    /// Interaction logic for CaseUserControl.xaml
    /// </summary>
    public partial class CaseUserControl : UserControl
    {

        // Permet la création dynamique et la gestion graphique de la grille
        private int x;
        private int y;
        private bool empty;


        public CaseUserControl()
        {
            InitializeComponent();
        }


        // Constructeur avec définition des propriétés x, y et empty
        public CaseUserControl(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
            this.empty = true;
        }


        // Getter and setter
        public int X { get { return x; } }
        public int Y { get { return y; } }
        public bool Empty
        {
            get { return empty; }
            set { empty = value; }

        }
    }
}
