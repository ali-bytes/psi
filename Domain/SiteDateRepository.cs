using System.Linq;
using Db;

namespace NewIspNL.Domain{
    public class SiteDateRepository{
        readonly ISPDataContext _context;


        public SiteDateRepository(ISPDataContext context){
            _context = context;
        }


        public virtual SiteData SiteData(){
            return _context.SiteDatas.FirstOrDefault();
        }


        public void Save(SiteData data){
            _context.SubmitChanges();
        }
    }
}
