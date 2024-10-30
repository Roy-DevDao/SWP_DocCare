using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using test2.Data;

namespace test2.DAO
{
    public class StaffDAO
    {
        private readonly DocCareContext dc;

        public StaffDAO(DocCareContext context)
        {
            dc = context;
        }

        public bool UpdateAppointmentStatus(string appointmentId, string newStatus =" ")
        {
            using (var transaction = dc.Database.BeginTransaction())
            {
                try
                {
                    var order = dc.Orders.Include(o => o.Option).FirstOrDefault(o => o.Oid == appointmentId); // sử dụng  Eager Loading
                    //var order = dc.Orders.FirstOrDefault(o => o.Oid == appointmentId);
                    if (order != null && order.Option != null) // Check null
                    {
                        order.Option.Status = newStatus;
                        dc.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Error updating appointment status: {ex.Message}");
                    return false;
                }
            }
        }




    }
}