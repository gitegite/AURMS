using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AURMS.Classes
{
    public class Reservation
    {
        public long ReservationRecordID;
        public String UserID;
        public String Date;
        public String StartTime;
        public String EndTime;
        public short Status;
        public long RoomID;
        public short Seat;
        public String Detail;
    }

    public class ReservationList
    {
        // use for tranform detail to ReservationList ( Admin )
        public long ReservationRecordID;
        public long UserID;
        public String Room;
        public String Date;
        public String StartTime;
        public String EndTime;
        public long Status;
    }

    public class RoomType
    {

        public long roomTypeId;
        public string roomTypeName;
    }
}