using System.Collections.Generic;
using System.Text;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Models{
    public class DemandsReportContainer{
        public List<Demand> After { get; set; }
        public string Title { get; set; }


        public override string ToString(){
            var tagEngine = new StringBuilder();

            if(!string.IsNullOrWhiteSpace(Title)){
                tagEngine.Append("<div>");
                tagEngine.Append(Title);
                tagEngine.Append("</div>");
            }
            tagEngine.Append("<div class=\"well\">");
            tagEngine.Append("<div>");
            tagEngine.Append("<table>");
            tagEngine.Append("<table class='table table-bordered table-condensed table-striped'>");
            tagEngine.Append("<thead>");
            tagEngine.Append("<tr>");

            tagEngine.Append("<th>");
            tagEngine.Append(Tokens.Customer);
            tagEngine.Append("</th>");
            tagEngine.Append("<th>");
            tagEngine.Append(Tokens.Phone);
            tagEngine.Append("</th>");
            tagEngine.Append("<th>");
            tagEngine.Append(Tokens.Offer);
            tagEngine.Append("</th>");
            tagEngine.Append("<th>");
            tagEngine.Append(Tokens.From);
            tagEngine.Append("</th>");

            tagEngine.Append("<th>");
            tagEngine.Append(Tokens.To);
            tagEngine.Append("</th>");

            tagEngine.Append("<th>");
            tagEngine.Append(Tokens.Amount);
            tagEngine.Append("</th>");
            
            tagEngine.Append("<th>");
            tagEngine.Append(Tokens.Notes);
            tagEngine.Append("</th>");

            tagEngine.Append("</tr>");
            tagEngine.Append("</thead>");
            tagEngine.Append("<tbody>");

            foreach(var demand in After){
                CreateRow(demand, tagEngine);
            }
            tagEngine.Append("</tbody>");
            tagEngine.Append("</table>");
            tagEngine.Append("</div>");

            tagEngine.Append("</div>");

            return tagEngine.ToString();
        }


        void CreateRow(Demand demand, StringBuilder tagEngine){
            tagEngine.Append("<tr>");
            tagEngine.Append("<td>");
            tagEngine.Append(demand.WorkOrder.CustomerName);
            tagEngine.Append("</td>");
            tagEngine.Append("<td>");
            tagEngine.Append(demand.WorkOrder.CustomerPhone);
            tagEngine.Append("</td>");
            tagEngine.Append("<td>");
            tagEngine.Append(demand.WorkOrder.Offer == null ? "-" : demand.WorkOrder.Offer.Title);
            tagEngine.Append("</td>");
            tagEngine.Append("<td>");
            tagEngine.Append(demand.StartAt.ToShortDateString());
            tagEngine.Append("</td>");

            tagEngine.Append("<td>");
            tagEngine.Append(demand.EndAt.ToShortDateString());
            tagEngine.Append("</td>");
           

            tagEngine.Append("<td>");
            tagEngine.Append(Helper.FixNumberFormat(demand.Amount));
            tagEngine.Append("</td>");
            tagEngine.Append("<td>");
            tagEngine.Append(demand.Notes);
            tagEngine.Append("</td>");
            tagEngine.Append("</tr>");
        }
    }
}
