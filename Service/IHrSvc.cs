using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using NewIspNL.Service.Hr;
using NewIspNL.Service;


namespace NewIspNL.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IHrSvc" in both code and config file together.
    [ServiceContract]
    public interface IHrSvc
    {

//        [WebInvoke(UriTemplate = "/HireDate",
//Method = "POST",
//BodyStyle = WebMessageBodyStyle.Bare,
//RequestFormat = WebMessageFormat.Json,
//ResponseFormat = WebMessageFormat.Json)]
//        [OperationContract]

//        DateTime HireDate(int id);



     


        [WebGet(UriTemplate = "/Getemployees",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
         List<EmployeeReport> Getemployees();


        [WebGet(UriTemplate = "/Getsavess",
           BodyStyle = WebMessageBodyStyle.Bare,
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<UserSave_ViewModel> Getsaves();

        [WebGet(UriTemplate = "/GetemployStates",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<EmployeeStatesModelView> GetemployStates();

        [WebInvoke(UriTemplate = "/SaveDayes",
          Method = "POST",
          BodyStyle = WebMessageBodyStyle.Bare,
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        HrDauesModelView SaveDayes(HrDauesModelView hrDay);

        [WebInvoke(UriTemplate = "/GetTravels",
      Method = "POST",
      BodyStyle = WebMessageBodyStyle.Bare,
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]


        List<TravlsModelView> GetTravels(EmployeeDateReport empreport);


        [WebInvoke(UriTemplate = "/Getdis",
       Method = "POST",
       BodyStyle = WebMessageBodyStyle.Bare,
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        decimal Getempdis(EmployeeDateReport empreport);

         [WebInvoke(UriTemplate = "/Getrew",
       Method = "POST",
       BodyStyle = WebMessageBodyStyle.Bare,
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        decimal Getemprewards(EmployeeDateReport empreport);

      



        [WebInvoke(UriTemplate = "/GetWithdrawals",
       Method = "POST",
       BodyStyle = WebMessageBodyStyle.Bare,
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<WithdrawalModelView> GetWithdrawals(EmployeeDateReport empreport);

        [WebInvoke(UriTemplate = "/GetEmployeeabsences",
        Method = "POST",
        BodyStyle = WebMessageBodyStyle.Bare,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<EmployeeabsenceModelView> GetEmployeeabsences(EmployeeDateReport empreport);

        [WebInvoke(UriTemplate = "/GetEmpDateState",
        Method = "POST",
        BodyStyle = WebMessageBodyStyle.Bare,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        HrDauesModelView GetEmpDateState(EmployeeDateReport empreport);
        [WebInvoke(UriTemplate = "/SaveSalaries",
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        EmployeeSalaryModelView SaveSalaries(EmployeeSalaryModelView salaryModel);

        [WebInvoke(UriTemplate = "/GetAllSalaries",
         Method = "POST",
         BodyStyle = WebMessageBodyStyle.Bare,
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<EmployeeSalaryModelView> GetAllSalaries(SalarySearch salarySearch);












    }
}
