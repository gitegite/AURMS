using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AURMS.Models
{
    public class RoomInformation
    {
        public long RoomID { get; set; }
        public string RoomName { get; set; }
        public long RoomTypeID { get; set; }
        public string RoomTypeName { get; set; }
        public long BuildingID { get; set; }
        public string BuildingName { get; set; }
        public long CampusID { get; set; }
        public string CampusName { get; set; }
        public short Floor { get; set; }
        public string Detail { get; set; }
        public short? Seat { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
    }
}