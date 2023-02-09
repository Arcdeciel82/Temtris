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
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;





namespace Temtris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private bool[,] blocks = new bool[25, 15];
        private int test_gg = 0;
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
            Rectangle temp = new Rectangle();
            Grid.SetRow(temp, 0);
            Grid.SetColumn(temp, 4);
            temp.Width = 54;
            temp.Height = 54;
            temp.Fill = brush;
            temp.StrokeThickness = 5;
            temp.Stroke = brush;
            if (test_gg == 0)
            {
                createGameBoard();
                test_gg++;
            }
            GameGrid.Children.Add(temp);
            Movement(temp);
        }
        private Grid GameGrid;
        private void createGameBoard()
        {
            GameGrid = new Grid();
            GameGrid.ShowGridLines = true;
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(54, GridUnitType.Pixel);
            for (int i = 1; i <= 20; i++)
            {
                GameGrid.RowDefinitions.Add(row);
                row = new RowDefinition();
                row.Height = new GridLength(54, GridUnitType.Pixel);
            };
            ColumnDefinition col = new ColumnDefinition();
            col.Width = new GridLength(54, GridUnitType.Pixel);
            for(int i = 1; i <= 10; i++)
            {
                GameGrid.ColumnDefinitions.Add(col);
                col = new ColumnDefinition();
                col.Width = new GridLength(54, GridUnitType.Pixel);
            }

            Grid.SetRow(GameGrid, 0);
            Grid.SetColumn(GameGrid, 1);
            Grid.SetRowSpan(GameGrid, 20);
            Grid.SetColumnSpan(GameGrid, 10);
            MainMenu_Grid.Children.Add(GameGrid);
        }   
        private void Movement(Rectangle test)
        {
            
            if (Checker(test) == false)
            {
                var backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += (s, v) => {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    while (sw.Elapsed.Milliseconds <= 500) { };
                    sw.Stop(); 
                };
                backgroundWorker.RunWorkerCompleted += (s, v) =>
                {
                    Grid.SetRow(test, Grid.GetRow(test) + 1);
                    if (Grid.GetRow(test) < 18)
                    {
                        if (Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.D))
                        {
                            if (Keyboard.IsKeyDown(Key.A))
                            {
                                Movement_2(true, test);
                            }
                            else
                            {
                                Movement_2(false, test);
                            }
                        }
                        Movement(test);
                    }
                    else
                    {
                        blocks[Grid.GetRow(test), Grid.GetColumn(test)] = true;
                    }
                };
                backgroundWorker.RunWorkerAsync();
            }
            else
            {
                blocks[Grid.GetRow(test), Grid.GetColumn(test)] = true;
            }
        }
        private void Movement_2(bool direction, Rectangle temp)
        {
            if (Grid.GetColumn(temp) >= 0 && Grid.GetColumn(temp) <= 10)
            {
                if (direction == true && Grid.GetColumn(temp) != 0 && blocks[Grid.GetRow(temp), Grid.GetColumn(temp) - 1] == false)
                {
                    Grid.SetColumn(temp, Grid.GetColumn(temp) - 1);
                }
                else if(direction == false && Grid.GetColumn(temp) != 10 && blocks[Grid.GetRow(temp), Grid.GetColumn(temp) + 1] == false)
                {
                    Grid.SetColumn(temp, Grid.GetColumn(temp) + 1);
                }
            }
        }
        private bool Checker(Rectangle test)
        {
            if (Grid.GetRow(test) + 1 < 20)
            {
                if (blocks[Grid.GetRow(test) + 1, Grid.GetColumn(test)] == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
    }
}
