using System;
using System.Web;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net;
using System.ComponentModel;
using System.Reflection;
using System.Collections;

namespace CareerCom.Cool.Gadgets
{
    public class Seog_Crr_Utility
    {
        private readonly Seog_Crr_Log objLog = new Seog_Crr_Log();
        private CultureInfo _nfCulture = new CultureInfo("en-US");

        /// <summary>
        /// 12345 -> $12,346
        /// </summary>
        public string SalFormatCurrency(double salary, int intNumDigitsAfterDecimal = 0)
        {
            return $"${ SalFormatNumber(salary, intNumDigitsAfterDecimal) }";
        }

        public string SalFormatCurrency(string strSalary)
        {
            double dblResult;
            if (double.TryParse(strSalary, out dblResult))
                return SalFormatCurrency(dblResult);

            return "$0";
        }

        /// <summary>
        /// 0.015 -> 1.5%
        /// -0.015 -> -1.5%
        /// </summary>
        public string SalFormatPercent(double dblValue, int intNumDigitsAfterDecimal, bool bolUseParensForNegativeNumbers = false, bool bolGroupDigits = true)
        {
            NumberFormatInfo objNumberFormatInfo = _nfCulture.NumberFormat;

            objNumberFormatInfo.PercentDecimalDigits = intNumDigitsAfterDecimal;
            if (!bolGroupDigits)
                objNumberFormatInfo.PercentGroupSeparator = "";
            if (bolUseParensForNegativeNumbers)
                objNumberFormatInfo.PercentNegativePattern = 1;
            objNumberFormatInfo.PercentPositivePattern = 1;

            string strExpression = dblValue.ToString("P", objNumberFormatInfo);
            if (dblValue < 0)
                strExpression = strExpression.Replace(" ", "");

            return strExpression;
        }

        /// <summary>
        /// 12345 -> 12,345
        /// </summary>
        public string SalFormatNumber(double dblExpression, int intNumDigitsAfterDecimal)
        {
            bool bolUseParensForNegativeNumbers = false;
            bool bolGroupDigits = true;
            string strExpression = null;
            NumberFormatInfo _nfi;
            _nfi = _nfCulture.NumberFormat;

            _nfi.NumberDecimalDigits = intNumDigitsAfterDecimal;
            if (!bolGroupDigits)
                _nfi.NumberGroupSeparator = "";
            else
                _nfi.NumberGroupSeparator = ",";
            if (bolUseParensForNegativeNumbers)
                _nfi.NumberNegativePattern = 0;
            else
                _nfi.NumberNegativePattern = 1;
            strExpression = dblExpression.ToString("N", _nfi);

            return strExpression;
        }

      
        public string SalGetShortedContent(string strJobDesc, int intLength, bool bolNeedThreeDot = false)
        {
            if (string.IsNullOrEmpty(strJobDesc))
                return string.Empty;

            if (strJobDesc.Length <= intLength)
                return strJobDesc;

            return bolNeedThreeDot ? strJobDesc.Substring(0, intLength) + "..." : strJobDesc.Substring(0, intLength);
        }


        public string SalGetQueryStringValue(HttpRequestBase request, string strParamName)
        {
            string result = null;
            var urlParameters = HttpUtility.ParseQueryString(request.Url.Query.ToLower());
            if (urlParameters.AllKeys.Contains(strParamName.ToLower()))
            {
                result = urlParameters.Get(strParamName.ToLower());
            }

            return result;
        }

        public string SalBoldString(string strSourceStringText, string strNeedToBoldText, StringComparison comparison = StringComparison.OrdinalIgnoreCase, string strDefaultBoldClass = null)
        {
            if (strSourceStringText == null)
                throw new ArgumentNullException(nameof(strSourceStringText));

            if (strSourceStringText.Length == 0)
                return strSourceStringText;

            if (strNeedToBoldText == null)
                throw new ArgumentNullException(nameof(strNeedToBoldText));

            if (strNeedToBoldText.Length == 0)
                throw new ArgumentException("String cannot be of zero length.");


            StringBuilder resultStringBuilder = new StringBuilder(strSourceStringText.Length);
            bool isReplacementNullOrEmpty = string.IsNullOrEmpty(strNeedToBoldText);

            const int valueNotFound = -1;
            int foundAt;
            int startSearchFromIndex = 0;

            while ((foundAt = strSourceStringText.IndexOf(strNeedToBoldText, startSearchFromIndex, comparison)) != valueNotFound)
            {
                int @charsUntilReplacment = foundAt - startSearchFromIndex;
                bool isNothingToAppend = @charsUntilReplacment == 0;
                if (!isNothingToAppend)
                    resultStringBuilder.Append(strSourceStringText, startSearchFromIndex, @charsUntilReplacment);

                if (!isReplacementNullOrEmpty)
                {
                    string @strNewValue = strSourceStringText.Substring(foundAt, strNeedToBoldText.Length);
                    if (string.IsNullOrEmpty(strDefaultBoldClass))
                        resultStringBuilder.Append($"<b>{strNewValue}</b>");
                    else
                        resultStringBuilder.Append($"<b class='{strDefaultBoldClass}'>{strNewValue}</b>");
                }

                startSearchFromIndex = foundAt + strNeedToBoldText.Length;
                if (startSearchFromIndex == strSourceStringText.Length)
                    return resultStringBuilder.ToString();
            }

            int @charsUntilStringEnd = strSourceStringText.Length - startSearchFromIndex;
            resultStringBuilder.Append(strSourceStringText, startSearchFromIndex, @charsUntilStringEnd);
            return resultStringBuilder.ToString();
        }

