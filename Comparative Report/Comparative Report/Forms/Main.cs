using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using System.IO;

namespace Comparative_Report.Forms
{
    public partial class Main : Form
    {

        public static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
          
            log.Info("Comparative Report Started " + DateTime.Now.ToString() + " " + Environment.UserName);
           
            currentyear = DateTime.Now.Year;
            txtCurrentYear.Text = currentyear.ToString();
            txtPrevYear.Text = (currentyear - 1).ToString();
            extractTimer.Start();
        }
        private int currentyear;
        private int prevYear;

        public void animateProgressBar(Boolean bol) 
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Boolean>(animateProgressBar), bol);
            }
            else if (bol)
            {

                progressBar1.Style = ProgressBarStyle.Marquee;
                progressBar1.Visible = true;
            }
            else 
            {
                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.Visible = false;
            }
        }
        public void setStatusLabel(String txt) 
        {
            if(InvokeRequired)
            {
                Invoke(new Action<String>(setStatusLabel),txt);
            }
            else
            {
                toolStripStatusLabel1.Text = txt;
            }
        }
        public void mainPanelEnabled(Boolean bol) 
        {
            if (InvokeRequired) 
            {
                Invoke(new Action<Boolean>(mainPanelEnabled), bol);
            }
            else 
            {
                grpParameterControl.Enabled = bol;
            }
        }

        private Boolean check() 
        {
            Boolean bol = true;
             if (cboMonth.SelectedIndex < 0) 
            {
                MessageBox.Show("Invalid month", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                bol = false;
                cboMonth.Focus();
            }else if (!int.TryParse(txtCurrentYear.Text, out currentyear)) 
            {
                MessageBox.Show("Current Year Invalid", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                bol=false;
                txtCurrentYear.Focus(); 
            }
            else if (currentyear > DateTime.Now.Year)
            {
                MessageBox.Show("Current Year Invalid", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                bol = false;
                txtCurrentYear.Focus(); 
            }
            else if (!int.TryParse(txtPrevYear.Text, out prevYear))
            {
                MessageBox.Show("Previous Year Invalid", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                bol = false;
                txtPrevYear.Focus();
            }
            else if (currentyear <= prevYear)
            {
                MessageBox.Show("Current must be greater than previous year", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                bol = false;
                txtCurrentYear.Focus();
            } 
           
            return bol;
        }
        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            if (!check())
                return;

            String currency = radiobtnPHP.Checked?"PHP":"USD";
            int m = cboMonth.SelectedIndex + 1;
            int cyear = int.Parse(txtCurrentYear.Text);
            int pyear = int.Parse(txtPrevYear.Text);
            Task.Factory.StartNew(() => manualExtract(m, cyear, pyear, currency));
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Info("Manual extract started " + DateTime.Now.ToString() + " " + Environment.UserName);

        }
        private void manualExtract(int m,int cyear,int pyear,String cr) 
        {
            
            mainPanelEnabled(false);
            animateProgressBar(true);


            setStatusLabel("Updating partners...");
            Response partnersResponse = new Query.Q356Query().startFetch(m, cyear, pyear, cr, "Partners");
            setStatusLabel("Updating File Upload...");
            Response fileUploadResponse = new Query.Q356Query().startFetch(m, cyear, pyear, cr, "FileUpload");
            setStatusLabel("Updating BillsPay ...");
            Response billspayResponse = new Query.Q356Query().startFetch(m, cyear, pyear, cr, "BillsPay");
            String ermsg = "";
            Boolean err = false;
            if (partnersResponse.responseCode != ResponseCode.OK)
            {
                err = true;
                ermsg += "Partners error: " + partnersResponse.responseMessage;
            }
            else
                if (fileUploadResponse.responseCode != ResponseCode.OK)
                {
                    err = true;
                    ermsg += "FileUpload error: " + fileUploadResponse.responseMessage;

                }
                else
                    if (billspayResponse.responseCode != ResponseCode.OK)
                    {
                        err = true;
                        ermsg += "billspayError error: " + billspayResponse.responseMessage;

                    }
            if (err)
            {
                log.Error("Manual extract completed with error: " + ermsg);
                setStatusLabel("Done with error: " + ermsg);
                MessageBox.Show(ermsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                else
                {
                    setStatusLabel("Done");
                    log.Info("Manual extract completed successfully");
                }
            mainPanelEnabled(true);
            animateProgressBar(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
    
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Info("Manual generate started :" + Environment.UserName);
            if (cboReport.SelectedIndex < 0)
            {
                MessageBox.Show("Please select report to generate", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cboReport.Focus();
            }
            else if(check()) 
            {
                int mo = cboMonth.SelectedIndex + 1;
                currentyear  = int.Parse(txtCurrentYear.Text);
                  prevYear= int.Parse(txtPrevYear.Text);
                String currency = radiobtnPHP.Checked?"PHP":"USD";
                String monthname = cboMonth.SelectedItem.ToString();
                int report = cboReport.SelectedIndex;
                string extension = radiobtnPDF.Checked ? ".pdf" : ".xls";

                Task.Factory.StartNew(() => generateManual(monthname,currency, mo, currentyear, prevYear,report,extension));

            }
            
        }
        private void generateManual(String monthname,String currency,int mo , int cyear,int pyear,int report,String extension) 
        {
            mainPanelEnabled(false);
            animateProgressBar(true);
            setStatusLabel("Fetching data...");

            if (report == 0)
            {
                Response response = new Query.QSummaryQuery().getSummary1data(currency, mo, cyear, pyear);
                if (response.responseCode != ResponseCode.OK)
                {
                    MessageBox.Show(response.responseMessage,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else 
                {
                    Reports.report rpt = new Reports.report();
                    setStatusLabel("Generating report..");
                    rpt.SetDataSource((List<DataClass.DSummary_data>)response.responseData);

                    rpt.SetParameterValue("currentMonth", monthname + " " + currentyear);
                    rpt.SetParameterValue("prevMonth", monthname + " " + prevYear);
                    rpt.SetParameterValue("cur", currency.ToUpper());

                    String filename = "_PartnerTxnComparativeReport" + monthname + cyear.ToString() + "_" +
                        monthname + pyear.ToString() + "_" + DateTime.Now.ToString("M-d-yyyy");
                    setStatusLabel("Exporting File..");
                    export(rpt,"C:\\ComparativeReport\\"+ "export\\"+currency + filename,extension);
         
                }
            }
            else 
            {

                Response response = new Query.QSummaryQuery2().getSummary2data(currency, mo, cyear,pyear);
                if (response.responseCode != ResponseCode.OK)
                {
                    MessageBox.Show(response.responseMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else 
                {
                    Reports.report2 rpt = new Reports.report2();
                    setStatusLabel("Generating Report");
                    rpt.SetDataSource((List<DataClass.DSummary_data2>)response.responseData);
                    rpt.SetParameterValue("currentMonth", monthname + " " + currentyear);
                    rpt.SetParameterValue("prevMonth", monthname + " " + prevYear);
                    rpt.SetParameterValue("cur", currency.ToUpper());

                    String filename = "_PartnerComparativeReport" + monthname + cyear.ToString() + "_" +
                       monthname + pyear.ToString() + "_" + DateTime.Now.ToString("M-d-yyyy");
                    setStatusLabel("Exporting file");
                    export(rpt, "C:\\ComparativeReport\\" + "export\\" + currency + filename, extension);
                }
                
            
            }
            mainPanelEnabled(true);
            animateProgressBar(false);
            setStatusLabel("Done.");
        }

        private void extractTimer_Tick(object sender, EventArgs e)
        {
            String inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            Util.INI inifile = new Util.INI(inipath);
            String gentime = inifile.IniReadValue("Auto Generation", "Time");
            if (gentime.Equals(DateTime.Now.ToShortTimeString())) 
            {
                extractTimer.Stop();
                Thread tr = new Thread(startAutoGenerate);
                tr.Start();  
            }
        }
        private void timerStart(Boolean bol) 
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Boolean>(timerStart), bol);
            }
            else
            {
                if (bol)
                {
                    extractTimer.Start();
                }
                else
                {
                    extractTimer.Stop();
                }
            }
        }
        private Boolean isDone { get; set; }
        private void monitor() 
        {
            while (!isDone) 
            {
                
            }

            timerStart(true);
        }
        private void startAutoGenerate() 
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Info("Auto Generate started");
            String inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            Util.INI inifile = new Util.INI(inipath);
            String day = inifile.IniReadValue("Auto Generation", "Day");

            Thread monitorthread = new Thread(monitor);
            monitorthread.Start();
            isDone = false;
            mainPanelEnabled(false);
            animateProgressBar(true);
            setStatusLabel("Initializing..");
            
            DateTime currentdate = new DateTime();
            currentdate = DateTime.Now.AddDays(-1);
            String currency = radiobtnPHP.Checked ? "PHP" : "USD";

              
            //update summaryData
            setStatusLabel("Updating partners PHP data");
            Response phppartnersResponse = new Query.Q356Query().startFetch(currentdate.Month, currentdate.Year, currentdate.Year-1, "PHP", "Partners");
            setStatusLabel("Updating fileupload PHP data");
            Response phpfileUploadResponse = new Query.Q356Query().startFetch(currentdate.Month, currentdate.Year, currentdate.Year - 1, "PHP", "FileUpload");
            setStatusLabel("Updating billspay  PHP data");
            Response phpbillspayResponse = new Query.Q356Query().startFetch(currentdate.Month, currentdate.Year, currentdate.Year-1, "PHP", "BillsPay");

            setStatusLabel("Updating partners USD data");
            Response usdpartnersResponse = new Query.Q356Query().startFetch(currentdate.Month, currentdate.Year, currentdate.Year - 1, "USD", "Partners");
            setStatusLabel("Updating fileupload USD data");
            Response usdfileUploadResponse = new Query.Q356Query().startFetch(currentdate.Month, currentdate.Year, currentdate.Year - 1, "USD", "FileUpload");
            setStatusLabel("Updating billspay USD data");
            Response usdbillspayResponse = new Query.Q356Query().startFetch(currentdate.Month, currentdate.Year, currentdate.Year - 1, "USD", "BillsPay");
            //update summaryData

            if (DateTime.Now.Day.ToString().Equals(day)) 
            {
                log.Info("Export date match exporting data");
                setStatusLabel("Exporting data");
                autoGenerateReport1();
                autoGenerateReport2();
            }
            
            Thread.Sleep(60000);
            animateProgressBar(false);
            mainPanelEnabled(true);
            setStatusLabel("Done");
            isDone = true;
        }
        private void autoGenerateReport1()
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            
            DateTime currentdate = new DateTime();
            currentdate = DateTime.Now.AddDays(-1);
            Response PHPresponse = new Query.QSummaryQuery().getSummary1data("PHP", currentdate.Month, currentdate.Year, currentdate.Year - 1);
            Response USDresponse = new Query.QSummaryQuery().getSummary1data("USD", currentdate.Month, currentdate.Year, currentdate.Year - 1);
            Reports.report rptPHP = new Reports.report();
            Reports.report rptUSD = new Reports.report();

            rptPHP.SetDataSource((List<DataClass.DSummary_data>)PHPresponse.responseData);
            rptPHP.SetParameterValue("currentMonth", currentdate.ToString("MMMM") + " " + currentdate.Year);
            rptPHP.SetParameterValue("prevMonth", currentdate.ToString("MMMM") + " " + (currentdate.Year - 1).ToString());
            rptPHP.SetParameterValue("cur", "PHP");


            rptUSD.SetDataSource((List<DataClass.DSummary_data>)USDresponse.responseData);
            rptUSD.SetParameterValue("currentMonth", currentdate.ToString("MMMM") + " " + currentdate.Year);
            rptUSD.SetParameterValue("prevMonth", currentdate.ToString("MMMM") + " " + (currentdate.Year - 1).ToString());
            rptUSD.SetParameterValue("cur", "USD");

            String filename = "_PartnerTxnComparativeReport" + currentdate.ToString("MMMM") + currentdate.Year.ToString() + "_" +
                currentdate.ToString("MMMM") + (currentdate.Year - 1).ToString() + "_" + DateTime.Now.ToString("M-d-yyyy");

            export(rptPHP, "C:\\ComparativeReport\\" + "export\\PHP" + filename);

            export(rptUSD, "C:\\ComparativeReport\\" + "export\\USD" + filename);
            
                 

        }

        private Response export(ReportDocument cryRpt, String filename, String extension = ".pdf") 
        {
            filename += extension;
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Response response = new Response();
            try
            {
                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            
                CrDiskFileDestinationOptions.DiskFileName = filename;
                CrExportOptions = cryRpt.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;                 
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    if (extension.Equals(".pdf"))
                    {
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.FormatOptions = new PdfRtfWordFormatOptions();
                    }

                    else 
                    {
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.FormatOptions = new ExcelFormatOptions();
                    }
                    
                }
                String directory = Path.GetDirectoryName(filename);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                cryRpt.Export();
                String inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
                Util.INI inifile = new Util.INI(inipath);
                String copypath = inifile.IniReadValue("Auto Generation", "Directory");
                String copypath2 = inifile.IniReadValue("Auto Generation", "Directory2");


                
                response.responseCode = ResponseCode.OK;
                if (!Directory.Exists(copypath))
                    Directory.CreateDirectory(copypath);
                if (!Directory.Exists(copypath2))
                    Directory.CreateDirectory(copypath2);
                if (copypath.EndsWith("\\") )
                {copypath = copypath + Path.GetFileName(filename);    }
                else 
                { copypath = copypath + "\\"+Path.GetFileName(filename);  }

                if (copypath2.EndsWith("\\"))
                {copypath2 = copypath2 + Path.GetFileName(filename);           }
                else
                { copypath2 = copypath2 + "\\" + Path.GetFileName(filename);   }
        
                if (File.Exists(copypath)) 
                {
                    File.Delete(copypath);
                }
                if (File.Exists(copypath2))
                {
                    File.Delete(copypath2);
                }
                              
                File.Copy(filename, copypath);
                File.Copy(filename, copypath2);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                response.responseCode=ResponseCode.Error;
                response.responseMessage = ex.Message;
            }
            return response;
        }
        private void autoGenerateReport2() 
        {
            DateTime currentdate = new DateTime();
            currentdate = DateTime.Now.AddDays(-1);
            Response PHPresponse = new Query.QSummaryQuery2().getSummary2data("PHP", currentdate.Month, currentdate.Year, currentdate.Year - 1);
            Response USDresponse = new Query.QSummaryQuery2().getSummary2data("USD", currentdate.Month, currentdate.Year, currentdate.Year - 1);

          
            Reports.report2 PHPrpt = new Reports.report2();
            Reports.report2 USDrpt = new Reports.report2();

            PHPrpt.SetDataSource((List<DataClass.DSummary_data2>)PHPresponse.responseData);
            PHPrpt.SetParameterValue("currentMonth", currentdate.ToString("MMMM") + " " + currentdate.Year.ToString());
            PHPrpt.SetParameterValue("prevMonth", currentdate.ToString("MMMM") + " " + (currentdate.Year-1).ToString());
            PHPrpt.SetParameterValue("cur", "PHP");

            USDrpt.SetDataSource((List<DataClass.DSummary_data2>)USDresponse.responseData);
            USDrpt.SetParameterValue("currentMonth", currentdate.ToString("MMMM") + " " + currentdate.Year.ToString());
            USDrpt.SetParameterValue("prevMonth", currentdate.ToString("MMMM") + " " + (currentdate.Year - 1).ToString());
            USDrpt.SetParameterValue("cur", "USD");

            String filename = "_PartnerComparativeReport" + currentdate.ToString("MMMM") + currentdate.Year.ToString() + "_" +
             currentdate.ToString("MMMM") + (currentdate.Year - 1).ToString() + "_" + DateTime.Now.ToString("M-d-yyyy");

            export(PHPrpt, "C:\\ComparativeReport\\" + "export\\PHP" + filename);
            export(USDrpt, "C:\\ComparativeReport\\" + "export\\USD" + filename);



        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
