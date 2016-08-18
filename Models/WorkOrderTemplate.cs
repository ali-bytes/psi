using System;
using Db;
using NewIspNL.Domain.Concrete;
using Resources;

namespace NewIspNL.Models{
    public class WorkOrderTemplate{
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Package { get; set; }

        public string Governate { get; set; }

        public string Central { get; set; }

        public string State { get; set; }

        public string Provider { get; set; }

        public string Reseller { get; set; }

        public string Branch { get; set; }

        public DateTime Activation { get; set; }

        public string TActivation { get; set; }

        public string Offer { get; set; }

        public int Id { get; set; }
    }

    public class OrderNoteModel : WorkOrderTemplate{
        public string Note { get; set; }

        public int NoteId { get; set; }

        public DateTime Time { get; set; }

        public string User { get; set; }

        public bool Processed { get; set; }

        public string TProcessed { get; set; }


        public static OrderNoteModel To(WorkOrderNote note,ISPDataContext context){
            var order = note.WorkOrder;
            var basicData = WorkOrderRepository.GetOrderBasicData(order, context);
            return new OrderNoteModel{
                Offer = basicData.Offer,
                Package = basicData.Package,
                Branch = basicData.Branch,
                Activation = basicData.ActivationDate,
                TActivation = basicData.TActivationDate,
                Central = basicData.Central,
                Reseller = basicData.Reseller,
                Governate = basicData.Govornorate,
                Provider = basicData.Provider,
                State = basicData.State,
                Id = basicData.Id,
                Phone = order.CustomerPhone,
                Name = order.CustomerName,
                Time = note.Time,
                User = note.User.UserName,
                Processed = note.Processed,
                TProcessed = note.Processed ? Tokens.Yes : Tokens.No,
                Note = note.Text,
                NoteId = note.Id
            };
        }
    }
}
