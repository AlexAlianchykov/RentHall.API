using CSharpFunctionalExtensions;
using RentHall.Application.AbstractionsServices;
using RentHall.Core.AbstractionsRepositories.HallAbstractions;

namespace RentHall.Application.Services
{
    public class AnalyticsService: IAnalyticsServices
    {
        private IAnalyticsRepositoty _analyticsRepositoty;
        public AnalyticsService(IAnalyticsRepositoty analyticsRepositoty)
        {
            _analyticsRepositoty = analyticsRepositoty;
        }

        public string GetAllAnalytics()
        {
            try
            {
                return _analyticsRepositoty.Get();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
