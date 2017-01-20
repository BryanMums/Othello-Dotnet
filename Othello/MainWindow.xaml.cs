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

        Gameboard gb;

        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("Bonjour : "+(char) 97);
            
            this.gb = new Gameboard();

            this.gb.drawBoard();

            if (this.gb.isPlayable(3, 2, true))
            {
                Console.WriteLine("OUIOUI");
            }

            // Test des endroits jouables par 0 au début
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (this.gb.isPlayable(i, j, false))
                    {
                        Console.WriteLine("("+i+", "+j+")");
                    }
                }
            }



            this.gb.drawBoard();

            // Initialisation de la grille
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    CaseUserControl control = new CaseUserControl(i, j);

                    // Enregistrement des événements
                    control.MouseLeftButtonUp += MouseLeftButtonUpCase;
                    control.MouseLeave += MouseLeaveCase;
                    control.MouseEnter += MouseEnterCase;

                    control.pawnBorder.Background = (i % 2 == 0) ? (j % 2 == 0) ? Brushes.ForestGreen : Brushes.DarkGreen : (j % 2 == 0) ? Brushes.DarkGreen : Brushes.ForestGreen;

                    Grid.SetColumn(control, i);
                    Grid.SetRow(control, j);

                    BoardGrid.Children.Add(control);



                }
            }

            

            

        }

        private void MouseLeftButtonUpCase(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("CLICKED");
            CaseUserControl control = (CaseUserControl) sender;
            //CaseUserControl.empty = false;
            // TESTER QUI JOUE POUR COULEUR if(this.gb.isPlayable(control.X, control.X, ))
            // PLAYMOVE
            control.pawnImage.Source = new BitmapImage(new Uri(@"img/white.png", UriKind.Relative));
        }

        private void MouseEnterCase(object sender, MouseEventArgs e)
        {
            Console.WriteLine("ENTER");
        }

        private void MouseLeaveCase(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Leave");
        }




    }
}
