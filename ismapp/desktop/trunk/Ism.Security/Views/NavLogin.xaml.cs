using Ism.Security.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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

namespace Ism.Security.Views
{

    [Export("NavLogin")]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NavLogin : UserControl
    {
        public NavLogin()
        {
            InitializeComponent();
        }

        [Import]
        NavLoginViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }
    }
}
