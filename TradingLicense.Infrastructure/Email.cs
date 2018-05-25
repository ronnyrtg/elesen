using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace TradingLicense.Infrastructure
{
    public class Email
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Email" /> class.
        /// </summary>
        public Email()
        {
        }

        /// <summary>
        /// Email type
        /// </summary>
        public enum EmailType
        {
            /// <summary>
            /// Default Type with Master Template
            /// </summary>
            Default,

            /// <summary>
            /// Mail without Master Template
            /// </summary>
            NoMaster
        }

        #endregion

        /// <summary>
        /// Sending An Email with master mail template
        /// </summary>
        /// <param name="mailFrom">Mail From</param>
        /// <param name="mailTo">Mail To</param>
        /// <param name="mailCC">Mail CC</param>
        /// <param name="mailBCC">Mail BCC</param>
        /// <param name="subject">Mail Subject</param>
        /// <param name="body">Mail Body</param>
        /// <param name="emailType">Email Type</param>
        /// <param name="attachment">Mail Attachment</param>
        /// <param name="attachmentByteList">Attachment byte list for the mail</param>
        /// <param name="attachmentName">Attachment file name for the mail</param>
        /// <returns>return send status</returns>
        public static bool Send(string mailFrom, string mailTo, string mailCC, string mailBCC, string subject, string body, EmailType emailType, string attachment, List<byte[]> attachmentByteList = null, string attachmentName = null)
        {
            if (ProjectConfiguration.IsEmailTest)
            {
                mailTo = ProjectConfiguration.TestEmailToAddress;
                mailCC = string.Empty;
                mailBCC = string.Empty;
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (!string.IsNullOrEmpty(mailTo))
            {
                mailTo = mailTo.Trim(';').Trim(',');
            }

            if (!string.IsNullOrEmpty(mailCC))
            {
                mailCC = mailCC.Trim(';').Trim(',');
            }

            if (!string.IsNullOrEmpty(mailBCC))
            {
                mailBCC = mailBCC.Trim(';').Trim(',');
            }

            if (ValidateEmail(mailFrom, mailTo) && (string.IsNullOrEmpty(mailCC) || ValidateEmail(mailCC)) && (string.IsNullOrEmpty(mailBCC) || ValidateEmail(mailBCC)))
            {
                System.Net.Mail.MailMessage mailMesg = new System.Net.Mail.MailMessage();
                mailMesg.From = new System.Net.Mail.MailAddress(mailFrom);
                if (!string.IsNullOrEmpty(mailTo))
                {
                    mailTo = mailTo.Replace(";", ",");
                    mailMesg.To.Add(mailTo);
                }

                if (!string.IsNullOrEmpty(mailCC))
                {
                    mailCC = mailCC.Replace(";", ",");
                    mailMesg.CC.Add(mailCC);
                }

                if (!string.IsNullOrEmpty(mailBCC))
                {
                    mailBCC = mailBCC.Replace(";", ",");
                    mailMesg.Bcc.Add(mailBCC);
                }

                if (!string.IsNullOrEmpty(attachment) && string.IsNullOrEmpty(attachmentName))
                {
                    string[] attachmentArray = attachment.Trim(';').Split(';');
                    foreach (string attachFile in attachmentArray)
                    {
                        try
                        {
                            System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(attachFile);
                            mailMesg.Attachments.Add(attach);
                        }
                        catch
                        {
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(attachment) && !string.IsNullOrEmpty(attachmentName))
                {
                    string[] attachmentArray = attachment.Trim(';').Split(';');
                    string[] attachmentNameArray = attachmentName.Trim(';').Split(';');

                    if (attachmentArray.Length == attachmentNameArray.Length)
                    {
                        for (int cnt = 0; cnt <= attachmentArray.Length - 1; cnt++)
                        {
                            if (System.IO.File.Exists(attachmentArray[cnt]))
                            {
                                try
                                {
                                    string fileName = ConvertTo.String(attachmentName[cnt]);
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        System.IO.FileStream fs = new System.IO.FileStream(attachmentArray[cnt], System.IO.FileMode.Open, System.IO.FileAccess.Read);
                                        System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(fs, fileName);
                                        mailMesg.Attachments.Add(attach);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }

                if (attachmentByteList != null && attachmentName != null)
                {
                    string[] attachmentNameArray = attachmentName.Trim(';').Split(';');

                    if (attachmentByteList.Count == attachmentNameArray.Length)
                    {
                        for (int cnt = 0; cnt <= attachmentByteList.Count - 1; cnt++)
                        {
                            string fileName = attachmentNameArray[cnt];
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                try
                                {
                                    MemoryStream ms = new MemoryStream(attachmentByteList[cnt]);
                                    System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(ms, fileName);
                                    mailMesg.Attachments.Add(attach);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }

                mailMesg.Subject = subject;
                mailMesg.AlternateViews.Add(GetMasterBody(body, subject, emailType));
                mailMesg.IsBodyHtml = true;

                System.Net.Mail.SmtpClient objSMTP = new System.Net.Mail.SmtpClient();
                try
                {
                    objSMTP.Send(mailMesg);
                    return true;
                }
                catch (Exception ex)
                {
                    LogWritter.WriteErrorFile(ex);
                }
                finally
                {
                    objSMTP.Dispose();
                    mailMesg.Dispose();
                    mailMesg = null;
                }
            }

            return false;
        }

        public static bool SendForWindowService(string mailFrom, string mailTo, string mailCC, string mailBCC, string subject, string body, string path)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (!string.IsNullOrEmpty(mailTo))
            {
                mailTo = mailTo.Trim(';').Trim(',');
            }

            if (!string.IsNullOrEmpty(mailCC))
            {
                mailCC = mailCC.Trim(';').Trim(',');
            }

            if (!string.IsNullOrEmpty(mailBCC))
            {
                mailBCC = mailBCC.Trim(';').Trim(',');
            }

            if (ValidateEmail(mailFrom, mailTo) && (string.IsNullOrEmpty(mailCC) || ValidateEmail(mailCC)) && (string.IsNullOrEmpty(mailBCC) || ValidateEmail(mailBCC)))
            {
                System.Net.Mail.MailMessage mailMesg = new System.Net.Mail.MailMessage();
                mailMesg.From = new System.Net.Mail.MailAddress(mailFrom);
                if (!string.IsNullOrEmpty(mailTo))
                {
                    mailTo = mailTo.Replace(";", ",");
                    mailMesg.To.Add(mailTo);
                }

                if (!string.IsNullOrEmpty(mailCC))
                {
                    mailCC = mailCC.Replace(";", ",");
                    mailMesg.CC.Add(mailCC);
                }

                if (!string.IsNullOrEmpty(mailBCC))
                {
                    mailBCC = mailBCC.Replace(";", ",");
                    mailMesg.Bcc.Add(mailBCC);
                }

                mailMesg.Subject = subject;

                mailMesg.AlternateViews.Add(GetMasterBodyForWindowService(body, subject, path));
                mailMesg.IsBodyHtml = true;

                System.Net.Mail.SmtpClient objSMTP = new System.Net.Mail.SmtpClient();
                try
                {
                    objSMTP.Send(mailMesg);
                    return true;
                }
                catch (Exception ex)
                {
                    LogWritter.WriteErrorFile(ex);
                }
                finally
                {
                    objSMTP.Dispose();
                    mailMesg.Dispose();
                    mailMesg = null;
                }
            }

            return false;
        }

        /// <summary>
        ///  Sending An Email with master mail template
        /// </summary>
        /// <param name="mailFrom">Mail From</param>
        /// <param name="mailTo">Mail To</param>
        /// <param name="mailCC">Mail CC</param>
        /// <param name="mailBCC">Mail BCC</param>
        /// <param name="subject">Mail Subject</param>
        /// <param name="body">Mail Body</param>

        /// <returns>return send status</returns>
        public static bool Send(string mailFrom, string mailTo, string mailCC, string mailBCC, string subject, string body)
        {
            return Send(mailFrom, mailTo, mailCC, mailBCC, subject, body, EmailType.Default, string.Empty);
        }

        /// <summary>
        /// Read the Template from Format and return
        /// </summary>
        /// <param name="emailTemplate">Email Template</param>
        /// <returns>Return body Of Email Template</returns>
        public static string GetEmailTemplate(string emailTemplate)
        {
            string bodyTemplate = string.Empty;
            string filePath = ProjectConfiguration.EmailTemplatePath + emailTemplate + ".html";

            if (File.Exists(filePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        bodyTemplate = reader.ReadToEnd();
                    }
                }
                catch
                {
                    throw;
                }
            }

            return bodyTemplate;
        }

        /// <summary>
        /// Read the Template from Format and return
        /// </summary>
        /// <param name="emailTemplate">Email Template</param>
        /// <returns>Return body Of Email Template</returns>
        public static string GetEmailTemplateForWindowService(string emailTemplate, string path)
        {
            string bodyTemplate = string.Empty;
            string filePath = path + ProjectConfiguration.EmailTemplateFloder + emailTemplate + ".html";
            if (File.Exists(filePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        bodyTemplate = reader.ReadToEnd();
                    }
                }
                catch
                {
                    throw;
                }
            }


            return bodyTemplate;
        }

        /// <summary>
        /// Method is used to Validate Email
        /// </summary>
        /// <param name="fromEmail">From email List</param>
        /// <param name="toEmail">To Email list</param>
        /// <returns>Returns validation result</returns>
        private static bool ValidateEmail(string fromEmail, string toEmail)
        {
            bool isValid = true;
            if (!IsEmail(fromEmail))
            {
                isValid = false;
            }

            isValid = ValidateEmail(toEmail);

            return isValid;
        }

        /// <summary>
        /// Method is used to Validate Email
        /// </summary>
        /// <param name="emails">Email list</param>
        /// <returns>Returns validation result</returns>
        private static bool ValidateEmail(string emails)
        {
            bool isValid = true;

            if (!string.IsNullOrEmpty(emails))
            {
                emails = emails.Replace(" ", string.Empty);
                string[] emailList = null;
                try
                {
                    emails = emails.Replace(';', ',');
                    emailList = emails.Split(',');
                }
                catch
                {
                    isValid = false;
                }

                if (emailList != null && emailList.Count() > 0)
                {
                    foreach (string email in emailList)
                    {
                        if (!IsEmail(email))
                        {
                            isValid = false;
                        }
                    }
                }
                else
                {
                    isValid = false;
                }
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Check email string is Email or not
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>return email validation result</returns>
        private static bool IsEmail(string email)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(strRegex);
            if (re.IsMatch(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get Master Body HTML
        /// </summary>
        /// <param name="body">Body Text</param>
        /// <param name="subject">Mail Subject</param>
        /// <param name="emailType">Email Type</param>
        /// <returns>Alternate View</returns>
        private static System.Net.Mail.AlternateView GetMasterBody(string body, string subject, EmailType emailType)
        {
            if (emailType == EmailType.Default)
            {
                string masterEmailTemplate = Email.GetEmailTemplate(SystemEnum.EmailTemplateFileName.MasterEmailTemplate.ToString());
                body = masterEmailTemplate.Replace("[@MainContent]", body);

                body = body.Replace("[@Subject]", subject);

                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(body, null, "text/html");

                string logo = ProjectConfiguration.ApplicationRootPath + "\\Content\\images\\logo.png";

                System.Net.Mail.LinkedResource logoResource = new System.Net.Mail.LinkedResource(logo, "image/gif");
                logoResource.ContentId = "LogoImage";
                logoResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                htmlView.LinkedResources.Add(logoResource);
                return htmlView;
            }
            else
            {
                return System.Net.Mail.AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            }
        }


        private static System.Net.Mail.AlternateView GetMasterBodyForWindowService(string body, string subject, string path)
        {
            string masterEmailTemplate = Email.GetEmailTemplateForWindowService(SystemEnum.EmailTemplateFileName.MasterEmailTemplate.ToString(), path);
            body = masterEmailTemplate.Replace("[@MainContent]", body);

            body = body.Replace("[@Subject]", subject);

            System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(body, null, "text/html");

            string logo = path + "\\Content\\images\\logo.png";

            System.Net.Mail.LinkedResource logoResource = new System.Net.Mail.LinkedResource(logo, "image/gif");
            logoResource.ContentId = "LogoImage";
            logoResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            htmlView.LinkedResources.Add(logoResource);
            return htmlView;


        }
    }
}
