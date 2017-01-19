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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("Bonjour : "+(char) 97);

            Gameboard gb = new Gameboard();

            gb.drawBoard();

            if (gb.isPlayable(3, 2, true))
            {
                Console.WriteLine("OUIOUI");
            }

            // Test des endroits jouables par 0 au début
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (gb.isPlayable(i, j, false))
                    {
                        Console.WriteLine("("+i+", "+j+")");
                    }
                }
            }

            // Les 2 permettent de tester l'ajout en colonne dans les 2 sens.
            //gb.playMove(3, 2, true);
            //gb.playMove(4, 5, true);

            // Les 2 permettent de tester l'ajout en ligne dans les 2 sens.
            //gb.playMove(2, 3, true);
            //gb.playMove(5, 4, true);

            //Les 2 permettent de tester l'ajout en colonne dans les 2 sens.
            //gb.playMove(5, 2, true);
            //gb.playMove(2, 5, true);
            //gb.playMove(5, 5, false);
            //gb.playMove(2, 2, false);

            gb.drawBoard();

            

            CheckerBoard.Rows = 8;
            CheckerBoard.Columns = 8;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Rectangle rectVert = new Rectangle();
                    rectVert.Fill = Brushes.ForestGreen;
                    Rectangle rectDark = new Rectangle();
                    rectDark.Fill = Brushes.DarkGreen;
                    if (i % 2 == 0)
                    {
                        CheckerBoard.Children.Add(rectDark);
                        CheckerBoard.Children.Add(rectVert);
                    }
                    else
                    {
                        CheckerBoard.Children.Add(rectVert);
                        CheckerBoard.Children.Add(rectDark);
                    }
                }

            }
        }
    }
}
