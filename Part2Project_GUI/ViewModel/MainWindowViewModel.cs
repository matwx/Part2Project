using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Part2Project.Infrastructure;

namespace Part2Project_GUI.ViewModel
{
    class MainWindowViewModel : ObservableObject
    {
        #region Properties
        
        #endregion

        public event EventHandler RequestClose;

        #region Commands

        private RelayCommand _closeCommand; 
        public RelayCommand CloseCommand
        {
            get
            {
                if (_closeCommand == null) { _closeCommand = new RelayCommand(x => RequestClose(this, EventArgs.Empty)); }
                return _closeCommand;
            }
        }

        #endregion
        
//        private BitmapImage DirectBitmapToBitmapImage(DirectBitmap dBMP)
//        {
//            MemoryStream ms = new MemoryStream();
//            dBMP.Bitmap.Save(ms, ImageFormat.Png);
//            ms.Position = 0;
//            BitmapImage bi = new BitmapImage();
//            bi.BeginInit();
//            bi.StreamSource = ms;
//            bi.EndInit();
//        
//            return bi;
//        }
    }
}
