namespace resturant
{
    internal class ApiStruct
    {
        public class TimeObj
        {
            public int day { get; set; }
            public string time { get; set; }
        }
        public class PeriodObj
        {
            public TimeObj close { get; set; }
            public TimeObj open { get; set; }
        }
        public class OpeningHoursObj
        {
            public bool open_now { get; set; }
            public PeriodObj[] periods { get; set; }
        }

        public class PlaceObj
        {
            public OpeningHoursObj opening_hours { get; set; }
        }
        public class ApiObj
        {
            public PlaceObj[] results { get; set; }
        }
    }
}