        public string UrlEncode(string encode)
        {
            if (encode == null) return null;
            string encoded = HttpUtility.UrlEncode(encode);
            if (encoded.Replace("%20", "") == encode.Replace(" ", "") &&
                encoded.Replace("+", "") == encode.Replace(" ", "") &&
                encoded.Replace("%2f", "") == encode.Replace("/", "") &&
                encoded.Replace("%2e", "") == encode.Replace(".", ""))
            {
                return encoded;
            }
            else
            {
                return "=" + ToBase64(encode);
            }
        }

        public string ToBase64(string encode)
        {
            Byte[] btByteArray = null;
            UTF8Encoding encoding = new UTF8Encoding();
            btByteArray = encoding.GetBytes(encode);
            string sResult = System.Convert.ToBase64String(btByteArray, 0, btByteArray.Length);
            sResult = sResult.Replace("+", "-").Replace("/", "_");
            return sResult;
        }

        public void SalPagination(int totalItems, ref int currentPage, out int totalPages, out int startPage, out int endPage, int pageSize = 10, int maxPages = 8)
        {
            totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            if (currentPage < 1)
                currentPage = 1;
            else if (currentPage > totalPages)
                currentPage = totalPages;

            startPage = 0;
            endPage = 0;
            if (totalPages <= maxPages)
            {
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                for (int i = 0; i < totalPages; i++)
                    if (currentPage < maxPages + i * (maxPages - 1))
                    {
                        startPage = (maxPages - 1) * i + 1;
                        endPage = (maxPages - 1) * (i + 1) + 1;
                        break;
                    }
                    else if (currentPage == maxPages + i * (maxPages - 1))
                    {
                        startPage = currentPage;
                        endPage = currentPage + (maxPages - 1);
                        break;
                    }
            }
            if (endPage > totalPages)
                endPage = totalPages;
        }

        public  string SalGetDeepCleanCompanyName(string strCompanyName)
        {
            if (strCompanyName == null)
                return null;
            string strCleanCompanyName = strCompanyName.Replace(" L.L.C", "");
            strCleanCompanyName = new Regex(@"\(.+\)").Replace(strCompanyName, "");
            strCleanCompanyName = new Regex(@"\b(incorporated|corporation|Corporate|inc|llc|corp|group|company|limited|.com|co|LLP|LP|GP)\b", RegexOptions.IgnoreCase).Replace(strCleanCompanyName, "");
            strCleanCompanyName = string.Join(" ", strCleanCompanyName.Tokenized().Distinct()).Trim(' ', '.');
            return strCleanCompanyName.Length == 0 ? strCompanyName : strCleanCompanyName;
        }

        #region IP Address Parser

        public string SalGetClientIp(HttpRequestBase request)
        {
            var ipAddress = request.UserHostAddress;

            // get client IP from reverse proxy
            var xForwardedFor = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(xForwardedFor))
            {
                // Search for public IP addresses 
                var publicForwardingIps = xForwardedFor.Split(',').Where(ip => !SalIsPrivateIpAddress(ip)).ToList();

                // If we found any public IP, return the last one, otherwise return the user host address
                return publicForwardingIps.Any() ? publicForwardingIps.Last().Trim() : ipAddress;
            }

            return ipAddress;

        }

        /// <summary>
        /// Parse IP by stripping port value if any 
        /// </summary>
        private IPAddress SalParseIp(string ipAddress)
        {
            ipAddress = ipAddress.Trim();
            int portDelimiterPos = ipAddress.LastIndexOf(":", StringComparison.InvariantCultureIgnoreCase);
            bool ipv6WithPortStart = ipAddress.StartsWith("[");
            int ipv6End = ipAddress.IndexOf("]");
            if (portDelimiterPos != -1
                && portDelimiterPos == ipAddress.IndexOf(":", StringComparison.InvariantCultureIgnoreCase)
                || ipv6WithPortStart && ipv6End != -1 && ipv6End < portDelimiterPos)
            {
                ipAddress = ipAddress.Substring(0, portDelimiterPos);
            }

            return IPAddress.Parse(ipAddress);
        }

