using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AURMS.Controllers.ADMIN
{
    public class ADMINequipmentController : Controller
    {
        // GET: ADMINequipment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StaffGroupList()
        {
            using (RMSDataContext rmsd = new RMSDataContext())
            {
                IQueryable _staffgroup = (from s in rmsd.StaffGroups
                                          select new
                                          {
                                              s.StaffGroupID,
                                              s.StaffGroupName
                                          }).ToList().AsQueryable();

                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new SelectList(_staffgroup, "StaffGroupID", "StaffGroupName"), JsonRequestBehavior.AllowGet);
                }
                return View(_staffgroup);
            }
        }

        public ActionResult EquipmentList()
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                List<Equipment> _equipment = rms.Equipments.ToList();
               // List<ReservationRecord> _userrecord = rmsd.ReservationRecords.Where(r => r.UserID == userID && (r.Status == 3 || r.Status == 0));
                return View(_equipment);
            }

            
        }

        
        public ActionResult CreateEquipment()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult CreateEquipment(FormCollection form)
        {
            string _staffGroupId = form["StaffGroup"];
            string _equipName = form["EquipmentName"];

            Equipment _equipment = new Equipment();

            _equipment.StaffGroupID = Convert.ToInt64(_staffGroupId);
            _equipment.EquipmentName = _equipName;

           
            using (RMSDataContext rms = new RMSDataContext())
            {
                rms.Equipments.InsertOnSubmit(_equipment);
                rms.SubmitChanges();
                return View("EquipmentList", rms.Equipments.ToList());
            }

            
        }

        public ActionResult EditEquipment(long id)
        {
            using(RMSDataContext rms = new RMSDataContext())
            {
                Equipment equipment = rms.Equipments.Where(eq => eq.EquipmentID == id).SingleOrDefault();
                return View(equipment);
            }
            
        }
        
        [HttpGet]
        public ActionResult EditEquipment(long id,FormCollection form)
        {
            string _staffGroupId = form["StaffGroupID"];
            string _equipName = form["EquipmentName"];

            Equipment _equipment = new Equipment();
            _equipment.StaffGroupID = Convert.ToInt64(_staffGroupId);
            _equipment.EquipmentName = _equipName;


            using (RMSDataContext rms = new RMSDataContext())
            {
                Equipment _equipments = rms.Equipments.Where(eq => eq.EquipmentID == id).SingleOrDefault();
                _equipments.StaffGroupID = _equipments.StaffGroupID;
                _equipments.EquipmentName = _equipments.EquipmentName;
                rms.SubmitChanges();
                return View("EquipmentList", rms.Equipments.ToList());
            }
           
        
        }

        public ActionResult DeleteEquipment(long id)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                Equipment _equipment = rms.Equipments.Where(eq => eq.EquipmentID == id).SingleOrDefault();
                rms.Equipments.DeleteOnSubmit(_equipment);
                rms.SubmitChanges();
                return View("EquipmentList", rms.Equipments.ToList());
            }
            
          
        }
           
      
    }
}