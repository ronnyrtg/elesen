using System;
using System.IO;
using System.Text;
using System.Web;

namespace TradingLicense.Infrastructure
{
    public class LogWritter
    {
        #region Methods

        /// <summary>
        /// write log files for exception
        /// </summary>
        /// <param name="ex">ex value</param>
        public static void WriteErrorFile(Exception ex)
        {
            if (ex != null && !string.IsNullOrEmpty(ex.Message) && !ex.Message.Contains("Thread was being aborted."))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("DateTime = " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + System.Environment.NewLine);
                sb.Append("ExceptionMesage = " + ex.Message + System.Environment.NewLine);
                if (ex.InnerException != null)
                {
                    sb.Append("Exception Inner Exception = " + ex.InnerException + System.Environment.NewLine);
                }

                sb.Append("Exception Source = " + ex.Source + System.Environment.NewLine);
                sb.Append("ExceptionStack = " + ex.StackTrace + System.Environment.NewLine);
                WriteErrorFile(sb.ToString(), true);
            }
        }

        /// <summary>
        /// write log files for error text
        /// </summary>
        /// <param name="textTowrite">Text To write value</param>
        /// <param name="isNewLine">is NewLine value</param>
        public static void WriteErrorFile(string textTowrite, bool isNewLine)
        {
            try
            {
                if (!ProjectConfiguration.IsLogError)
                {
                    return;
                }

                string fileName = DateTime.Now.ToString("ddMMyyyy") + "error.txt";

                if (HttpContext.Current != null && HttpContext.Current.Server != null)
                {
                    IOManager.CreateDirectory(HttpContext.Current.Server.MapPath("~/Error"));
                }

                string txtFolderPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Error");
                string txtPath = txtFolderPath + "/" + fileName;

                IOManager.CreateFile(txtPath);

                if (System.IO.File.Exists(txtPath))
                {
                    File.AppendAllText(txtPath, System.Environment.NewLine + textTowrite);

                    if (isNewLine)
                    {
                        File.AppendAllText(txtPath, "---------------------------------------------");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
