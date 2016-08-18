using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Db;

namespace NewIspNL.FawryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FawryService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FawryService.svc or FawryService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FawryService : IFawryService
    {
        public processRequestResponse processRequestResponse(processRequest processRequest)
        {

            processRequestResponse processRequestResponse2;
            try
            {
                FawryPay.ProcessRequestResponseInQuery(processRequest, out processRequestResponse2);
                return processRequestResponse2;
            }
            catch 
            {
                
               
            }
            
        return new processRequestResponse();
            
        }
    }

}
