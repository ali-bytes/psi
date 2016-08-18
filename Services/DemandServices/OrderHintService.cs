using System;
using System.Collections.Generic;
using System.Linq;
using Db;
using NewIspNL.Models;

namespace NewIspNL.Services.DemandServices{
    public class OrderHintService{
        readonly ISPDataContext _context;


        public OrderHintService(ISPDataContext context){
            _context = context;
        }


        public WorkOrderNote Create(int orderId, bool processed, int userId, DateTime time, string hint, int to){
            return new WorkOrderNote{
                WorkOrderId = orderId,
                Processed = processed,
                UserId = userId,
                Time = time,
                Text = hint,
                ToUserId = to
            };
        }


        public WorkOrderNote Submit(int orderId, bool processed, int userId, DateTime time, string hint, int to, bool commit = false){
            var note = Create(orderId, processed, userId, time, hint, to);
            _context.WorkOrderNotes.InsertOnSubmit(note);
            if(commit) _context.SubmitChanges();
            return note;
        }


        public List<WorkOrderNote> Search(List<WorkOrder> orders, bool processed){
            var notes = new List<WorkOrderNote>();
            orders.ForEach(x => notes.AddRange(x.WorkOrderNotes.Where(h => h.Processed == processed)));
            return notes;
        }


        public List<OrderNoteModel> SearchModels(List<WorkOrder> orders, bool processed, int userId){
            var allNotes = new List<WorkOrderNote>();
            var user = _context.Users.FirstOrDefault(u => u.ID == userId);
            if(user != null && user.GroupID != null && user.GroupID.Value == 1){
                foreach(var order in orders){
                    var notes = _context.WorkOrderNotes
                        .Where(x => x.WorkOrderId == order.ID
                                    && x.Processed == processed).ToList();
                    allNotes.AddRange(notes);
                }
            } else{
                foreach(var order in orders){
                    var notes = _context.WorkOrderNotes
                        .Where(x => x.ToUserId == userId && x.WorkOrderId == order.ID
                                    && x.Processed == processed).ToList();
                    allNotes.AddRange(notes);
                }
            }


            return allNotes.Select(x => OrderNoteModel.To(x, _context)).ToList();
        }



        public bool Process(int noteId, bool processed, string comment){
            var note = _context.WorkOrderNotes.FirstOrDefault(x => x.Id == noteId);
            if(note == null) return false;
            note.Processed = processed;
            note.Comment = comment;
            _context.SubmitChanges();
            return true;
        }


        public List<WorkOrderNote> OrderNotes(int id){
            return _context.WorkOrderNotes.Where(x => x.WorkOrderId == id).OrderByDescending(x => x.Id).ToList();
        }
    }
}
