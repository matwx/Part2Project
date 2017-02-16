using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Part2Project.Infrastructure;

namespace Part2Project_GUI.ViewModel
{
    class MainWindowViewModel : BaseViewModel
    {
        #region Properties

        private BaseViewModel _viewModel;
        public BaseViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                OnPropertyChanged("ViewModel");
            } 
        }

        #endregion

        public MainWindowViewModel()
        {
            ViewModel = new StartScreenViewModel(this);
        }

        #region Commands

        public event EventHandler RequestClose;

        private RelayCommand _closeCommand;
        public RelayCommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(x => RequestClose(this, EventArgs.Empty));
                }
                return _closeCommand;
            }
        }

        private RelayCommand _displayFourImageSelectorCommand;
        public RelayCommand DisplayFourImageSelectorCommand
        {
            get
            {
                if (_displayFourImageSelectorCommand == null)
                {
                    _displayFourImageSelectorCommand = new RelayCommand(x => ViewModel = new FourImageSelectorViewModel(), x => !(ViewModel is FourImageSelectorViewModel));
                }
                return _displayFourImageSelectorCommand;
            }
        }

        #endregion
    }
}