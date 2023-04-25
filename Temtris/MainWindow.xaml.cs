// Curt Lynch
// CSCI 352
// 04/25/2023
// UI for Temtris

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            Color lineColor = (Color)new ColorConverter().ConvertFrom("#ffffff");
            Pen pen = new Pen(new SolidColorBrush(lineColor), 2);
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
        TextBlock scoreBox;
        Rectangle[,] gameRects = new Rectangle[10, 20];
        List<Rectangle> gamePreviewRects = new List<Rectangle>();
        double gameAreaX = 0.0;
        double gameAreaY = 0.0;
        Difficulty difficulty = Difficulty.Menu;
        Difficulty difficultySelector = Difficulty.Easy;
        Image controls_img;

        public MainWindow()
        {
            gameRects = new Rectangle[10, 20];
            InitializeComponent();
            InitializeMainMenu();
        }

        private void InitializeMainMenu()
        {
            difficulty = Difficulty.Menu;

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
            Button_StartGame.FontSize = 28;
            Button_StartGame.Background = Brushes.Green;
            Button_StartGame.Click += Button_StartGame_Click;
            Button_StartGame.Width = gameWindow.Width * 0.25;
            Button_StartGame.Height = gameWindow.Height * 0.1;
            Canvas.SetTop(Button_StartGame, gameWindow.Height * 0.4);
            Canvas.SetLeft(Button_StartGame, gameWindow.Width * 0.35);
            Canvas.SetZIndex(Button_StartGame, 1);
            gameCanvas.Children.Add(Button_StartGame);

            Button Button_ExitGame = new Button();
            Button_ExitGame.Background = Brushes.Red;
            Button_ExitGame.Content = "Exit Game";
            Button_ExitGame.FontSize = 28;
            Button_ExitGame.Click += (s, e) => { this.Close(); };
            Button_ExitGame.Width = gameWindow.Width * 0.3;
            Button_ExitGame.Height = gameWindow.Height * 0.1;
            Canvas.SetTop(Button_ExitGame, gameWindow.Height * 0.6);
            Canvas.SetLeft(Button_ExitGame, gameWindow.Width * 0.35);
            Canvas.SetZIndex(Button_ExitGame, 1);
            gameCanvas.Children.Add(Button_ExitGame);

            difficultySelector = Difficulty.Easy;
            Button Button_Difficulty = new Button();
            Button_Difficulty.Background = Brushes.LightGreen;
            Button_Difficulty.Content = "Easy";
            Button_Difficulty.Click += Button_Difficulty_Click;
            Button_Difficulty.Width = gameWindow.Width * 0.05;
            Button_Difficulty.Height = gameWindow.Height * 0.1;
            Canvas.SetTop(Button_Difficulty, gameWindow.Height * 0.4);
            Canvas.SetLeft(Button_Difficulty, gameWindow.Width * 0.35 + Button_StartGame.Width);
            Canvas.SetZIndex(Button_Difficulty, 1);
            gameCanvas.Children.Add(Button_Difficulty);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    gameRects[i, j] = new Rectangle();
                    gameRects[i, j].Fill = Brushes.White;
                    gameRects[i, j].Width = gameWindow.Width / 10;
                    gameRects[i, j].Height = (gameWindow.Height - 40) / 20;
                    Canvas.SetLeft(gameRects[i, j], i * gameRects[i, j].Width);
                    Canvas.SetTop(gameRects[i, j], j * gameRects[i, j].Height);
                    gameCanvas.Children.Add(gameRects[i, j]);
                }
            }

            InitializeWorker();
            gameWorker.RunWorkerAsync(new TemtrisGame());
        }

        private void InitializeGame()
        {
            // Set up UI for a running game here
            gameWorker.CancelAsync();

            // Resize gameCanvas
            gameCanvas = new GridCanvas();
            gameCanvas.Background = Brushes.Black;
            gameWindow.Content = gameCanvas;
            gameWindow.UpdateLayout();
            gameAreaX = gameCanvas.ActualWidth * 0.3;
            gameAreaY = gameCanvas.ActualHeight * 0.1;

            controls_img = new Image();
            controls_img.Source = new BitmapImage(new Uri("./Gfx/WASD.png", UriKind.Relative));
            controls_img.Width = gameWindow.Width;
            controls_img.Name = "controls_img";
            Canvas.SetTop(controls_img, gameWindow.Height * 0.2);
            Canvas.SetLeft(controls_img, 0);
            Canvas.SetZIndex(controls_img, 1);
            gameCanvas.Children.Add(controls_img);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    gameRects[i, j] = new Rectangle();
                    gameRects[i, j].Fill = Brushes.White;
                    gameRects[i, j].Width = (gameCanvas.ActualWidth - gameAreaX) / 10 - 2;
                    gameRects[i, j].Height = (gameCanvas.ActualHeight - gameAreaY) / 20 - 2;
                    Canvas.SetLeft(gameRects[i, j], i * (gameRects[i, j].Width + 2) + gameAreaX + 1);
                    Canvas.SetTop(gameRects[i, j], j * (gameRects[i, j].Height + 2) + gameAreaY + 1);
                    gameCanvas.Children.Add(gameRects[i, j]);
                }
            }

            // Score
            scoreBox = new TextBlock();
            scoreBox.Foreground = Brushes.White;
            scoreBox.Width = 300;
            scoreBox.Height = 50;
            scoreBox.Text = "Score:";
            scoreBox.FontSize = 32;
            Canvas.SetTop(scoreBox, 0);
            Canvas.SetLeft(scoreBox, 0);
            gameCanvas.Children.Add(scoreBox);

            // Nextpiece preview
            TextBlock previewText = new TextBlock();
            previewText.Foreground = Brushes.White;
            previewText.Width = 150;
            previewText.Height = 50;
            previewText.Text = "Next Piece";
            previewText.FontSize = 32;
            Canvas.SetTop(previewText, 50);
            Canvas.SetLeft(previewText, 0);
            gameCanvas.Children.Add(previewText);

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
            if (game.IsCancelled)
                return;
            if (scoreBox != null)
            {
                scoreBox.Text = "Score: " + (int)matrix.score;
            }
            foreach (Rectangle r in gameRects)
            {
                r.Fill = gameCanvas.Background;
            }
            AddMino(matrix.inactive_Tetra);
            AddMino(matrix.active_Tetra);
            AddPreviewMino(matrix.preview_Tetra);
            if (difficulty != Difficulty.Menu && controls_img != null)
            {
                double opacity = 1000.0 / matrix.score;
                controls_img.Opacity = opacity;
                if (opacity < 0.35)
                    gameCanvas.Children.Remove(controls_img);
            }
        }
        private void AddMino(List<Mino> minos)
        {
            foreach (Mino m in minos)
            {
                if (m.y >= 0)
                    gameRects[m.x, m.y].Fill = new SolidColorBrush(m.color);
            }
        }

        private void AddPreviewMino(List<Mino> Minos)
        {
            if (difficulty == Difficulty.Menu)
                return;

            foreach (Rectangle r in gamePreviewRects)
            {
                gameCanvas.Children.Remove(r);
            }
            gamePreviewRects.Clear();
            foreach (Mino m in Minos)
            {
                Rectangle rect = new Rectangle();
                rect.Width = (gameCanvas.ActualWidth - gameAreaX) / 10;
                rect.Height = (gameCanvas.ActualHeight - gameAreaY) / 20;
                Canvas.SetLeft(rect, m.x * rect.Width - 130);
                Canvas.SetTop(rect, m.y * rect.Height + 100);

                rect.Fill = new SolidColorBrush(m.color);
                gamePreviewRects.Add(rect);
                gameCanvas.Children.Add(rect);
            }
        }

        void Game_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            TemtrisGame game = e.Result as TemtrisGame;
            if (game.IsCancelled)
                return;
            if (difficulty == Difficulty.Menu && !game.IsRunning)
            {
                InitializeWorker();
                gameWorker.RunWorkerAsync(new TemtrisGame());
                return;
            }

            InitializeMainMenu();
            string messageBoxText = "Score: " + (int)game.GetMatrix().score;
            string caption = "Game Over!";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.None;
            MessageBox.Show(messageBoxText, caption, button, icon);
        }

        private void Button_StartGame_Click(object sender, RoutedEventArgs e)
        {
            difficulty = difficultySelector;
            InitializeGame();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            gameWorker.CancelAsync();
        }

        private void Button_Difficulty_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            switch (difficultySelector)
            {
                case Difficulty.Easy:
                    difficultySelector = Difficulty.Hard;
                    b.Content = "Hard";
                    b.Background = Brushes.Red;
                    break;
                case Difficulty.Hard:
                    difficultySelector = Difficulty.Easy;
                    b.Content = "Easy";
                    b.Background = Brushes.LightGreen;
                    break;
            }
        }
    }
}
