using AURMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AURMS.Controllers
{
    public class EventPickerController : Controller
    {
        // GET: EventPicker
        public ActionResult Index()
        {
            
            return View("../USERrecord/EventsPickerView", GetEvent());
        }

        public List<EventReservation> GetEvent()
        {
            string usercode = Session["username"].ToString().Trim();
            using (RMSDataContext rms = new RMSDataContext())
            {
                var q = (from events in rms.Events
                         join eventUser in rms.EventUsers on events.EventID equals eventUser.EventID
                         where eventUser.UserCode == usercode
                         select new EventReservation
                         {
                             EventID = events.EventID,
                             EventName = events.EventName,
                             EventStartDate = events.EventStartDate,
                             EventEndDate = events.EventEndDate,
                             EventStatus = events.EventStatus,
                             EventReserverID = eventUser.UserCode
                         }).ToList();
                for (int i = 0; i < q.Count(); ++i)
                {
                    q[i].EventReservations = (from eventRoom in rms.EventRooms
                              join events in rms.Events on eventRoom.EventID equals events.EventID
                              join record in rms.ReservationRecords on eventRoom.RoomID equals record.ReservationRecordID
                              join room in rms.Rooms on record.RoomID equals room.RoomID
                              join building in rms.Buildings on room.BuildingID equals building.BuildingID
                              where events.EventID == q[i].EventID
                              select new ReservationInformation
                              {
                                  ReservationRecordID = record.ReservationRecordID,
                                  RoomID = record.RoomID,
                                  RoomNumber = building.BuildingName + " " + room.RoomName,
                                  Date = record.Date,
                                  StartTime = record.StartTime,
                                  EndTime = record.EndTime,
                                  Status = record.Status,
                                  Detail = record.Detail
                              }).ToList();
                }

                    return q;
            }
        }

        public ActionResult GetEventDetail(string eventID)
        {
            long EventID = Int64.Parse(eventID);
            using (RMSDataContext rms = new RMSDataContext())
            {
                EventReservation eventReservation = (from events in rms.Events
                                      where EventID == events.EventID
                                      select new EventReservation
                                      {
                                          EventID = EventID,
                                          EventName = events.EventName,
                                          EventStartDate = events.EventStartDate,
                                          EventEndDate = events.EventEndDate,
                                          EventStatus = events.EventStatus
                                      }).SingleOrDefault();
                eventReservation.EventReservations = (from events in rms.Events
                                join eventRoom in rms.EventRooms on events.EventID equals eventRoom.EventID
                                join record in rms.ReservationRecords on eventRoom.RoomID equals record.ReservationRecordID
                                join user in rms.Users on record.UserID equals user.UserID
                                join room in rms.Rooms on record.RoomID equals room.RoomID
                                join building in rms.Buildings on room.BuildingID equals building.BuildingID
                                where events.EventID == EventID
                                select new ReservationInformation
                                {
                                    ReservationRecordID = record.ReservationRecordID,
                                    StudentID = user.UserCode,
                                    RoomID = record.RoomID,
                                    RoomNumber = building.BuildingName + " " + room.RoomName,
                                    Date = record.Date,
                                    StartTime = record.StartTime,
                                    EndTime = record.EndTime,
                                    Status = record.Status,
                                    Detail = record.Detail,
                                    EquipmentList = (from request in rms.RequestEquipments
                                                     join equipment in rms.Equipments on request.EquipmentID equals equipment.EquipmentID
                                                     where request.ReservationRecordID == record.ReservationRecordID
                                                     select new EquipmentReservation
                                                     {
                                                         EquipmentID = request.EquipmentID,
                                                         EquipmentName = equipment.EquipmentName,
                                                         EquipmentAmount = request.Number
                                                     }).ToList()

                                }).ToList();
                return View("../USERrecord/EventDetail", eventReservation);
            }
            
        }

        public ActionResult RoomDetail(long id)
        {
            return View("../ADMINreservation/Details2", ManageReservation.ReservationDetail(id));
        }

        public ActionResult GetEventForDirector()
        {
            string usercode = Session["username"].ToString().Trim();
            long usergroup = Int64.Parse(Session["usergroup"].ToString().Trim());
            using (RMSDataContext rms = new RMSDataContext())
            {
                var q = (from events in rms.Events
                         join eventUser in rms.EventUsers on events.EventID equals eventUser.EventID
                         join user in rms.Users on eventUser.UserCode equals user.UserCode
                         where eventUser.UserCode == usercode || usergroup - 100 == user.GroupID ||
                               usergroup - 200 == user.GroupID
                         select new EventReservation
                         {
                             EventID = events.EventID,
                             EventName = events.EventName,
                             EventStartDate = events.EventStartDate,
                             EventEndDate = events.EventEndDate,
                             EventStatus = events.EventStatus,
                             EventReserverID = user.UserCode
                         }).ToList();
                for (int i = 0; i < q.Count(); ++i)
                {
                    q[i].EventReservations = (from eventRoom in rms.EventRooms
                                              join events in rms.Events on eventRoom.EventID equals events.EventID
                                              join record in rms.ReservationRecords on eventRoom.RoomID equals record.ReservationRecordID
                                              join room in rms.Rooms on record.RoomID equals room.RoomID
                                              join building in rms.Buildings on room.BuildingID equals building.BuildingID
                                              where events.EventID == q[i].EventID
                                              select new ReservationInformation
                                              {
                                                  ReservationRecordID = record.ReservationRecordID,
                                                  RoomID = record.RoomID,
                                                  RoomNumber = building.BuildingName + " " + room.RoomName,
                                                  Date = record.Date,
                                                  StartTime = record.StartTime,
                                                  EndTime = record.EndTime,
                                                  Status = record.Status,
                                                  Detail = record.Detail
                                              }).ToList();
                }

                return View("../ADMINreservation/EventsPickerView", q);
            }
        }

        public ActionResult GetEventDetailForDirector(string eventID)
        {
            long EventID = Int64.Parse(eventID);
            using (RMSDataContext rms = new RMSDataContext())
            {
                EventReservation eventReservation = (from events in rms.Events
                                                     join user in rms.EventUsers on events.EventID equals user.EventID
                                                     where EventID == events.EventID
                                                     select new EventReservation
                                                     {
                                                         EventReserverID = user.UserCode,
                                                         EventID = EventID,
                                                         EventName = events.EventName,
                                                         EventStartDate = events.EventStartDate,
                                                         EventEndDate = events.EventEndDate,
                                                         EventStatus = events.EventStatus
                                                     }).SingleOrDefault();
                eventReservation.EventReservations = (from events in rms.Events
                                                      join eventRoom in rms.EventRooms on events.EventID equals eventRoom.EventID
                                                      join record in rms.ReservationRecords on eventRoom.RoomID equals record.ReservationRecordID
                                                      join user in rms.Users on record.UserID equals user.UserID
                                                      join room in rms.Rooms on record.RoomID equals room.RoomID
                                                      join building in rms.Buildings on room.BuildingID equals building.BuildingID
                                                      where events.EventID == EventID
                                                      select new ReservationInformation
                                                      {
                                                          ReservationRecordID = record.ReservationRecordID,
                                                          StudentID = user.UserCode,
                                                          RoomID = record.RoomID,
                                                          RoomNumber = building.BuildingName + " " + room.RoomName,
                                                          Date = record.Date,
                                                          StartTime = record.StartTime,
                                                          EndTime = record.EndTime,
                                                          Status = record.Status,
                                                          Detail = record.Detail,
                                                          EquipmentList = (from request in rms.RequestEquipments
                                                                           join equipment in rms.Equipments on request.EquipmentID equals equipment.EquipmentID
                                                                           where request.ReservationRecordID == record.ReservationRecordID
                                                                           select new EquipmentReservation
                                                                           {
                                                                               EquipmentID = request.EquipmentID,
                                                                               EquipmentName = equipment.EquipmentName,
                                                                               EquipmentAmount = request.Number
                                                                           }).ToList()

                                                      }).ToList();
                return View("../ADMINreservation/EventDetail", eventReservation);
            }

        }

        public ActionResult RoomDetailForDirector(long id)
        {
            return View("../ADMINreservation/Details",ManageReservation.ReservationDetail(id));
        }

        public void AddEvent(string eventName, string eventStartDate, string eventEndDate)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                Event e = new Event
                {
                    EventName = eventName,
                    EventStartDate = eventStartDate,
                    EventEndDate = eventEndDate
                };
                rms.Events.InsertOnSubmit(e);
                rms.SubmitChanges();

                rms.EventUsers.InsertOnSubmit(new EventUser
                    {
                        EventID = e.EventID,
                        UserCode = Session["username"].ToString()
                    });
                rms.SubmitChanges();
            }
        }



    }
}