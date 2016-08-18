using System;

namespace NewIspNL.BL.Abstract{
    public interface IIspClientService{
        DateTime ? GetLastActivationDate(int id);
    }
}