        private bool SalIsPrivateIpAddress(string ipAddress)
        {
            // http://en.wikipedia.org/wiki/Private_network
            // Private IP Addresses are: 
            //  24-bit block: 10.0.0.0 through 10.255.255.255
            //  20-bit block: 172.16.0.0 through 172.31.255.255
            //  16-bit block: 192.168.0.0 through 192.168.255.255
            //  Link-local addresses: 169.254.0.0 through 169.254.255.255 (http://en.wikipedia.org/wiki/Link-local_address)

            var ip = SalParseIp(ipAddress);
            var octets = ip.GetAddressBytes();

            bool isIpv6 = octets.Length == 16;

            if (isIpv6)
            {
                bool isUniqueLocalAddress = octets[0] == 253;
                return isUniqueLocalAddress;
            }
            else
            {
                var is24BitBlock = octets[0] == 10;
                if (is24BitBlock) return true;

                var is20BitBlock = octets[0] == 172 && octets[1] >= 16 && octets[1] <= 31;
                if (is20BitBlock) return true;

                var is16BitBlock = octets[0] == 192 && octets[1] == 168;
                if (is16BitBlock) return true;

                var isLinkLocalAddress = octets[0] == 169 && octets[1] == 254;
                return isLinkLocalAddress;
            }
        }

        #endregion  IP Address Parser

        public string SalUpperCaseFirstLetter(string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
                return string.Empty;

            TextInfo objTextInfo = _nfCulture.TextInfo;
            return objTextInfo.ToTitleCase(strValue.ToLower());
        }

        public string SalGetPostDateDesc(DateTime dtPostDate)
        {
            DateTime now = DateTime.Now;
            if (now.AddMonths(-1) <= dtPostDate)
            {
                TimeSpan tsSpan = now.Subtract(dtPostDate);
                if (tsSpan.Days == 0)
                {
                    return "Just Posted";
                }
                else if (tsSpan.Days == 1)
                {
                    return "1 Day Ago";
                }
                return tsSpan.Days + " Days Ago";
            }
            else
            {
                int intYearDiff = now.Year - dtPostDate.Year;
                int intMonthDiff = now.Month - dtPostDate.Month;
                int intTotalMonth = intYearDiff * 12 + intMonthDiff;
                int intMonth = intTotalMonth % 12;
                if (intMonth == 1)
                {
                    return "1 Month Ago";
                }
                else if (intMonth <= 11)
                {
                    return intMonth + " Months Ago";
                }
                else
                {
                    return "1 Year Ago";
                }
            }
        }

