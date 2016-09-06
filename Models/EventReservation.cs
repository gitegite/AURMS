using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AURMS.Models
{
    public class EventReservation
    {
        public long EventID { get; set; }
        public string EventName { get; set; }
        public string EventStartDate { get; set; }
        public string EventEndDate { get; set; }
        public string EventStatus { get; set; }
        public string EventReserverID { get; set; }
        public string EventReserverName { get; set; }
        public List<ReservationInformation> EventReservations { get; set; }
    }
}