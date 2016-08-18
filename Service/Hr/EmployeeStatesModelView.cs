using System.Runtime.Serialization;
using Db;

namespace NewIspNL.Service.Hr
{
    [DataContract]
    public class EmployeeStatesModelView
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }


        public static EmployeeStatesModelView To(EmployeeState employeeState)
        {
            return new EmployeeStatesModelView
            {
                Id = employeeState.Id,
                Name = employeeState.Name
            };
        }
    }
}
