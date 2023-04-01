using Entities.User.Enum;
using Entities.User.Owned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.BaseEntityFolder
{
    public interface IEntity
    {
    }

    public abstract class BaseEntity<TKey>:IEntity
    {
        public TKey Id { get; set; }
        public string CreateDate { get; set; }
        public string? UpdateDate { get; set; }

    }

    /// <summary>
    /// در صورتی که ای دی موجودیت هایی مانند محموله و شهرداری از نوع اینتیجر هست از این کلاس ارث بری میکنند در 
    /// غیر اینصورت از خود بیس انتیتی جنریک ارث بری میکنند و نوعی که میخواهند را به ان پاس میدهند
    /// </summary>
    public abstract class IntBaseEntity:BaseEntity<int>
    {
    }


}
