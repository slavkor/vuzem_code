using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public interface IBaseModel
    {
        int Id { get; set; }
        long Iid { get; set; }
        string UuId { get; set; }
        int Active { get; set; }
        DateTime CreateDate { get; set; }
        DateTime ModifyDate { get; set; }
    }
}
