
using System.Linq;
using static WebApplication1.Models.ResturantsModel;

namespace WebApplication1.Models
{
    public class PlaceObj
    {
        public OpeningHoursObj? opening_hours { get; set; }
        public string[]? types { get; set; }
        public string? name { get; set; }
        public string? vicinity { get; set; }
        public string? business_status { get; set; }
        public bool? permanently_closed { get; set; }
        public decimal? rating { get; set; }
        public int? price_level { get; set; }
        public string? place_id { get; set; }
    }

    public class ResturantsModel
    {
        public class TimeObj
        {
            public int? day { get; set; }
            public string? time { get; set; }
        }

        public class OpeningHoursObj
        {
            public bool? open_now { get; set; }
        }

        public class UnsortedResults
        {
            public PlaceObj[]? results { get; set; }
            public string? status { get; set; }
        }

        public class SortedResults { 
            public IOrderedEnumerable<PlaceObj>? results { get; set; }
            public string? status { get; set; }
        }

        public class PlaceResult
        {
            public string status { get; set; }
            public PlaceObj result { get; set; }
        }
    }
}
