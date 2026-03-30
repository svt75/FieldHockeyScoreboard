using System.Windows;

namespace FieldHockeyScoreboard
{
    public partial class ScoreboardWindow : Window
    {
        public ScoreboardWindow()
        {
            InitializeComponent();
        }

        public void MoveToScreen(System.Windows.Forms.Screen screen)
        {
            var bounds = screen.Bounds;
            Left = bounds.Left;
            Top = bounds.Top;
        }
    }
}
