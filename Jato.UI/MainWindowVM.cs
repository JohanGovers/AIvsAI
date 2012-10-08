using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Timers;
using System.Windows.Input;

namespace Jato.UI
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        Timer _timer = new Timer(400);
        bool _gameStarted = false;

        public static List<JatoPlayer> Players = new List<JatoPlayer>();

        public MainWindowVM(Func<bool> exitCriteria)
        {
            Players.Add(new JatoPlayer { KeepHealth = 100, Money = 100, Score = 0 });
            Player1KeepHealth = Players.First().KeepHealth;

            _gameStarted = true;
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Start();
        }

        private int _player1KeepHealth;
        public int Player1KeepHealth
        {
            get
            {
                return _player1KeepHealth;
            }
            set
            {
                _player1KeepHealth = value;
                RaisePropertyChanged("Player1KeepHealth");
            }
        }


        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_gameStarted)
            {
                Players.ForEach(p => p.Update());
                Player1KeepHealth = Players.First().KeepHealth;
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        
    }
}
