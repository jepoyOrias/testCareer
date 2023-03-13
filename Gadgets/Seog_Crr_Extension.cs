using System;
using System.Linq;
using System.Data;
using System.Globalization;

namespace CareerCom.Cool.Gadgets
{
    public static class Seog_Crr_Extension
    {
        /// <summary>
        /// due to data table from xml, so column property value type is string forever.
        /// </summary>
        /// <returns>return empty if column name non exist or value is empty</returns>
        public static string FieldValue(this DataRow row, string columnName, bool checkColumn = true)
        {
            return checkColumn && !row.Table.Columns.Contains(columnName)
                ? string.Empty
                : row.Field<string>(columnName);
        }

        public static T FieldValue<T>(this DataRow row, string columnName, bool checkColumn = true)
        {
            bool bolNonExist = checkColumn && !row.Table.Columns.Contains(columnName) && row.IsNull(columnName);
            if (bolNonExist)
                return default(T);

            if(row[columnName].GetType().Name.Equals("Decimal"))
            {
                decimal d = row.Field<Decimal>(columnName);
                return ChangeType<T>(d);
            }
               
            return row.Field<T>(columnName);
        }

        public static T ChangeType<T>(this object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public static bool IsEmpty(this DataSet dataSet)
        {
            return dataSet == null ||
              !(from DataTable t in dataSet.Tables where t.Rows.Count > 0 select t).Any();
        }

        public static string DateTimeStandardize(this string strDateTime)
        {
            if (string.IsNullOrEmpty(strDateTime)) return strDateTime;

            CultureInfo enUS = CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                var dateTime = Convert.ToDateTime(strDateTime);
                DateTimeFormatInfo dtfi = enUS.DateTimeFormat;
                dtfi.ShortDatePattern = "MMMM dd, yyyy";

                string dtmStandardize = dateTime.ToString("d", enUS);
                return dtmStandardize;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static string[] Tokenized(this string strText)
        {
            if (string.IsNullOrEmpty(strText)) return new[] { strText };

            var separator = new char[] { ' ', '-', '+', '|', '[', ']', '_', '#', '$', '*', ',', ':', ',', '/', '!', '?', ';', '+', '@', '<', '>', '\\', '~', '%', '^', '{', '}', '&' };
            return strText.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool IsNumeric(this string strNumber)
        {
            double dbl;
            return Double.TryParse(strNumber, out dbl);
        }

        public static string ToUpperFirstCharacter(this string s)
        {
            if(string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("there is no first letter");
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static Uri AddOrUpdateParameter(this Uri url, string paramName, string paramValue)
        {
            var host = System.Configuration.ConfigurationManager.AppSettings["DestinationHost"].ToString();
            var uriBuilder = new UriBuilder($"{host}{url.PathAndQuery}");
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }

        public static Uri RemoveParameter(this Uri url, params string[] paramNames)
        {
            var host = System.Configuration.ConfigurationManager.AppSettings["DestinationHost"].ToString();
            var uriBuilder = new UriBuilder($"{host}{url.PathAndQuery}");
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var item in paramNames)
                query.Remove(item);

            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }

        public static string PrintAllInnerException(this System.Exception e, int level = int.MaxValue)
        {
            var sb = new System.Text.StringBuilder();
            var exception = e;
            var counter = 1;
            while (exception != null && counter <= level)
            {
                sb.AppendLine($"{counter}-> Level: {counter}");
                sb.AppendLine($"{counter}-> Message: {exception.Message}");
                sb.AppendLine($"{counter}-> Source: {exception.Source}");
                sb.AppendLine($"{counter}-> Target Site: {exception.TargetSite}");
                sb.AppendLine($"{counter}-> Stack Trace: {exception.StackTrace}");

                exception = exception.InnerException;
                counter++;
            }

            return sb.ToString();
        }

    }
}