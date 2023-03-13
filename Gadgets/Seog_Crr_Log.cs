using System;
using System.Text;
using System.Web.Configuration;

namespace CareerCom.Cool.Gadgets
{
    public class Seog_Crr_Log
    {
        private readonly string strErrorFilePath;
        private readonly bool IsEnableDebugMode;
        public Seog_Crr_Log()
        {
            strErrorFilePath = String.Format("{0}\\CareerCloud\\{1}_{2}.log", WebConfigurationManager.AppSettings["ErrorFilePath"].ToString(), WebConfigurationManager.AppSettings["ProjectName"].ToString(), DateTime.Now.ToString("yyyyMMdd"));
            IsEnableDebugMode = Convert.ToInt32(WebConfigurationManager.AppSettings["IsEnableDebugMode"].ToString()) != 0 ? true : false;

        }
        public void SalError(string strMessage, Exception ex)
        {
            try
            {
                string strMessageContent = String.Format("{0}\t{1}", DateTime.Now.ToString("HH:mm:ss"), strMessage);
                if (ex != null)
                    strMessageContent = String.Format("{0}\t{1}\r\n{2}", strMessageContent, ex.Message, ex.StackTrace);

                System.IO.File.AppendAllText(strErrorFilePath, strMessageContent + "\r\n", Encoding.UTF8);
            }
            catch
            {
            }
        }

        public void SalError(string strMessage)
        {

            SalError(strMessage, null);
        }


    }
}