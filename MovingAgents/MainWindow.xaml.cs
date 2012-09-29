using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace MovingAgents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Status.Content = "Starting...";

            var workerThread = new BackgroundWorker();

            workerThread.DoWork += new DoWorkEventHandler(workerThread_DoWork);
            workerThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerThread_RunWorkerCompleted);

            workerThread.RunWorkerAsync();
        }

        void workerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Status.Content = "Completed";
            return;
        }

        public void workerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(500);
            for (int i = 0; i < 200; i++)
            {
                Thread.Sleep(10);
                Status.Dispatcher.BeginInvoke((Action)(() => Status.Content = "Tick " + i));
                Tick();
            }
        }

        public void Tick()
        {
            Action tick = () =>
            {
                var bottom = (double)Charlie.GetValue(Canvas.BottomProperty);
                
                Charlie.SetValue(Canvas.BottomProperty, bottom+1);
            };
            Canvas.Dispatcher.BeginInvoke(tick);
        }
    }
}
