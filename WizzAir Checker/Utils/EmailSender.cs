using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;

using Logger_Viewer;

namespace WizzAir_Checker.Utils
{
    public class EmailSender
    {
        private string _mailUser;
        private string _mailUserPassword;
        private string _mailUsers;

        internal bool ProvideMailCredentials()
        {
            _mailUser = ConfigurationManager.AppSettings["MailUser"];
            if (string.IsNullOrEmpty(_mailUser))
            {
                MessageBox.Show("The MailUser field in empty! Please check the configuration file.", "WizzAir Checker - Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            _mailUserPassword = ConfigurationManager.AppSettings["MailPassword"];
            if (string.IsNullOrEmpty(_mailUserPassword))
            {
                MessageBox.Show("The MailPassword field in empty! Please check the configuration file.", "WizzAir Checker - Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            _mailUsers = ConfigurationManager.AppSettings["MailUsers"];
            if (string.IsNullOrEmpty(_mailUsers))
            {
                MessageBox.Show("The MailUsers field in empty! Please check the configuration file.", "WizzAir Checker - Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            return true;
        }

        public void SendMail(int? lastPrice, int? currentPrice)
        {
            try
            {
                //Intâi mergi la adresa https://www.google.com/settings/security/lesssecureapps si asigura-te ca accesul la aplicatii mai putin sigure e bifat
                int portNumber = 587;
                bool enableSSL = true;
                string smtpAddress = "smtp.gmail.com";
                string mailFrom = _mailUser;
                string password = _mailUserPassword;
                string[] mailToMany = _mailUsers.Split(';');
                string subject = "Your WizzAir fly reservatrion requires attention!";
                string body = BuildMailBody(lastPrice, currentPrice);
                string logText = "";

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(mailFrom);
                    foreach (string mailToOne in mailToMany)
                        mail.To.Add(mailToOne);
                    mail.Subject = subject;
                    mail.IsBodyHtml = true; // Can set to false, if you are sending pure text.
                    mail.Body = body;

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(mailFrom, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                    MessageWriter.LogMessage($"Mail sent: \n {body}" + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} {ex.StackTrace}", "WizzAir Checker - Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string BuildMailBody(int? lastPrice, int? currentPrice)
        {
            return $"Now, at the current date and time, {DateTime.Now.ToString("dd.MM.yyy hh:mm tt")}: " +
                   $"\nthe current price is <span style=\"color: #FF0000;\"><b>{currentPrice}</b></span> lei" +
                   $"\nand the last was <span style=\"color: #FF0000;\"><b>{lastPrice}</b></span> lei!" + 
                   $"The difference is <span style=\"color: #FF0000;\"><b>{lastPrice - currentPrice}</b></span> lei!" +
                   $"\n\nTake action!!!";
        }
    }
}
