using AURMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AURMS.Controllers.USER
{
    public class USERrecordController : Controller
    {

        public ActionResult UserPending()
        {

            string userID = Session["UserName"]!= null ? Session["UserName"].ToString() : "";
            using (RMSDataContext rms = new RMSDataContext())
            {
                var pendingReservation = (from reservation in rms.ReservationRecords
                                         join room in rms.Rooms on reservation.RoomID equals room.RoomID
                                         join building in rms.Buildings on room.BuildingID equals building.BuildingID
                                         join campus in rms.Campus on building.CampusID equals campus.CampusID
                                         join user in rms.Users on reservation.UserID equals user.UserID
                                         where user.UserCode == userID && 
                                         !(from record in rms.EventRooms
                                           select record.RoomID).Contains(reservation.ReservationRecordID)
                                         
                                         orderby reservation.ReservationRecordID
                                         select new ReservationInformation
                                         {
                                             ReservationRecordID = reservation.ReservationRecordID,
                                             RoomID = room.RoomID,
                                             RoomNumber = room.RoomName,
                                             StartTime = reservation.StartTime,
                                             EndTime = reservation.EndTime,
                                             Status = reservation.Status,
                                             Seat = room.Seat,
                                             Detail = reservation.Detail,
                                             Date = reservation.Date,
                                             StudentID = userID
                                         }).ToList();

                return View("Pending", pendingReservation);
            }
        }

        public ActionResult History()
        {
            string userID = Session["UserName"].ToString();
            using (RMSDataContext rms = new RMSDataContext())
            {
                var pendingReservation = (from reservation in rms.ReservationRecords
                                          join room in rms.Rooms on reservation.RoomID equals room.RoomID
                                          join building in rms.Buildings on room.BuildingID equals building.BuildingID
                                          join campus in rms.Campus on building.CampusID equals campus.CampusID
                                          join user in rms.Users on reservation.UserID equals user.UserID
                                          where user.UserCode == userID
                                          && (reservation.Status == 1000 || reservation.Status == 0) 
                                          && reservation.Date.CompareTo(DateTime.Now) < 0
                                          orderby reservation.ReservationRecordID
                                          select new ReservationInformation
                                          {
                                              ReservationRecordID = reservation.ReservationRecordID,
                                              RoomID = room.RoomID,
                                              RoomNumber = building.BuildingName + " " + room.RoomName,
                                              StartTime = reservation.StartTime,
                                              EndTime = reservation.EndTime,
                                              Status = reservation.Status,
                                              Seat = room.Seat,
                                              Detail = reservation.Detail,
                                              Date = reservation.Date,
                                          }).ToList();
                long usergroup = Int64.Parse(Session["usergroup"].ToString().Trim());

                return View(pendingReservation);
            }
        }

        public ActionResult Event()
        {
            string userID = Session["UserName"].ToString();
            using (RMSDataContext rms = new RMSDataContext())
            {
                var e = (from events in rms.Events
                         join userEvent in rms.EventUsers on events.EventID equals userEvent.EventID
                         join user in rms.Users on userEvent.UserCode equals user.UserCode
                         where user.UserCode == userID
                         select new EventReservation
                         {
                             EventID = events.EventID,
                             EventName = events.EventName,
                             EventStartDate = events.EventStartDate,
                             EventEndDate = events.EventEndDate,
                             EventStatus = events.EventStatus
                         }).ToList();
                return View("EventsPickerView", e);
            }
        }

        public ActionResult EventDetail(long eventID)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                ViewBag.eventName = (from events in rms.Events
                                 where events.EventID == eventID
                                 select events.EventName).SingleOrDefault();


                var q = (from events in rms.Events
                         join eventRoom in rms.EventRooms on events.EventID equals eventRoom.EventID
                         join record in rms.ReservationRecords on eventRoom.RoomID equals record.RoomID
                         join room in rms.Rooms on record.RoomID equals room.RoomID
                         join building in rms.Buildings on room.BuildingID equals building.BuildingID
                         where events.EventID == eventID
                         select new ReservationInformation
                         {
                             ReservationRecordID = record.ReservationRecordID,
                             RoomID = record.RoomID,
                             Date = record.Date,
                             StartTime = record.StartTime,
                             EndTime = record.EndTime,
                             RoomNumber = building.BuildingName + " " + room.RoomName,
                             Status = record.Status,
                             Detail = record.Detail
                         }).ToList();

                return View(q);
            }
            
        }

        public ActionResult Details(long id)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                var _reservation = (from reserva in rms.ReservationRecords
                                    where reserva.ReservationRecordID == id
                                    join u in rms.Users on reserva.UserID equals u.UserID
                                    join r in rms.Rooms on reserva.RoomID equals r.RoomID
                                    join b in rms.Buildings on r.BuildingID equals b.BuildingID
                                    where reserva.ReservationRecordID == id
                                    select new ReservationInformation
                                    {
                                        ReservationRecordID = reserva.ReservationRecordID,
                                        StudentID = u.UserCode,
                                        RoomNumber = b.BuildingName + " " + r.RoomName,
                                        StartTime = reserva.StartTime,
                                        EndTime = reserva.EndTime,
                                        Date = reserva.Date,
                                        Status = reserva.Status,
                                        RoomID = reserva.RoomID,
                                        Detail = reserva.Detail,
                                        EquipmentList = (from request in rms.RequestEquipments
                                                         join equipment in rms.Equipments on request.EquipmentID equals equipment.EquipmentID
                                                         where reserva.ReservationRecordID == request.ReservationRecordID
                                                         select new EquipmentReservation
                                                         {
                                                             EquipmentID = equipment.EquipmentID,
                                                             EquipmentName = equipment.EquipmentName,
                                                             EquipmentAmount = request.Number
                                                         }).ToList()
                                    }).SingleOrDefault();

                return View(_reservation);
            }
        }


        public ActionResult Details2(long id)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                var _reservation = (from reserva in rms.ReservationRecords
                                    where reserva.ReservationRecordID == id
                                    join u in rms.Users on reserva.UserID equals u.UserID
                                    join r in rms.Rooms on reserva.RoomID equals r.RoomID
                                    join b in rms.Buildings on r.BuildingID equals b.BuildingID
                                    where reserva.ReservationRecordID == id
                                    select new ReservationInformation
                                    {
                                        ReservationRecordID = reserva.ReservationRecordID,
                                        StudentID = u.UserCode,
                                        RoomNumber = b.BuildingName + " " + r.RoomName,
                                        StartTime = reserva.StartTime,
                                        EndTime = reserva.EndTime,
                                        Date = reserva.Date,
                                        Status = reserva.Status,
                                        RoomID = reserva.RoomID,
                                        Detail = reserva.Detail,
                                        EquipmentList = (from request in rms.RequestEquipments
                                                         join equipment in rms.Equipments on request.EquipmentID equals equipment.EquipmentID
                                                         where reserva.ReservationRecordID == request.ReservationRecordID
                                                         select new EquipmentReservation
                                                         {
                                                             EquipmentID = equipment.EquipmentID,
                                                             EquipmentName = equipment.EquipmentName,
                                                             EquipmentAmount = request.Number
                                                         }).ToList()
                                    }).SingleOrDefault();

                return View(_reservation);
            }
        }

        public ActionResult Cancel(long id)
        {
            using (RMSDataContext rmsd = new RMSDataContext())
            {
                ReservationRecord _reservation = rmsd.ReservationRecords.Where(r => r.ReservationRecordID == id).SingleOrDefault();
                rmsd.ReservationRecords.DeleteOnSubmit(_reservation);
                rmsd.SubmitChanges();
                return View("Pending", rmsd.ReservationRecords.ToList());
            }

        }
    }
}
