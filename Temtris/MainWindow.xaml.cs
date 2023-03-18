using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D.Converters;
using System.Windows.Shapes;

namespace Temtris
{
    public partial class MainWindow : Window
    {
        BackgroundWorker gameWorker;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWorker();
            // Should probably generate the Canvas programmatically 
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
        }

        void Game_Update(object sender, ProgressChangedEventArgs e)
        {
            TemtrisGame game = (TemtrisGame)e.UserState;
            Matrix matrix = game.GetMatrix();
            
            gameCanvas.Children.Clear();

            //TODO: set color correctly. Correctly scale size/pos to canvas.

            foreach (Mino m in matrix.inactive_Minos)
            {
                Rectangle rect = new Rectangle();
                rect.Fill = new SolidColorBrush(Colors.Blue);
                rect.Stroke = new SolidColorBrush(Colors.Blue);
                rect.Width = gameCanvas.ActualWidth / 10;
                rect.Height = gameCanvas.ActualHeight / 20;
                gameCanvas.Children.Add(rect);
                Canvas.SetLeft(rect, m.x * rect.Width);
                Canvas.SetTop(rect, m.y * rect.Height);

            }
            foreach (Mino m in matrix.active_Tetra)
            {
                Rectangle rect = new Rectangle();
                rect.Fill = new SolidColorBrush(Colors.Blue);
                rect.Stroke = new SolidColorBrush(Colors.Blue);
                rect.Width = gameCanvas.ActualWidth / 10;
                rect.Height = gameCanvas.ActualHeight / 20;
                gameCanvas.Children.Add(rect);
                Canvas.SetLeft(rect, m.x * rect.Width);
                Canvas.SetTop(rect, m.y * rect.Height);
            }

            // Test code to see if backgroud worker is working
            Button_Start_Game.Content = "Game Running!";
        }

        void Game_Completed(object sender, RunWorkerCompletedEventArgs e)
        {

            // Transition UI to game completed here
            Button_Start_Game.Content = "Game Finished!";
        }

        private void Button_Start_Game_Click(object sender, RoutedEventArgs e)
        {
            // Set up UI for a running game here

            // start game with new GameEngine object
            if (gameWorker.IsBusy == false)
            {
                gameWorker.RunWorkerAsync(new TemtrisGame());
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            gameWorker.CancelAsync();
        }
    }
}
