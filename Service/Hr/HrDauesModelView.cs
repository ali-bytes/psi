using System.Runtime.Serialization;
using Db;

namespace NewIspNL.Service.Hr
{
    [DataContract]
    public class HrDauesModelView
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int EmployeeId { get; set; }
        [DataMember]
        public string Time { get; set; }

        [DataMember]
        public string date { get; set; }


        [DataMember]
        public int StateId { get; set; }

        public static HrDauesModelView To(HrDaye daye)
        {
            return new HrDauesModelView
            {
                EmployeeId = daye.EmployeeId,
                Id = daye.Id,
                StateId = daye.StateId,
                Time = daye.Time.ToShortDateString()

            };
        }

    }
}
