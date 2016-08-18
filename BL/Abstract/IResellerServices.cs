using System.Collections.Generic;
using Db;

namespace NewIspNL.BL.Abstract{
    public interface IResellerServices{
        //string GetResellerName(int ? userId);
        IEnumerable<ResellerPreviewItem> GetResellersUponUserGroup(User user);
        IEnumerable<ResellerPreviewItem> GetResellersUponUserGroup(int userId);
        IEnumerable<ResellerPreviewItem> GetResellersUponUserGroupAccountManager(User user);

        User GetUser(int userId);
    }

    public class ResellerPreviewItem{
        public int Id { get; set; }

        public string UserName { get; set; }
    }
}
