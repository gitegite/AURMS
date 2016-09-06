using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AURMS.Controllers.USER
{
    public class USERreservationController : Controller
    {
        // GET: USERreservation
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Index(string id = "", string eventname = "")
        //{
        //    ViewBag.EventID = id;
        //    ViewBag.EventName = eventname;
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult IndexWithEventView(string id = "", string eventname = "")
        //{
        //    ViewBag.EventID = id;
        //    ViewBag.EventName = eventname;
        //    return View();
        //}

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            long groupid = Int64.Parse(Session["usergroup"].ToString().Trim());
            using (RMSDataContext rmsd = new RMSDataContext())
            {
                string equipmentString = form["Equipment"];
                string eventName = form["EventName"];
                long eventID = (form["EventID"] == null || form["EventID"] ==  "") ? -1 : Int64.Parse(form["EventID"].Trim());
                equipmentString = equipmentString.Length != 0 ? equipmentString.Substring(0, equipmentString.Length - 1) : "";
                ReservationRecord _reservation = new ReservationRecord();
                string roomtmp = form["RoomID"];
                roomtmp = roomtmp.Replace(",", "");
                _reservation.RoomID = Convert.ToInt64(roomtmp);
                _reservation.Date = form["Date"];
                _reservation.StartTime = form["StartTime"];
                _reservation.EndTime = form["EndTime"];
                _reservation.Detail = form["Detail"];
                _reservation.UserID = Int64.Parse(Session["userid"].ToString().Trim());
                _reservation.Status = (short)(Int16.Parse(Session["usergroup"].ToString().Trim()) % 100 + 200);
                if (Int16.Parse(Session["usergroup"].ToString().Trim()) == 5555)
                {
                    _reservation.Status = 1000;
                }
                else if (Int16.Parse(Session["usergroup"].ToString().Trim()) > 200)
                {
                    _reservation.Status = 999;
                }
                rmsd.ReservationRecords.InsertOnSubmit(_reservation);
                rmsd.SubmitChanges();

                if (eventID != -1)
                {
                    rmsd.EventRooms.InsertOnSubmit(new EventRoom
                        {
                            EventID = eventID,
                            RoomID = _reservation.ReservationRecordID
                        });
                    rmsd.SubmitChanges();
                }


                if (equipmentString.Length != 0)
                {
                    string[] equipments = equipmentString.Split(';');
                    foreach (var e in equipments)
                    {
                        string[] equipment = e.Split(',');
                        rmsd.RequestEquipments.InsertOnSubmit(new RequestEquipment
                        {
                            ReservationRecordID = _reservation.ReservationRecordID,
                            EquipmentID = Int64.Parse(equipment[0]),
                            Number = Int16.Parse(equipment[1])
                        });
                    }
                    rmsd.SubmitChanges();
                }


                
                if (groupid < 100)
                {
                    return RedirectToAction("UserPending", "USERrecord");
                }
                return RedirectToAction("List", "ADMINreservation");
                
                //return View("~/Views/USERrecord/Pending.cshtml", rmsd.ReservationRecords.ToList());
            }
        }

        public ActionResult Cancel(long id)
        {
            using (RMSDataContext rmsd = new RMSDataContext())
            {
                ReservationRecord _reservation = rmsd.ReservationRecords.Where(r => r.ReservationRecordID == id).SingleOrDefault();
                rmsd.ReservationRecords.DeleteOnSubmit(_reservation);
                rmsd.SubmitChanges();
                return RedirectToAction("UserPending", "USERrecord");
               // return View("/Views/USERrecord/Pending.cshtml", rmsd.ReservationRecords.ToList());
            }

        }

        public ActionResult CampusList()
        {
            using (RMSDataContext rmsd = new RMSDataContext())
            {
                IQueryable _campus = (from c in rmsd.Campus
                                      select new
                                      {
                                          c.CampusID,
                                          c.CampusName
                                      }).ToList().AsQueryable();

                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new SelectList(_campus, "CampusID", "CampusName"), JsonRequestBehavior.AllowGet);
                }
                return View(_campus);
            }
        }

        public ActionResult BuildingList(string CampusID)
        {
            if (!String.IsNullOrWhiteSpace(CampusID))
            {
                using (RMSDataContext rmsd = new RMSDataContext())
                {
                    IQueryable _building = (from b in rmsd.Buildings
                                            where b.CampusID == Convert.ToInt64(CampusID)
                                            select new
                                            {
                                                b.BuildingID,
                                                b.BuildingName
                                            }).ToList().AsQueryable();
                    if (HttpContext.Request.IsAjaxRequest())
                    {
                        return Json(new SelectList(_building, "BuildingID", "BuildingName"), JsonRequestBehavior.AllowGet);
                    }
                    return View(_building);
                }
            }
            return View();
        }

        public ActionResult RoomList(string BuildingID)
        {

            using (RMSDataContext rmsd = new RMSDataContext())
            {

                IQueryable _room = (from r in rmsd.Rooms
                                    where r.BuildingID == Convert.ToInt64(BuildingID)
                                    select new
                                    {
                                        r.RoomID,
                                        r.RoomName
                                    }).ToList().AsQueryable();
                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new SelectList(_room, "RoomID", "RoomName"), JsonRequestBehavior.AllowGet);
                }
                return View(_room);
            }
        }

        public ActionResult StartTimeList(string RoomID)
        {
            if (!String.IsNullOrWhiteSpace(RoomID))
            {
                using (RMSDataContext rmsd = new RMSDataContext())
                {
                    Room _room = new Room();
                    _room.OpenTime = (from r in rmsd.Rooms
                                      where (r.RoomID == Convert.ToInt64(RoomID))
                                      select r.OpenTime).FirstOrDefault();

                    _room.CloseTime = (from r in rmsd.Rooms
                                       where (r.RoomID == Convert.ToInt64(RoomID))
                                       select r.CloseTime).FirstOrDefault();

                    int starttime = Convert.ToInt32(_room.OpenTime.Substring(0, _room.OpenTime.IndexOf(":")));
                    int endtime = Convert.ToInt32(_room.CloseTime.Substring(0, _room.CloseTime.IndexOf(":")));
                    string[] time = new string[endtime - starttime];

                    for (int i = 0, j = starttime; i != time.Length; i++, j++)
                    {
                        time[i] = j.ToString() + ":00";
                    }

                    if (HttpContext.Request.IsAjaxRequest())
                    {
                        return Json(new SelectList(time), JsonRequestBehavior.AllowGet);
                    }
                    return View(time);
                }
            }
            return View();
        }

        public ActionResult EndTimeList(string StartTime, string RoomID)
        {
            if ((!String.IsNullOrWhiteSpace(StartTime) && !String.IsNullOrWhiteSpace(RoomID)))
            {
                using (RMSDataContext rmsd = new RMSDataContext())
                {
                    Room _room = new Room();
                    _room.CloseTime = (from r in rmsd.Rooms
                                       where (r.RoomID == Convert.ToInt64(RoomID))
                                       select r.CloseTime).FirstOrDefault();

                    int starttime = Convert.ToInt32(StartTime.Substring(0, StartTime.IndexOf(":"))) + 1;
                    int endtime = Convert.ToInt32(_room.CloseTime.Substring(0, _room.CloseTime.IndexOf(":")));
                    string[] time = new string[endtime - starttime + 1];

                    for (int i = 0, j = starttime; i != time.Length; i++, j++)
                    {
                        time[i] = j.ToString() + ":00";
                    }

                    if (HttpContext.Request.IsAjaxRequest())
                    {
                        return Json(new SelectList(time), JsonRequestBehavior.AllowGet);
                    }
                    return View(time);
                }
            }
            return View();
        }

        public ActionResult EquipmentList()
        {
            using (RMSDataContext rmsd = new RMSDataContext())
            {
                IQueryable equipment = (from e in rmsd.Equipments
                                        select new
                                        {
                                            e.EquipmentID,
                                            e.EquipmentName
                                        }).ToList().AsQueryable();


                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new SelectList(equipment, "EquipmentID", "EquipmentName"), JsonRequestBehavior.AllowGet);

                }
                return null;
            }
        }
    }
}