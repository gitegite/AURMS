using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AURMS.Classes;
using System.Threading.Tasks;

namespace AURMS.Models
{

    public class ManageReservation
    {
        public static List<ReservationInformation> ReservationList()
        //public static List<ReservationList> ReservationList()
        {

            

            using (RMSDataContext rms = new RMSDataContext())
            {

                    var _reservation = (from reserva in rms.ReservationRecords
                                        join u in rms.Users on reserva.UserID equals u.UserID
                                        join r in rms.Rooms on reserva.RoomID equals r.RoomID
                                        join b in rms.Buildings on r.BuildingID equals b.BuildingID// where reserva.Status == 1
                                        select new ReservationInformation
                                        {
                                            ReservationRecordID = reserva.ReservationRecordID,
                                            StudentID = u.UserCode,
                                            RoomNumber = b.BuildingName + " " + r.RoomName,
                                            StartTime = reserva.StartTime,
                                            EndTime = reserva.EndTime,
                                            Date = reserva.Date,
                                            Status = reserva.Status
                                        }).ToList();
                    return _reservation;
                
            }
        }

        public static ReservationInformation ReservationDetail(long id)
        {

            using (RMSDataContext rms = new RMSDataContext())
            {
                //ReservationRecord _reservation = rms.ReservationRecords.Where(x => x.ReservationRecordID == id).FirstOrDefault();
                var _reservation = (from reserva in rms.ReservationRecords
                                    where reserva.ReservationRecordID == id
                                    join u in rms.Users on reserva.UserID equals u.UserID
                                    join r in rms.Rooms on reserva.RoomID equals r.RoomID
                                    join b in rms.Buildings on r.BuildingID equals b.BuildingID
                                    where reserva.ReservationRecordID == id
                                    select new ReservationInformation
                                    {
                                        ReservationRecordID = reserva.ReservationRecordID,
                                        StudentID = u.UserCode,
                                        RoomNumber = b.BuildingName + " " + r.RoomName,
                                        StartTime = reserva.StartTime,
                                        EndTime = reserva.EndTime,
                                        Date = reserva.Date,
                                        Status = reserva.Status,
                                        RoomID = reserva.RoomID,
                                        Detail = reserva.Detail,
                                        EquipmentList = (from request in rms.RequestEquipments
                                                         join equipment in rms.Equipments on request.EquipmentID equals equipment.EquipmentID
                                                         where reserva.ReservationRecordID == request.ReservationRecordID
                                                         select new EquipmentReservation
                                                         {
                                                             EquipmentID = equipment.EquipmentID,
                                                             EquipmentName = equipment.EquipmentName,
                                                             EquipmentAmount = request.Number
                                                         }).ToList()
                                    }).SingleOrDefault();

                return _reservation;
            }
        }

        //public static List<ReservationList> ReservationApprve(long ans)
        public static void ReservationApprove(long reservationID, long groupid)
        {
            
            using (RMSDataContext rms = new RMSDataContext())
            {
                ReservationRecord _reservation = rms.ReservationRecords.Where(x => x.ReservationRecordID == 
                    reservationID).FirstOrDefault();


                //if (_reservation.Status < 100)
                //{
                //    _reservation.Status += 100;
                //}
                //else if(_reservation.Status < 999 ) // 210 the last fac code
                //{
                //    _reservation.Status = 999;
                //}
                //else if (_reservation.Status == 999)
                //{
                //    _reservation.Status = 1000;
                //}
               // _reservation.Status = 3;


                _reservation.Status = groupid == 5555 ? (short)1000 : (short)999;
                
                rms.SubmitChanges();
            }
        }

        public static void ReservationReject(long ans)
        {
            using (RMSDataContext rms = new RMSDataContext())
            {
                ReservationRecord _reservation = rms.ReservationRecords.Where(x => x.ReservationRecordID == ans).FirstOrDefault();
                _reservation.Status = 0;
                rms.SubmitChanges();
            }
        }

    }
}
