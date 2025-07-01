using eventLib.Models;
using System.Reflection;
using System.Text.Json.Nodes;
using Microsoft.Data.SqlClient;
using Azure;
using System.Data;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.Extensions.Logging;


namespace eventLib.Dal
{
    // COLLAPSE CTRL+M+O <-----

    internal class SqlRepository : IRepository
    {

        private static string? sqlcon = Config.defaultSqlCon;
        private static int nullIntVal; // solving problem with int.TryParse
        private static double nullDoubleVal; // solving problem with double.TryParse
        private static bool nullBoolVal; // solving problem with bool.TryParse


        // USER
        public int UserAdd(User user)
        {

            //await connection.OpenAsync(); // ovo treba natjerati da radi!
            using var connection = new SqlConnection(sqlcon);
            connection.Open();

            using SqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(User.Username), user.Username);
            cmd.Parameters.AddWithValue(nameof(User.PwdHash), user.PwdHash);
            cmd.Parameters.AddWithValue(nameof(User.PwdSalt), user.PwdSalt);
            cmd.Parameters.AddWithValue(nameof(User.FirstName), user.FirstName);
            cmd.Parameters.AddWithValue(nameof(User.LastName), user.LastName);
            cmd.Parameters.AddWithValue(nameof(User.Email), user.Email);
            cmd.Parameters.AddWithValue(nameof(User.Phone), user.Phone);
            cmd.Parameters.AddWithValue(nameof(User.UserRoleId), user.UserRoleId);

            var id = new SqlParameter(nameof(User.IDUser), System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(id);
            
            cmd.ExecuteNonQuery();

            return (int)id.Value;

        }
        public void UserDelete(int? id)
        {
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(User.IDUser), id);

            cmd.ExecuteScalar();
 


        }
        // All users
        public IList<User> UsersGet()
        {
            IList<User> list = new List<User>();

            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(UserRead(dr));
            }

