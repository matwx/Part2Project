using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2Project_GUI.ViewModel
{
    class FourImageSelectorViewModel : BaseViewModel
    {
        private ImageSorting _imageSorting;

        public FourImageSelectorViewModel(MainWindowViewModel w, BaseViewModel p, ImageSorting iS) : base(w, p)
        {
            _imageSorting = iS;
        }
    }
}
