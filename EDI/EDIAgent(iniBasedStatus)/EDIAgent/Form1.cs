using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using log4net;
using log4net.Config;

namespace EDIAgent
{
    public partial class Form1 : Form
    {
       public static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Form1()
        {
         //   log4net.Config.DOMConfigurator.Configure();
            
          //  log.Info("Agent started");
            InitializeComponent();
        //    notification = ((System.Drawing.Icon)(resources.GetObject("AppDomain.CurrentDomain.BaseDirectory "+"logo.ico")));
         //   notification.Icon = (System.Drawing.Icon)AppDomain.CurrentDomain.BaseDirectory + "I.ini";
        }
        Thread startProc;
        private string baseSynergyStatus = "";
        private string updateSynergyStatus = "";
        private void Form1_Load(object sender, EventArgs e)

        {
            log4net.Config.XmlConfigurator.Configure();
    
            this.Visible = false;
            
            string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            util.INI ini = new util.INI(inipath);
            this.Name ="EDIAgent-"+ini.IniReadValue("SYS", "AgentName");
            this.Text = this.Name;
            this.Hide();

            baseSynergyStatus = ini.IniReadValue("synergyStatus", "baseSynergyStatus");
            updateSynergyStatus =ini.IniReadValue("synergyStatus", "updateSynergyStatus");
           // this.ShowInTaskbar = false;
        
            notifyIcon1.Text = this.Name;
            
               startProc = new Thread(startLink);
           startProc.Start();

            
            
         
          //  Process.Start(AppDomain.CurrentDomain.BaseDirectory + "EDIAgent.exe");
           // this.Close();
            
        }
        private void startLink() 
        {
            Thread.Sleep(6000);
            try 
           {
               
                   
               new query.TransactionsPending( baseSynergyStatus,updateSynergyStatus).start();
               //new query.allocation_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.EDIREPORT_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.thirmonth_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.basethirmonth_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.baseallocation_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.CORPINCOME_CORP(baseSynergyStatus, updateSynergyStatus).start();
               //new query.smart3_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.globesmartbill_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.ICONNECT_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.FOREX_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.ICONNECT_REPLENISH(baseSynergyStatus, updateSynergyStatus).start();
               //new query.insurance_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.kp_income(baseSynergyStatus, updateSynergyStatus).start();
               //new query.midyear_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.basemidyear_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.mmd_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.MoneyGram_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.sp_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.basecorpallo_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.pea_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.provision_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.baseprovision_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.recurring_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.rt_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.basecorprt_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.mealallowance2_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.basecorpsg1_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.sg_corp1(baseSynergyStatus, updateSynergyStatus).start();
               //new query.SSMI_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.UnusedSickLeave_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.withholding_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.xoom_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.MoneyExpress_corp(baseSynergyStatus, updateSynergyStatus).start();
               //new query.BaseCorpUnusedSickLeave_corp(baseSynergyStatus, updateSynergyStatus).start();
     
            }
            catch (Exception e) 
            {
               log.Error(e.Message+ "  "+ this.Name, e );
            }
            

            Process.Start(AppDomain.CurrentDomain.BaseDirectory + "EDIAgent.exe");
            this.closeForm(this);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {

                while (startProc.ThreadState == System.Threading.ThreadState.Running || startProc.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
                {
                    startProc.Abort();
                }
            }
            catch (Exception) { }
            this.Close();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.Visible = false;
                this.ShowInTaskbar = false;
            }
            else
            {
                this.Visible = true;
                this.ShowInTaskbar = true;
            }
        }

        private delegate void delegateForm(Form form);
        private void closeForm(Form form) 
        {
            if (form.InvokeRequired)
            {
                delegateForm control = new delegateForm(closeForm);
                form.Invoke(control, form);

            }
            else
                form.Close();
        }
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
          //  this.Hide();
            //this.ShowInTaskbar = false;
        }
       // private 
    }
}
