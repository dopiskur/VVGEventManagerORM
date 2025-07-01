using eventLib.Models;
using System.Text.Json.Nodes;

namespace eventLib.Dal
{
    public interface IRepository
    {
        // Server Config

        #region USER
        // MANAGE USER
        int UserAdd(User user);
        void UserUpdate(User user);
        void UserDelete(int? idUser);
        User UserGet(int? idUser, string? username);
        IList<User> UsersGet();
        IList<UserRole> UserRolesGet();



        #endregion

        #region
        // Manage Event
        int EventAdd(Event value);
        void EventUpdate(Event value);
        void EventDelete(int? idEvent);
        Event EventGet(int? idEvent);
        IList<Event> EventsGet(string search);
        IList<EventType> EventTypesGet();


        #endregion

        #region
        // Manage EventPerformer
        void EventPerformerAdd(int? eventID, int? performerID);
        void EventPerformerDelete(int? eventID, int? performerID);
        IList<EventPerformer> EventPerformersGet(int? eventID);
        #endregion

        #region Performer
        IList<Performer> PerformersGet(string? search);

        void PerformerAdd(Performer performer);

        Performer PerformerGet(int? idPerformer);

        void PerformerUpdate(Performer? value);

        void PerformerDelete(int? idPerformer);

        #endregion Performer


        #region Event registration

        IList<Event> MyEventsGet(string search);
        IList<EventRegistration> EventRegistrationsGet(int? userID, string? search);

        void EventRegistrationAdd(int? eventID, int? userID);

        void EventRegistrationDelete(int? eventID, int? userID);


        #endregion

        #region Logs
        int LogsCount();
        IList<LogModel> LogsGet(int? limit, string? search);
        void LogAdd(int? level, string? infoMessage);
        #endregion Logs


    }
}
