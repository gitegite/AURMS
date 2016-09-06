using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;

using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;

using AURMS.Models;


namespace AURMS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private RMSDataContext db = new RMSDataContext();

        //
        // GET: /Home/
        public ActionResult Index()
        {
            var scheduler = new DHXScheduler(this);
            scheduler.Skin = DHXScheduler.Skins.Flat;

            scheduler.Config.first_hour = 9;
            scheduler.Config.last_hour = 19;

            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;

            return View(scheduler);
        }

        public List<AppointmentCalendar> PrepareData()
        {
            List<AppointmentCalendar> appoint = new List<AppointmentCalendar>();

            var dbSet = from dbApp in db.ReservationRecords
                        select new
                        {
                            dbApp.ReservationRecordID,
                            dbApp.Detail,
                            dbApp.Date,
                            dbApp.StartTime,
                            dbApp.EndTime
                        };

            foreach (var a in dbSet)
            {

                string startTime = a.StartTime;
                if (startTime[0] == 32)
                    continue;

                AppointmentCalendar newApp = new AppointmentCalendar();
                newApp.Id = Convert.ToInt32(a.ReservationRecordID);
                newApp.Description = a.Detail;
                newApp.StartDate = setDateTime(a.Date, a.StartTime);
                newApp.EndDate = setDateTime(a.Date, a.EndTime);
                appoint.Add(newApp);
            }

            return appoint;

        }

        public DateTime setDateTime(string date, string time)
        {
            //IFormatProvider culture = new CultureInfo("fr-FR", true);

            //var month = (date.Substring(0, 2));
            //var day = (date.Substring(3, 2));
            //var year = (Convert.ToInt32(date.Substring(6, 4)));
            //var setDate = day + "/" + month + "/" + year;

            //if (time[1] == ':')
            //    time = "0" + time;
            //var startH = (time.Substring(0, 2));
            //var startM = (time.Substring(3, 2));

            //var setDateTime = setDate + " " + time;

            var month = Convert.ToInt32((date.Substring(0, 2)));
            var day = Convert.ToInt32((date.Substring(3, 2)));
            var year = (Convert.ToInt32(date.Substring(6, 4))) - 543;
            var setDate = day + "/" + month + "/" + year;

            if (time[1] == ':')
                time = "0" + time;

            var startH = Convert.ToInt32(time.Substring(0, 2));
            var startM = Convert.ToInt32(time.Substring(3, 2));

            DateTime d = new DateTime(year, month, day, startH, startM, 0);
            //string dStr = d.ToString("dd/MM/yyyy HH:mm");            
            return d;

        }

        public ContentResult Data()
        {
            var apps = PrepareData();
            return new SchedulerAjaxData(apps);
        }

        public ActionResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);

            try
            {
                var changedEvent = DHXEventsHelper.Bind<AppointmentCalendar>(actionValues);
                switch (action.Type)
                {
                    //case DataActionTypes.Insert:
                    //    db.Appointments.Add(changedEvent);
                    //    break;
                    //case DataActionTypes.Delete:
                    //    db.Entry(changedEvent).State = EntityState.Deleted;
                    //    break;
                    //default:// "update"  
                    //    db.Entry(changedEvent).State = EntityState.Modified;
                    //    break;
                }
                //db.SaveChanges();
                action.TargetId = changedEvent.Id;
            }
            catch (Exception a)
            {
                action.Type = DataActionTypes.Error;
            }

            return (new AjaxSaveResponse(action));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}