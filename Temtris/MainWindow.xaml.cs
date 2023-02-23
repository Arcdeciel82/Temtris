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
        private bool[,] blocks = new bool[20, 10];
        private int test_gg = 0;
        private List <mino> inactive = new List<mino>();
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            List <mino> Poly = new List <mino>();
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
            if (test_gg == 0)
            {
                createGameBoard();
                test_gg++;
            }
            mino one = new mino(4,1);
            Poly.Add(one);
            mino two = new mino(5,1);
            Poly.Add(two);
            mino three = new mino(5,2);
            Poly.Add(three);
            mino four = new mino(6,2);
            Poly.Add(four);
            for(int i = 0; i < 4; i++)
            {
                Rectangle temp = new Rectangle();
                temp.Width = 54;
                temp.Height = 54;
                temp.StrokeThickness = 5;
                temp.Fill = brush;
                temp.Stroke = brush;
                Grid.SetColumn(temp, Poly[i].pos.x);
                Grid.SetRow(temp, Poly[i].pos.y);
                GameGrid.Children.Add(temp);
            }


            time = TimeSpan.FromMilliseconds(0);
            Start(Poly);
            
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

        private TimeSpan time = new TimeSpan();
        
        private void Start(List <mino> Poly)
        {
            int n = 0;
            while(n != 100000)
            {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if(Checker(Poly) == false)
            {
                if(time >= TimeSpan.FromMilliseconds(500))
                {
                    time = time - TimeSpan.FromMilliseconds(500);
                    Poly = down(Poly);
                } 
            }
            sw.Stop();
            time = time + (long)sw.ElapsedMilliseconds;
            n++;
            }
        }

        private List<mino> down(List <mino> Poly)
        {
            for(int i = 0; i < 4; i++)
            {
                Poly[i].pos.y++;
            }
            for(int i = 0; i < 4; i++)
            {
                RemoveGE(GameGrid, Poly[i].pos.y, Poly[i].pos.x);
            }
            return Poly;
        }
        private void RemoveGE(Grid g, int r, int c)
        {
            for(int i = 0; i <  g.Children.Count; i++)
            {
                UIElement e = g.Children[i];
                if(Grid.GetRow(e)==r && Grid.GetColumn(e) == c)
                {
                    g.Children.Remove(e);
                }
            }
        }




/*        private void Movement(Rectangle temp)
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
*/
    private bool Checker(List <mino> Poly)
        {
            for(int i = 0; i < 4; i++)
            {
                if (Poly[i].pos.y + 1 < 20)
                {
                    if (blocks[Poly[i].pos.y + 1, Poly[i].pos.x] == true)
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
    public class Position
    {
        public int x; //(cols)
        public int y; //(rows)
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Position()
        {
            x = 0;
            y = 0;
        }
        public static Position operator +(Position lhs, Position rhs)
        {
            Position temp = new Position();
            temp.x = lhs.x + rhs.x;
            temp.y = lhs.y + rhs.y;
            return temp;
        }
        public static Position operator -(Position lhs, Position rhs)
        {
            Position temp = new Position();
            temp.x = lhs.x - rhs.x;
            temp.y = lhs.y - rhs.y;
            return temp;
        }
    }
    public class mino
    {
        public mino(int x, int y)
        {
            pos = new Position(x,y); 
        }
        public Position pos;
        public SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
    }
}
