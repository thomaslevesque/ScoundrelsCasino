using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Roulette.ViewModel;
using XamlAnimatedGif;

namespace Roulette
{

    public partial class MainWindow
    {
        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainWindowViewModel();
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            DataContext = _viewModel;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.IsSpinning))
            {
                var animator = AnimationBehavior.GetAnimator(rouletteAnimation);
                if (animator != null)
                {
                    if (_viewModel.IsSpinning)
                        animator.Play();
                    else
                        animator.Pause();
                }
            }
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
                    _viewModel.SetSeed(seed);
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
