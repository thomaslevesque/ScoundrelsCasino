using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Roulette.ViewModel;

namespace Roulette
{

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }


        private static readonly Key[] _konamiCode =
        {
            Key.Up, Key.Up,
            Key.Down, Key.Down,
            Key.Left, Key.Right, 
            Key.Left, Key.Right, 
            Key.B, Key.A
        };

        private readonly List<Key> _lastKeys = new List<Key>();
        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            _lastKeys.Add(e.Key);
            while (_lastKeys.Count > _konamiCode.Length)
                _lastKeys.RemoveAt(0);
            if (_lastKeys.SequenceEqual(_konamiCode))
            {
                txtCheat.Clear();
                txtCheat.IsEnabled = true;
                txtCheat.Opacity = .25;
                txtCheat.Focus();
            }
        }

        private void txtCheat_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int seed;
                if (int.TryParse(txtCheat.Text, out seed))
                {
                    ((MainWindowViewModel) DataContext).SetSeed(seed);
                }
                txtCheat.IsEnabled = false;
                txtCheat.Opacity = 0;
            }
        }

        private void txtCheat_OnLostFocus(object sender, RoutedEventArgs e)
        {
            txtCheat.IsEnabled = false;
            txtCheat.Opacity = 0;
        }
    }
}
