using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Interaction
{
    public class ListInteractionEx<T> : ListInteraction<T>
    {
        public Action<Action<List<T>>> DataProvider { get; set; }

        public Action<T> AddAction { get; set; }
        public Action<T> EditAction { get; set; }
    }
}
