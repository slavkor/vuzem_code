using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class CarListItem
    {
        public Car Car { get; set; }

        public DelegateCommand AddCar{ get; set; }
        public DelegateCommand<CarListItem> RemoveCar{ get; set; }
        public bool IsAddItem { get; set; }
    }
}
