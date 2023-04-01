using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Utility
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value) {
            PersianCalendar pc = new PersianCalendar();

            return pc.GetYear(value) + "/" + pc.GetMonth(value) +"/" +pc.GetDayOfMonth(value).ToString("00");
        
        }

    }
}
