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

namespace Ism.Controls
{
    /// <summary>
    /// Interaction logic for LabeledCheckBox.xaml
    /// </summary>
    public partial class LabeledCheckBox : UserControl
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty
       .Register("Label",
               typeof(string),
               typeof(LabeledCheckBox),
               new FrameworkPropertyMetadata("Unnamed Label"));

        public static readonly DependencyProperty TextProperty = DependencyProperty
            .Register("IsChecked",
                typeof(bool),
                typeof(LabeledCheckBox),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public LabeledCheckBox()
        {
            InitializeComponent();
            Root.DataContext = this;
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public bool IsChecked
        {
            get { return (bool)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
