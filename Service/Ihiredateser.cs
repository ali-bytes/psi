using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using NewIspNL.Service.Hr;

namespace NewIspNL.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Ihiredateser" in both code and config file together.
    [ServiceContract]
    public interface Ihiredateser
    {
       

        [WebInvoke(UriTemplate = "/Getdate",
             Method = "POST",
      BodyStyle = WebMessageBodyStyle.Bare,
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        HireDate Getdate(EmpIdHire empid);
    }
}
