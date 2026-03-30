using System.Windows;

namespace FieldHockeyScoreboard
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var gameState = new GameState();

            var scoreboard = new ScoreboardWindow();
            scoreboard.DataContext = gameState;
            scoreboard.Show();

            var control = new ControlWindow(gameState, scoreboard);
            control.Show();
        }
    }
}
