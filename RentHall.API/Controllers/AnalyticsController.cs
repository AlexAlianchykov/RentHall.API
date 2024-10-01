using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentHall.Application.AbstractionsServices;

namespace RentHall.API.Controllers
{
    [Authorize(Policy = "AnalyticsPolisy")]
    [Route("api/AnalyticsController")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private IAnalyticsServices _analyticsServices;
        public AnalyticsController(IAnalyticsServices analyticsServices)
        {
            _analyticsServices = analyticsServices;
        }

        [HttpGet("AllAnalytics")]  // вывести аналитику по api 

        public IActionResult GetAllAnalytics()
        {
            var htmlContent = _analyticsServices.GetAllAnalytics();
            return new ContentResult
            {
                Content = htmlContent,
                ContentType = "text/html",
                StatusCode = 200
            };
        }
    }
}
