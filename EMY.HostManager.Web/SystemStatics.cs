using System;

namespace EMY.HostManager.Web
{
    public static class SystemStatics
    {
        public const string DefaultScheme = "EmyHostManagerScheme";

        public static bool IsBetween(this DateTime selectedDate, DateTime dtBegin, DateTime dtEnd) =>
            (selectedDate <= dtEnd && selectedDate >= dtBegin) || (selectedDate <= dtBegin && selectedDate >= dtEnd);

    }
}
