using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Model;
using System.Windows.Media;

namespace Ism.Infrastructure.Extensions
{
    public static class Extensions
    {
        public delegate void SetPropertyDelegate(BaseModel model);
        public static string ConvertToUnsecureString(this SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static IEnumerable<byte[]> ReadChunks(this FileInfo fileInfo, int megabyte = 1024, long whereToStartReading = 0)
        {
            FileStream fileStram = fileInfo.OpenRead();
            using (fileStram)
            {
                byte[] buffer = new byte[megabyte];
                fileStram.Seek(whereToStartReading, SeekOrigin.Begin);
                int bytesRead = fileStram.Read(buffer, 0, megabyte);
                while (bytesRead > 0)
                {
                    yield return buffer;
                    bytesRead = fileStram.Read(buffer, 0, megabyte);
                }
            }
        }


        public static bool IsInScope(this IList<Scope> scopes, string scope)
        {
            try
            {
                return null != scopes && (scopes.Any(s => s.Identifier == scope) || scopes.Any(s => s.Identifier == "admin"));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static long ToProjectNumber(this DateTime date)
        {
            //Random rnd = new Random();
            //return int.Parse($"{date:yyMMdd}{rnd.Next(0, 9999):D4}");
            return long.Parse($"{date:yyMMddhhmmss}");
        }

        public static string Clean(this string data)
        {
            return data;
        }
        public static bool Between(this DateTime input, DateTime date1, DateTime date2)
        {
            return (input > date1 && input < date2);
        }

        public static List<ProjectDateInfo> ProjectDays(this TimeSpan span, DateTime start, int noWorkers)
        {

            List<ProjectDateInfo> range = new List<ProjectDateInfo>();
            for (int day = 0; day <= span.Days; day++)
            {
                range.Add(new ProjectDateInfo(start.AddDays(day), noWorkers));
            }
            return range;
        }

        public static List<WorkPlaceDateInfo> WorkPlaceDays(this TimeSpan span, DateTime start, int noWorkers, WorkPlace workplace)
        {

            List<WorkPlaceDateInfo> range = new List<WorkPlaceDateInfo>();
            for (int day = 0; day <= span.Days; day++)
            {
                range.Add(new WorkPlaceDateInfo(start.AddDays(day), noWorkers,0 ,workplace));
            }
            return range;
        }

        public static Color ChangeColorBrightness( this Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var cur in enumerable)
            {
                action(cur);
            }
        }
    }
}