        public string SalGetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
        }


        public string SalGetFirstSentence(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml))
                return null;

            var regex = new Regex("(<.*?>)");
            var newText = regex.Replace(strHtml, "\n");
            var sentences = newText.Split('\n', '.', ';', ',');
            var sentence = sentences.FirstOrDefault(a => a.Split(' ').Length >= 3);
            if (sentence == null)
                return null;
            sentence = sentence.Trim(' ', '\t', '*', '#', '-');
            sentence = sentence.Substring(0, Math.Min(sentence.Length, 30));
            return sentence;
        }

        public string SalGetLastSentence(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml))
                return null;

            var regex = new Regex("(<.*?>)");
            var newText = regex.Replace(strHtml, "\n");
            var sentences = newText.Split('\n', '.', ';', ',');
            var sentence = sentences.LastOrDefault(a => a.Split(' ').Length >= 3);
            if (sentence == null)
                return null;
            sentence = sentence.Trim(' ', '\t', '*', '#', '-');
            sentence = sentence.Substring(0, Math.Min(sentence.Length, 30));
            return sentence;
        }
        public bool SalIsStartWithVowel(char c)
        {
            int intVowelMask = (1 << 1) | (1 << 5) | (1 << 9) | (1 << 15) | (1 << 21);
            return (c > 64) && ((intVowelMask & (1 << ((c | 0x20) % 32))) != 0);
        }

        public string SalJavaScriptStringEncode(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            StringBuilder b = null;
            int startIndex = 0;
            int count = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];

                // Append the unhandled characters (that do not require special treament)
                // to the string builder when special characters are detected.
                if (CharRequiresJavaScriptEncoding(c))
                {
                    if (b == null)
                    {
                        b = new StringBuilder(value.Length + 5);
                    }

                    if (count > 0)
                    {
                        b.Append(value, startIndex, count);
                    }

                    startIndex = i + 1;
                    count = 0;
                }

                switch (c)
                {
                    case '\r':
                        b.Append("\\r");
                        break;
                    case '\t':
                        b.Append("\\t");
                        break;
                    case '\"':
                        b.Append("\\\"");
                        break;
                    case '\\':
                        b.Append("\\\\");
                        break;
                    case '\n':
                        b.Append("\\n");
                        break;
                    case '\b':
                        b.Append("\\b");
                        break;
                    case '\f':
                        b.Append("\\f");
                        break;
                    default:
                        if (CharRequiresJavaScriptEncoding(c))
                        {
                            AppendCharAsUnicodeJavaScript(b, c);
                        }
                        else
                        {
                            count++;
                        }
                        break;
                }
            }

            if (b == null)
            {
                return value;
            }

            if (count > 0)
            {
                b.Append(value, startIndex, count);
            }

            return b.ToString();
        }
        private bool CharRequiresJavaScriptEncoding(char c)
        {
            return c < 0x20 // control chars always have to be encoded
                || c == '\"' // chars which must be encoded per JSON spec
                || c == '\\'
                || c == '\'' // HTML-sensitive chars encoded for safety
                             //|| c == '<'
                             //|| c == '>'
                             //|| (c == '&') // Bug Dev11 #133237. Encode '&' to provide additional security for people who incorrectly call the encoding methods (unless turned off by backcompat switch)
                || c == '\u0085' // newline chars (see Unicode 6.2, Table 5-1 [http://www.unicode.org/versions/Unicode6.2.0/ch05.pdf]) have to be encoded (DevDiv #663531)
                || c == '\u2028'
                || c == '\u2029';
        }

        private void AppendCharAsUnicodeJavaScript(StringBuilder builder, char c)
        {
            builder.Append("\\u");
            builder.Append(((int)c).ToString("x4", CultureInfo.InvariantCulture));
        }

        public Hashtable SalGetStateList()
        {
            Hashtable states = new Hashtable(StringComparer.OrdinalIgnoreCase);
            states.Add("AL", "Alabama");
            states.Add("AK", "Alaska");
            states.Add("AZ", "Arizona");
            states.Add("AR", "Arkansas");
            states.Add("CA", "California");
            states.Add("CO", "Colorado");
            states.Add("CT", "Connecticut");
            states.Add("DE", "Delaware");
            states.Add("DC", "District of Columbia");
            states.Add("FL", "Florida");
            states.Add("GA", "Georgia");
            states.Add("HI", "Hawaii");
            states.Add("ID", "Idaho");
            states.Add("IL", "Illinois");
            states.Add("IN", "Indiana");
            states.Add("IA", "Iowa");
            states.Add("KS", "Kansas");
            states.Add("KY", "Kentucky");
            states.Add("LA", "Louisiana");
            states.Add("ME", "Maine");
            states.Add("MD", "Maryland");
            states.Add("MA", "Massachusetts");
            states.Add("MI", "Michigan");
            states.Add("MN", "Minnesota");
            states.Add("MS", "Mississippi");
            states.Add("MO", "Missouri");
            states.Add("MT", "Montana");
            states.Add("NE", "Nebraska");
            states.Add("NV", "Nevada");
            states.Add("NH", "New Hampshire");
            states.Add("NJ", "New Jersey");
            states.Add("NM", "New Mexico");
            states.Add("NY", "New York");
            states.Add("NC", "North Carolina");
            states.Add("ND", "North Dakota");
            states.Add("OH", "Ohio");
            states.Add("OK", "Oklahoma");
            states.Add("OR", "Oregon");
            states.Add("PA", "Pennsylvania");
            states.Add("RI", "Rhode Island");
            states.Add("SC", "South Carolina");
            states.Add("SD", "South Dakota");
            states.Add("TN", "Tennessee");
            states.Add("TX", "Texas");
            states.Add("UT", "Utah");
            states.Add("VT", "Vermont");
            states.Add("VA", "Virginia");
            states.Add("WA", "Washington");
            states.Add("WV", "West Virginia");
            states.Add("WI", "Wisconsin");
            states.Add("WY", "Wyoming");
            return states;
        }

        public string SalMakeSEOFriendlyName(string strValue, bool bolConvertToLower = true)
        {
            if (String.IsNullOrEmpty(strValue))
                return "";
            else
            {
                strValue = strValue.Trim();
                //strValue = Regex.Replace(strValue, "C++", "C-Plus-Plus", RegexOptions.IgnoreCase);
                //strValue = Regex.Replace(strValue, "C#", "CSharp", RegexOptions.IgnoreCase);

                strValue = Regex.Replace(strValue, "[^0-9a-zA-Z]+", "-", RegexOptions.IgnoreCase);
                strValue = Regex.Replace(strValue, "[-]+", "-", RegexOptions.IgnoreCase);
                strValue = strValue.Trim('-');

                if (bolConvertToLower)
                {
                    strValue = strValue.ToLower();
                }
                return strValue;
            }
        }
    } // class Seog_Crr_Utility
}