using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Db;

namespace NewIspNL.Service.Hr
{
     [DataContract]
    public class HireDate
    {
         [DataMember]
         public string Date { get; set; }


         public static HireDate To(Employe employee)
         {
             return new HireDate
             {
                 Date = employee.HiringDate.ToString()
             };
         }


    }
}