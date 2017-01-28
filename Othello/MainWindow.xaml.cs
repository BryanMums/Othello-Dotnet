﻿using System;
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
using Microsoft.Win32;

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


            // Mise à jour des scores
            scoreLabelPlayer1.Content = this.gb.getBlackScore();
            scoreLabelPlayer2.Content = this.gb.getWhiteScore();



            if (!activePlayer)
            {
                //StatusLabel.Content = "Au tour du joueur noir !";
                activePlayerImage.Source = new BitmapImage(new Uri(@"img/black.png", UriKind.Relative));
                
            }
            else
            {
                //StatusLabel.Content = "Au tour du joueur blanc !";
                activePlayerImage.Source = new BitmapImage(new Uri(@"img/white.png", UriKind.Relative));
            }
        }

        //Ne fonctionne pas encore.
        public void checkVictoryOrSkippingTurn()
        {
            Console.WriteLine("Moi : "+gb.nbPossibilities(this.activePlayer));
            Console.WriteLine("Lui : "+gb.nbPossibilities(!this.activePlayer));
            //Tester si le nombre de possibilités totals = 0
            // Si oui -> Fin du match.
            if(gb.nbPossibilities(true) + gb.nbPossibilities(false) == 0)
            {
                endGame();
            }
            //Tester si le nombre de possibilités du joueur = 0.
            //Si oui, on change de joueur.
            else if(gb.nbPossibilities(this.activePlayer) == 0)
            {
                MessageBoxResult result = MessageBox.Show("Le joueur " + (activePlayer ? "blanc" : "noir") + " ne peut pas jouer !", "Confirmation");
                Console.WriteLine("NE PEUT PAS JOUER");
                this.activePlayer = !this.activePlayer;
                MAJ();
            }
        }

        public void endGame()
        {
            Console.WriteLine("Fin de la partie");
            StatusLabel.Content = "Fin de partie !";
            String text;
            if (gb.getBlackScore() > gb.getWhiteScore())
            {
                text = "Joueur noir a gagné !";
                Console.WriteLine("Le joueur noir a gagné");
            }else if(gb.getBlackScore() < gb.getWhiteScore())
            {
                text = "Joueur blanc a gagné !";
                Console.WriteLine("Le joueur blanc a gagné");
            }else
            {
                text = "Egalité !";
                Console.WriteLine("Egalité !");
            }
            MessageBox.Show(text, "Confirmation");
            string message = "Voulez-vous rejouer ?";
            string caption = "Fin de partie";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
            {
                Console.WriteLine("Nouvelle partie !");
                startNewGame();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        public void startNewGame()
        {
            // Remettre à 0 le plateau
            // Remettre à 0 les temps et scores
            // Remettre le joueur actif le joueur NOIR = false
            this.gb = new Gameboard();
            activePlayer = false;
            MAJ();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                // Créer l'état du jeu
                StateGame sg = new StateGame(gb.getBoard(), 0, 1, true);
                File.WriteAllText(saveFileDialog.FileName, sg.getJson());
            }
        }

        private void btnload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                String state = File.ReadAllText(openFileDialog.FileName);
                StateGame st = JsonConvert.DeserializeObject<StateGame>(state);
                // Mise à jour du board.
                gb.setBoard(st.getCaseBoard());
                // Mise à jour du joueur
                this.activePlayer = st.ActivePlayer;
                gb.majScores();
                MAJ();
                Console.WriteLine(state);

            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            startNewGame();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
