using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Temtris
{
    public partial class MainWindow : Window
    {
        BackgroundWorker gameWorker;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWorker();
            TemtrisGame game = new TemtrisGame();
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
            BackgroundWorker worker = sender as BackgroundWorker;
            GameEngine game = (GameEngine) e.Argument;
            
            e.Result = game.Start(worker);
        }

        void Game_Update(object sender, ProgressChangedEventArgs e)
        {
            TemtrisGame game = (TemtrisGame)e.UserState;
            Matrix matrix = game.GetMatrix();
            

            // Test code to see if backgroud worker is working
            Button_Start_Game.Content = "GameUpdate worked!";
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
            if (gameWorker.IsBusy == false) {
                gameWorker.RunWorkerAsync(new TemtrisGame());
            }
        }
    }
}
