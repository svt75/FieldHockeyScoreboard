using System;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace FieldHockeyScoreboard
{
    public partial class ControlWindow : Window
    {
        private readonly GameState _game;
        private readonly ScoreboardWindow _scoreboard;
        private bool _onSecondMonitor;

        public ControlWindow(GameState game, ScoreboardWindow scoreboard)
        {
            InitializeComponent();
            _game = game;
            _scoreboard = scoreboard;
            DataContext = _game;
        }

        // Score
        private void ScoreAPlus_Click(object sender, RoutedEventArgs e) => _game.ScoreA++;
        private void ScoreAMinus_Click(object sender, RoutedEventArgs e) => _game.ScoreA--;
        private void ScoreBPlus_Click(object sender, RoutedEventArgs e) => _game.ScoreB++;
        private void ScoreBMinus_Click(object sender, RoutedEventArgs e) => _game.ScoreB--;

        // Timer
        private void StartPause_Click(object sender, RoutedEventArgs e)
        {
            if (_game.IsRunning)
            {
                _game.PauseTimer();
                BtnStartPause.Content = "СТАРТ";
            }
            else
            {
                _game.StartTimer();
                BtnStartPause.Content = "ПАУЗА";
            }
        }

        private void ResetTimer_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TxtMinutes.Text, out int minutes) && minutes > 0)
                _game.ResetTimer(minutes);
            else
                _game.ResetTimer(35);
            BtnStartPause.Content = "СТАРТ";
        }

        private void SetTimer_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TxtMinutes.Text, out int minutes) && minutes > 0)
            {
                _game.PauseTimer();
                _game.TimeRemaining = TimeSpan.FromMinutes(minutes);
                BtnStartPause.Content = "СТАРТ";
            }
        }

        // Period — 4 тайма
        private void Period1_Click(object sender, RoutedEventArgs e) => _game.Period = "1 ТАЙМ";
        private void Period2_Click(object sender, RoutedEventArgs e) => _game.Period = "2 ТАЙМ";
        private void Period3_Click(object sender, RoutedEventArgs e) => _game.Period = "3 ТАЙМ";
        private void Period4_Click(object sender, RoutedEventArgs e) => _game.Period = "4 ТАЙМ";
        private void PeriodBreak_Click(object sender, RoutedEventArgs e) => _game.Period = "ПЕРЕРЫВ";
        private void PeriodShootout_Click(object sender, RoutedEventArgs e) => _game.Period = "БУЛЛИТЫ";

        // Logos
        private void LoadLogoA_Click(object sender, RoutedEventArgs e)
        {
            var path = PickImageFile();
            if (path != null) _game.LoadLogoA(path);
        }

        private void LoadLogoB_Click(object sender, RoutedEventArgs e)
        {
            var path = PickImageFile();
            if (path != null) _game.LoadLogoB(path);
        }

        private static string PickImageFile()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp;*.gif|Все файлы|*.*",
                Title = "Выберите логотип"
            };
            return dlg.ShowDialog() == true ? dlg.FileName : null;
        }

        // Monitor switch
        private void MoveToMonitor_Click(object sender, RoutedEventArgs e)
        {
            var screens = System.Windows.Forms.Screen.AllScreens;

            if (screens.Length < 2)
            {
                if (_onSecondMonitor)
                {
                    _scoreboard.MoveToScreen(System.Windows.Forms.Screen.PrimaryScreen!);
                    _onSecondMonitor = false;
                    BtnMonitor.Content = "\U0001F5B5 2-й МОНИТОР";
                }
                else
                {
                    MessageBox.Show("Второй монитор не обнаружен.", "Монитор",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                return;
            }

            if (_onSecondMonitor)
            {
                _scoreboard.MoveToScreen(System.Windows.Forms.Screen.PrimaryScreen!);
                _onSecondMonitor = false;
                BtnMonitor.Content = "\U0001F5B5 2-й МОНИТОР";
            }
            else
            {
                var secondary = screens.First(s => !s.Primary);
                _scoreboard.MoveToScreen(secondary);
                _onSecondMonitor = true;
                BtnMonitor.Content = "\U0001F5B5 ОСНОВНОЙ";
            }
        }

        // Reset match
        private void ResetMatch_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сбросить все данные матча?", "Сброс матча",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _game.ResetMatch();
                BtnStartPause.Content = "СТАРТ";
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
