using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;





namespace Temtris
{
    // adds a 20x10 grid to the canvas where the tetriminos will be located.
    class GridCanvas : Canvas
    {
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            Color lineColor = (Color)new ColorConverter().ConvertFrom("#ffffff");
            Pen pen = new Pen(new SolidColorBrush(lineColor), 1);
            double gameAreaX = this.ActualWidth * 0.3;
            double gameAreaY = this.ActualHeight * 0.1;
            double width = (this.ActualWidth - gameAreaX) / 10;
            double height = (this.ActualHeight - gameAreaY) / 20;

            // Draw row lines.
            for (int i = 0; i < 20; i++)
            {
                dc.DrawLine(pen, new Point(gameAreaX, i * height + gameAreaY), new Point(this.ActualWidth, i * height + gameAreaY));
            }
            // Draw column lines.
            for (int i = 0; i < 10; i++)
            {
                dc.DrawLine(pen, new Point(i * width + gameAreaX, gameAreaY), new Point(i * width + gameAreaX, this.ActualHeight));
            }
        }
    }

    public partial class MainWindow : Window
    {
        BackgroundWorker gameWorker;
        Canvas gameCanvas;
        List<Rectangle> gameRects;
        double gameAreaX = 0.0;
        double gameAreaY = 0.0;
        Difficulty difficulty = Difficulty.Menu;

        public MainWindow()
        {
            gameRects = new List<Rectangle>();
            InitializeComponent();
            InitializeMainMenu();
        }

        private void InitializeMainMenu()
        {
            gameCanvas = new Canvas();
            gameCanvas.Background = Brushes.Black;
            gameWindow.Content = gameCanvas;

            Image Logo = new Image();
            Logo.Source = new BitmapImage(new Uri("./Gfx/TemtrisLogo.png", UriKind.Relative));
            Logo.Width = gameWindow.Width * 0.78;
            Canvas.SetTop(Logo, 0);
            Canvas.SetLeft(Logo, gameWindow.Width * 0.1);
            Canvas.SetZIndex(Logo, 1);
            gameCanvas.Children.Add(Logo);

            Button Button_StartGame = new Button();
            Button_StartGame.Content = "Start Game";
            Button_StartGame.Background = Brushes.Green;
            Button_StartGame.Click += Button_StartGame_Click;
            Button_StartGame.Width = gameWindow.Width * 0.3;
            Button_StartGame.Height = gameWindow.Height * 0.1;
            Canvas.SetTop(Button_StartGame, gameWindow.Height * 0.4);
            Canvas.SetLeft(Button_StartGame, gameWindow.Width * 0.35);
            Canvas.SetZIndex(Button_StartGame, 1);
            gameCanvas.Children.Add(Button_StartGame);

            Button Button_ExitGame = new Button();
            Button_ExitGame.Background = Brushes.Red;
            Button_ExitGame.Content = "Exit Game";
            Button_ExitGame.Click += (s, e) => { this.Close(); };
            Button_ExitGame.Width = gameWindow.Width * 0.3;
            Button_ExitGame.Height = gameWindow.Height * 0.1;
            Canvas.SetTop(Button_ExitGame, gameWindow.Height * 0.6);
            Canvas.SetLeft(Button_ExitGame, gameWindow.Width * 0.35);
            Canvas.SetZIndex(Button_ExitGame, 1);
            gameCanvas.Children.Add(Button_ExitGame);

            InitializeWorker();
            gameWorker.RunWorkerAsync(new TemtrisGame());
        }

        private void InitializeWorker()
        {
            gameWorker = new BackgroundWorker();
            gameWorker.WorkerReportsProgress = true;
            gameWorker.WorkerSupportsCancellation = true;

            gameWorker.DoWork += Game_DoWork;
            gameWorker.ProgressChanged += Game_Update;
            gameWorker.RunWorkerCompleted += Game_Completed;
        }

        void Game_DoWork(object sender, DoWorkEventArgs e)
        {
            //work for game thread
            BackgroundWorker worker = (BackgroundWorker)sender;
            GameEngine game = (GameEngine)e.Argument;

            e.Result = game.Start(worker, difficulty);
        }

        void Game_Update(object sender, ProgressChangedEventArgs e)
        {
            TemtrisGame game = (TemtrisGame)e.UserState;
            Matrix matrix = game.GetMatrix();

            foreach (Rectangle r in gameRects)
            {
                gameCanvas.Children.Remove(r);
            }
            gameRects.Clear();

            AddMino(matrix.inactive_Tetra);
            AddMino(matrix.active_Tetra);
        }

        private void AddMino(List<Mino> minos)
        {
            foreach (Mino m in minos)
            {
                Rectangle rect = new Rectangle();

                rect.Width = (gameCanvas.ActualWidth - gameAreaX) / 10;
                rect.Height = (gameCanvas.ActualHeight - gameAreaY) / 20;
                Canvas.SetLeft(rect, m.x * rect.Width + gameAreaX);
                Canvas.SetTop(rect, m.y * rect.Height + gameAreaY);

                rect.Fill = new SolidColorBrush(m.color);

                gameRects.Add(rect);
                gameCanvas.Children.Add(rect);
            }
        }

        void Game_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            TemtrisGame game = e.Result as TemtrisGame;
            if (difficulty == Difficulty.Menu && !game.IsRunning)
            {
                InitializeMainMenu();
            }
            // Transition UI to game completed here
            // TODO: handle gameover
        }

        private void Button_StartGame_Click(object sender, RoutedEventArgs e)
        {
            // Set up UI for a running game here
            // Resize gameCanvas
            difficulty = Difficulty.Easy;
            gameWorker.CancelAsync();
            gameCanvas = new GridCanvas();
            gameCanvas.Background = Brushes.Black;
            gameWindow.Content = gameCanvas;
            gameWindow.UpdateLayout();
            gameAreaX = gameCanvas.ActualWidth * 0.3;
            gameAreaY = gameCanvas.ActualHeight * 0.1;

            // TODO: Score

            // TODO: Nextpiece preview

            InitializeWorker();
            gameWorker.RunWorkerAsync(new TemtrisGame());
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            gameWorker.CancelAsync();
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