            return list;
        }
        private User UserRead(SqlDataReader dr) => new User
        {
            IDUser = int.TryParse(dr[nameof(User.IDUser)].ToString(), out nullIntVal) ? nullIntVal : null,

            Username = dr[nameof(User.Username)].ToString(),
            PwdHash = dr[nameof(User.PwdHash)].ToString(),
            PwdSalt = dr[nameof(User.PwdSalt)].ToString(),
            FirstName = dr[nameof(User.FirstName)].ToString(),
            LastName = dr[nameof(User.LastName)].ToString(),
            Email = dr[nameof(User.Email)].ToString(),
            Phone = dr[nameof(User.Phone)].ToString(),
            RoleName = dr[nameof(User.RoleName)].ToString(),

            UserRoleId = int.TryParse(dr[nameof(User.UserRoleId)].ToString(), out nullIntVal) ? nullIntVal : null

        };
        // Single user
        public User UserGet(int? idUser, string? username)
        {
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(User.IDUser), idUser);
            cmd.Parameters.AddWithValue(nameof(User.Username), username);

            using SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return UserRead(dr);
            }

            return null;

        }

        public void UserUpdate(User user)
        {
            using var connection = new SqlConnection(sqlcon);

            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(nameof(User.IDUser), user.IDUser);
            cmd.Parameters.AddWithValue(nameof(User.Username), user.Username);
            cmd.Parameters.AddWithValue(nameof(User.PwdHash), user.PwdHash);
            cmd.Parameters.AddWithValue(nameof(User.PwdSalt), user.PwdSalt);
            cmd.Parameters.AddWithValue(nameof(User.FirstName), user.FirstName);
            cmd.Parameters.AddWithValue(nameof(User.LastName), user.LastName);
            cmd.Parameters.AddWithValue(nameof(User.Email), user.Email);
            cmd.Parameters.AddWithValue(nameof(User.Phone), user.Phone);
            cmd.Parameters.AddWithValue(nameof(User.UserRoleId), user.UserRoleId);

            cmd.ExecuteNonQuery();


        }




        public IList<UserRole> UserRolesGet()
        {
            IList<UserRole> userRoles = new List<UserRole>();
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                userRoles.Add(UserRoleRead(dr));
            }

            return userRoles;

        }
        private UserRole UserRoleRead(SqlDataReader dr) => new UserRole
        {
            IDUserRole = int.TryParse(dr[nameof(UserRole.IDUserRole)].ToString(), out nullIntVal) ? nullIntVal : null,
            RoleName = dr[nameof(UserRole.RoleName)].ToString()
        };

        #region EVENTS
        public IList<Event> EventsGet(string search)
        {
            IList<Event> list = new List<Event>();

            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("search", search);

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(EventRead(dr));
            }

            return list;
        }



        public Event EventGet(int? idEvent)
        {
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(Event.IDEvent), idEvent);

            using SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return EventRead(dr);
            }

            return null;

        }

        private Event EventRead(SqlDataReader dr) => new Event
        {
            IDEvent = int.TryParse(dr[nameof(Event.IDEvent)].ToString(), out nullIntVal) ? nullIntVal : null,

            EventName = dr[nameof(Event.EventName)].ToString(),
            Description = dr[nameof(Event.Description)].ToString(),
            eventTypeID = int.TryParse(dr[nameof(Event.eventTypeID)].ToString(), out nullIntVal) ? nullIntVal : null,
            EventTypeName = dr[nameof(Event.EventTypeName)].ToString(),
            Date = (DateTime)dr[nameof(Event.Date)],
            ImageID = int.TryParse(dr[nameof(Event.ImageID)].ToString(), out nullIntVal) ? nullIntVal : null,
            ImageName = dr[nameof(Event.ImageName)].ToString(),
            ImageData = (byte[])dr[nameof(Event.ImageData)]

        };

        public void EventUpdate(Event? value)
        {
            using var connection = new SqlConnection(sqlcon);

            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(Event.IDEvent), value.IDEvent);
            cmd.Parameters.AddWithValue(nameof(Event.EventName), value.EventName);
            cmd.Parameters.AddWithValue(nameof(Event.Date), value.Date);
            cmd.Parameters.AddWithValue(nameof(Event.eventTypeID), value.eventTypeID);
            cmd.Parameters.AddWithValue(nameof(Event.Description), value.Description);
            cmd.Parameters.AddWithValue(nameof(Event.ImageID), value.ImageID);
            cmd.Parameters.AddWithValue(nameof(Event.ImageName), value.ImageName);
            cmd.Parameters.AddWithValue(nameof(Event.ImageData), value.ImageData);


            cmd.ExecuteNonQuery();

        }



        public int EventAdd(Event value)
        {

            //await connection.OpenAsync(); // ovo treba natjerati da radi!
            using var connection = new SqlConnection(sqlcon);
            connection.Open();

            using SqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(Event.EventName), value.EventName);
            cmd.Parameters.AddWithValue(nameof(Event.Description), value.Description);
            cmd.Parameters.AddWithValue(nameof(Event.Date), value.Date);
            cmd.Parameters.AddWithValue(nameof(Event.eventTypeID), value.eventTypeID);
            cmd.Parameters.AddWithValue(nameof(Event.ImageName), value.ImageName);
            cmd.Parameters.AddWithValue(nameof(Event.ImageData), value.ImageData);

            var id = new SqlParameter(nameof(Event.IDEvent), System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(id);

            cmd.ExecuteNonQuery();

            return (int)id.Value;

        }
        public void EventDelete(int? eventID)
        {
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(EventPerformer.EventID), eventID);


            cmd.ExecuteScalar();

        }



        public IList<EventType> EventTypesGet()
        {
            IList<EventType> eventTypes = new List<EventType>();
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                eventTypes.Add(EventTypeRead(dr));
            }

            return eventTypes;

        }
        private EventType EventTypeRead(SqlDataReader dr) => new EventType
        {
            IDEventType = int.TryParse(dr[nameof(EventType.IDEventType)].ToString(), out nullIntVal) ? nullIntVal : null,
            EventTypeName = dr[nameof(EventType.EventTypeName)].ToString()
        };
        #endregion EVENTS

        #region EventPerformer
        public IList<EventPerformer> EventPerformersGet(int? eventID)
        {
            IList<EventPerformer> list = new List<EventPerformer>();
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(EventPerformer.EventID), eventID);

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(EventPerformerRead(dr));
            }

            return list;

        }

        private EventPerformer EventPerformerRead(SqlDataReader dr) => new EventPerformer
        {
            IDEventPerformer = int.TryParse(dr[nameof(EventPerformer.IDEventPerformer)].ToString(), out nullIntVal) ? nullIntVal : null,
            EventID = int.TryParse(dr[nameof(EventPerformer.EventID)].ToString(), out nullIntVal) ? nullIntVal : null,
            PerformerID = int.TryParse(dr[nameof(EventPerformer.PerformerID)].ToString(), out nullIntVal) ? nullIntVal : null,
            PerformerName = dr[nameof(EventPerformer.PerformerName)].ToString()
        };



        public void EventPerformerAdd(int? eventID, int? performerID)
        {

            //await connection.OpenAsync(); // ovo treba natjerati da radi!
            using var connection = new SqlConnection(sqlcon);
            connection.Open();

            using SqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(EventPerformer.EventID), eventID);
            cmd.Parameters.AddWithValue(nameof(EventPerformer.PerformerID), performerID);

            cmd.ExecuteNonQuery();


        }
        public void EventPerformerDelete(int? eventID, int? performerID)
        {
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(EventPerformer.EventID), eventID);
            cmd.Parameters.AddWithValue(nameof(EventPerformer.PerformerID), performerID);
            

            cmd.ExecuteScalar();
            //using SqlDataReader dr = cmd.ExecuteReader();

        }


        #endregion EventPerformer


        #region Performer

        public IList<Performer> PerformersGet(string? search = null)
        {
            IList<Performer> list = new List<Performer>();
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("search", search);

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(PerformerRead(dr));
            }

            return list;

        }

        private Performer PerformerRead(SqlDataReader dr) => new Performer
        {
            IDPerformer = int.TryParse(dr[nameof(Performer.IDPerformer)].ToString(), out nullIntVal) ? nullIntVal : null,
            PerformerName = dr[nameof(Performer.PerformerName)].ToString()
        };


        public void PerformerAdd(Performer performer)
        {

            //await connection.OpenAsync(); // ovo treba natjerati da radi!
            using var connection = new SqlConnection(sqlcon);
            connection.Open();

            using SqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(Performer.PerformerName), performer.PerformerName);

            cmd.ExecuteNonQuery();

        }

        public Performer PerformerGet(int? idPerformer)
        {
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(Performer.IDPerformer), idPerformer);

            using SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return PerformerRead(dr);
            }

            return null;

        }


        public void PerformerUpdate(Performer? value)
        {
            using var connection = new SqlConnection(sqlcon);

            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(Performer.IDPerformer), value.IDPerformer);
            cmd.Parameters.AddWithValue(nameof(Performer.PerformerName), value.PerformerName);

            cmd.ExecuteNonQuery();

        }

        public void PerformerDelete(int? idPerformer)
        {
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(Performer.IDPerformer), idPerformer);


            cmd.ExecuteScalar();

        }

        #endregion Performer


        #region EventRegistration
        public IList<EventRegistration> EventRegistrationsGet(int? userID, string? search)
        {
            IList<EventRegistration> list = new List<EventRegistration>();
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(EventRegistration.UserID), userID);
            cmd.Parameters.AddWithValue("search", search);

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(EventRegistrationRead(dr));
            }

            return list;

        }

        private EventRegistration EventRegistrationRead(SqlDataReader dr) => new EventRegistration
        {
            IDEventRegistration = int.TryParse(dr[nameof(EventRegistration.IDEventRegistration)].ToString(), out nullIntVal) ? nullIntVal : null,
            EventID = int.TryParse(dr[nameof(EventRegistration.EventID)].ToString(), out nullIntVal) ? nullIntVal : null,
            UserID = int.TryParse(dr[nameof(EventRegistration.UserID)].ToString(), out nullIntVal) ? nullIntVal : null,
            EventName = dr[nameof(EventRegistration.EventName)].ToString(),
            Description = dr[nameof(EventRegistration.Description)].ToString(),
            EventTypeName = dr[nameof(EventRegistration.EventTypeName)].ToString(),
            Date = (DateTime)dr[nameof(EventRegistration.Date)],
            ImageName = dr[nameof(Event.ImageName)].ToString(),
            ImageData = (byte[])dr[nameof(Event.ImageData)]
        };



        public void EventRegistrationAdd(int? eventID, int? userID)
        {

            //await connection.OpenAsync(); // ovo treba natjerati da radi!
            using var connection = new SqlConnection(sqlcon);
            connection.Open();

            using SqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(EventRegistration.EventID), eventID);
            cmd.Parameters.AddWithValue(nameof(EventRegistration.UserID), userID);

            cmd.ExecuteNonQuery();


        }
        public void EventRegistrationDelete(int? eventID, int? userID)
        {
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(EventRegistration.EventID), eventID);
            cmd.Parameters.AddWithValue(nameof(EventRegistration.UserID), userID);


            cmd.ExecuteScalar();
            //using SqlDataReader dr = cmd.ExecuteReader();

        }

        public IList<Event> MyEventsGet(string search)
        {
            IList<Event> list = new List<Event>();

            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("search", search);

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(EventRead(dr));
            }

            return list;
        }
        #endregion EventRegistration


        #region Logs

        public IList<LogModel> LogsGet(int? limit, string? search)
        {
            IList<LogModel> list = new List<LogModel>();
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("limit", limit);
            cmd.Parameters.AddWithValue("search", search);

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(LogRead(dr));
            }

            return list;

        }

        private LogModel LogRead(SqlDataReader dr) => new LogModel
        {
            IDLog = int.TryParse(dr[nameof(LogModel.IDLog)].ToString(), out nullIntVal) ? nullIntVal : null,
            IDLevel = int.TryParse(dr[nameof(LogModel.IDLevel)].ToString(), out nullIntVal) ? nullIntVal : null,
            LevelName = dr[nameof(LogModel.LevelName)].ToString(),
            InfoMessage = dr[nameof(LogModel.InfoMessage)].ToString(),
            Timestamp = (DateTime)dr[nameof(LogModel.Timestamp)]
        };

        public int LogsCount()
        {
            int count = -1;
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;


            using SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                count = dr.GetInt32(0);
            }

            return count;

        }


        public void LogAdd(int? level, string? infoMessage)
        {

            //await connection.OpenAsync(); // ovo treba natjerati da radi!
            using var connection = new SqlConnection(sqlcon);
            connection.Open();

            using SqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("LevelID", level);
            cmd.Parameters.AddWithValue("InfoMessage", infoMessage);

            cmd.ExecuteNonQuery();



        }


        #endregion Logs


        #region Images
        private LogModel ImageRead(SqlDataReader dr) => new LogModel
        {
            IDLog = int.TryParse(dr[nameof(LogModel.IDLog)].ToString(), out nullIntVal) ? nullIntVal : null,
            IDLevel = int.TryParse(dr[nameof(LogModel.IDLevel)].ToString(), out nullIntVal) ? nullIntVal : null,
            LevelName = dr[nameof(LogModel.LevelName)].ToString(),
            InfoMessage = dr[nameof(LogModel.InfoMessage)].ToString(),
            Timestamp = (DateTime)dr[nameof(LogModel.Timestamp)]
        };

        public void ImageAdd(int? level, string? infoMessage)
        {

            //await connection.OpenAsync(); // ovo treba natjerati da radi!
            using var connection = new SqlConnection(sqlcon);
            connection.Open();

            using SqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("LevelID", level);
            cmd.Parameters.AddWithValue("InfoMessage", infoMessage);

            cmd.ExecuteNonQuery();



        }


        public void ImageDelete(int? idImage)
        {
            using var connection = new SqlConnection(sqlcon);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(nameof(Image.IDImage), idImage);


            cmd.ExecuteScalar();

        }

        #endregion
    }
}
