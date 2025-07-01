using eventLib.Models;
using eventLib.Data;
using Microsoft.EntityFrameworkCore;

namespace eventLib.Dal
{
    public class EfRepository : IRepository
    {
        private readonly EventManagerContext _context;

        public EfRepository(EventManagerContext context)
        {
            _context = context;
        }

        #region USER
        public int UserAdd(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.IDUser ?? 0;
        }

        public void UserDelete(int? id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public IList<User> UsersGet()
        {
            return _context.Users
                .Include(u => u.UserRole)
                .Select(u => new User
                {
                    IDUser = u.IDUser,
                    Username = u.Username,
                    PwdHash = u.PwdHash,
                    PwdSalt = u.PwdSalt,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Phone = u.Phone,
                    UserRoleId = u.UserRoleId,
                    RoleName = u.UserRole != null ? u.UserRole.RoleName : null
                })
                .ToList();
        }

        public User UserGet(int? idUser, string? username)
        {
            var query = _context.Users.Include(u => u.UserRole).AsQueryable();
            
            if (idUser.HasValue)
                query = query.Where(u => u.IDUser == idUser);
            
            if (!string.IsNullOrEmpty(username))
                query = query.Where(u => u.Username == username);

            var user = query.FirstOrDefault();
            
            if (user != null)
            {
                return new User
                {
                    IDUser = user.IDUser,
                    Username = user.Username,
                    PwdHash = user.PwdHash,
                    PwdSalt = user.PwdSalt,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    UserRoleId = user.UserRoleId,
                    RoleName = user.UserRole != null ? user.UserRole.RoleName : null
                };
            }

            return null;
        }

        public void UserUpdate(User user)
        {
            var existingUser = _context.Users.Find(user.IDUser);
            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.PwdHash = user.PwdHash;
                existingUser.PwdSalt = user.PwdSalt;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.UserRoleId = user.UserRoleId;
                
                _context.SaveChanges();
            }
        }

        public IList<UserRole> UserRolesGet()
        {
            return _context.UserRoles.ToList();
        }
        #endregion

        #region EVENTS
        public IList<Event> EventsGet(string search)
        {
            var query = _context.Events
                .Include(e => e.EventType)
                .Include(e => e.Image)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.EventName.Contains(search) || 
                                        e.Description.Contains(search) ||
                                        (e.EventType != null && e.EventType.EventTypeName.Contains(search)));
            }

