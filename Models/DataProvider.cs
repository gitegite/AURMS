using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AURMS.Models;


namespace AURMS
{
    public class DataProvider
    {
        public static List<RoomType> GetRoomType()
        {
            using(RMSDataContext rms = new RMSDataContext())
            {
                var rt = from roomType in rms.RoomTypes select new { roomType.RoomTypeName, roomType.RoomTypeID };
                
               List<RoomType> lstRoomType = new List<RoomType>();

                foreach(var a in rt)
                {
                    RoomType rmType = new RoomType();
                    rmType.RoomTypeID = a.RoomTypeID;
                    rmType.RoomTypeName = a.RoomTypeName;
                    lstRoomType.Add(rmType);
                }
                return lstRoomType;
            }
        }
    }
}