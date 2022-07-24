using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WebApplication1.Service
{
    public class MailService
    {
        private string gmail_add = "ula14108@gmail.com";
        private string gmail_psw = "zmwjzyhqnbhzcpjq";
        private string gmail_mail = "ula14108@gmail.com";

        #region 產生驗證碼
        public string GetValidateCode()
        {
            string[] Code = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K"
                            , "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y"
                            , "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b"
                            , "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n"
                            , "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"  };
            string ValidateCode = string.Empty;

            Random rd = new Random();
            for(int i = 0; i <10; i++)
            {
                ValidateCode += Code[rd.Next(Code.Count())];
            }
            return ValidateCode;
        }
        #endregion
        #region 資料填入驗證信範本
        public string GetRegisterMailBody(string TempMail, string UserName, string ValidateUrl)
        {
            TempMail = TempMail.Replace("{{UserName}}", UserName);
            TempMail = TempMail.Replace("{{ValidateUrl}}", ValidateUrl);
            return TempMail;
        }
        #endregion
        #region 寄送驗證信
        public void SendRegisterMailBody(string MailBody, string ToEmail)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(gmail_add, gmail_psw);
            SmtpServer.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(gmail_mail);
            mail.Subject = "會員註冊驗證信";
            mail.Body = MailBody;
            mail.IsBodyHtml = true;
            mail.To.Add(ToEmail);
            SmtpServer.Send(mail);
        }
        #endregion
    }
}