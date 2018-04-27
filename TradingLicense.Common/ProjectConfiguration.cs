using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Common
{
    public class ProjectConfiguration
    {
        public static string AttachmentDocument
        {
            get
            {
                return ConvertTo.String(ConfigurationManager.AppSettings.Get("Attachment"));
            }
        }
    }
}
