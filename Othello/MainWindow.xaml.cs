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
