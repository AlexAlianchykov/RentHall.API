using Microsoft.EntityFrameworkCore;
using RentHall.Core.AbstractionsRepositories.HallAbstractions;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RentHall.DataAccess.Repositories
{
    public class AnalyticsRepositoty: IAnalyticsRepositoty
    {
        private ApplicationDbContext _dbContext;
        public AnalyticsRepositoty(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public string Get()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<html lang=\"ru\">");
            htmlBuilder.Append("<head><meta charset='UTF-8'></head>");
            htmlBuilder.Append("<html><body>");
            htmlBuilder.Append("<h1>Statistics</h1>");
            htmlBuilder.Append("<h2>Demand for conference rooms</h2>");
            htmlBuilder.Append("<table border='1'><tr>");
            htmlBuilder.Append("<th style='width:100px;'>Name</th>");
            htmlBuilder.Append("<th style='width:100px;'>Price</th>");
            htmlBuilder.Append("<th style='width:100px;'>Number of bookings</th>");
            htmlBuilder.Append("</tr></table>");
            htmlBuilder.Append("</body></html>");

            var resultHalls = _dbContext.Halls
                            .Select(hall => new
                            {
                                HallName = hall.Name,
                                HallPrice = hall.Amount,
                                BookingCount = hall.Bookings.Count()
                            })
                            .ToList();

            foreach (var hall in resultHalls)
            {
                htmlBuilder.Append("<html><body>");
                htmlBuilder.Append("<table border='1'><tr>");
                htmlBuilder.Append($"<th style='width:100px;'>{hall.HallName}</th>" +
                                   $"<th style='width:100px;'>{hall.HallPrice}</th>" +
                                   $"<th style='width:100px;'>{hall.BookingCount}</th></tr>");
                htmlBuilder.Append("</table></body></html>");

            }

            htmlBuilder.Append("<div style= 'margin-top: 15px;' ></div>");
            htmlBuilder.Append("<html><body><h2>Demand for additional services</h2>");
            htmlBuilder.Append("<table border='1'><tr>");
            htmlBuilder.Append("<th style='width:100px;'>Name</th>");
            htmlBuilder.Append("<th style='width:100px;'>Price</th>");
            htmlBuilder.Append("<th style='width:100px;'>Number of bookings</th>");
            htmlBuilder.Append("</tr></table>");
            htmlBuilder.Append("</body></html>");

            var resultServices = _dbContext.Additionals
                            .Select(service => new
                            {
                                ServiceName = service.Name,
                                ServicePrice = service.Amount,
                                BookingCount = service.BookingsServces.Count()
                            })
                            .ToList();

            foreach (var service in resultServices)
            {
                htmlBuilder.Append("<html><body>");
                htmlBuilder.Append("<table border='1'><tr>");
                htmlBuilder.Append($"<th style='width:100px;'>{service.ServiceName}</th>" +
                                   $"<th style='width:100px;'>{service.ServicePrice}</th>" +
                                   $"<th style='width:100px;'>{service.BookingCount}</th></tr>");
                htmlBuilder.Append("</table></body></html>");
            }

            htmlBuilder.Append("<div style= 'margin-top: 15px;' ></div>");
            htmlBuilder.Append("<html><body><h2>Number of users</h2>");
            htmlBuilder.Append("<table border='1'><tr>");
            htmlBuilder.Append("<th style='width:100px;'>Role</th>");
            htmlBuilder.Append("<th style='width:100px;'>Count</th>");
            htmlBuilder.Append("</tr></table>");
            htmlBuilder.Append("</body></html>");

            var userCount = _dbContext.Users.Count(u => u.Role == "user");
            var adminCount = _dbContext.Users.Count(u => u.Role == "admin");

            htmlBuilder.Append("<html><body>");
            htmlBuilder.Append("<table border='1'><tr>");
            htmlBuilder.Append($"<th style='width:100px;'>Users</th>" +
                               $"<th style='width:100px;'>{userCount}</th></tr>");
            htmlBuilder.Append("</table></body></html>");

            htmlBuilder.Append("<html><body>");
            htmlBuilder.Append("<table border='1'><tr>");
            htmlBuilder.Append($"<th style='width:100px;'>Admin</th>" +
                               $"<th style='width:100px;'>{adminCount}</th></tr>");
            htmlBuilder.Append("</table></body></html>");

            return htmlBuilder.ToString();
        }
    }
}
