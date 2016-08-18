namespace NewIspNL.Models{
    public class CountName{
        public int Count { get; set; }
        public string Name { get; set; }
        public string TName { get; set; }
        public StatusName StatusName { get; set; }
    }

    public enum StatusName{
      
        New,

        Active,

        Suspend,

        Cancelled,

        Hold,

        SystemProblem,
        IsNew
    }
}
