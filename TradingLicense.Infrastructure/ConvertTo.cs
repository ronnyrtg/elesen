using System;
using System.Globalization;

namespace TradingLicense.Infrastructure
{
    /// <summary>
    ///  Manage to Convert the Data Type to other Data Type
    /// </summary>
    /// <CreatedBy></CreatedBy>
    /// <CreatedDate>21-04-2018</CreatedDate>
    /// <ModifiedBy></ModifiedBy>
    /// <ModifiedDate></ModifiedDate>
    /// <ReviewBy></ReviewBy>
    /// <ReviewDate></ReviewDate>
    public sealed class ConvertTo
    {
        #region Constructor

        /// <summary>
        /// Prevents a default instance of the ConvertTo class from being created.
        /// </summary>
        private ConvertTo()
        {
        }

        #endregion

        #region Variable/Property Declaration
        #endregion

        #region Methods/Functions

        /// <summary> 
        /// check for given value is null string 
        /// </summary> 
        /// <param name="readField">object to convert</param> 
        /// <returns>if value=string return string else ""</returns> 
        public static string String(object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    return Convert.ToString(readField, CultureInfo.InvariantCulture);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }



        /// <summary> 
        /// check given value is boolean or null 
        /// </summary> 
        /// <param name="readField">object to convert</param> 
        /// <returns>return true else false</returns> 
        public static bool Boolean(object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    bool x;
                    if (bool.TryParse(Convert.ToString(readField, CultureInfo.InvariantCulture), out x))
                    {
                        return x;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary> 
        /// check given value is integer or null 
        /// </summary> 
        /// <param name="readField">object to convert</param> 
        /// <returns>return integer else 0</returns> 
        public static int Integer(object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    if (readField.ToString().Trim().Length == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        int toReturn;
                        if (int.TryParse(Convert.ToString(readField, CultureInfo.InvariantCulture), out toReturn))
                        {
                            return toReturn;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }


        #endregion
    }
}
