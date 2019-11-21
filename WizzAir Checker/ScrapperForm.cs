using Logger_Viewer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WatiN.Core;
using WizzAir_Checker.Utils;
using WizzAir_Checking.Utils;
using Form = System.Windows.Forms.Form;

namespace WizzAir_Checker
{
    public partial class ScrapperForm : Form
    {
        private readonly EmailSender emailSender;
        private readonly double clearAllIeDataTime;
        private readonly string url = @"https://wizzair.com/ro-ro/#/booking/select-flight/IAS/TSF/2018-05-18/2018-05-25/3/0/0";
        private string trackingPriceFile;
        private IE ie;
        private bool ieVisible = false;
        private TimeSpan waitTime;

        public ScrapperForm()
        {
            InitializeComponent();

            emailSender = new EmailSender();
            emailSender.ProvideMailCredentials();

            trackingPriceFile = GetLogFilePath();

            // Configured Wait time 
            double secondsToWait;
            try { secondsToWait = double.Parse(ConfigurationManager.AppSettings["WaitTime"], CultureInfo.InvariantCulture); }
            catch { secondsToWait = 1; }
            waitTime = TimeSpan.FromSeconds(secondsToWait);

            // Configured time interval for clearing all IE data
            try { clearAllIeDataTime = double.Parse(ConfigurationManager.AppSettings["ClearAllIeDataTime"], CultureInfo.InvariantCulture); }
            catch { clearAllIeDataTime = 1; }


            // KIll all instances of IE before continue
            StopIe();

            // Initialize the Log Form
            MessageWriter.InitViewer(DebugWindowClosed);

            Thread thread = new Thread(Scrap);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void DebugWindowClosed()
        {
            showHideLogWindowToolStripMenuItem.Text = "Show log";
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            if(ie != null) StopIe();
            wizzAirNotifyIcon.Visible = false;
            Environment.Exit(1);
        }

        private void ShowHideLogWindowToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (showHideLogWindowToolStripMenuItem.Text == "Show log")
            {
                showHideLogWindowToolStripMenuItem.Text = "Hide log";
                MessageWriter.ShowDebugger();
            }
            else
            {
                showHideLogWindowToolStripMenuItem.Text = "Show log";
                MessageWriter.HideDebugger();
            }
        }

