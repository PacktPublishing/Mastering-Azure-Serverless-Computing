using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class UnixTimeExtensions
    {
        public static DateTimeOffset ToUtcDateTimeOffset(this long unixTime)
        {
            return new DateTimeOffset(1970, 01, 01, 00, 00, 00, TimeSpan.Zero).AddSeconds(unixTime);
        }
    }
}
