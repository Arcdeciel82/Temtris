using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Temtris
{
    // adds a 20x10 grid to the canvas where the tetriminos will be located.
    class GridCanvas : Canvas
    {
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            Color lineColor = (Color)new ColorConverter().ConvertFrom("#121212");
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

            e.Result = game.Start(worker);
            e.Cancel = true;
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

            AddMino(matrix.inactive_Minos);
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
            // Transition UI to game completed here
            // TODO: handle gameover
        }

        private void Button_StartGame_Click(object sender, RoutedEventArgs e)
        {
            // Set up UI for a running game here
            // Resize gameCanvas
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
    }
}
