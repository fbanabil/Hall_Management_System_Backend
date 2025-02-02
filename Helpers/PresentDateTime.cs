using Student_Hall_Management.Data;
using System;

namespace Student_Hall_Management.Helpers
{
    public class PresentDateTime : IPresentDateTime
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        public PresentDateTime(IConfiguration config)
        {
            _config = config;
            _dapper = new DataContextDapper(_config);
        }

        public DateTime GetPresentDateTime()
        {
            var utcNow = _dapper.LoadDataSingle<DateTime>("SELECT GETUTCDATE()");
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.Id);
            return TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZone);
        }
    }
}
