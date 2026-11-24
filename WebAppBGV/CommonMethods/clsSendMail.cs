using BAL;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using ViewModels;
using static WebAppBGV.CommonMethods.EmailSmartping;

namespace WebAppBGV.CommonMethods
{
    public class clsSendMail
    {
        //private string Host = ConfigurationManager.AppSettings["SMTPServer"].ToString();
        //private string Port = ConfigurationManager.AppSettings["SMTPPort"].ToString();
        //private string UserID = ConfigurationManager.AppSettings["SMTPUserID"].ToString();
        //private string Pass = ConfigurationManager.AppSettings["SMTPPwd"].ToString();
        //private string EnableSsl = ConfigurationManager.AppSettings["EnableSsl"].ToString();

        ICompanyRepository repoCompany = new CompanyRepository();
        private string Host = string.Empty;
        private string Port = string.Empty;
        private string UserID = string.Empty;
        private string Pass = string.Empty;
        private bool EnableSsl = false;
        EmailSmartping emailSmartping = new EmailSmartping();
        public clsSendMail()
        {
            try
            {
                CompanyMailAuthenticationViewModel model = repoCompany.GetMailAuthenticationById(1);

                if (model != null)
                {
                    this.Host = model.SMTPServer;
                    this.Port = model.Port;
                    this.UserID = model.SMTPUserName;
                    this.Pass = clsCommonMethods.PasswordDecrypt(model.SMTPPassword);
                    this.EnableSsl = model.EnableSsl;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public clsSendMail(string SMTPServer, string SMTPPort, string SMTPUserID, string SMTPPwd, bool EnableSsl)
        {
            this.Host = SMTPServer;
            this.Port = SMTPPort;
            this.UserID = SMTPUserID;
            this.Pass = SMTPPwd;
            this.EnableSsl = EnableSsl;
        }
        public bool SendmicrolandMail(string display, string from, string to, string subj, string msg)
        {
            string to1 = "Bhoopoendra.malik@securitas-india.com";
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            bool Result = false;
            try
            {
                if (to.Contains(','))
                {
                    string[] arrTO = to.Split(',');
                    if (to.Length > 0)
                    {
                        foreach (string val in arrTO)
                        {
                            Message.To.Add(new MailAddress(val.Trim()));
                        }
                    }
                }
                else
                {
                    Message.To.Add(new MailAddress(to));
                }
                try
                {
                    string res = emailSmartping.SendMail(0, from, to, "", "", subj, msg, null);
                    if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                    {
                        return Result = true;
                    }
                }
                catch (Exception ex)
                {

                }
                //Message.To.Add("bejgam.rahul@microland.com");
                Message.Subject = subj; //"Securitas Test Mail";
                Message.Body = msg;//"Test Message";
                Message.From = new MailAddress("TalentAcquisition@microland.com", subj);
                Message.IsBodyHtml = true;
                SMTP.Host = "ob1.hc2993-97.iphmx.com";
                SMTP.Port = Convert.ToInt32(25);
                SMTP.Send(Message);
                Result = true;

            }
            catch (SmtpException SmtpEx)
            {
                //MessageBox.Show(SmtpEx.Message);
                // MessageBox.Show(SmtpEx.InnerException.ToString());
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendmicrolandMail(string display, string from, string to, string cc, string subj, string msg)
        {
            string to1 = "Bhoopoendra.malik@securitas-india.com";
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            bool Result = false;
            try
            {

                string ToMails = "";
                string ccMails = "";

                ToMails = to.Replace(",", ";");
                ccMails = cc.Replace(",", ";");
                if (ToMails != "")
                {
                    if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                        ToMails = ToMails.Substring(0, ToMails.Length - 1);


                    if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                        ToMails = ToMails.Substring(1, ToMails.Length - 1);
                }

                if (ccMails != "")
                {
                    if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                        ccMails = ccMails.Substring(0, ccMails.Length - 1);

                    if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                        ccMails = ccMails.Substring(1, ccMails.Length - 1);
                }

                string[] arToMails = ToMails.Split(';');
                ToMails = "";
                foreach (string a in arToMails)
                {
                    if (!ToMails.Contains(a))
                    {
                        ToMails = ToMails + a.Trim() + ",";
                    }
                }
                string[] arccMails = ccMails.Split(';');
                ccMails = "";
                foreach (string b in arccMails)
                {
                    if (!ccMails.Contains(b))
                    {
                        ccMails = ccMails + b.Trim() + ",";
                    }
                }

                if (ToMails != "")
                {
                    if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                        ToMails = ToMails.Substring(0, ToMails.Length - 1);


                    if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                        ToMails = ToMails.Substring(1, ToMails.Length - 1);
                }

                if (ccMails != "")
                {
                    if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                        ccMails = ccMails.Substring(0, ccMails.Length - 1);

                    if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                        ccMails = ccMails.Substring(1, ccMails.Length - 1);
                }


                if (ToMails.Contains(','))
                {
                    string[] arrTO = ToMails.Split(',');
                    if (ToMails.Length > 0)
                    {
                        foreach (string val in arrTO)
                        {
                            Message.To.Add(new MailAddress(val.Trim()));
                        }
                    }
                }
                else
                {
                    Message.To.Add(new MailAddress(ToMails));
                }

                //+++++++++++ Multiple CC
                if (ccMails.Contains(','))
                {
                    string[] arrCC = ccMails.Split(',');
                    if (ccMails.Length > 0)
                    {
                        foreach (string val in arrCC)
                        {
                            Message.CC.Add(new MailAddress(val.Trim()));
                        }
                    }
                }
                else
                {
                    Message.CC.Add(new MailAddress(ccMails));
                }
                try
                {
                    string res = emailSmartping.SendMail(0, from, to, cc, "", subj, msg, null);
                    if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                    {
                        return Result = true;
                    }
                }
                catch (Exception ex)
                {

                }
                //Message.To.Add("bejgam.rahul@microland.com");
                Message.Subject = subj; //"Securitas Test Mail";
                Message.Body = msg;//"Test Message";
                Message.From = new MailAddress("TalentAcquisition@microland.com", subj);
                Message.IsBodyHtml = true;
                SMTP.Host = "ob1.hc2993-97.iphmx.com";
                SMTP.Port = Convert.ToInt32(25);
                SMTP.Send(Message);
                Result = true;

            }
            catch (SmtpException SmtpEx)
            {
                //MessageBox.Show(SmtpEx.Message);
                // MessageBox.Show(SmtpEx.InnerException.ToString());
                throw SmtpEx;
            }

            return Result;
        }



        private string ApplyCtrlsMobile(string to, string cc, string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                try
                {
                    ClientCustomize ccObj = new ClientCustomize();
                    string customMob = ccObj.GetCustomMobileByEmail(to, cc);
                    if (!string.IsNullOrEmpty(customMob))
                        msg = msg.Replace("9599177996", customMob);
                }
                catch (Exception) { }
            }
            return msg;
        }

        public bool SendMail(string display, string from, string to, string subj, string msg, int personalRowid = 0)
        {
            msg = ApplyCtrlsMobile(to, "", msg);
            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            ClientCustomize cc = new ClientCustomize();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && from.Length > 0)
                {

                    string ToMails = "";


                    ToMails = to.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }



                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }


                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(personalRowid, from, to, "", "", subj, msg, null);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress("bgv.request@securitas-india.com", display);
                    Message.IsBodyHtml = true;
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential("bgv.request@securitas-india.com", "wpCz1TNUZP66CBfmBJJq5g");
                    SMTP.Host = "smtp.mandrillapp.com";
                    SMTP.Port = Convert.ToInt32(587);
                    SMTP.Credentials = myCredential;

                    SMTP.EnableSsl = true;
                    SMTP.Send(Message);

                    //NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    //SMTP.Host = Host;
                    //SMTP.Port = Convert.ToInt32(Port);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = EnableSsl;
                    //SMTP.Send(Message);
                    //Result = true;


                    //Message.Subject = subj;
                    //Message.Body = msg;
                    //Message.From = new MailAddress("bgv.request@securitas-india.com", display);
                    //Message.IsBodyHtml = true;
                    //Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    //Message.Priority = MailPriority.High;
                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);
                    //Result = true;
                }
                else
                {
                    cc.InsertReportEmailSentDetails(personalRowid, subj, "Custom Error: Email To & From are invalid! :-" + msg);
                    throw new Exception("Custom Error: Email To & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                cc.InsertReportEmailSentDetails(personalRowid, subj, SmtpEx.Message + " :-" + msg);
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMail_Report(string display, string from, string to, string subj, string msg, int PersonalRowID = 0)
        {
            msg = ApplyCtrlsMobile(to, "", msg);
            bool Result = false;
            ClientCustomize cc = new ClientCustomize();
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && from.Length > 0)
                {

                    string ToMails = "";


                    ToMails = to.Replace(",", ";");

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }



                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }


                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                if (val != "")
                                    Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        if (to != "")
                            Message.To.Add(new MailAddress(to));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(PersonalRowID, from, to, "", "", subj, msg, null);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    cc.InsertReportEmailSentDetails(PersonalRowID, subj, "Custom Error: Email To & From are invalid! :-" + msg);
                    throw new Exception("Custom Error: Email To & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                cc.InsertReportEmailSentDetails(PersonalRowID, subj, SmtpEx.Message + " :-" + msg);
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }


        public bool SendMail_ReportWithAttachment(string display, string from, string to, string subj, string msg, int PersonalRowID = 0 , Attachment attach= null)
        {
            msg = ApplyCtrlsMobile(to, "", msg);
            bool Result = false;
            ClientCustomize cc = new ClientCustomize();
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && from.Length > 0  &&  attach != null)
                {

                    string ToMails = "";


                    ToMails = to.Replace(",", ";");

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }



                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }


                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                if (val != "")
                                    Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        if (to != "")
                            Message.To.Add(new MailAddress(to));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(PersonalRowID, from, to, "", "", subj, msg,  attach, attach.Name );
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Attachments.Add(attach);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    cc.InsertReportEmailSentDetails(PersonalRowID, subj, "Custom Error: Email To & From are invalid! :-" + msg);
                    throw new Exception("Custom Error: Email To & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                cc.InsertReportEmailSentDetails(PersonalRowID, subj, SmtpEx.Message + " :-" + msg);
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMail_ReportWithAttachmentwithCCNew(string ccm, string display, string from, string to, string subj, string msg, int PersonalRowID = 0, List<Attachment> attach = null)
        {
            msg = ApplyCtrlsMobile(to, ccm, msg);
            bool Result = false;
            ClientCustomize cc = new ClientCustomize();
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && from.Length > 0 && attach != null)
                {

                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = ccm.Replace(",", ";");

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }

                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    foreach (string b in arccMails)
                    {
                        if (!ccMails.Contains(b))
                        {
                            ccMails = ccMails + b.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }


                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                if (val != "")
                                    Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        if (to != "")
                            Message.To.Add(new MailAddress(to));
                    }


                    //+++++++++++ Multiple CC
                    if (ccm.Contains(','))
                    {
                        string[] arrCC = ccm.Split(',');
                        if (ccm.Length > 0)
                        {
                            foreach (string val in arrCC)
                            {
                                Message.CC.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(ccm));
                    }
                    try
                    {
                        EmailSmartping emailSmartping = new EmailSmartping();
                        string res = emailSmartping.SendMailmultiAttach(PersonalRowID, from, to, ccm, "", subj, msg, attach, "");
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    foreach (Attachment attach1 in attach)
                    {
                        Message.Attachments.Add(attach1);
                    }
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    cc.InsertReportEmailSentDetails(PersonalRowID, subj, "Custom Error: Email To & From are invalid! :-" + msg);
                    throw new Exception("Custom Error: Email To & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                cc.InsertReportEmailSentDetails(PersonalRowID, subj, SmtpEx.Message + " :-" + msg);
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }


        public bool SendMail_ReportWithAttachmentwithCC(string ccm ,string display, string from, string to, string subj, string msg, int PersonalRowID = 0, Attachment attach = null)
        {
            msg = ApplyCtrlsMobile(to, ccm, msg);
            bool Result = false;
            ClientCustomize cc = new ClientCustomize();
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && from.Length > 0 && attach != null)
                {

                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = ccm.Replace(",", ";");

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }

                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    foreach (string b in arccMails)
                    {
                        if (!ccMails.Contains(b))
                        {
                            ccMails = ccMails + b.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }


                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                if (val != "")
                                    Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        if (to != "")
                            Message.To.Add(new MailAddress(to));
                    }


                    //+++++++++++ Multiple CC
                    if (ccm.Contains(','))
                    {
                        string[] arrCC = ccm.Split(',');
                        if (ccm.Length > 0)
                        {
                            foreach (string val in arrCC)
                            {
                                Message.CC.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(ccm));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(PersonalRowID, from, to, ccm, "", subj, msg, attach, attach.Name);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Attachments.Add(attach);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    cc.InsertReportEmailSentDetails(PersonalRowID, subj, "Custom Error: Email To & From are invalid! :-" + msg);
                    throw new Exception("Custom Error: Email To & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                cc.InsertReportEmailSentDetails(PersonalRowID, subj, SmtpEx.Message + " :-" + msg);
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }


        public bool SendMail_ReportWithAttachmentwithCC_Ctrl(string ccm, string display, string from, string to, string subj, string msg, int PersonalRowID = 0, Attachment attach = null)
        {
            msg = ApplyCtrlsMobile(to, ccm, msg);
            bool Result = false;
            ClientCustomize cc = new ClientCustomize();
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && from.Length > 0 && attach != null)
                {

                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = ccm.Replace(",", ";");

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }

                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    foreach (string b in arccMails)
                    {
                        if (!ccMails.Contains(b))
                        {
                            ccMails = ccMails + b.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }


                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                if (val != "")
                                    Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        if (to != "")
                            Message.To.Add(new MailAddress(to));
                    }


                    //+++++++++++ Multiple CC
                    if (ccm.Contains(','))
                    {
                        string[] arrCC = ccm.Split(',');
                        if (ccm.Length > 0)
                        {
                            foreach (string val in arrCC)
                            {
                                Message.CC.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(ccm));
                    }
                    //try
                    //{
                    //    string res = emailSmartping.SendMail(PersonalRowID, from, to, ccm, "", subj, msg, attach, attach.Name);
                    //    if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                    //    {
                    //        return Result = true;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Attachments.Add(attach);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    Result = true;


                }
                else
                {
                    cc.InsertReportEmailSentDetails(PersonalRowID, subj, "Custom Error: Email To & From are invalid! :-" + msg);
                    throw new Exception("Custom Error: Email To & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                cc.InsertReportEmailSentDetails(PersonalRowID, subj, SmtpEx.Message + " :-" + msg);
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }



        public bool SendMail_ReportB2C(string display, string from, string to, string subj, string msg, AlternateView alternateView)
        {
            msg = ApplyCtrlsMobile(to, "", msg);
            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && from.Length > 0)
                {

                    string ToMails = "";


                    ToMails = to.Replace(",", ";");

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }



                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }


                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                if (val != "")
                                    Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        if (to != "")
                            Message.To.Add(new MailAddress(to));
                    }
                    //try
                    //{
                    //    string res = emailSmartping.SendMail(0, from, to, "", "", subj, msg, null);
                    //    if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                    //    {
                    //        return Result = true;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.AlternateViews.Add(alternateView);
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);
                    //Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMail(string display, string from, string to, string subj, string msg, Attachment attach, string fileName = "")
        {
            msg = ApplyCtrlsMobile(to, "", msg);
            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && from.Length > 0 && attach != null)
                {
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, "", "", subj, msg, attach, fileName);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Attachments.Add(attach);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;

                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMailPurg(string display, string from, string to, string subj, string msg, Attachment attach)
        {
            msg = ApplyCtrlsMobile(to, "", msg);
            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && from.Length > 0 && attach != null)
                {
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }
                    try
                    {
                        if(subj.ToLower().Trim() != "purging mail")
                        {
                            string res = emailSmartping.SendMail(0, from, to, "", "", subj, msg, attach);
                            if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                            {
                                return Result = true;
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Attachments.Add(attach);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;

                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMailPurg(string display, string from, string to, string cc, string subj, string msg, Attachment attach)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && cc.Length > 0 && from.Length > 0 && attach != null)
                {
                    //+++++++++++ Multiple To

                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = cc.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }
                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    foreach (string b in arccMails)
                    {
                        if (!ccMails.Contains(b))
                        {
                            ccMails = ccMails + b.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }

                    //+++++++++++ Multiple CC
                    if (cc.Contains(','))
                    {
                        string[] arrCC = cc.Split(',');
                        if (cc.Length > 0)
                        {
                            foreach (string val in arrCC)
                            {
                                Message.CC.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(cc));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, cc, "", subj, msg, attach);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Attachments.Add(attach);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);
                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }
        public bool SendMailPurgWithoutCC(string display, string from, string to, string subj, string msg, Attachment attach)
        {
            msg = ApplyCtrlsMobile(to, "", msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && from.Length > 0 && attach != null)
                {
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }



                    //+++++++++++ Multiple BCC                    
                    //if (bcc.Contains(','))
                    //{
                    //    string[] arrBCC = bcc.Split(',');
                    //    if (bcc.Length > 0)
                    //    {
                    //        foreach (string val in arrBCC)
                    //        {
                    //            Message.Bcc.Add(new MailAddress(val));
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    Message.Bcc.Add(new MailAddress(bcc));
                    //}
                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, "", "", subj, msg, attach);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Attachments.Add(attach);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Bcc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMailPurgWithoutCC(string display, string from, string to, string bcc, string subj, string msg, Attachment attach)
        {
            msg = ApplyCtrlsMobile(to, "", msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && bcc.Length > 0 && from.Length > 0 && attach != null)
                {
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }



                    //+++++++++++ Multiple BCC                    
                    if (bcc.Contains(','))
                    {
                        string[] arrBCC = bcc.Split(',');
                        if (bcc.Length > 0)
                        {
                            foreach (string val in arrBCC)
                            {
                                Message.Bcc.Add(new MailAddress(val));
                            }
                        }
                    }
                    else
                    {
                        Message.Bcc.Add(new MailAddress(bcc));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, "", bcc, subj, msg, attach);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Attachments.Add(attach);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Bcc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMail(string display, string from, string to, string cc, string subj, string msg, int personalRowid = 0)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);
            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            ClientCustomize ccs = new ClientCustomize();
            try
            {
                if (to.Length > 0 && cc.Length > 0 && from.Length > 0)
                {
                    //+++++++++++ Multiple To
                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = cc.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }
                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    foreach (string b in arccMails)
                    {
                        if (!ccMails.Contains(b))
                        {
                            ccMails = ccMails + b.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }


                    if (ToMails.Contains(','))
                    {
                        string[] arrTO = ToMails.Split(',');
                        if (ToMails.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(ToMails));
                    }

                    //+++++++++++ Multiple CC
                    if (ccMails.Contains(','))
                    {
                        string[] arrCC = ccMails.Split(',');
                        if (ccMails.Length > 0)
                        {
                            foreach (string val in arrCC)
                            {
                                Message.CC.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(ccMails));
                    }
                    try
                    {
                        //string res = emailSmartping.SendMail(0, from, to, cc, "", subj, msg, null);
                        //if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        //{
                        //    return Result = true;
                        //}
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    //NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    //SMTP.Host = Host;
                    //SMTP.Port = Convert.ToInt32(Port);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = EnableSsl;
                    //SMTP.Send(Message);

                    NetworkCredential myCredential = new NetworkCredential("bgv.request@securitas-india.com", "wpCz1TNUZP66CBfmBJJq5g");
                    SMTP.Host = "smtp.mandrillapp.com";
                    SMTP.Port = Convert.ToInt32(587);
                    SMTP.Credentials = myCredential;

                    SMTP.EnableSsl = true;
                    SMTP.Send(Message);


                    Result = true;
                }
                else
                {
                    ccs.InsertReportEmailSentDetails(personalRowid, subj, "Custom Error: Email To & From are invalid! :-" + msg);
                    throw new Exception("Custom Error: Email To, Bcc & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                ccs.InsertReportEmailSentDetails(personalRowid, subj, SmtpEx.Message + " :-" + msg);
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool ClientOnBoardingSendMail(string display, string from, string to, string subj, string msg, AlternateView alternateView)
        {
            msg = ApplyCtrlsMobile(to, "", msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && from.Length > 0)
                {
                    //+++++++++++ Multiple To
                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    //ccMails = cc.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    //if (ccMails != "")
                    //{
                    //    if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                    //        ccMails = ccMails.Substring(0, ccMails.Length - 1);

                    //    if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                    //        ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    //}

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }
                    //string[] arccMails = ccMails.Split(';');
                    //ccMails = "";
                    //foreach (string b in arccMails)
                    //{
                    //    if (!ccMails.Contains(b))
                    //    {
                    //        ccMails = ccMails + b.Trim() + ",";
                    //    }
                    //}

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    //if (ccMails != "")
                    //{
                    //    if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                    //        ccMails = ccMails.Substring(0, ccMails.Length - 1);

                    //    if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                    //        ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    //}


                    if (ToMails.Contains(','))
                    {
                        string[] arrTO = ToMails.Split(',');
                        if (ToMails.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(ToMails));
                    }

                    //+++++++++++ Multiple CC
                    //if (ccMails.Contains(','))
                    //{
                    //    string[] arrCC = ccMails.Split(',');
                    //    if (ccMails.Length > 0)
                    //    {
                    //        foreach (string val in arrCC)
                    //        {
                    //            Message.CC.Add(new MailAddress(val.Trim()));
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    Message.CC.Add(new MailAddress(ccMails));
                    //}
                    //try
                    //{
                    //    string res = emailSmartping.SendMail(0, from, to, "", "", subj, msg, null);
                    //    if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                    //    {
                    //        return Result = true;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    Message.Subject = subj;
                    Message.Body = msg;
                    if (alternateView != null)
                    {
                        Message.AlternateViews.Add(alternateView);
                    }
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);
                    Result = true;

                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);
                    //Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Bcc & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }


        public bool CandidateLoginSendMailToSpocAndClient(string display, string from, string to, string cc, string subj, string msg, Attachment attach, AlternateView alternateView)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);
            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && from.Length > 0)
                {
                    string ToMails = "";
                    ToMails = to.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }
                    if (cc.Length > 0)
                    {
                        string CCMails = "";
                        CCMails = cc.Replace(",", ";");
                        if (CCMails != "")
                        {
                            if ((CCMails.Substring(CCMails.Length - 1, 1) == ",") || (CCMails.Substring(CCMails.Length - 1, 1) == ";"))
                                CCMails = CCMails.Substring(0, CCMails.Length - 1);
                            if ((CCMails.Substring(0, 1) == ",") || (CCMails.Substring(0, 1) == ";"))
                                CCMails = CCMails.Substring(1, CCMails.Length - 1);
                        }
                        string[] arccMails = CCMails.Split(';');
                        CCMails = "";
                        foreach (string a in arToMails)
                        {
                            if (!CCMails.Contains(a))
                            {
                                CCMails = CCMails + a.Trim() + ",";
                            }
                        }
                        if (CCMails != "")
                        {
                            if ((CCMails.Substring(CCMails.Length - 1, 1) == ",") || (CCMails.Substring(CCMails.Length - 1, 1) == ";"))
                                CCMails = CCMails.Substring(0, CCMails.Length - 1);


                            if ((CCMails.Substring(0, 1) == ",") || (CCMails.Substring(0, 1) == ";"))
                                CCMails = CCMails.Substring(1, CCMails.Length - 1);
                        }
                        if (cc.Contains(','))
                        {
                            string[] arrcc = cc.Split(',');
                            if (cc.Length > 0)
                            {
                                foreach (string val in arrcc)
                                {
                                    Message.CC.Add(new MailAddress(val.Trim()));
                                }
                            }
                        }
                        else
                        {
                            Message.To.Add(new MailAddress(cc));
                        }
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, cc, "", subj, msg, attach);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    if (attach != null)
                    {
                        Message.Attachments.Add(attach);
                    }
                    if (alternateView != null)
                    {
                        Message.AlternateViews.Add(alternateView);
                    }
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool CandidateLoginSendMailToSpocAndClientExl(string display, string from, string to, string cc, string subj, string msg, Attachment attach, AlternateView alternateView , string filename = "")
        {
            msg = ApplyCtrlsMobile(to, cc, msg);
            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && from.Length > 0)
                {
                    string ToMails = "";
                    ToMails = to.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }
                    if (cc.Length > 0)
                    {
                        string CCMails = "";
                        CCMails = cc.Replace(",", ";");
                        if (CCMails != "")
                        {
                            if ((CCMails.Substring(CCMails.Length - 1, 1) == ",") || (CCMails.Substring(CCMails.Length - 1, 1) == ";"))
                                CCMails = CCMails.Substring(0, CCMails.Length - 1);
                            if ((CCMails.Substring(0, 1) == ",") || (CCMails.Substring(0, 1) == ";"))
                                CCMails = CCMails.Substring(1, CCMails.Length - 1);
                        }
                        string[] arccMails = CCMails.Split(';');
                        CCMails = "";
                        foreach (string a in arToMails)
                        {
                            if (!CCMails.Contains(a))
                            {
                                CCMails = CCMails + a.Trim() + ",";
                            }
                        }
                        if (CCMails != "")
                        {
                            if ((CCMails.Substring(CCMails.Length - 1, 1) == ",") || (CCMails.Substring(CCMails.Length - 1, 1) == ";"))
                                CCMails = CCMails.Substring(0, CCMails.Length - 1);


                            if ((CCMails.Substring(0, 1) == ",") || (CCMails.Substring(0, 1) == ";"))
                                CCMails = CCMails.Substring(1, CCMails.Length - 1);
                        }
                        if (cc.Contains(','))
                        {
                            string[] arrcc = cc.Split(',');
                            if (cc.Length > 0)
                            {
                                foreach (string val in arrcc)
                                {
                                    Message.CC.Add(new MailAddress(val.Trim()));
                                }
                            }
                        }
                        else
                        {
                            Message.To.Add(new MailAddress(cc));
                        }
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, cc, "", subj, msg, attach , filename);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    if (attach != null)
                    {
                        Message.Attachments.Add(attach);
                    }
                    if (alternateView != null)
                    {
                        Message.AlternateViews.Add(alternateView);
                    }
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }


        public bool SendMail(string display, string from, string to, string cc, string subj, string msg, Attachment attach, string fileName = "")
        {
            msg = ApplyCtrlsMobile(to, cc, msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && cc.Length > 0 && from.Length > 0 && attach != null)
                {
                    //+++++++++++ Multiple To

                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = cc.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }
                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    foreach (string b in arccMails)
                    {
                        if (!ccMails.Contains(b))
                        {
                            ccMails = ccMails + b.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }

                    //+++++++++++ Multiple CC
                    if (cc.Contains(','))
                    {
                        string[] arrCC = cc.Split(',');
                        if (cc.Length > 0)
                        {
                            foreach (string val in arrCC)
                            {
                                Message.CC.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(cc));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, cc, "", subj, msg, attach, fileName);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Attachments.Add(attach);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);
                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool CandidateLoginSendMail(string display, string from, string to, string subj, string msg, Attachment attach, AlternateView alternateView)
        {
            msg = ApplyCtrlsMobile(to, "", msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && from.Length > 0 && attach != null)
                {
                    //+++++++++++ Multiple To

                    string ToMails = "";

                    ToMails = to.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }
                    //try
                    //{
                    //    string res = emailSmartping.SendMail(0, from, to, "", "", subj, msg, attach);
                    //    if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                    //    {
                    //        return Result = true;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Attachments.Add(attach);
                    Message.AlternateViews.Add(alternateView);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);
                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool CandidateLoginSendMailIngram(string display, string from, string to, string subj, string msg, Attachment attach, AlternateView alternateView)
        {
            msg = ApplyCtrlsMobile(to, "", msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && from.Length > 0)
                {
                    //+++++++++++ Multiple To

                    string ToMails = "";

                    ToMails = to.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }
                    //try
                    //{
                    //    string res = emailSmartping.SendMail(0, from, to, "", "", subj, msg, attach);
                    //    if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                    //    {
                    //        return Result = true;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    //Message.Attachments.Add(attach);
                    Message.AlternateViews.Add(alternateView);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);
                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMailAdidas(string display, string from, string to, string subj, string msg)
        {
            msg = ApplyCtrlsMobile(to, "", msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && from.Length > 0)
                {
                    //+++++++++++ Multiple To

                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, "", "", subj, msg, null);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    //Message.Attachments.Add(attach);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);
                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMailAdidasForSpocAndClient(string display, string from, string to, string cc, string subj, string msg)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && from.Length > 0)
                {
                    string ToMails = "";
                    ToMails = to.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }
                    if (cc.Length > 0)
                    {
                        string CCMails = "";
                        CCMails = cc.Replace(",", ";");
                        if (CCMails != "")
                        {
                            if ((CCMails.Substring(CCMails.Length - 1, 1) == ",") || (CCMails.Substring(CCMails.Length - 1, 1) == ";"))
                                CCMails = CCMails.Substring(0, CCMails.Length - 1);
                            if ((CCMails.Substring(0, 1) == ",") || (CCMails.Substring(0, 1) == ";"))
                                CCMails = CCMails.Substring(1, CCMails.Length - 1);
                        }
                        string[] arccMails = CCMails.Split(';');
                        CCMails = "";
                        foreach (string a in arToMails)
                        {
                            if (!CCMails.Contains(a))
                            {
                                CCMails = CCMails + a.Trim() + ",";
                            }
                        }
                        if (CCMails != "")
                        {
                            if ((CCMails.Substring(CCMails.Length - 1, 1) == ",") || (CCMails.Substring(CCMails.Length - 1, 1) == ";"))
                                CCMails = CCMails.Substring(0, CCMails.Length - 1);


                            if ((CCMails.Substring(0, 1) == ",") || (CCMails.Substring(0, 1) == ";"))
                                CCMails = CCMails.Substring(1, CCMails.Length - 1);
                        }
                        if (cc.Contains(','))
                        {
                            string[] arrcc = cc.Split(',');
                            if (cc.Length > 0)
                            {
                                foreach (string val in arrcc)
                                {
                                    Message.CC.Add(new MailAddress(val.Trim()));
                                }
                            }
                        }
                        else
                        {
                            Message.To.Add(new MailAddress(cc));
                        }
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, cc, "", subj, msg, null);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMailInternational(string display, string from, string to, string cc, string subj, string msg, Attachment attach, Attachment attach2)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && cc.Length > 0 && from.Length > 0 && attach != null)
                {
                    //+++++++++++ Multiple To

                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = cc.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }
                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    foreach (string b in arccMails)
                    {
                        if (!ccMails.Contains(b))
                        {
                            ccMails = ccMails + b.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }

                    //+++++++++++ Multiple CC
                    if (cc.Contains(','))
                    {
                        string[] arrCC = cc.Split(',');
                        if (cc.Length > 0)
                        {
                            foreach (string val in arrCC)
                            {
                                Message.CC.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(cc));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail_Att(0, from, to, cc, "", subj, msg, attach, attach2);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Attachments.Add(attach);
                    Message.Attachments.Add(attach2);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);
                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }


        public bool SendMail(string display, string from, string to, string cc, string bcc, string subj, string msg)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && bcc.Length > 0 && from.Length > 0)
                {

                    string ToMails = "";
                    string ccMails = "";
                    string bccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = cc.Replace(",", ";");
                    bccMails = bcc.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    if (bccMails != "")
                    {
                        if ((bccMails.Substring(bccMails.Length - 1, 1) == ",") || (bccMails.Substring(bccMails.Length - 1, 1) == ";"))
                            bccMails = bccMails.Substring(0, bccMails.Length - 1);

                        if ((bccMails.Substring(0, 1) == ",") || (bccMails.Substring(0, 1) == ";"))
                            bccMails = bccMails.Substring(1, bccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    if (arToMails[0] != "")
                    {
                        foreach (string a in arToMails)
                        {
                            if (!ToMails.Contains(a))
                            {
                                ToMails = ToMails + a.Trim() + ",";
                            }
                        }
                    }
                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    if (arccMails[0] != "")
                    {
                        foreach (string b in arccMails)
                        {
                            if (!ccMails.Contains(b))
                            {
                                ccMails = ccMails + b.Trim() + ",";
                            }
                        }
                    }


                    string[] arbccMails = bccMails.Split(';');
                    bccMails = "";
                    if (arbccMails[0] != "")
                    {
                        foreach (string b in arbccMails)
                        {
                            if (!bccMails.Contains(b))
                            {
                                bccMails = bccMails + b.Trim() + ",";
                            }
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    if (bccMails != "")
                    {
                        if ((bccMails.Substring(bccMails.Length - 1, 1) == ",") || (bccMails.Substring(bccMails.Length - 1, 1) == ";"))
                            bccMails = bccMails.Substring(0, bccMails.Length - 1);

                        if ((bccMails.Substring(0, 1) == ",") || (bccMails.Substring(0, 1) == ";"))
                            bccMails = bccMails.Substring(1, bccMails.Length - 1);
                    }

                    if (ToMails != "")
                    {
                        if (ToMails.Contains(','))
                        {
                            string[] arrTO = ToMails.Split(',');
                            if (ToMails.Length > 0)
                            {
                                foreach (string val in arrTO)
                                {
                                    Message.To.Add(new MailAddress(val.Trim()));
                                }
                            }
                        }
                        else
                        {
                            Message.To.Add(new MailAddress(ToMails));
                        }
                    }


                    //+++++++++++ Multiple CC
                    if (ccMails != "")
                    {
                        if (ccMails.Contains(','))
                        {
                            string[] arrCC = ccMails.Split(',');
                            if (ccMails.Length > 0)
                            {
                                foreach (string val in arrCC)
                                {
                                    Message.CC.Add(new MailAddress(val.Trim()));
                                }
                            }
                        }
                        else
                        {
                            Message.CC.Add(new MailAddress(ccMails));
                        }
                    }

                    //+++++++++++ Multiple BCC
                    if (bccMails != "")
                    {
                        if (bccMails.Contains(','))
                        {
                            string[] arrBCC = bccMails.Split(',');
                            if (bccMails.Length > 0)
                            {
                                foreach (string val in arrBCC)
                                {
                                    Message.Bcc.Add(new MailAddress(val));
                                }
                            }
                        }
                        else
                        {
                            Message.Bcc.Add(new MailAddress(bccMails));
                        }
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, cc, bcc, subj, msg, null);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, Bcc & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMail1(string display, string from, string to, string cc, string subj, string msg)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && cc.Length > 0 && from.Length > 0)
                {
                    //+++++++++++ Multiple To
                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = cc.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }
                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    foreach (string b in arccMails)
                    {
                        if (!ccMails.Contains(b))
                        {
                            ccMails = ccMails + b.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }


                    if (ToMails.Contains(','))
                    {
                        string[] arrTO = ToMails.Split(',');
                        if (ToMails.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(ToMails));
                    }

                    //+++++++++++ Multiple CC
                    if (ccMails.Contains(','))
                    {
                        string[] arrCC = ccMails.Split(',');
                        if (ccMails.Length > 0)
                        {
                            foreach (string val in arrCC)
                            {
                                Message.CC.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(ccMails));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, cc, "", subj, msg, null);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);
                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Bcc & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }
        public bool SendMail(string display, string from, string to, string cc, string bcc, string subj, string msg, Attachment attach)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && cc.Length > 0 && bcc.Length > 0 && from.Length > 0 && attach != null)
                {
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }

                    //+++++++++++ Multiple CC                   
                    if (cc.Contains(','))
                    {
                        string[] arrCC = cc.Split(',');
                        if (cc.Length > 0)
                        {
                            foreach (string val in arrCC)
                            {
                                Message.CC.Add(new MailAddress(val));
                            }
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(cc));
                    }

                    //+++++++++++ Multiple BCC                    
                    if (bcc.Contains(','))
                    {
                        string[] arrBCC = bcc.Split(',');
                        if (bcc.Length > 0)
                        {
                            foreach (string val in arrBCC)
                            {
                                Message.Bcc.Add(new MailAddress(val));
                            }
                        }
                    }
                    else
                    {
                        Message.Bcc.Add(new MailAddress(bcc));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, cc, bcc, subj, msg, attach);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Attachments.Add(attach);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, Bcc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }
        public CompanyMailSignatureViewModel GetCompanyMailSignature()
        {
            try
            {
                return repoCompany.GetDefaultSignatureDetails(1);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public bool SendMailClientOTP(string display, string from, string to, string subj, string msg)
        {
            msg = ApplyCtrlsMobile(to, "", msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && from.Length > 0)
                {
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, "", "", subj, msg, null);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;

                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }
        public bool SendMailStopVendor(string display, string from, string to, string subj, string msg, string cc)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && from.Length > 0)
                {
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }

                    if (cc.Contains(','))
                    {
                        string[] arrCC = cc.Split(',');
                        if (cc.Length > 0)
                        {
                            foreach (string val in arrCC)
                            {
                                Message.CC.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(cc));
                    }

                    try
                    {
                        string res = emailSmartping.SendMail(0, from, to, cc, "", subj, msg, null);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    //Message.Attachments.Add(attach);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }
        public bool SendDigilockerMail(string display, string from, string to, string cc, string subj, string msg, AlternateView alternateView)
        {
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            bool Result = false;
            try
            {

                string ToMails = "";
                string ccMails = "";

                ToMails = to.Replace(",", ";");
                ccMails = cc.Replace(",", ";");
                if (ToMails != "")
                {
                    if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                        ToMails = ToMails.Substring(0, ToMails.Length - 1);


                    if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                        ToMails = ToMails.Substring(1, ToMails.Length - 1);
                }

                if (ccMails != "")
                {
                    if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                        ccMails = ccMails.Substring(0, ccMails.Length - 1);

                    if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                        ccMails = ccMails.Substring(1, ccMails.Length - 1);
                }

                string[] arToMails = ToMails.Split(';');
                ToMails = "";
                foreach (string a in arToMails)
                {
                    if (!ToMails.Contains(a))
                    {
                        ToMails = ToMails + a.Trim() + ",";
                    }
                }
                string[] arccMails = ccMails.Split(';');
                ccMails = "";
                foreach (string b in arccMails)
                {
                    if (!ccMails.Contains(b))
                    {
                        ccMails = ccMails + b.Trim() + ",";
                    }
                }

                if (ToMails != "")
                {
                    if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                        ToMails = ToMails.Substring(0, ToMails.Length - 1);


                    if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                        ToMails = ToMails.Substring(1, ToMails.Length - 1);
                }

                if (ccMails != "")
                {
                    if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                        ccMails = ccMails.Substring(0, ccMails.Length - 1);

                    if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                        ccMails = ccMails.Substring(1, ccMails.Length - 1);
                }


                if (ToMails.Contains(','))
                {
                    string[] arrTO = ToMails.Split(',');
                    if (ToMails.Length > 0)
                    {
                        foreach (string val in arrTO)
                        {
                            Message.To.Add(new MailAddress(val.Trim()));
                        }
                    }
                }
                else
                {
                    Message.To.Add(new MailAddress(ToMails));
                }

                //+++++++++++ Multiple CC
                if (ccMails.Contains(','))
                {
                    string[] arrCC = ccMails.Split(',');
                    if (ccMails.Length > 0)
                    {
                        foreach (string val in arrCC)
                        {
                            Message.CC.Add(new MailAddress(val.Trim()));
                        }
                    }
                }
                else
                {
                    Message.CC.Add(new MailAddress(ccMails));
                }
                //try
                //{
                //    string res = emailSmartping.SendMail(0, from, to,cc, "", subj, msg, null);
                //    if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                //    {
                //        return Result = true;
                //    }
                //}
                //catch (Exception ex)
                //{

                //}
                Message.Subject = subj;
                Message.Body = msg;
                Message.From = new MailAddress("bgv.request@securitas-india.com", display);
                Message.IsBodyHtml = true;
                Message.AlternateViews.Add(alternateView);
                Message.Headers.Add("X-MC-PreserveRecipients", "true");
                Message.Priority = MailPriority.High;
                NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                SMTP.Host = Host;
                SMTP.Port = Convert.ToInt32(Port);
                SMTP.Credentials = myCredential;
                SMTP.EnableSsl = EnableSsl;
                SMTP.Send(Message);

                Result = true;

            }
            catch (SmtpException SmtpEx)
            {
                //MessageBox.Show(SmtpEx.Message);
                // MessageBox.Show(SmtpEx.InnerException.ToString());
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMailToCandidateForCtrls(string display, string from, string to, string subj, string msg, Attachment attach, Attachment attach1)
        {
            msg = ApplyCtrlsMobile(to, "", msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && from.Length > 0 && attach != null)
                {
                    //+++++++++++ Multiple To
                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail_Att(0, from, to, "", "", subj, msg, attach, attach1);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    if (attach != null)
                    {
                        Message.Attachments.Add(attach);
                    }
                    if (attach1 != null)
                    {
                        Message.Attachments.Add(attach1);
                    }
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;
                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMailToCandidateForCtrlsForSpocAndClient(string display, string from, string to, string cc, string subj, string msg, Attachment attach, Attachment attach1)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && cc.Length > 0 && from.Length > 0 && attach != null)
                {
                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = cc.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }
                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    foreach (string b in arccMails)
                    {
                        if (!ccMails.Contains(b))
                        {
                            ccMails = ccMails + b.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }


                    if (ToMails.Contains(','))
                    {
                        string[] arrTO = ToMails.Split(',');
                        if (ToMails.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(ToMails));
                    }

                    //+++++++++++ Multiple CC
                    if (ccMails.Contains(','))
                    {
                        string[] arrCC = ccMails.Split(',');
                        if (ccMails.Length > 0)
                        {
                            foreach (string val in arrCC)
                            {
                                Message.CC.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(ccMails));
                    }
                    try
                    {
                        string res = emailSmartping.SendMail_Att(0, from, to, cc, "", subj, msg, attach, attach1);
                        if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                        {
                            return Result = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    if (attach != null)
                    {
                        Message.Attachments.Add(attach);
                    }
                    if (attach1 != null)
                    {
                        Message.Attachments.Add(attach1);
                    }
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool ClientOnboardingMail(string display, string from, string to, string cc, string subj, string msg, AlternateView alternateView)
        {
            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && cc.Length > 0 && from.Length > 0)
                {
                    //+++++++++++ Multiple To

                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = cc.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }
                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    foreach (string b in arccMails)
                    {
                        if (!ccMails.Contains(b))
                        {
                            ccMails = ccMails + b.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }
                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }

                    //+++++++++++ Multiple CC
                    if (cc.Contains(','))
                    {
                        string[] arrCC = cc.Split(',');
                        if (cc.Length > 0)
                        {
                            foreach (string val in arrCC)
                            {
                                Message.CC.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(cc));
                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.AlternateViews.Add(alternateView);
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);
                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, From & Attachment are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }
        public bool AnnexureInvoicingSendMail(string display, string from, string to, string cc, string bcc, string subj, string msg, List<Attachment> attachList, AlternateView alternateView)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && bcc.Length > 0 && from.Length > 0)
                {

                    string ToMails = "";
                    string ccMails = "";
                    string bccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = cc.Replace(",", ";");
                    bccMails = bcc.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    if (bccMails != "")
                    {
                        if ((bccMails.Substring(bccMails.Length - 1, 1) == ",") || (bccMails.Substring(bccMails.Length - 1, 1) == ";"))
                            bccMails = bccMails.Substring(0, bccMails.Length - 1);

                        if ((bccMails.Substring(0, 1) == ",") || (bccMails.Substring(0, 1) == ";"))
                            bccMails = bccMails.Substring(1, bccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    if (arToMails[0] != "")
                    {
                        foreach (string a in arToMails)
                        {
                            if (!ToMails.Contains(a))
                            {
                                ToMails = ToMails + a.Trim() + ",";
                            }
                        }
                    }
                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    if (arccMails[0] != "")
                    {
                        foreach (string b in arccMails)
                        {
                            if (!ccMails.Contains(b))
                            {
                                ccMails = ccMails + b.Trim() + ",";
                            }
                        }
                    }


                    string[] arbccMails = bccMails.Split(';');
                    bccMails = "";
                    if (arbccMails[0] != "")
                    {
                        foreach (string b in arbccMails)
                        {
                            if (!bccMails.Contains(b))
                            {
                                bccMails = bccMails + b.Trim() + ",";
                            }
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    if (bccMails != "")
                    {
                        if ((bccMails.Substring(bccMails.Length - 1, 1) == ",") || (bccMails.Substring(bccMails.Length - 1, 1) == ";"))
                            bccMails = bccMails.Substring(0, bccMails.Length - 1);

                        if ((bccMails.Substring(0, 1) == ",") || (bccMails.Substring(0, 1) == ";"))
                            bccMails = bccMails.Substring(1, bccMails.Length - 1);
                    }

                    if (ToMails != "")
                    {
                        if (ToMails.Contains(','))
                        {
                            string[] arrTO = ToMails.Split(',');
                            if (ToMails.Length > 0)
                            {
                                foreach (string val in arrTO)
                                {
                                    Message.To.Add(new MailAddress(val.Trim()));
                                }
                            }
                        }
                        else
                        {
                            Message.To.Add(new MailAddress(ToMails));
                        }
                    }


                    //+++++++++++ Multiple CC
                    if (ccMails != "")
                    {
                        if (ccMails.Contains(','))
                        {
                            string[] arrCC = ccMails.Split(',');
                            if (ccMails.Length > 0)
                            {
                                foreach (string val in arrCC)
                                {
                                    Message.CC.Add(new MailAddress(val.Trim()));
                                }
                            }
                        }
                        else
                        {
                            Message.CC.Add(new MailAddress(ccMails));
                        }
                    }

                    //+++++++++++ Multiple BCC
                    if (bccMails != "")
                    {
                        if (bccMails.Contains(','))
                        {
                            string[] arrBCC = bccMails.Split(',');
                            if (bccMails.Length > 0)
                            {
                                foreach (string val in arrBCC)
                                {
                                    Message.Bcc.Add(new MailAddress(val));
                                }
                            }
                        }
                        else
                        {
                            Message.Bcc.Add(new MailAddress(bccMails));
                        }
                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    foreach (var newList in attachList)
                    {
                        Message.Attachments.Add(newList);
                    }
                    if (alternateView != null)
                    {
                        Message.AlternateViews.Add(alternateView);
                    }
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, Bcc & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        #region ********** Send Mail With Attachment **********

        public bool SendMailAttach(string display, string from, string to, string cc, string subj, string msg, Attachment attach)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                if (to.Length > 0 && cc.Length > 0 && from.Length > 0)
                {
                    //+++++++++++ Multiple To
                    string ToMails = "";
                    string ccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = cc.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }
                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    foreach (string b in arccMails)
                    {
                        if (!ccMails.Contains(b))
                        {
                            ccMails = ccMails + b.Trim() + ",";
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }


                    if (ToMails.Contains(','))
                    {
                        string[] arrTO = ToMails.Split(',');
                        if (ToMails.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(ToMails));
                    }

                    //+++++++++++ Multiple CC
                    if (ccMails.Contains(','))
                    {
                        string[] arrCC = ccMails.Split(',');
                        if (ccMails.Length > 0)
                        {
                            foreach (string val in arrCC)
                            {
                                Message.CC.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(ccMails));
                    }

                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    if (attach != null)
                    {
                        Message.Attachments.Add(attach);
                    }
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    //NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    //SMTP.Host = Host;
                    //SMTP.Port = Convert.ToInt32(Port);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = EnableSsl;
                    //SMTP.Send(Message);

                    NetworkCredential myCredential = new NetworkCredential("bgv.request@securitas-india.com", "wpCz1TNUZP66CBfmBJJq5g");
                    SMTP.Host = "smtp.mandrillapp.com";
                    SMTP.Port = Convert.ToInt32(587);
                    SMTP.Credentials = myCredential;

                    SMTP.EnableSsl = true;
                    SMTP.Send(Message);


                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Bcc & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMailAttach(string display, string from, string to, string cc, string bcc, string subj, string msg, Attachment attach)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && bcc.Length > 0 && from.Length > 0)
                {

                    string ToMails = "";
                    string ccMails = "";
                    string bccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = cc.Replace(",", ";");
                    bccMails = bcc.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    if (bccMails != "")
                    {
                        if ((bccMails.Substring(bccMails.Length - 1, 1) == ",") || (bccMails.Substring(bccMails.Length - 1, 1) == ";"))
                            bccMails = bccMails.Substring(0, bccMails.Length - 1);

                        if ((bccMails.Substring(0, 1) == ",") || (bccMails.Substring(0, 1) == ";"))
                            bccMails = bccMails.Substring(1, bccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    if (arToMails[0] != "")
                    {
                        foreach (string a in arToMails)
                        {
                            if (!ToMails.Contains(a))
                            {
                                ToMails = ToMails + a.Trim() + ",";
                            }
                        }
                    }
                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    if (arccMails[0] != "")
                    {
                        foreach (string b in arccMails)
                        {
                            if (!ccMails.Contains(b))
                            {
                                ccMails = ccMails + b.Trim() + ",";
                            }
                        }
                    }


                    string[] arbccMails = bccMails.Split(';');
                    bccMails = "";
                    if (arbccMails[0] != "")
                    {
                        foreach (string b in arbccMails)
                        {
                            if (!bccMails.Contains(b))
                            {
                                bccMails = bccMails + b.Trim() + ",";
                            }
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    if (bccMails != "")
                    {
                        if ((bccMails.Substring(bccMails.Length - 1, 1) == ",") || (bccMails.Substring(bccMails.Length - 1, 1) == ";"))
                            bccMails = bccMails.Substring(0, bccMails.Length - 1);

                        if ((bccMails.Substring(0, 1) == ",") || (bccMails.Substring(0, 1) == ";"))
                            bccMails = bccMails.Substring(1, bccMails.Length - 1);
                    }

                    if (ToMails != "")
                    {
                        if (ToMails.Contains(','))
                        {
                            string[] arrTO = ToMails.Split(',');
                            if (ToMails.Length > 0)
                            {
                                foreach (string val in arrTO)
                                {
                                    Message.To.Add(new MailAddress(val.Trim()));
                                }
                            }
                        }
                        else
                        {
                            Message.To.Add(new MailAddress(ToMails));
                        }
                    }


                    //+++++++++++ Multiple CC
                    if (ccMails != "")
                    {
                        if (ccMails.Contains(','))
                        {
                            string[] arrCC = ccMails.Split(',');
                            if (ccMails.Length > 0)
                            {
                                foreach (string val in arrCC)
                                {
                                    Message.CC.Add(new MailAddress(val.Trim()));
                                }
                            }
                        }
                        else
                        {
                            Message.CC.Add(new MailAddress(ccMails));
                        }
                    }

                    //+++++++++++ Multiple BCC
                    if (bccMails != "")
                    {
                        if (bccMails.Contains(','))
                        {
                            string[] arrBCC = bccMails.Split(',');
                            if (bccMails.Length > 0)
                            {
                                foreach (string val in arrBCC)
                                {
                                    Message.Bcc.Add(new MailAddress(val));
                                }
                            }
                        }
                        else
                        {
                            Message.Bcc.Add(new MailAddress(bccMails));
                        }
                    }
                    //try
                    //{
                    //    string res = emailSmartping.SendMail(0, from, to, cc, "", subj, msg, null);
                    //    if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
                    //    {
                    //        return Result = true;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    if (attach != null)
                    {
                        Message.Attachments.Add(attach);
                    }
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, Bcc & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool SendMailAttach(string display, string from, string to, string subj, string msg, Attachment attach)
        {
            msg = ApplyCtrlsMobile(to, "", msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && from.Length > 0)
                {

                    string ToMails = "";


                    ToMails = to.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }



                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    foreach (string a in arToMails)
                    {
                        if (!ToMails.Contains(a))
                        {
                            ToMails = ToMails + a.Trim() + ",";
                        }
                    }


                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (to.Contains(','))
                    {
                        string[] arrTO = to.Split(',');
                        if (to.Length > 0)
                        {
                            foreach (string val in arrTO)
                            {
                                Message.To.Add(new MailAddress(val.Trim()));
                            }
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(to));
                    }

                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress("bgv.request@securitas-india.com", display);
                    if (attach != null)
                    {
                        Message.Attachments.Add(attach);
                    }
                    Message.IsBodyHtml = true;
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    Message.Priority = MailPriority.High;
                    NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);
                    Result = true;

                    //Message.Subject = subj;
                    //Message.Body = msg;
                    //Message.From = new MailAddress("bgv.request@securitas-india.com", display);
                    //Message.IsBodyHtml = true;
                    //Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    //Message.Priority = MailPriority.High;
                    //NetworkCredential myCredential = new NetworkCredential("bgv.requests@securitas-india.com", "%request7832");
                    //SMTP.Host = "smtp.rediffmailpro.com";
                    //SMTP.Port = Convert.ToInt32(587);
                    //SMTP.Credentials = myCredential;
                    //SMTP.EnableSsl = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //SMTP.Send(Message);
                    //Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        public bool InsuffSupportSendMail(string display, string from, string to, string cc, string bcc, string subj, string msg, List<Attachment> attachList, AlternateView alternateView)
        {
            msg = ApplyCtrlsMobile(to, cc, msg);

            bool Result = false;
            SmtpClient SMTP = new SmtpClient();
            MailMessage Message = new MailMessage();
            try
            {
                //+++++++++++ Multiple To
                if (to.Length > 0 && bcc.Length > 0 && from.Length > 0)
                {

                    string ToMails = "";
                    string ccMails = "";
                    string bccMails = "";

                    ToMails = to.Replace(",", ";");
                    ccMails = cc.Replace(",", ";");
                    bccMails = bcc.Replace(",", ";");
                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    if (bccMails != "")
                    {
                        if ((bccMails.Substring(bccMails.Length - 1, 1) == ",") || (bccMails.Substring(bccMails.Length - 1, 1) == ";"))
                            bccMails = bccMails.Substring(0, bccMails.Length - 1);

                        if ((bccMails.Substring(0, 1) == ",") || (bccMails.Substring(0, 1) == ";"))
                            bccMails = bccMails.Substring(1, bccMails.Length - 1);
                    }

                    string[] arToMails = ToMails.Split(';');
                    ToMails = "";
                    if (arToMails[0] != "")
                    {
                        foreach (string a in arToMails)
                        {
                            if (!ToMails.Contains(a))
                            {
                                ToMails = ToMails + a.Trim() + ",";
                            }
                        }
                    }
                    string[] arccMails = ccMails.Split(';');
                    ccMails = "";
                    if (arccMails[0] != "")
                    {
                        foreach (string b in arccMails)
                        {
                            if (!ccMails.Contains(b))
                            {
                                ccMails = ccMails + b.Trim() + ",";
                            }
                        }
                    }


                    string[] arbccMails = bccMails.Split(';');
                    bccMails = "";
                    if (arbccMails[0] != "")
                    {
                        foreach (string b in arbccMails)
                        {
                            if (!bccMails.Contains(b))
                            {
                                bccMails = bccMails + b.Trim() + ",";
                            }
                        }
                    }

                    if (ToMails != "")
                    {
                        if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
                            ToMails = ToMails.Substring(0, ToMails.Length - 1);


                        if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
                            ToMails = ToMails.Substring(1, ToMails.Length - 1);
                    }

                    if (ccMails != "")
                    {
                        if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
                            ccMails = ccMails.Substring(0, ccMails.Length - 1);

                        if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
                            ccMails = ccMails.Substring(1, ccMails.Length - 1);
                    }

                    if (bccMails != "")
                    {
                        if ((bccMails.Substring(bccMails.Length - 1, 1) == ",") || (bccMails.Substring(bccMails.Length - 1, 1) == ";"))
                            bccMails = bccMails.Substring(0, bccMails.Length - 1);

                        if ((bccMails.Substring(0, 1) == ",") || (bccMails.Substring(0, 1) == ";"))
                            bccMails = bccMails.Substring(1, bccMails.Length - 1);
                    }

                    if (ToMails != "")
                    {
                        if (ToMails.Contains(','))
                        {
                            string[] arrTO = ToMails.Split(',');
                            if (ToMails.Length > 0)
                            {
                                foreach (string val in arrTO)
                                {
                                    Message.To.Add(new MailAddress(val.Trim()));
                                }
                            }
                        }
                        else
                        {
                            Message.To.Add(new MailAddress(ToMails));
                        }
                    }


                    //+++++++++++ Multiple CC
                    if (ccMails != "")
                    {
                        if (ccMails.Contains(','))
                        {
                            string[] arrCC = ccMails.Split(',');
                            if (ccMails.Length > 0)
                            {
                                foreach (string val in arrCC)
                                {
                                    Message.CC.Add(new MailAddress(val.Trim()));
                                }
                            }
                        }
                        else
                        {
                            Message.CC.Add(new MailAddress(ccMails));
                        }
                    }

                    //+++++++++++ Multiple BCC
                    if (bccMails != "")
                    {
                        if (bccMails.Contains(','))
                        {
                            string[] arrBCC = bccMails.Split(',');
                            if (bccMails.Length > 0)
                            {
                                foreach (string val in arrBCC)
                                {
                                    Message.Bcc.Add(new MailAddress(val));
                                }
                            }
                        }
                        else
                        {
                            Message.Bcc.Add(new MailAddress(bccMails));
                        }
                    }
                    Message.Subject = subj;
                    Message.Body = msg;
                    Message.From = new MailAddress(from, display);
                    Message.Headers.Add("X-MC-PreserveRecipients", "true");
                    foreach (var newList in attachList)
                    {
                        Message.Attachments.Add(newList);
                    }
                    if (alternateView != null)
                    {
                        Message.AlternateViews.Add(alternateView);
                    }
                    Message.IsBodyHtml = true;
                    Message.Priority = MailPriority.High;

                    /*NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
                    SMTP.Host = Host;
                    SMTP.Port = Convert.ToInt32(Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = EnableSsl;
                    SMTP.Send(Message);*/

                    CompanyMailAuthenticationViewModel Model_InsuffSupport = repoCompany.GetMailAuthenticationById(5);
                    // Table Name - MasterCompanies and CompanyRowID 5 for Means Insuff support entry in table MasterCompanies
                    string SMTP_Host = string.Empty;
                    string SMTP_Port = string.Empty;
                    string SMTP_UserID = string.Empty;
                    string SMTP_Pass = string.Empty;
                    bool SMTP_EnableSsl = false;
                    if (Model_InsuffSupport != null)
                    {
                        SMTP_Host = Model_InsuffSupport.SMTPServer;
                        SMTP_Port = Model_InsuffSupport.Port;
                        SMTP_UserID = Model_InsuffSupport.SMTPUserName;
                        SMTP_Pass = clsCommonMethods.PasswordDecrypt(Model_InsuffSupport.SMTPPassword);
                        SMTP_EnableSsl = Model_InsuffSupport.EnableSsl;
                    }
                    NetworkCredential myCredential = new NetworkCredential(SMTP_UserID, SMTP_Pass);
                    SMTP.Host = SMTP_Host;
                    SMTP.Port = Convert.ToInt32(SMTP_Port);
                    SMTP.Credentials = myCredential;
                    SMTP.EnableSsl = SMTP_EnableSsl;
                    SMTP.Send(Message);

                    Result = true;
                }
                else
                {
                    throw new Exception("Custom Error: Email To, Cc, Bcc & From are invalid!");
                }
            }
            catch (SmtpException SmtpEx)
            {
                Result = false;
                throw SmtpEx;
            }

            return Result;
        }

        #region ********** Send Mail With Attachment **********

        //public bool SendMailAttach(string display, string from, string to, string cc, string subj, string msg, Attachment attach)
        //{
        //    bool Result = false;
        //    SmtpClient SMTP = new SmtpClient();
        //    MailMessage Message = new MailMessage();
        //    try
        //    {
        //        if (to.Length > 0 && cc.Length > 0 && from.Length > 0)
        //        {
        //            //+++++++++++ Multiple To
        //            string ToMails = "";
        //            string ccMails = "";

        //            ToMails = to.Replace(",", ";");
        //            ccMails = cc.Replace(",", ";");
        //            if (ToMails != "")
        //            {
        //                if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
        //                    ToMails = ToMails.Substring(0, ToMails.Length - 1);


        //                if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
        //                    ToMails = ToMails.Substring(1, ToMails.Length - 1);
        //            }

        //            if (ccMails != "")
        //            {
        //                if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
        //                    ccMails = ccMails.Substring(0, ccMails.Length - 1);

        //                if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
        //                    ccMails = ccMails.Substring(1, ccMails.Length - 1);
        //            }

        //            string[] arToMails = ToMails.Split(';');
        //            ToMails = "";
        //            foreach (string a in arToMails)
        //            {
        //                if (!ToMails.Contains(a))
        //                {
        //                    ToMails = ToMails + a.Trim() + ",";
        //                }
        //            }
        //            string[] arccMails = ccMails.Split(';');
        //            ccMails = "";
        //            foreach (string b in arccMails)
        //            {
        //                if (!ccMails.Contains(b))
        //                {
        //                    ccMails = ccMails + b.Trim() + ",";
        //                }
        //            }

        //            if (ToMails != "")
        //            {
        //                if ((ToMails.Substring(ToMails.Length - 1, 1) == ",") || (ToMails.Substring(ToMails.Length - 1, 1) == ";"))
        //                    ToMails = ToMails.Substring(0, ToMails.Length - 1);


        //                if ((ToMails.Substring(0, 1) == ",") || (ToMails.Substring(0, 1) == ";"))
        //                    ToMails = ToMails.Substring(1, ToMails.Length - 1);
        //            }

        //            if (ccMails != "")
        //            {
        //                if ((ccMails.Substring(ccMails.Length - 1, 1) == ",") || (ccMails.Substring(ccMails.Length - 1, 1) == ";"))
        //                    ccMails = ccMails.Substring(0, ccMails.Length - 1);

        //                if ((ccMails.Substring(0, 1) == ",") || (ccMails.Substring(0, 1) == ";"))
        //                    ccMails = ccMails.Substring(1, ccMails.Length - 1);
        //            }


        //            if (ToMails.Contains(','))
        //            {
        //                string[] arrTO = ToMails.Split(',');
        //                if (ToMails.Length > 0)
        //                {
        //                    foreach (string val in arrTO)
        //                    {
        //                        Message.To.Add(new MailAddress(val.Trim()));
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Message.To.Add(new MailAddress(ToMails));
        //            }

        //            //+++++++++++ Multiple CC
        //            if (ccMails.Contains(','))
        //            {
        //                string[] arrCC = ccMails.Split(',');
        //                if (ccMails.Length > 0)
        //                {
        //                    foreach (string val in arrCC)
        //                    {
        //                        Message.CC.Add(new MailAddress(val.Trim()));
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Message.CC.Add(new MailAddress(ccMails));
        //            }
        //            try
        //            {
        //                string res = emailSmartping.SendMail(0, from, to, cc, "", subj, msg, attach);
        //                if (res.ToLower().Contains("bulkid") || res.ToLower().Contains("successfully:"))
        //                {
        //                    return Result = true;
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //            Message.Subject = subj;
        //            Message.Body = msg;
        //            Message.From = new MailAddress(from, display);
        //            if (attach != null)
        //            {
        //                Message.Attachments.Add(attach);
        //            }
        //            Message.Headers.Add("X-MC-PreserveRecipients", "true");
        //            Message.IsBodyHtml = true;
        //            Message.Priority = MailPriority.High;

        //            //NetworkCredential myCredential = new NetworkCredential(UserID, Pass);
        //            //SMTP.Host = Host;
        //            //SMTP.Port = Convert.ToInt32(Port);
        //            //SMTP.Credentials = myCredential;
        //            //SMTP.EnableSsl = EnableSsl;
        //            //SMTP.Send(Message);

        //            NetworkCredential myCredential = new NetworkCredential("bgv.request@securitas-india.com", "wpCz1TNUZP66CBfmBJJq5g");
        //            SMTP.Host = "smtp.mandrillapp.com";
        //            SMTP.Port = Convert.ToInt32(587);
        //            SMTP.Credentials = myCredential;

        //            SMTP.EnableSsl = true;
        //            SMTP.Send(Message);


        //            Result = true;
        //        }
        //        else
        //        {
        //            throw new Exception("Custom Error: Email To, Bcc & From are invalid!");
        //        }
        //    }
        //    catch (SmtpException SmtpEx)
        //    {
        //        Result = false;
        //        throw SmtpEx;
        //    }

        //    return Result;
        //}
        public CompanyMailSignatureViewModel GetCompanyMailSignatureInsuffSupport()
        {
            try
            {
                return repoCompany.GetDefaultSignatureDetails(5); // 5 for Inuff Support email id
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #endregion
    }
}