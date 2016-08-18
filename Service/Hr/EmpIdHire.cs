using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NewIspNL.Service.Hr
{
    [DataContract]
    public class EmpIdHire
    {
        [DataMember]
        public string EmployeeId { get; set; }

    }
}