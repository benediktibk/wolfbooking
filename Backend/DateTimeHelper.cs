using System;

namespace Backend
{
    public static class DateTimeHelper
    {
        public static DateTime MaxValue
        {
            get
            {
                // SQL Server Compact produces an overflow error for DateTime.MaxValue
                return DateTime.MaxValue.AddMilliseconds(-1);
            }
        }
    }
}