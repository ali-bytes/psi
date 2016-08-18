using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NewIspNL.Service
{
      [DataContract]
    public class UserSave_ViewModel
    {
        [DataMember]
        public int id { get; set; }
            [DataMember]
        public string savename { get; set; }
            [DataMember]
        public int? User_id { get; set; }
              [DataMember]
            public int? Save_id { get; set; }
    }
}