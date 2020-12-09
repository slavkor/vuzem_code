using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Controls.TimeLine
{
    public class HierarchyData : INotifyPropertyChanged
    {
        #region Fields
        private bool _IsSelected;
        private bool _IsExpanded;
        #endregion

        #region Properties
        public HierarchyData Parent { get; set; }

        public object Value { get; set; }

        public IEnumerable<HierarchyData> Children { get; set; }

        public int Level { get; set; }

        public bool HasChildren
        {
            get
            {
                return Children?.Any() ?? false;
            }
        }

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                _IsExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
