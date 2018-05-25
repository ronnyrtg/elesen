using System;

namespace TradingLicense.Infrastructure
{
    /// <summary>
    /// This class is used to define all IO related operation
    /// </summary>
    /// <CreatedBy></CreatedBy>
    /// <CreatedDate>03-01-2018</CreatedDate>
    /// <ModifiedBy></ModifiedBy>
    /// <ModifiedDate></ModifiedDate>
    /// <ReviewBy></ReviewBy>
    /// <ReviewDate></ReviewDate>
    public class IOManager
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IOManager" /> class.
        /// </summary>
        public IOManager()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create Directory
        /// </summary>
        /// <param name="directory">directory Name</param>
        /// <returns>Return Created Directory Status</returns>
        public static bool CreateDirectory(string directory)
        {
            try
            {
                if (!System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Create File
        /// </summary>
        /// <param name="fileName">file Name</param>
        /// <returns>Return File Created Status</returns>
        public static bool CreateFile(string fileName)
        {
            try
            {
                if (!System.IO.File.Exists(fileName))
                {
                    System.IO.File.Create(fileName).Close();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
