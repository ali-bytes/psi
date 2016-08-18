using Db;

namespace NewIspNL.Models{
    public class ProviderPackages{
        public ServiceProvider Provider { get; set; }     
        public ServicePackage Package  { get; set; }
        public bool Checked { get; set; }
    }
    
}