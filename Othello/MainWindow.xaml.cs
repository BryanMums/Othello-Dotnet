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
using Microsoft.Win32;
using System.Timers;
using System.Windows.Threading;
using System.Media;

namespace Othello
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Gameboard gb;
        private DispatcherTimer updateTimer;


        public MainWindow()
        {
            InitializeComponent();
            SoundPlayer player = new SoundPlayer("../../sounds/music.wav");
            player.Load();
            player.PlayLooping();
            this.gb = new Gameboard();
            MAJ();

            updateTimer = new DispatcherTimer(DispatcherPriority.SystemIdle);
            updateTimer.Tick += new EventHandler(OnUpdateTimerTick);
            updateTimer.Interval = TimeSpan.FromMilliseconds(1000);
            updateTimer.Start();

        }

        private void OnUpdateTimerTick(object sender, EventArgs e)
        {
            if (!this.gb.activePlayer)
            {
                this.gb.blackTime += 1;
            }else
            {
                this.gb.whiteTime += 1;
            }
            MAJDisplayTime();
        }

        private void MouseLeftButtonUpCase(object sender, MouseButtonEventArgs e)
        {
            CaseUserControl control = (CaseUserControl) sender;
            if(this.gb.PlayMove(control.X, control.Y, gb.activePlayer))
            {
                control.pawnImage.Source = new BitmapImage(new Uri(@"img/black.png", UriKind.Relative));
                gb.activePlayer = !gb.activePlayer; 
                
                Console.WriteLine("Le prochain coup the best ever il oe you bryan :" + gb.GetNextMove(this.gb.GetBoard(),  5, gb.activePlayer));
                MAJ();
                checkVictoryOrSkippingTurn();
            }
        }

        private void MouseEnterCase(object sender, MouseEventArgs e)
        {
            CaseUserControl control = (CaseUserControl)sender;
            if(this.gb.IsPlayable(control.X, control.Y, gb.activePlayer))
            {
                if (gb.activePlayer)
                    control.pawnImage.Source = new BitmapImage(new Uri(@"img/white.png", UriKind.Relative));
                if (!gb.activePlayer)
                    control.pawnImage.Source = new BitmapImage(new Uri(@"img/black.png", UriKind.Relative));
            }
        }

        private void MouseLeaveCase(object sender, MouseEventArgs e)
        {
            CaseUserControl control = (CaseUserControl)sender;
            if(control.Empty == true)
            {
                if (this.gb.IsPlayable(control.X, control.Y, gb.activePlayer))
                {
                    if (gb.activePlayer)
                        control.pawnImage.Source = new BitmapImage(new Uri(@"img/white_hover.png", UriKind.Relative));
                    if (!gb.activePlayer)
                        control.pawnImage.Source = new BitmapImage(new Uri(@"img/black_hover.png", UriKind.Relative));
                }else
                {
                    control.pawnImage.Source = new BitmapImage(new Uri(@"img/trans.png", UriKind.Relative));
                }
            }
        }

        public void MAJ()
        {
            Brush borderColor = Brushes.Gray;
            if(this.gb.GetBlackScore() > this.gb.GetWhiteScore())
            {
                borderColor = Brushes.Black;
            }else if(this.gb.GetBlackScore() < this.gb.GetWhiteScore())
            {
                borderColor = Brushes.White;
            }
            // Initialisation de la grille
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    CaseUserControl control = new CaseUserControl(j, i);

                    // Enregistrement des événements
                    control.MouseLeftButtonUp += MouseLeftButtonUpCase;
                    control.MouseLeave += MouseLeaveCase;
                    control.MouseEnter += MouseEnterCase;

                    control.pawnBorder.Background = (j % 2 == 0) ? (i % 2 == 0) ? Brushes.ForestGreen : Brushes.DarkGreen : (i % 2 == 0) ? Brushes.DarkGreen : Brushes.ForestGreen;
                    control.pawnBorder.BorderBrush = borderColor;

                    Grid.SetColumn(control, j);
                    Grid.SetRow(control, i);
                    if (this.gb.getBoard()[j, i].getState() == 0)
                    {
                        control.pawnImage.Source = new BitmapImage(new Uri(@"img/white.png", UriKind.Relative));
                        control.Empty = false;
                    }
                    else if (this.gb.getBoard()[j, i].getState() == 1)
                    {
                        control.pawnImage.Source = new BitmapImage(new Uri(@"img/black.png", UriKind.Relative));
                        control.Empty = false;
                    }
                    if(this.gb.IsPlayable(j, i, gb.activePlayer))
                    {
                        if (gb.activePlayer)
                            control.pawnImage.Source = new BitmapImage(new Uri(@"img/white_hover.png", UriKind.Relative));
                        if (!gb.activePlayer)
                            control.pawnImage.Source = new BitmapImage(new Uri(@"img/black_hover.png", UriKind.Relative));
                    }
                    BoardGrid.Children.Add(control);

                }
            }


            // Mise à jour des scores
            scoreLabelPlayer1.Content = this.gb.GetBlackScore();
            scoreLabelPlayer2.Content = this.gb.GetWhiteScore();



            if (!gb.activePlayer)
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
            //Tester si le nombre de possibilités totals = 0
            // Si oui -> Fin du match.
            if(gb.nbPossibilities(true) + gb.nbPossibilities(false) == 0)
            {
                endGame();
            }
            //Tester si le nombre de possibilités du joueur = 0.
            //Si oui, on change de joueur.
            else if(gb.nbPossibilities(gb.activePlayer) == 0)
            {
                MessageBoxResult result = MessageBox.Show("Le joueur " + (gb.activePlayer ? "blanc" : "noir") + " ne peut pas jouer !", "Confirmation");
                gb.activePlayer = !gb.activePlayer;
                MAJ();
            }
        }

        public void endGame()
        {
            StatusLabel.Content = "Fin de partie !";
            String text;
            if (gb.GetBlackScore() > gb.GetWhiteScore())
            {
                text = "Joueur noir a gagné !";
            }else if(gb.GetBlackScore() < gb.GetWhiteScore())
            {
                text = "Joueur blanc a gagné !";
            }else
            {
                text = "Egalité !";
                // On va tester celui qui a utilisé le moins de temps
                if (this.gb.blackTime < this.gb.whiteTime)
                {
                    text += " Le joueur noir gagne car il a été plus rapide !";
                }else if(this.gb.blackTime > this.gb.whiteTime)
                {
                    text += " Le joueur blanc gagne car il a été plus rapide !";
                }
            }
            MessageBox.Show(text, "Confirmation");
            string message = "Voulez-vous rejouer ?";
            string caption = "Fin de partie";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
            {
                startNewGame();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        public void MAJDisplayTime()
        {
            timeLabelPlayer1.Content = string.Format("{0:00} : {1:00}", this.gb.blackTime / 60, this.gb.blackTime % 60);
            timeLabelPlayer2.Content = string.Format("{0:00} : {1:00}", this.gb.whiteTime / 60, this.gb.whiteTime % 60);
        }

        public void startNewGame()
        {

            // On crée une nouvelle partie donc board remis à l'état initial
            this.gb = new Gameboard();
            // Mettre à jour l'affichage du temps
            MAJDisplayTime();
            // Mettre à jour le reste de l'affichage
            MAJ();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "sauvegardOthello.json";
            saveFileDialog.DefaultExt = "json";
            saveFileDialog.Filter = "json files (*.json)|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                // Créer l'état du jeu
                StateGame sg = new StateGame(gb.getBoard(), this.gb.blackTime, this.gb.whiteTime, this.gb.activePlayer);
                // Sauvegarder dans le fichier
                File.WriteAllText(saveFileDialog.FileName, sg.getJson());
            }
        }

        private void btnload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "json files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                String state = File.ReadAllText(openFileDialog.FileName);
                StateGame st = JsonConvert.DeserializeObject<StateGame>(state);
                // Mise à jour du board.
                gb.setBoard(st.getCaseBoard());
                // Mise à jour du joueur actif
                this.gb.activePlayer = st.ActivePlayer;
                // Mise à jour des temps
                this.gb.blackTime = st.TimeBlack;
                this.gb.whiteTime = st.TimeWhite;
                // Mise à jour des scores
                gb.majScores();
                // Mise à jour de l'affichage
                MAJ();
                // Mise à jour de l'affichage du temps
                MAJDisplayTime();
            
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
