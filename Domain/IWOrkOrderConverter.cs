using Db;
using NewIspNL.Models;

namespace NewIspNL.Domain{
    public interface IWOrkOrderConverter{
        WorkOrderTemplate ToPreviewTemplate(WorkOrder order);
      
    }
}
