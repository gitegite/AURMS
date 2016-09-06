using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AURMS.Models
{
    public class EquipmentReservation
    {
        public long EquipmentID { get; set; }
        public string EquipmentName { get; set; }
        public int EquipmentAmount { get; set; }
    }
}