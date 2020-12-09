using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class UuidEqualityComparer<T> : IEqualityComparer<T> where T : BaseModel

    {
        public bool Equals(T x, T y)
        {
            return x.UuId == x.UuId;
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
