﻿using System;
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

        public MainWindowViewModel() : base(null, null)
        {
            // Start screen is the root View, so hasn't got a parent viewmodel
            ViewModel = new StartScreenViewModel(this, null);
        }

        #region Commands

//        private RelayCommand _displayFourImageSelectorCommand;
//        public RelayCommand DisplayFourImageSelectorCommand
//        {
//            get
//            {
//                if (_displayFourImageSelectorCommand == null)
//                {
//                    _displayFourImageSelectorCommand = new RelayCommand(x => ViewModel = new FourImageSelectorViewModel(), x => !(ViewModel is FourImageSelectorViewModel));
//                }
//                return _displayFourImageSelectorCommand;
//            }
//        }

        #endregion
    }
}