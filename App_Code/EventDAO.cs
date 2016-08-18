using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace NewIspNL.App_Code
{
    public class EventDAO
    {

        //change the connection string as per your database connection.
        //private static string connectionString = "Data Source=.;Initial Catalog=angular;Integrated Security=True";
        private static string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

        //this method retrieves all events within range start-end
        public static List<CalendarEvent> getEvents(DateTime start, DateTime end)
        {

            List<CalendarEvent> events = new List<CalendarEvent>();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd =
                new SqlCommand(
                    "SELECT event_id, description, title, event_start, event_end FROM event where UserId=@userid AND event_start>=@start AND event_end<=@end",
                    con);
            cmd.Parameters.AddWithValue("@userid", HttpContext.Current.Session["User_ID"]);
            cmd.Parameters.AddWithValue("@start", start);
            cmd.Parameters.AddWithValue("@end", end);

            try
            {
                using (con)
                {
                    if (con.State!= ConnectionState.Open || con.State!= ConnectionState.Connecting)
                    {
                       
                        con.Open();
                    }
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CalendarEvent cevent = new CalendarEvent();
                        cevent.id = (int)reader["event_id"];
                        cevent.title = (string)reader["title"];
                        cevent.description = (string)reader["description"];
                        cevent.start = (DateTime)reader["event_start"];
                        cevent.end = (DateTime)reader["event_end"];
                        events.Add(cevent);
                    }
                }
            }
            catch 
            {
                
                
            }
               
                
            
            return events;
            //side note: if you want to show events only related to particular users,
            //if user id of that user is stored in session as Session["userid"]
            //the event table also contains a extra field named 'user_id' to mark the event for that particular user
            //then you can modify the SQL as:
            //SELECT event_id, description, title, event_start, event_end FROM event where user_id=@user_id AND event_start>=@start AND event_end<=@end
            //then add paramter as:cmd.Parameters.AddWithValue("@user_id", HttpContext.Current.Session["userid"]);
        }

        //this method updates the event title and description
        public static void updateEvent(int id, String title, String description)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd =
                new SqlCommand("UPDATE event SET title=@title, description=@description WHERE event_id=@event_id", con);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@event_id", id);
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }


        }

        //this method updates the event start and end time
        public static void updateEventTime(int id, DateTime start, DateTime end)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd =
                new SqlCommand(
                    "UPDATE event SET event_start=@event_start, event_end=@event_end WHERE event_id=@event_id", con);
            cmd.Parameters.AddWithValue("@event_start", start);
            cmd.Parameters.AddWithValue("@event_end", end);
            cmd.Parameters.AddWithValue("@event_id", id);
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //this mehtod deletes event with the id passed in.
        public static void deleteEvent(int id)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("DELETE FROM event WHERE (event_id = @event_id)", con);
            cmd.Parameters.AddWithValue("@event_id", id);
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //this method adds events to the database
        public static int addEvent(CalendarEvent cevent)
        {
            //add event to the database and return the primary key of the added event row

            //insert
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd =
                new SqlCommand(
                    "INSERT INTO event(UserId, title, description, event_start, event_end) VALUES(@userid, @title, @description, @event_start, @event_end)",
                    con);
            cmd.Parameters.AddWithValue("@userid", HttpContext.Current.Session["User_ID"]);
            cmd.Parameters.AddWithValue("@title", cevent.title);
            cmd.Parameters.AddWithValue("@description", cevent.description);
            cmd.Parameters.AddWithValue("@event_start", cevent.start);
            cmd.Parameters.AddWithValue("@event_end", cevent.end);

            int key = 0;
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();

                //get primary key of inserted row
                cmd =
                    new SqlCommand(
                        "SELECT max(event_id) FROM event where UserId=@userid AND title=@title AND description=@description AND event_start=@event_start AND event_end=@event_end",
                        con);
                cmd.Parameters.AddWithValue("@userid", HttpContext.Current.Session["User_ID"]);
                cmd.Parameters.AddWithValue("@title", cevent.title);
                cmd.Parameters.AddWithValue("@description", cevent.description);
                cmd.Parameters.AddWithValue("@event_start", cevent.start);
                cmd.Parameters.AddWithValue("@event_end", cevent.end);

                key = (int)cmd.ExecuteScalar();
            }

            return key;

        }




    }
}