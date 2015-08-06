using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Roulette.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private RouletteGenerator _generator;

        public MainWindowViewModel()
        {
            _generator = new RouletteGenerator();
            History = new ObservableCollection<NumberViewModel>();
        }

        public ObservableCollection<NumberViewModel> History { get; }

        private NumberViewModel _number;
        public NumberViewModel Number
        {
            get { return _number; }
            set { Set(ref _number, value); }
        }

        private bool _isSpinning;

        public bool IsSpinning
        {
            get { return _isSpinning; }
            set
            {
                if (Set(ref _isSpinning, value))
                    RaisePropertyChanged(nameof(IsNotSpinning));
            }
        }

        public bool IsNotSpinning => !IsSpinning;

        private RelayCommand _spinCommand;

        public ICommand SpinCommand => _spinCommand ?? (_spinCommand = new RelayCommand(Spin, CanSpin));

        private bool CanSpin() => !IsSpinning;

        private async void Spin()
        {
            try
            {
                IsSpinning = true;
                _spinCommand.RaiseCanExecuteChanged();
                await Task.Delay(2000);
                Number = new NumberViewModel(_generator.Next());
                History.Insert(0, Number);
            }
            catch
            {
                // ignored
            }
            finally
            {
                IsSpinning = false;
                _spinCommand.RaiseCanExecuteChanged();
            }
        }

        public void SetSeed(int seed)
        {
            _generator = new RouletteGenerator(new Random(seed));
        }
    }
}
