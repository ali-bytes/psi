using System.Configuration;
using System.Linq;
using Db;

namespace NewIspNL
{
    public interface ICallRepository{
        IQueryable<Call> Calls { get; }


        void SaveCall(Call call);


        void DeleteCall(Call call);
    }

    public class LtsCallRepository : ICallRepository{
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region ICallRepository Members


        public IQueryable<Call> Calls{
            get { return _context.Calls; }
        }


        public void SaveCall(Call call){
            if(call.Id == 0)
                _context.Calls.InsertOnSubmit(call);
            _context.SubmitChanges();
        }


        public void DeleteCall(Call call){
            if(call == null) return;
            _context.Calls.DeleteOnSubmit(call);
            _context.SubmitChanges();
        }


        #endregion
    }
}