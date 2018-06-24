using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class ManageIndividualModel
    {
        public IndividualModel Individual { get; set; }
        public string CompanyIds { get; set; }

        private string _tempIndividualLoc = string.Empty;
        public string TempIndividualLoc
        {
            get
            {
                if (_tempIndividualLoc == string.Empty)
                {
                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                    var stringChars = new char[6];
                    var random = new Random();

                    for (int i = 0; i < stringChars.Length; i++)
                    {
                        stringChars[i] = chars[random.Next(chars.Length)];
                    }

                    var randomString = new String(stringChars);
                    _tempIndividualLoc = "_" + randomString;
                }
                return _tempIndividualLoc;
            }

            set
            {
                _tempIndividualLoc = value;
            }
        }
    }
}
