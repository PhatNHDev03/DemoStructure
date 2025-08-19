using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class TimeZoneHelper
    {
        // Timezone Việt Nam
        public static readonly TimeZoneInfo VietnamTimeZone = GetVietnamTimeZone();

        private static TimeZoneInfo GetVietnamTimeZone()
        {
            try
            {
                // Thử Windows timezone ID trước
                return TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            }
            catch
            {
                try
                {
                    // Nếu fail thì thử Linux/macOS timezone ID
                    return TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");
                }
                catch
                {
                    // Fallback: tạo custom timezone UTC+7
                    return TimeZoneInfo.CreateCustomTimeZone(
                        "Vietnam Standard Time",
                        TimeSpan.FromHours(7),
                        "Vietnam Standard Time",
                        "Vietnam Standard Time");
                }
            }
        }

        /// <summary>
        /// Lấy thời gian hiện tại ở Việt Nam
        /// </summary>
        public static DateTime GetVietnamNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, VietnamTimeZone);
        }

        /// <summary>
        /// Chuyển đổi từ UTC sang giờ Việt Nam
        /// </summary>
        /// <summary>
        /// Chuyển từ UTC → giờ Việt Nam
        /// </summary>
        public static DateTime ConvertUtcToVietnam(DateTime utcDateTime)
        {
            // Đảm bảo utcDateTime là UTC
            var utc = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTimeFromUtc(utc, VietnamTimeZone);
        }

        /// <summary>
        /// Chuyển từ giờ Việt Nam → UTC
        /// </summary>
        public static DateTime ConvertVietnamToUtc(DateTime vietnamDateTime)
        {
            // Đảm bảo vietnamDateTime không có Kind hoặc là Local thì phải chuyển về Unspecified để xử lý đúng
            var vn = DateTime.SpecifyKind(vietnamDateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(vn, VietnamTimeZone);
        }

        /// <summary>
        /// Chuyển đổi string time (HH:mm) từ Việt Nam sang UTC
        /// </summary>
        public static DateTime ParseVietnamDateTimeToUtc(string dateTimeString)
        {
            if (DateTime.TryParse(dateTimeString, out var parsedDateTime))
            {
                // Treat parsed datetime as Vietnam time
                var vietnamTime = DateTime.SpecifyKind(parsedDateTime, DateTimeKind.Unspecified);
                return ConvertVietnamToUtc(vietnamTime);
            }
            throw new ArgumentException($"Invalid datetime format: {dateTimeString}");
        }

        /// <summary>
        /// Chuyển đổi DateTime từ UTC sang string time (HH:mm) theo giờ Việt Nam
        /// </summary>
        public static string ConvertUtcToVietnamTimeString(DateTime utcDateTime)
        {
            var vietnamTime = ConvertUtcToVietnam(utcDateTime);
            return vietnamTime.ToString("HH:mm");
        }

        /// <summary>
        /// Format DateTime theo định dạng Việt Nam
        /// </summary>
        public static string FormatVietnamDateTime(DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return dateTime.ToString(format);
        }
    }
}