        private void OpenLogFileToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (File.Exists(trackingPriceFile))
            {
                Process.Start(trackingPriceFile);
            }
            else
            {
                MessageBox.Show($@"No {trackingPriceFile} file has been created yet.", @"WizzAir Checker Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        [STAThread]
        private void Scrap()
        {
            // Keep a variable for delete IE data
            DateTime lastRestart = DateTime.Now;
            while (true)
            {
                try
                {
                    if (ie != null)
                    {
                        StopIe();
                        ComputeSleep(Convert.ToInt32(waitTime.TotalSeconds), "for the next checking");
                    }

                    InitializeIe();

                    if (LoadingWizzairPage())
                    {
                        trackingPriceFile = GetLogFilePath();
                        string currentTotalPrice = ExtractPrice();
                        if (!string.IsNullOrEmpty(currentTotalPrice))
                        {
                            CheckingPrice(currentTotalPrice);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageWriter.LogMessage($"{ex.Message} {ex.StackTrace}" + Environment.NewLine);
                }
                if ((DateTime.Now - lastRestart).TotalMinutes >= clearAllIeDataTime)
                {
                    StopIe();
                    MessageWriter.LogMessage("Clearing all Internet Explorer data.." + Environment.NewLine);
                    GeneralUtils.ClearAllIEStoredData();
                    lastRestart = DateTime.Now;
                }
            }
        }

        private void ComputeSleep(int numberOfSeconds, string continuationMessage)
        {
            bool overiteLastRow = false;
            do
            {
                TimeSpan time = TimeSpan.FromSeconds(numberOfSeconds);
                string timeStr = time.Hours > 0 ? time.ToString(@"hh\:mm\:ss") : time.ToString(@"mm\:ss");
                MessageWriter.LogMessage($"Waiting {timeStr} {continuationMessage}.." + Environment.NewLine, overiteLastRow);

                overiteLastRow = true;
                --numberOfSeconds;

                Thread.Sleep(1000);
            }
            while (numberOfSeconds >= 0);

            MessageWriter.LogMessage(string.Empty, true);
        }

        private void CheckingPrice(string currentTotalPrice)
        {
            int? currentPrice = GeneralUtils.ExtractNumberBeginningOfString(currentTotalPrice, '.');
            string lastTotalPrice = GetLastPrice();
            int? lastPrice = GeneralUtils.ExtractNumberEndOfString(lastTotalPrice, '.');

            UpdateFileWithCurrentPrice(currentPrice);

            if (currentPrice < lastPrice)
            {
                MessageWriter.LogMessage($"Attention: lastPrice < currentPrice ({lastPrice} > {currentPrice}) ({DateTime.Now})" + Environment.NewLine);
                emailSender.SendMail(lastPrice, currentPrice);
            }
            else if (currentPrice > currentPrice)
            {
                MessageWriter.LogMessage($"Attention: lastPrice > currentPrice ({lastPrice} > {currentPrice}) ({DateTime.Now})" + Environment.NewLine);
            }
            else
            {
                MessageWriter.LogMessage($"Price is the same ({DateTime.Now})" + Environment.NewLine);
            }
        }

        private bool LoadingWizzairPage()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            MessageWriter.LogMessage($"Go to {url}" + Environment.NewLine);
            ie.GoTo(url);

            // wait until page completed loading
            MessageWriter.LogMessage("Wait for loading page to complete.." + Environment.NewLine);
            while (ie.Divs.Where(div => div.ClassName != null && div.ClassName.Equals("rf-fare__price")).ToArray().Length != 6)
            {
                Thread.Sleep(1000);
                if (sw.Elapsed.TotalSeconds >= 60)
                {
                    MessageWriter.LogMessage("IE does not finished loading page in the specified amount of time (60 seconds)" + Environment.NewLine);
                    return false;
                }
            }

            return true;
        }

        private string ExtractPrice()
        {
            try
            {
                MessageWriter.LogMessage("Checking WizzAir" + Environment.NewLine);

                Div basicPriceFrom = ie.Divs.Where(div => div.ClassName != null && div.ClassName.Equals("rf-fare__price")).ToArray()[0];
                Div basicPriceTo = ie.Divs.Where(div => div.ClassName != null && div.ClassName.Equals("rf-fare__price")).ToArray()[3];

                basicPriceFrom.Click();
                Thread.Sleep(1000);
                basicPriceTo.Click();
                Thread.Sleep(1000);

                Div totalPrice = ie.Div(Find.ByClass("booking-flow__itinerary__step__price"));

                MessageWriter.LogMessage($"Price readed: {totalPrice.Text}" + Environment.NewLine);
                return totalPrice.Text.Trim();
            }
            catch
            {
                return null;
            }
        }

        private string GetLastPrice()
        {
            if (File.Exists(trackingPriceFile))
                return File.ReadAllLines(trackingPriceFile).Last();
            return null;
        }

        private void InitializeIe()
        {
            MessageWriter.LogMessage(string.Empty + Environment.NewLine);
            MessageWriter.LogMessage("Start Internet Explorer" + Environment.NewLine);

            Settings.AutoMoveMousePointerToTopLeft = false;
            Settings.MakeNewIeInstanceVisible = ieVisible;
            ie = new IE();
        }

        private void StopIe()
        {
            MessageWriter.LogMessage("Stop Internet Explorer" + Environment.NewLine);
            if (ie != null)
            {
                ie.ClearCache();
                ie.ClearCookies();
            }

            // kill all instances of Internet Explorer
            GeneralUtils.KillProcessByPartialName("iexplore");
            ie = null;
        }

        private void UpdateFileWithCurrentPrice(int? currentTotalPrice)
        {
            string time = $"[{GeneralUtils.GetCurDay()}.{GeneralUtils.GetCurMonth()}.{DateTime.Now.Year} {GeneralUtils.GetCurHour()}:{GeneralUtils.GetCurMinute()}:{GeneralUtils.GetCurSecond()}] ";
            string line = $"{time} - Iași (IAS) -> Veneția Treviso (TSF) : {currentTotalPrice}";
            FileInfo fInfo = new FileInfo(trackingPriceFile);
            if (!fInfo.Directory.Exists)
                fInfo.Directory.Create();
            File.AppendAllLines(trackingPriceFile, new List<string> { line });
        }

        private string GetLogFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                          DateTime.Now.Year.ToString(),
                                          GeneralUtils.GetCurMonth(),
                                          GeneralUtils.GetCurDay(),
                                          "Iasi - Treviso (Price History).txt");
        }
    }
}
