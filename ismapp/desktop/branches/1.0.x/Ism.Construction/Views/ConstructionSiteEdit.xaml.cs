using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prism.Unity;
using Microsoft.Practices.Unity;

namespace Ism.Construction.Views
{
    /// <summary>
    /// Interaction logic for ConstructionSiteEdit.xaml
    /// </summary>
    public partial class ConstructionSiteEdit : UserControl
    {
        //static IRegionManager _regionManager;
        public ConstructionSiteEdit()
        {
            InitializeComponent();
        }

        //public ConstructionSiteEdit(IRegionManager regionManager)
        //{
        //    InitializeComponent();
        //    RegionManager.SetRegionName(Bp, RegionNames.CsiteBpRegion);
        //    RegionManager.SetRegionManager(this, regionManager);
        //}
    }
}
