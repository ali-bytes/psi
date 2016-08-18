using System.Configuration;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IUserRepository{
        IQueryable<User> Users { get; }


        //string GetUserName(int userId);
    }

    public class UserRepository : IUserRepository{
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region IUserRepository Members


        /*public string GetUserName(int userId){
            var user = _context.Users.FirstOrDefault(u => u.ID == userId);
            return user == null ? null : user.UserName;
        }
         */


        public IQueryable<User> Users{
            get { return _context.Users; }
        }


        #endregion
    }
}
