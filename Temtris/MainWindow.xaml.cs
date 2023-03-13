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
        TemtrisGame game;
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

            gameWorker.DoWork += worker_DoWork;
            gameWorker.ProgressChanged += worker_ProgressChanged;
            gameWorker.RunWorkerCompleted += worker_WorkCompleted;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        void worker_WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void Button_Start_Game_Click(object sender, RoutedEventArgs e)
        {
            
        }


    }
}
