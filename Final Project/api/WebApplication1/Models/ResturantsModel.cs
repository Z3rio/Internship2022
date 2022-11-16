
namespace WebApplication1.Models
{
    public class ResturantsModel
    {
        public class TimeObj
        {
            public int? day { get; set; }
            public string? time { get; set; }
        }

        public class PeriodObj
        {
            public TimeObj? close { get; set; }
            public TimeObj? open { get; set; }
        }

        public class OpeningHoursObj
        {
            public bool? open_now { get; set; }
            public PeriodObj[]? periods { get; set; }
        }

        public class PlaceObj
        {
            public OpeningHoursObj? opening_hours { get; set; }
            public string[]? types { get; set; }
            public string? name { get; set; }
            public decimal? rating { get; set; }
        }

        public PlaceObj[]? results { get; set; }
    }
}
