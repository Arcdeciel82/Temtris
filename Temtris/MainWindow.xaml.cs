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
        private bool[,] blocks = new bool[20, 10];
        private int row = 0;
        private int column = 5;
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
            Rectangle temp = new Rectangle();
            Grid.SetRow(temp, row);
            Grid.SetColumn(temp, column);
            temp.Width = 54;
            temp.Height = 54;
            temp.Fill = brush;
            temp.StrokeThickness = 5;
            temp.Stroke = brush;
            MainMenu_Grid.Children.Add(temp);
            Movement(temp);
        }
        private void Movement(Rectangle test)
        {
            if (Checker(test) == false)
            {
                var backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += (s, v) => { Thread.Sleep(500); };
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
                        blocks[Grid.GetRow(test), (Grid.GetColumn(test) - 1)] = true;
                    }
                };
                backgroundWorker.RunWorkerAsync();
            }
            else
            {
                blocks[Grid.GetRow(test), Grid.GetColumn(test) - 1] = true;
            }
        }
        private void Movement_2(bool direction, Rectangle temp)
        {
            if (Grid.GetColumn(temp) >= 1 && Grid.GetColumn(temp) <= 10)
            {
                if (direction == true && Grid.GetColumn(temp) != 1)
                {
                    Grid.SetColumn(temp, Grid.GetColumn(temp) - 1);
                }
                else if(direction == false && Grid.GetColumn(temp) != 10)
                {
                    Grid.SetColumn(temp, Grid.GetColumn(temp) + 1);
                }
            }
        }
        private bool Checker(Rectangle test)
        {
            if (Grid.GetRow(test) + 1 < 20)
            {
                if (blocks[Grid.GetRow(test) + 1, (Grid.GetColumn(test) - 1)] == true)
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
