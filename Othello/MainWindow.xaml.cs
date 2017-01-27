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
        bool activePlayer = false;

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

            // Initialisation de la grille
            MAJ();
            /*************************************
             * Joueur actif = noir
             * **********************************/
            bool activePlayer = false;


            /*************************************
             * Boucle principale
             * **********************************/
            /*while (gb.nbPossibilities(activePlayer) + gb.nbPossibilities(!activePlayer) > 0)
            {
                Console.WriteLine("C'est le tour de : " + activePlayer);
                gb.drawBoard();
                /********_Start Timer_************
                stopWatch.Start();
                if(!activePlayer)
                    gb.playMove(5, 3, activePlayer);
                if (activePlayer)
                    gb.playMove(5, 4, activePlayer);
                //while (hasPlayed == false) ;
                Thread.Sleep(3000);
                // Va passer ça quand il aura cliqué
                hasPlayed = false;

                /*********_Arrêt du temps_********
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


            }*/
        }

        private void MouseLeftButtonUpCase(object sender, MouseButtonEventArgs e)
        {
            CaseUserControl control = (CaseUserControl) sender;
            if(this.gb.playMove(control.Y, control.X, this.activePlayer))
            {
                control.pawnImage.Source = new BitmapImage(new Uri(@"img/black.png", UriKind.Relative));
                Console.WriteLine("MAJ");
                this.activePlayer = !this.activePlayer;
                MAJ();
                checkVictoryOrSkippingTurn();
            }
        }

        private void MouseEnterCase(object sender, MouseEventArgs e)
        {
            CaseUserControl control = (CaseUserControl)sender;
            if(this.gb.isPlayable(control.Y, control.X, this.activePlayer))
            {
                if (this.activePlayer)
                    control.pawnImage.Source = new BitmapImage(new Uri(@"img/white.png", UriKind.Relative));
                if (!this.activePlayer)
                    control.pawnImage.Source = new BitmapImage(new Uri(@"img/black.png", UriKind.Relative));
            }
        }

        private void MouseLeaveCase(object sender, MouseEventArgs e)
        {
            CaseUserControl control = (CaseUserControl)sender;
            if(control.Empty == true)
            {
                if (this.gb.isPlayable(control.Y, control.X, activePlayer))
                {
                    if (this.activePlayer)
                        control.pawnImage.Source = new BitmapImage(new Uri(@"img/white_hover.png", UriKind.Relative));
                    if (!this.activePlayer)
                        control.pawnImage.Source = new BitmapImage(new Uri(@"img/black_hover.png", UriKind.Relative));
                }else
                {
                    control.pawnImage.Source = new BitmapImage(new Uri(@"img/trans.png", UriKind.Relative));
                }
            }
        }

        public void MAJ()
        {
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
                    if (this.gb.getBoard()[i, j].getState() == 0)
                    {
                        control.pawnImage.Source = new BitmapImage(new Uri(@"img/white.png", UriKind.Relative));
                        control.Empty = false;
                    }
                    else if (this.gb.getBoard()[i, j].getState() == 1)
                    {
                        control.pawnImage.Source = new BitmapImage(new Uri(@"img/black.png", UriKind.Relative));
                        control.Empty = false;
                    }
                    if(this.gb.isPlayable(j, i, activePlayer))
                    {
                        if (this.activePlayer)
                            control.pawnImage.Source = new BitmapImage(new Uri(@"img/white_hover.png", UriKind.Relative));
                        if (!this.activePlayer)
                            control.pawnImage.Source = new BitmapImage(new Uri(@"img/black_hover.png", UriKind.Relative));
                    }
                    BoardGrid.Children.Add(control);

                }
            }
            if (!activePlayer)
            {
                StatusLabel.Content = "Au tour du joueur noir !";
            }
            else
            {
                StatusLabel.Content = "Au tour du joueur blanc !";
            }
        }

        //Ne fonctionne pas encore.
        public void checkVictoryOrSkippingTurn()
        {
            //Tester si le nombre de possibilités totals = 0
            // Si oui -> Fin du match.
            if(gb.nbPossibilities(true) + gb.nbPossibilities(false) == 0)
            {
                StatusLabel.Content = "Fin du match.";
            }
            //Tester si le nombre de possibilités du joueur = 0.
            //Si oui, on change de joueur.
            if(gb.nbPossibilities(this.activePlayer) == 0)
            {
                StatusLabel.Content = "Le joueur n'a pas de possibilités";
                System.Threading.Thread.Sleep(3000);
                this.activePlayer = !this.activePlayer;
                MAJ();
            }
        }

    }
}
