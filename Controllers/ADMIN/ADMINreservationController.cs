using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AURMS.Classes;
using System.Threading;
using AURMS.Models;

namespace AURMS.Controllers.ADMIN
{
    public class ADMINreservationController : Controller
    {
        // GET: ADMINreservation
        public ActionResult Index()
        {
            return View();
        }

        public List<ReservationInformation> GetNormalList()
        {
            short groupid = -1;
            if (Session["usergroup"] != null)
            {
                groupid = Int16.Parse(Session["usergroup"].ToString().Trim());

            }

            using (RMSDataContext rms = new RMSDataContext())
            {
                //List<ReservationRecord> _reservation = rms.ReservationRecords.Where(x=>x.Status==2).ToList();
                var _reservation = (from reserva in rms.ReservationRecords
                                    join u in rms.Users on reserva.UserID equals u.UserID
                                    join r in rms.Rooms on reserva.RoomID equals r.RoomID
                                    join b in rms.Buildings on r.BuildingID equals b.BuildingID// where reserva.Status == 1
                                    where reserva.Status == groupid - 100 || reserva.Status == groupid ||
                                    (from reservation in rms.ReservationRecords
                                     join user in rms.Users on reservation.UserID equals user.UserID
                                     where reservation.Status == 999 &&
                                     (groupid == user.GroupID + 100 || groupid == user.GroupID + 200 || user.GroupID == groupid)
                                     select reservation.ReservationRecordID).ToList().Contains(reserva.ReservationRecordID)
                                     orderby reserva.ReservationRecordID
                                    select new ReservationInformation
                                    {
                                        ReservationRecordID = reserva.ReservationRecordID,
                                        StudentID = u.UserCode,
                                        RoomNumber = b.BuildingName + " " + r.RoomName,
                                        StartTime = reserva.StartTime,
                                        EndTime = reserva.EndTime,
                                        Date = reserva.Date,
                                        Status = reserva.Status
                                    }).ToList();
                return _reservation;
            }
        }

        public List<ReservationInformation> GetListForAdmin()
        {
            short groupid = -1;
            if (Session["usergroup"] != null)
            {
                groupid = Int16.Parse(Session["usergroup"].ToString().Trim());

            }
            using (RMSDataContext rms = new RMSDataContext())
            {
                //List<ReservationRecord> _reservation = rms.ReservationRecords.Where(x=>x.Status==2).ToList();
                var _reservation = (from reserva in rms.ReservationRecords
                                    join u in rms.Users on reserva.UserID equals u.UserID
                                    join r in rms.Rooms on reserva.RoomID equals r.RoomID
                                    join b in rms.Buildings on r.BuildingID equals b.BuildingID// where reserva.Status == 1
                                    where reserva.Status == 999 || reserva.Status == 0 || reserva.Status == 1000
                                    //where reserva.Status == groupid - 100 || reserva.Status == groupid ||
                                    //(from reservation in rms.ReservationRecords
                                    // join user in rms.Users on reservation.UserID equals user.UserID
                                    // where reservation.Status == 1000
                                    // select reservation.ReservationRecordID).ToList().Contains(reserva.ReservationRecordID)
                                    select new ReservationInformation
                                    {
                                        ReservationRecordID = reserva.ReservationRecordID,
                                        StudentID = u.UserCode,
                                        RoomNumber = b.BuildingName + " " + r.RoomName,
                                        StartTime = reserva.StartTime,
                                        EndTime = reserva.EndTime,
                                        Date = reserva.Date,
                                        Status = reserva.Status
                                    }).ToList();
                return _reservation;
            }
        }


        public ActionResult List()
        {
            return View(GetNormalList());
        }



        public ActionResult ListForAdmin()
        {
            return View(GetListForAdmin());
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
                                          && reservation.Date.CompareTo(DateTime.Now.Date) < 0
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

                return View(pendingReservation);
            }
        }



        public ActionResult Details(long id)
        {
            ReservationInformation newReser = ManageReservation.ReservationDetail(id);
            return View(newReser);
        }

        public ActionResult Deatail2(long id)
        {
            ReservationInformation newReser = ManageReservation.ReservationDetail(id);
            return View(newReser);
        }

        public ActionResult Details2(long id)
        {
            ReservationInformation reView = ManageReservation.ReservationDetail(id);
            return View(reView);
        }


        public ActionResult EventDetail(long id)
        {

            return View();
        }

        [HttpPost]
        public ActionResult approve(long reservationID)
        {

            long groupid = Int64.Parse(Session["usergroup"].ToString().Trim());
            ManageReservation.ReservationApprove(reservationID, groupid);

            if (groupid == 5555)
            {
                return View("ListForAdmin", GetListForAdmin());
            }
            else
            {
                return View("List", GetNormalList());
            }
            

        }

        [HttpPost]
        public ActionResult reject(long reservationRejectID)
        {
            ManageReservation.ReservationReject(reservationRejectID);
            long groupid = Int64.Parse(Session["usergroup"].ToString().Trim());

            if (groupid == 5555)
            {
                return View("ListForAdmin", GetListForAdmin());
            }
            else
            {
                return View("List", GetNormalList());
            }
            

        }

        public ActionResult show()
        {

            return View("List");
        }

    }
}