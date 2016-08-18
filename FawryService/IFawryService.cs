using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net.Security;
namespace NewIspNL.FawryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFawryService" in both code and config file together.
    [ServiceContract]
    
    public interface IFawryService
    {
        [OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign)]
        processRequestResponse processRequestResponse(processRequest processRequest);
    }
}
