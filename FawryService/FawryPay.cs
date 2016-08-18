using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using Antlr.Runtime.Misc;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.FawryService
{
    public class FawryPay
    {
        public static bool ProcessRequestResponseInQuery(processRequest processRequest,
            out processRequestResponse processRequestResponse2)
        {
            string phone = string.Empty;
            if (processRequest.arg0.Request.PresSvcRq != null)
            {
                phone = processRequest.arg0.Request.PresSvcRq.BillInqRq.BillingAcct;

            }
            else if (processRequest.arg0.Request.PaySvcRq != null)
            {
                phone = processRequest.arg0.Request.PaySvcRq.PmtNotifyRq.PmtRec.PmtInfo.BillingAcct;
            }


            int worId = 0;
            var amount = default(decimal);
            var ctx = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
            using (ctx)
            {
                if (phone != null && phone != "?")
                {
                    try
                    {

                        var wor = ctx.WorkOrders.FirstOrDefault(x => x.CustomerPhone == phone);
                        if (wor == null)
                        {
                            AdditionalStatus additionalStatus = new AdditionalStatus
                            {
                                StatusCode = "604",
                                Severity = "Error",
                                StatusDesc = "Bill account is not found"
                            };
                            processRequestResponse2 = ProcessRequestResponseInQueryFailure(processRequest, additionalStatus);
                            return false;
                        }

                        worId = wor.ID;
                        var demand =
                            ctx.Demands.Where(x => x.WorkOrderId == worId && x.Paid == false)
                                .OrderByDescending(a => a.Id)
                                .FirstOrDefault();

                        var processRequestResponse = new processRequestResponse();
                        // -----
                        if (processRequest.arg0.Request.SignonRq.SignonProfile.MsgCode.Equals("BillInqRq"))
                        {
                            // first request just return amount no paying in our system
                            try
                            {
                                if (demand == null)
                                {
                                    AdditionalStatus additionalStatus2 = new AdditionalStatus
                                    {
                                        StatusCode = "600",
                                        Severity = "Error",
                                        StatusDesc = "there is no bill for customer"
                                    };
                                    processRequestResponse2 = ProcessRequestResponseInQueryFailure(processRequest, additionalStatus2);
                                    return false;
                                }

                                amount = demand.Amount;
                                processRequestResponse = InQuery(processRequest, amount);
                            }
                            catch
                            {
                                AdditionalStatus additionalStatus2 = new AdditionalStatus
                                {
                                    StatusCode = "606",
                                    Severity = "Error",
                                    StatusDesc = "Failed to retrieve bill information due to system error"
                                };
                                processRequestResponse2 = ProcessRequestResponseInQueryFailure(processRequest, additionalStatus2);
                                return false;
                            }


                        }
                        else if (processRequest.arg0.Request.SignonRq.SignonProfile.MsgCode.Equals("PmtNotifyRq"))
                        {
                            // second request  for pay demand
                            try
                            {
                                PmtTransId pmtTransId = new PmtTransId();
                                string reqFptnId = string.Empty;

                                if (processRequest.arg0.Request.PaySvcRq != null)
                                {
                                    // check fptn
                                    pmtTransId =
                                        processRequest.arg0.Request.PaySvcRq.PmtNotifyRq.PmtRec.PmtTransId
                                            .FirstOrDefault(x => x.PmtIdType == "FPTN");
                                    if (pmtTransId != null) reqFptnId = pmtTransId.PmtId.Trim();
                                }
                                if (pmtTransId == null)
                                {
                                    AdditionalStatus additionalStatus2 = new AdditionalStatus
                                    {
                                        StatusCode = "600",
                                        Severity = "Info",
                                        StatusDesc = "There is no FBTN in request"
                                    };
                                    processRequestResponse2 = PaymentFailResponse(processRequest, additionalStatus2);
                                    return false;
                                }
                                if (demand == null)
                                {
                                    AdditionalStatus additionalStatus2 = new AdditionalStatus
                                    {
                                        StatusCode = "600",
                                        Severity = "Info",
                                        StatusDesc = "There is no bill for customer"
                                    };
                                    processRequestResponse2 = PaymentFailResponse(processRequest, additionalStatus2);
                                    return false;
                                }


                                if (processRequest.arg0.Request.IsRetry == "false")
                                {
                                    demand.Paid = true;
                                    demand.PaymentDate = DateTime.Now.AddHours();
                                    demand.PaymentComment = "تم الدفع من خلال خدمة فورى";
                                    FawryPaymentRecord record = new FawryPaymentRecord
                                    {
                                        CustomerPhone = phone,
                                        DemandId = demand.Id,
                                        Fptn = reqFptnId
                                    };

                                    ctx.FawryPaymentRecords.InsertOnSubmit(record);

                                }
                                else if (processRequest.arg0.Request.IsRetry == "true")
                                {
                                    // check fptn and paying the customer in database
                                    bool isPaid = false;
                                    if (processRequest.arg0.Request.PaySvcRq != null)
                                    {
                                        // check fptn
                                        if (!string.IsNullOrEmpty(reqFptnId))
                                        {
                                            var fptn = ctx.FawryPaymentRecords.FirstOrDefault(x => x.Fptn.Equals(reqFptnId));
                                            if (fptn != null)
                                            {
                                                isPaid = true;
                                            }
                                        }
                                    }

                                    if (!isPaid)
                                    {
                                        demand.Paid = true;
                                        demand.PaymentDate = DateTime.Now.AddHours();
                                        demand.PaymentComment = "تم الدفع من خلال خدمة فورى";
                                        FawryPaymentRecord record = new FawryPaymentRecord
                                        {
                                            CustomerPhone = phone,
                                            DemandId = demand.Id,
                                            Fptn = reqFptnId
                                        };
                                        ctx.FawryPaymentRecords.InsertOnSubmit(record);
                                    }

                                }

                                ctx.SubmitChanges();
                                processRequestResponse = SuccessPayResponse(processRequest, pmtTransId, demand.Id.ToString());
                            }
                            catch
                            {

                                AdditionalStatus additionalStatus2 = new AdditionalStatus
                                {
                                    StatusCode = "606",
                                    Severity = "Info",
                                    StatusDesc = "Failed to retrieve bill information due to system error"
                                };
                                processRequestResponse2 = PaymentFailResponse(processRequest, additionalStatus2);
                                return false;
                            }
                        }


                        processRequestResponse2 = processRequestResponse;
                        return true;
                    }
                    catch
                    {
                        AdditionalStatus additionalStatus2 = new AdditionalStatus
                        {
                            StatusCode = "606",
                            Severity = "Info",
                            StatusDesc = "Failed to retrieve bill information due to system error"
                        };
                        processRequestResponse2 = ProcessRequestResponseInQueryFailure(processRequest, additionalStatus2);
                        return false;

                    }
                }
                else
                {
                    AdditionalStatus additionalStatus4 = new AdditionalStatus
                    {
                        StatusCode = "660",
                        Severity = "Info",
                        StatusDesc = "Failed to retrieve bill information due to BillingAcct does not exist in Request "
                    };
                    processRequestResponse2 = ProcessRequestResponseInQueryFailure(processRequest, additionalStatus4);
                    return false;
                }

            }

        }

        private static processRequestResponse InQuery(processRequest processRequest, decimal amount)
        {
            processRequestResponse processRequestResponse = new processRequestResponse
            {
                Return = new Return
                {
                    Response = new Response
                    {
                        SignonRs = new SignonRs
                        {
                            ServerDt = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                            Language = processRequest.arg0.Request.SignonRq.CustLangPref,
                            SignonProfile = new SignonProfile
                            {
                                Sender = processRequest.arg0.Request.SignonRq.SignonProfile.Receiver,
                                MsgCode = "BillInqRs",
                                Version = processRequest.arg0.Request.SignonRq.SignonProfile.Version
                            }
                        },
                        PresSvcRs = new PresSvcRs
                        {
                            RqUID = processRequest.arg0.Request.PresSvcRq.RqUID,
                            Status = new Status
                            {
                                StatusCode = "200",
                                Severity = "Info",
                                StatusDesc = "Success"
                            },
                            BillInqRs = new BillInqRs
                            {
                                BillRec = new BillRec
                                {
                                    BillingAcct = processRequest.arg0.Request.PresSvcRq.BillInqRq.BillingAcct,
                                    BillTypeCode = processRequest.arg0.Request.PresSvcRq.BillInqRq.BillTypeCode,
                                    BillInfo = new BillInfo
                                    {
                                        BillSummAmt = new BillSummAmt
                                        {
                                            CurAmt = new CurAmt
                                            {
                                                Amt = amount.ToString(CultureInfo.InvariantCulture)
                                            }
                                        }
                                    }
                                }
                            },
                        }
                    }
                }
            };
            return processRequestResponse;
        }
        private static processRequestResponse SuccessPayResponse(processRequest processRequest, PmtTransId pmtTransId, string demandId)
        {
            //var PmtTransId = new PmtTransId[2];
            //PmtTransId[0] = new PmtTransId()
            //{
            //    PmtId = pmtTransId != null ? pmtTransId.PmtId : null,
            //    CreatedDt = pmtTransId != null ? pmtTransId.CreatedDt : null,
            //    PmtIdType = pmtTransId != null ? pmtTransId.PmtIdType : null
            //};
            //PmtTransId[1] = new PmtTransId()
            //{
            //    PmtId = demandId,
            //    CreatedDt = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            //    PmtIdType = "BLRPTN"
            //};
            processRequestResponse processRequestResponse = new processRequestResponse
           {
               Return = new Return
               {
                   Response = new Response
                   {
                       SignonRs = new SignonRs
                       {
                           ServerDt = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                           Language = processRequest.arg0.Request.SignonRq.CustLangPref,
                           ClientDt = processRequest.arg0.Request.SignonRq.ClientDt,
                           CustLangPref = processRequest.arg0.Request.SignonRq.CustLangPref,
                           SignonProfile = new SignonProfile
                           {
                               Sender = processRequest.arg0.Request.SignonRq.SignonProfile.Receiver,
                               MsgCode = "PmtNotifyRs",
                               Version = processRequest.arg0.Request.SignonRq.SignonProfile.Version,
                               Receiver = processRequest.arg0.Request.SignonRq.SignonProfile.Sender
                           }
                       },
                       PaySvcRs = new PaySvcRs
                       {
                           RqUID = processRequest.arg0.Request.PaySvcRq.RqUID,
                           AsyncRqUID = processRequest.arg0.Request.PaySvcRq.AsyncRqUID,
                           Status = new Status
                           {
                               StatusCode = "200",
                               Severity = "Info",
                               StatusDesc = "Success"
                           },
                           PmtNotifyRs = new PmtNotifyRs
                           {
                               PmtStatusRec = new PmtStatusRec
                               {
                                   status = new Status
                                   {
                                       StatusCode = "200",
                                       Severity = "Info",
                                       StatusDesc = "Success"
                                   },
                                   //PmtTransId = PmtTransId
                                   PmtTransId = new PmtTransId
                                   {

                                       PmtId = pmtTransId != null ? pmtTransId.PmtId : null,
                                       CreatedDt = pmtTransId != null ? pmtTransId.CreatedDt : null,
                                       PmtIdType = pmtTransId != null ? pmtTransId.PmtIdType : null

                                   }
                               }
                           }
                       },
                   }
               }

           };
            return processRequestResponse;



            return null;
        }

        public static processRequestResponse ProcessRequestResponseInQueryFailure(processRequest processRequest, AdditionalStatus addStatus)
        {
            processRequestResponse processRequestResponse = new processRequestResponse
            {
                Return = new Return
                {
                    Response = new Response
                    {
                        SignonRs = new SignonRs
                        {
                            ServerDt = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                            Language = processRequest.arg0.Request.SignonRq.CustLangPref,
                            SignonProfile = new SignonProfile
                            {
                                Sender = processRequest.arg0.Request.SignonRq.SignonProfile.Receiver,
                                MsgCode = "BillInqRs",
                                Version = processRequest.arg0.Request.SignonRq.SignonProfile.Version
                            }
                        },
                        PresSvcRs = new PresSvcRs
                        {
                            RqUID = processRequest.arg0.Request.PresSvcRq.RqUID,
                            Status = new Status
                            {
                                StatusCode = "12006",
                                Severity = "Error",
                                StatusDesc = "Bill account is not available at biller repository.",
                                AdditionalStatus = new AdditionalStatus
                                {
                                    StatusCode = addStatus.StatusCode,
                                    Severity = addStatus.Severity,
                                    StatusDesc = addStatus.StatusDesc,
                                }
                            },
                            BillInqRs = new BillInqRs
                            {
                                BillRec = new BillRec
                                {
                                    BillingAcct = processRequest.arg0.Request.PresSvcRq.BillInqRq.BillingAcct,
                                    BillTypeCode = processRequest.arg0.Request.PresSvcRq.BillInqRq.BillTypeCode,
                                }
                            },
                        }
                    }
                }
            };


            return processRequestResponse;

        }

        public static processRequestResponse PaymentFailResponse(processRequest processRequest, AdditionalStatus addStatus)
        {
            processRequestResponse processRequestResponse = new processRequestResponse
            {
                Return = new Return
                {
                    Response = new Response
                    {
                        SignonRs = new SignonRs
                        {
                            ServerDt = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                            Language = processRequest.arg0.Request.SignonRq.CustLangPref,
                            SignonProfile = new SignonProfile
                            {
                                Sender = processRequest.arg0.Request.SignonRq.SignonProfile.Receiver,
                                MsgCode = "PmtNotifyRs",
                                Version = processRequest.arg0.Request.SignonRq.SignonProfile.Version
                            }
                        },
                        PaySvcRs = new PaySvcRs
                        {
                            RqUID = processRequest.arg0.Request.PaySvcRq.RqUID,
                            Status = new Status
                            {
                                StatusCode = "21021",
                                Severity = "Error",
                                StatusDesc = "Failed to retrieve bill information due to system error",
                                AdditionalStatus = addStatus
                            }
                        }
                    }
                }
            };


            return processRequestResponse;

        }

    }
}