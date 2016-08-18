using System.Configuration;
using System.Linq;
using Db;

namespace NewIspNL.Services{
    public class CultureService{
        readonly ISPDataContext _dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        public void UpdateUserCulture(int cultureId, int userId){
            var user = GetUser(userId);
            var culture = GetCulture(cultureId);
            if(user != null && culture != null){
                var userCulture = _dataContext.UserCultures.FirstOrDefault(uc => uc.UserId == userId);
                if(userCulture != null){
                    userCulture.CultureId = cultureId;
                    _dataContext.SubmitChanges();
                } else{
                    userCulture = new UserCulture{
                        CultureId = cultureId,
                        UserId = userId
                    };
                    _dataContext.UserCultures.InsertOnSubmit(userCulture);
                    _dataContext.SubmitChanges();
                }
            }
        }


        public Culture GetCulture(int cultureId){
            var selectedCulture = _dataContext.Cultures.FirstOrDefault(c => c.Id == cultureId);
            return selectedCulture;
        }


        User GetUser(int userId){
           
            return _dataContext.Users.FirstOrDefault(u => u.ID == userId);
        }


        public string GetUserCulture(int userId){
            var user = GetUser(userId);
            if(user != null)
            {
                //return user.Culture.Culture1;
                var userCulture = user.UserCultures.FirstOrDefault();
                return userCulture != null ? userCulture.Culture.Culture1 : "ar-EG";
            }
            return "ar-EG";
        }

        public Culture GetUserCultureObject(int userId){
            var user = GetUser(userId);
            var culture = _dataContext.Cultures.First();
            if(user != null){
                var userCulture = user.UserCultures.FirstOrDefault();

                return userCulture != null ? userCulture.Culture : culture;
            }
            return culture;
        }


        public string GetUserCultureName(int userId){
            var user = GetUser(userId);
            if(user != null){
                var userCulture = user.UserCultures.FirstOrDefault(u => u.UserId == userId);
                return userCulture != null ? userCulture.Culture.Id.ToString() : "1";
            }
            return "1";
        }
    }
}
