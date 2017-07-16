using System.Collections.Generic;

namespace MB.TestTask.Models
{
    public class UserInfoFilter
    {
        public List<long> Ids { get; set; }
        public List<string> Logins { get; set; }
        public List<OrderingField> OrderBy { get; set; }
        public long? Page { get; set; }
        public long? PageSize { get; set; }
    }

    public class OrderingField
    {
        public string Field { get; set; }
        public bool Ascend { get; set; }
    }
}
