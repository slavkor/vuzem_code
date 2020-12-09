using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Ism.Infrastructure.Model
{
    public class GridSearch
    {
        public string SearchString { get; set; }
        public IList<RadGridView> Grids { get; set; }
    }
}
