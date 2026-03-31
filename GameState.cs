using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FieldHockeyScoreboard
{
    public class GameState : INotifyPropertyChanged
    {
        private string _teamAName = "КОМАНДА А";
        private string _teamBName = "КОМАНДА Б";
        private int _scoreA;
        private int _scoreB;
        private int _greenCardsA;
        private int _yellowCardsA;
        private int _greenCardsB;
        private int _yellowCardsB;
        private string _period = "1 ТАЙМ";
        private TimeSpan _timeRemaining = TimeSpan.FromMinutes(35);
        private bool _isRunning;
        private readonly DispatcherTimer _timer;

        public GameState()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _timer.Tick += Timer_Tick;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string TeamAName
        {
            get => _teamAName;
            set { _teamAName = value; OnPropertyChanged(); }
        }

        public string TeamBName
        {
            get => _teamBName;
            set { _teamBName = value; OnPropertyChanged(); }
        }

        public int ScoreA
        {
            get => _scoreA;
            set { _scoreA = Math.Max(0, value); OnPropertyChanged(); OnPropertyChanged(nameof(ScoreDisplay)); }
        }

        public int ScoreB
        {
            get => _scoreB;
            set { _scoreB = Math.Max(0, value); OnPropertyChanged(); OnPropertyChanged(nameof(ScoreDisplay)); }
        }

        public string ScoreDisplay => $"{_scoreA} : {_scoreB}";

        public int GreenCardsA
        {
            get => _greenCardsA;
            set { _greenCardsA = Math.Max(0, value); OnPropertyChanged(); OnPropertyChanged(nameof(CardsADisplay)); }
        }

        public int YellowCardsA
        {
            get => _yellowCardsA;
            set { _yellowCardsA = Math.Max(0, value); OnPropertyChanged(); OnPropertyChanged(nameof(CardsADisplay)); }
        }

        public int GreenCardsB
        {
            get => _greenCardsB;
            set { _greenCardsB = Math.Max(0, value); OnPropertyChanged(); OnPropertyChanged(nameof(CardsBDisplay)); }
        }

        public int YellowCardsB
        {
            get => _yellowCardsB;
            set { _yellowCardsB = Math.Max(0, value); OnPropertyChanged(); OnPropertyChanged(nameof(CardsBDisplay)); }
        }

        public string CardsADisplay => $"\u25CF{_greenCardsA}  \u25CF{_yellowCardsA}";
        public string CardsBDisplay => $"\u25CF{_greenCardsB}  \u25CF{_yellowCardsB}";

        private ImageSource _logoASource;
        private ImageSource _logoBSource;

        public ImageSource LogoASource
        {
            get => _logoASource;
            set { _logoASource = value; OnPropertyChanged(); }
        }

        public ImageSource LogoBSource
        {
            get => _logoBSource;
            set { _logoBSource = value; OnPropertyChanged(); }
        }

        public void LoadLogoA(string path)
        {
            LogoASource = LoadImage(path);
        }

        public void LoadLogoB(string path)
        {
            LogoBSource = LoadImage(path);
        }

        private static BitmapImage LoadImage(string path)
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(path, UriKind.Absolute);
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.DecodePixelHeight = 80;
            bmp.EndInit();
            bmp.Freeze();
            return bmp;
        }

        public string Period
        {
            get => _period;
            set { _period = value; OnPropertyChanged(); }
        }

        public TimeSpan TimeRemaining
        {
            get => _timeRemaining;
            set { _timeRemaining = value; OnPropertyChanged(); OnPropertyChanged(nameof(TimerDisplay)); }
        }

        public string TimerDisplay => TimeRemaining.ToString(@"mm\:ss");

        public bool IsRunning
        {
            get => _isRunning;
            set { _isRunning = value; OnPropertyChanged(); }
        }

        private DateTime _lastTick;

        public void StartTimer()
        {
            if (_timeRemaining <= TimeSpan.Zero) return;
            _lastTick = DateTime.Now;
            _timer.Start();
            IsRunning = true;
        }

        public void PauseTimer()
        {
            _timer.Stop();
            IsRunning = false;
        }

        public void ResetTimer(int minutes = 35)
        {
            PauseTimer();
            TimeRemaining = TimeSpan.FromMinutes(minutes);
        }

        public void ResetMatch()
        {
            PauseTimer();
            ScoreA = 0;
            ScoreB = 0;
            GreenCardsA = 0;
            YellowCardsA = 0;
            GreenCardsB = 0;
            YellowCardsB = 0;
            Period = "1 ТАЙМ";
            TimeRemaining = TimeSpan.FromMinutes(35);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            var now = DateTime.Now;
            var elapsed = now - _lastTick;
            _lastTick = now;

            var newTime = _timeRemaining - elapsed;
            if (newTime <= TimeSpan.Zero)
            {
                TimeRemaining = TimeSpan.Zero;
                PauseTimer();
            }
            else
            {
                TimeRemaining = newTime;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
