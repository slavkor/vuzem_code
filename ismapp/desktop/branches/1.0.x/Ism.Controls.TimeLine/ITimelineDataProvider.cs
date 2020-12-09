using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Controls.TimeLine
{
    public interface ITimelineDataProvider
    {
        void GetMatchesAsync(HierarchyData filter, Action<IEnumerable> callback);
        IEnumerable GetFilters();
    }
}
