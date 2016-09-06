using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AURMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            AdminEquipment(routes);

            UserReservationRoutes(routes);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "LOGINlink", action = "Index", id = UrlParameter.Optional }
            );
        }

        public static void AdminEquipment(RouteCollection routes)
        {
            routes.MapRoute(
                "StaffGroupList",
                "ADMINequipment/GroupStaff/List",
                new { controller = "ADMINequipment", action = "EquipmentList" }
                );
        }
        public static void UserReservationRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "EquipmentList",
                "USERreservation/EquipmentList",
                new { controller = "USERreservation", action = "EquipmentList" }
                );

            routes.MapRoute(
               "RoomList",
               "USERreservation/RoomList/{BuildingID},{Seat}",
               new { controller = "USERreservation", action = "RoomList" }
               );

            routes.MapRoute(
                "BuildingList",
                "USERreservation/BuildingList/{CampusID}",
                new { controller = "USERreservation", action = "BuildingList" }
            );

            routes.MapRoute(
                "CampusList",
                "USERreservation/Campus/List",
                new { controller = "USERreservation", action = "CampusList" }
            );

            routes.MapRoute(
                "StartTimeList",
                "USERreservation/StartTimeList/{RoomID}",
                new { controller = "USERreservation", action = "StartTimeList" }
            );

            routes.MapRoute(
                "EndTimeList",
                "USERreservation/EndTimeList/{StartTime},{RoomID}",
                new { controller = "USERreservation", action = "EndTimeList" }
            );
        }
    }
}
