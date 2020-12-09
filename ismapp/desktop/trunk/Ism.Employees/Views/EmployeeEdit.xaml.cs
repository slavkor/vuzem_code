using System.Windows.Controls;
using Ism.Employees.ViewModels;
using Telerik.Windows.Controls;
using System.Linq;

namespace Ism.Employees.Views
{
    /// <summary>
    /// Interaction logic for EmployeeEdit.xaml
    /// </summary>
    public partial class EmployeeEdit : UserControl
    {
        public EmployeeEdit()
        {
            InitializeComponent();
            //RegionManager.SetRegionManager(this, regionManager);
        }

        private void langList_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //var listBox = sender as RadListBox;
            //var listBoxItems = (listBox.DataContext as EmployeeEditViewModel).Languages;
            //this.langList.SelectionHelper.AddToSelection(listBoxItems.Where(i => i.IsSelected));

        }
    }
}