            return query.Select(e => new Event
            {
                IDEvent = e.IDEvent,
                EventName = e.EventName,
                Date = e.Date,
                Description = e.Description,
                eventTypeID = e.eventTypeID,
                EventTypeName = e.EventType != null ? e.EventType.EventTypeName : null,
                ImageID = e.ImageID,
                ImageName = e.Image != null ? e.Image.ImageName : null,
                ImageData = e.Image != null ? e.Image.ImageData : null
            }).ToList();
        }

        public Event EventGet(int? idEvent)
        {
            var evt = _context.Events
                .Include(e => e.EventType)
                .Include(e => e.Image)
                .FirstOrDefault(e => e.IDEvent == idEvent);

            if (evt != null)
            {
                return new Event
                {
                    IDEvent = evt.IDEvent,
                    EventName = evt.EventName,
                    Date = evt.Date,
                    Description = evt.Description,
                    eventTypeID = evt.eventTypeID,
                    EventTypeName = evt.EventType != null ? evt.EventType.EventTypeName : null,
                    ImageID = evt.ImageID,
                    ImageName = evt.Image != null ? evt.Image.ImageName : null,
                    ImageData = evt.Image != null ? evt.Image.ImageData : null
                };
            }

            return null;
        }

        public void EventUpdate(Event value)
        {
            var existingEvent = _context.Events.Find(value.IDEvent);
            if (existingEvent != null)
            {
                existingEvent.EventName = value.EventName;
                existingEvent.Date = value.Date;
                existingEvent.Description = value.Description;
                existingEvent.eventTypeID = value.eventTypeID;
                existingEvent.ImageID = value.ImageID;
                existingEvent.ImageName = value.ImageName;
                existingEvent.ImageData = value.ImageData;
                
                _context.SaveChanges();
            }
        }

        public int EventAdd(Event value)
        {
            _context.Events.Add(value);
            _context.SaveChanges();
            return value.IDEvent ?? 0;
        }

        public void EventDelete(int? eventID)
        {
            var evt = _context.Events.Find(eventID);
            if (evt != null)
            {
                _context.Events.Remove(evt);
                _context.SaveChanges();
            }
        }

        public IList<EventType> EventTypesGet()
        {
            return _context.EventTypes.ToList();
        }
        #endregion

        #region EVENT PERFORMER
        public IList<EventPerformer> EventPerformersGet(int? eventID)
        {
            return _context.EventPerformers
                .Include(ep => ep.Performer)
                .Where(ep => ep.EventID == eventID)
                .Select(ep => new EventPerformer
                {
                    IDEventPerformer = ep.IDEventPerformer,
                    EventID = ep.EventID,
                    PerformerID = ep.PerformerID,
                    PerformerName = ep.Performer != null ? ep.Performer.PerformerName : null
                })
                .ToList();
        }

        public void EventPerformerAdd(int? eventID, int? performerID)
        {
            var eventPerformer = new EventPerformer
            {
                EventID = eventID,
                PerformerID = performerID
            };
            
            _context.EventPerformers.Add(eventPerformer);
            _context.SaveChanges();
        }

        public void EventPerformerDelete(int? eventID, int? performerID)
        {
            var eventPerformer = _context.EventPerformers
                .FirstOrDefault(ep => ep.EventID == eventID && ep.PerformerID == performerID);
                
            if (eventPerformer != null)
            {
                _context.EventPerformers.Remove(eventPerformer);
                _context.SaveChanges();
            }
        }
        #endregion

        #region PERFORMER
        public IList<Performer> PerformersGet(string? search = null)
        {
            var query = _context.Performers.AsQueryable();
            
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.PerformerName.Contains(search));
            }
            
            return query.ToList();
        }

        public void PerformerAdd(Performer performer)
        {
            _context.Performers.Add(performer);
            _context.SaveChanges();
        }

        public Performer PerformerGet(int? idPerformer)
        {
            return _context.Performers.Find(idPerformer);
        }

        public void PerformerUpdate(Performer value)
        {
            var existingPerformer = _context.Performers.Find(value.IDPerformer);
            if (existingPerformer != null)
            {
                existingPerformer.PerformerName = value.PerformerName;
                _context.SaveChanges();
            }
        }

        public void PerformerDelete(int? idPerformer)
        {
            var performer = _context.Performers.Find(idPerformer);
            if (performer != null)
            {
                _context.Performers.Remove(performer);
                _context.SaveChanges();
            }
        }
        #endregion

        #region EVENT REGISTRATION
        public IList<Event> MyEventsGet(string search)
        {
            var query = _context.Events
                .Include(e => e.EventType)
                .Include(e => e.Image)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.EventName.Contains(search) || 
                                        e.Description.Contains(search) ||
                                        (e.EventType != null && e.EventType.EventTypeName.Contains(search)));
            }

            return query.Select(e => new Event
            {
                IDEvent = e.IDEvent,
                EventName = e.EventName,
                Date = e.Date,
                Description = e.Description,
                eventTypeID = e.eventTypeID,
                EventTypeName = e.EventType != null ? e.EventType.EventTypeName : null,
                ImageID = e.ImageID,
                ImageName = e.Image != null ? e.Image.ImageName : null,
                ImageData = e.Image != null ? e.Image.ImageData : null
            }).ToList();
        }

        public IList<EventRegistration> EventRegistrationsGet(int? userID, string? search)
        {
            var query = _context.EventRegistrations
                .Include(er => er.Event)
                .ThenInclude(e => e.EventType)
                .Include(er => er.Event)
                .ThenInclude(e => e.Image)
                .AsQueryable();

            if (userID.HasValue)
                query = query.Where(er => er.UserID == userID);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(er => (er.Event != null && er.Event.EventName.Contains(search)) || 
                                         (er.Event != null && er.Event.Description.Contains(search)) ||
                                         (er.Event != null && er.Event.EventType != null && er.Event.EventType.EventTypeName.Contains(search)));
            }

            return query.Select(er => new EventRegistration
            {
                IDEventRegistration = er.IDEventRegistration,
                EventID = er.EventID,
                UserID = er.UserID,
                EventName = er.Event != null ? er.Event.EventName : null,
                Description = er.Event != null ? er.Event.Description : null,
                EventTypeName = er.Event != null && er.Event.EventType != null ? er.Event.EventType.EventTypeName : null,
                Date = er.Event != null ? er.Event.Date : null,
                ImageName = er.Event != null && er.Event.Image != null ? er.Event.Image.ImageName : null,
                ImageData = er.Event != null && er.Event.Image != null ? er.Event.Image.ImageData : null
            }).ToList();
        }

        public void EventRegistrationAdd(int? eventID, int? userID)
        {
            var registration = new EventRegistration
            {
                EventID = eventID,
                UserID = userID
            };
            
            _context.EventRegistrations.Add(registration);
            _context.SaveChanges();
        }

        public void EventRegistrationDelete(int? eventID, int? userID)
        {
            var registration = _context.EventRegistrations
                .FirstOrDefault(er => er.EventID == eventID && er.UserID == userID);
                
            if (registration != null)
            {
                _context.EventRegistrations.Remove(registration);
                _context.SaveChanges();
            }
        }
        #endregion

        #region LOGS
        public int LogsCount()
        {
            return _context.Logs.Count();
        }

        public IList<LogModel> LogsGet(int? limit, string? search)
        {
            var query = _context.Logs.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(l => l.InfoMessage.Contains(search));
            }

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            return query.OrderByDescending(l => l.Timestamp).ToList();
        }

        public void LogAdd(int? level, string? infoMessage)
        {
            var log = new LogModel
            {
                IDLevel = level,
                InfoMessage = infoMessage,
                Timestamp = DateTime.Now
            };
            
            _context.Logs.Add(log);
            _context.SaveChanges();
        }
        #endregion
    }
} 