using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Ide.ViewModel {

    public class DockManagerViewModel {

        public ObservableCollection<DockWindowViewModel> Documents { get; }

        public ObservableCollection<object> Anchorables { get; }

        public DockManagerViewModel() {
            Documents = new ObservableCollection<DockWindowViewModel>();
            Anchorables = new ObservableCollection<object>();


        }

    }
}
