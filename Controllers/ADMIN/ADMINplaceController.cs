using AURMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AURMS.Controllers.ADMIN
{
    public class ADMINplaceController : Controller
    {
        // GET: ADMINplace
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Admin()
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                List<Campus> CampusList = rms.Campus.ToList();
                return View(CampusList);
            }
        }

        public ActionResult CreateCampus()
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateCampus(FormCollection Form)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                String _name = Form["CampusName"];

                Campus _campus = new Campus();
                _campus.CampusName = _name;

                rms.Campus.InsertOnSubmit(_campus);
                rms.SubmitChanges();
                return View("Admin", rms.Campus.ToList());
            }
        }

        public ActionResult CampusDetails(long id)
        {
            Session["CampusID"] = id;
            using (RMSDataContext rms = new RMSDataContext())
            {
                List<Building> BuildingList = rms.Buildings.Where(s => s.CampusID == id).ToList();
                return View("CampusDetails", BuildingList);
            }
        }

        public ActionResult CreateBuilding()
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateBuilding(FormCollection Form)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                String _buildingName = Form["BuildingName"];
                String _minFloor = Form["MinFloor"];
                String _maxFloor = Form["MaxFloor"];

                Building _building = new Building();
                _building.BuildingName = _buildingName;
                _building.MinFloor = Convert.ToInt16(_minFloor);
                _building.MaxFloor = Convert.ToInt16(_maxFloor);
                _building.CampusID = Convert.ToInt64(Session["CampusID"]);

                rms.Buildings.InsertOnSubmit(_building);
                rms.SubmitChanges();

                List<Building> BuildingList = rms.Buildings.Where(s => s.CampusID == _building.CampusID).ToList();

                return View("CampusDetails", BuildingList);
            }
        }

        public ActionResult BuildingDetails(long id)
        {
            Session["BuildingID"] = id;
            using (RMSDataContext rms = new RMSDataContext())
            {
                List<Room> RoomList = rms.Rooms.Where(s => s.BuildingID == id).ToList();
                return View("BuildingDetails", RoomList);
            }
        }

        public ActionResult CreateRoom()
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateRoom(FormCollection Form)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                String _roomTypeID = Form["RoomType"];
                String _floor = Form["Floor"];
                String _roomName = Form["RoomName"];
                String _openTime = Form["OpenTime"];
                String _closeTime = Form["CloseTime"];
                String _seat = Form["Seat"];
                String _detail = Form["Detail"];

                Room _room = new Room();
                _room.BuildingID = Convert.ToInt64(Session["BuildingID"]);
                _room.RoomTypeID = Convert.ToInt64(_roomTypeID);
                _room.Floor = Convert.ToInt16(_floor);
                _room.RoomName = _roomName;
                _room.OpenTime = _openTime;
                _room.CloseTime = _closeTime;
                _room.Seat = Convert.ToInt16(_seat);
                _room.Detail = _detail;

                rms.Rooms.InsertOnSubmit(_room);
                rms.SubmitChanges();

                List<Room> RoomList = rms.Rooms.Where(s => s.BuildingID == Convert.ToInt64(Session["BuildingID"])).ToList();
                return View("BuildingDetails", RoomList);
            }
        }

        public ActionResult RoomtypeList()
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                List<RoomType> Roomtype = rms.RoomTypes.ToList();
                return View("RoomtypeList", Roomtype);
            }
        }

        public ActionResult CreateRoomtype()
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateRoomtype(FormCollection Form)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                String _name = Form["RoomTypeName"];

                RoomType _roomType = new RoomType();
                _roomType.RoomTypeName = _name;

                rms.RoomTypes.InsertOnSubmit(_roomType);
                rms.SubmitChanges();
                return View("RoomtypeList", rms.RoomTypes.ToList());
            }
        }

        public ActionResult DeleteRoom(long id)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                Room _room = rms.Rooms.Where(s => s.RoomID == id).FirstOrDefault();
                rms.Rooms.DeleteOnSubmit(_room);
                rms.SubmitChanges();

                List<Room> RoomList = rms.Rooms.Where(s => s.BuildingID == Convert.ToInt64(Session["BuildingID"])).ToList();
                return View("BuildingDetails", RoomList);
            }
        }

        public ActionResult DeleteBuilding(long id)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                List<Room> _room = rms.Rooms.Where(s => s.BuildingID == id).ToList();
                rms.Rooms.DeleteAllOnSubmit(_room);
                rms.SubmitChanges();

                Building _building = rms.Buildings.Where(s => s.BuildingID == id).FirstOrDefault();
                rms.Buildings.DeleteOnSubmit(_building);
                rms.SubmitChanges();

                List<Building> BuildingList = rms.Buildings.Where(s => s.CampusID == _building.CampusID).ToList();
                return View("CampusDetails", BuildingList);
            }
        }

        public ActionResult DeleteCampus(long id)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                Campus _campus = rms.Campus.Where(s => s.CampusID == id).SingleOrDefault();
                rms.Campus.DeleteOnSubmit(_campus);
                rms.SubmitChanges();

                return View("Admin", rms.Campus.ToList());
            }
        }

        public ActionResult EditCampus(long id)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                Campus _campus = rms.Campus.Where(s => s.CampusID == id).SingleOrDefault();
                return View(_campus);
            }
        }

        [HttpPost]
        public ActionResult EditCampus(FormCollection Form, long id)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                String _campusName = Form["CampusName"];

                Campus _campus = rms.Campus.Where(s => s.CampusID == id).SingleOrDefault();
                _campus.CampusName = _campusName;
                rms.SubmitChanges();

                List<Campus> CampusList = rms.Campus.ToList();
                return View("Admin", CampusList);
            }
        }

        public ActionResult EditBuilding(long id)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                Building _building = rms.Buildings.Where(s => s.BuildingID == id).SingleOrDefault();
                return View(_building);
            }
        }

        [HttpPost]
        public ActionResult EditBuilding(FormCollection Form, long id)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                String _buildingName = Form["BuildingName"];
                String _minFloor = Form["MinFloor"];
                String _maxFloor = Form["MaxFloor"];

                Building _building = rms.Buildings.Where(s => s.BuildingID == id).SingleOrDefault();
                _building.CampusID = Convert.ToInt64(Session["CampusID"]);
                _building.BuildingName = _buildingName;
                _building.MinFloor = Convert.ToInt16(_minFloor);
                _building.MaxFloor = Convert.ToInt16(_maxFloor);

                rms.SubmitChanges();

                List<Building> BuildingList = rms.Buildings.Where(s => s.CampusID == Convert.ToInt64(Session["CampusID"])).ToList();

                return View("CampusDetails", BuildingList);
            }
        }

        public ActionResult EditRoom(long id)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                Room _room = rms.Rooms.Where(s => s.RoomID == id).SingleOrDefault();
                return View(_room);
            }
        }

        [HttpPost]
        public ActionResult EditRoom(FormCollection Form, long id)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                String _roomTypeID = Form["RoomTypeID"];
                String _floor = Form["Floor"];
                String _roomName = Form["RoomName"];
                String _openTime = Form["OpenTime"];
                String _closeTime = Form["CloseTime"];
                String _seat = Form["Seat"];
                String _detail = Form["Detail"];

                Room _room = rms.Rooms.Where(s => s.RoomID == id).SingleOrDefault();
                _room.RoomTypeID = Convert.ToInt64(_roomTypeID);
                _room.Floor = Convert.ToInt16(_floor);
                _room.RoomName = _roomName;
                _room.OpenTime = _openTime;
                _room.CloseTime = _closeTime;
                _room.Seat = Convert.ToInt16(_seat);
                _room.Detail = _detail;
                _room.BuildingID = Convert.ToInt64(Session["BuildingID"]);

                rms.SubmitChanges();

                List<Room> RoomList = rms.Rooms.Where(s => s.BuildingID == Convert.ToInt64(Session["BuildingID"])).ToList();
                return View("BuildingDetails", RoomList);
            }
        }

        public ActionResult RoomDetails(long id)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {

                var roomDetail = (from room in rms.Rooms
                                  join building in rms.Buildings on room.BuildingID equals building.BuildingID
                                  join campus in rms.Campus on building.CampusID equals campus.CampusID
                                  join type in rms.RoomTypes on room.RoomTypeID equals type.RoomTypeID
                                  where room.RoomID == id
                                  select new RoomInformation
                                  {
                                      RoomID = room.RoomID,
                                      RoomName = room.RoomName,
                                      RoomTypeID = room.RoomTypeID,
                                      RoomTypeName = type.RoomTypeName,
                                      BuildingID = building.BuildingID,
                                      BuildingName = building.BuildingName,
                                      CampusID = campus.CampusID,
                                      CampusName = campus.CampusName,
                                      Floor = room.Floor,
                                      Detail = room.Detail,
                                      Seat = room.Seat,
                                      CloseTime = room.CloseTime,
                                      OpenTime = room.OpenTime
                                  }).SingleOrDefault();

                return View(roomDetail);
            }
        }
    }
}