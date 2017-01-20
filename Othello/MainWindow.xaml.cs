using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Newtonsoft.Json;
using System.IO;

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
            /*************************************
             * Création du gameboard 
             * **********************************/

            //StateGame stateGame = new StateGame(gb.getBoard(), 0, 1, true);
            //stateGame.saveInFile("C:/tmp/save.txt");

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            bool hasPlayed = false;
            

            /*************************************
             * Création interface et affichage
             * **********************************/
            
            this.gb = new Gameboard();

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
            /*************************************
             * Joueur actif = noir
             * **********************************/
            bool activePlayer = false;


            /*************************************
             * Boucle principale
             * **********************************/
            while (gb.nbPossibilities(activePlayer) + gb.nbPossibilities(!activePlayer) > 0)
            {
                Console.WriteLine("C'est le tour de : " + activePlayer);
                gb.drawBoard();
                /********_Start Timer_************/
                stopWatch.Start();
                if(!activePlayer)
                    gb.playMove(5, 3, activePlayer);
                if (activePlayer)
                    gb.playMove(5, 4, activePlayer);
                //while (hasPlayed == false) ;
                Thread.Sleep(3000);
                // Va passer ça quand il aura cliqué
                hasPlayed = false;

                /*********_Arrêt du temps_********/
                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);

                Console.WriteLine("Score blanc : " + gb.getScore(true));
                Console.WriteLine("Score noir : " + gb.getScore(false));
                activePlayer = !activePlayer;


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
