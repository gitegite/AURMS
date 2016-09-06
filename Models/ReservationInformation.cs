using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AURMS.Models
{
    public class ReservationInformation
    {
        public long   ReservationRecordID { get; set; }
        public string StudentID { get; set; }
        public long RoomID { get; set; }
        public string RoomNumber { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public short Status { get; set; }
        public short? Seat { get; set; }
        public string Detail { get; set; }
        public List<EquipmentReservation> EquipmentList { get; set; }
    }
}