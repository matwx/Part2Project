using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2Project_GUI.ViewModel
{
    class BaseViewModel : ObservableObject
    {
        protected MainWindowViewModel _window;
        protected BaseViewModel _parent;

        public BaseViewModel(MainWindowViewModel window, BaseViewModel parent)
        {
            _window = window;
            _parent = parent;

            // When this view is closed, we see it's parent again
            RequestClose += delegate
            {
                if (_window != null)
                {
                    if (_parent == null)
                    {
                        // We're the root view, so just close the window
                        _window.RequestClose(this, EventArgs.Empty);
                    }
                    else
                    {
                        // Load the parent, thus closing this view
                        _window.ViewModel = _parent;
                    }
                }
            };
        }

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
    }
}
