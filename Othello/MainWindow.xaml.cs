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
        public MainWindow()
        {
            InitializeComponent();

            /*************************************
             * Création du gameboard 
             * **********************************/

            Gameboard gb = new Gameboard();
            //StateGame stateGame = new StateGame(gb.getBoard(), 0, 1, true);
            //stateGame.saveInFile("C:/tmp/save.txt");

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            bool hasPlayed = false;
            

            /*************************************
             * Création interface et affichage
             * **********************************/
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

            // Test des endroits jouables par 0 au début
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (gb.isPlayable(i, j, true))
                    {
                        Console.WriteLine("("+i+", "+j+")");
                    }
                }
            }
            gb.drawBoard();

            
        }
    }
}
