using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Validation
{
    public interface IDirty
    {
        bool IsDirty { get; set; }

       void SetDirty(bool dirty);
    }
}
