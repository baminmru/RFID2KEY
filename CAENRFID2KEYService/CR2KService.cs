using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using com.caen.RFIDLibrary;

namespace CR2KService
{
    public partial class CR2KService : ServiceBase
    {
         
        
        public ManualResetEvent pStopServiceEvent;
        public ManualResetEvent pStopApprovedEvent;

       
        private System.Threading.Thread pMainThread = null;
        
        private const int cWaitInterval = 120000;
        private Object thisLock = new Object();
        private LogInfo pLogParams = new LogInfo();

        CAENRFIDReader m_RFIDReader = new CAENRFIDReader();
        com.caen.RFIDLibrary.CAENRFIDTag[] m_RFIDTags = new CAENRFIDTag[0];
        CAENRFIDLogicalSource m_Source0 = null;
        CAENRFIDPort connection_type;

       
        public CR2KService()
        {
            InitializeComponent();
            pStopServiceEvent = new ManualResetEvent(false);
            pStopApprovedEvent = new ManualResetEvent(false);
        }

        protected override void OnStart(string[] args)
        {
            InfoReport("Starting CR2KService...");
            if (pMainThread == null)
            {
                //InfoReport("Creating CR2KService main thread...");
                pMainThread = new Thread(new ThreadStart(MainThread));
                //pMainThread.ApartmentState = ApartmentState.MTA;
                pMainThread.SetApartmentState(ApartmentState.MTA);
                pMainThread.Name = "CR2KService Main thread! It's need for CR2KService's working";
            }
            //InfoReport("Starting CR2KService main thread...");
            pMainThread.Start();
            base.OnStart(args); 
        }
       
        private void MainThread()
        {
           eventLog1.BeginInit();
           int RequestInterval = 1000;
           string lastcode = "";
           bool bLogged = false;
      
                do
                {
                    // случилось команда на выход из сервиса
                    if (pStopServiceEvent.WaitOne(1, false))
                    {
                        try
                        {

                            InfoReport("Exiting working thread...");
                            
                            pStopApprovedEvent.Set();
                            return;
                        }
                        catch (Exception Ex)
                        {
                            ErrorReport("Exiting working thread error: " + Ex.Message);
                            return;
                        }
                    }

                    InfoReport("Connecting to reader...");
                     try
                    {
                        //m_inventory_listBox.Items.Clear();
                        string address;
                        address = "com22";
                        m_RFIDReader.Connect(connection_type, address);

                        //CAENRFIDProtocol protocol = m_RFIDReader.GetProtocol();
              
                        com.caen.RFIDLibrary.CAENRFIDLogicalSource[] logical_sources = m_RFIDReader.GetSources();
                        if (logical_sources.Length == 0)
                        {
                            bLogged=false;
                            ErrorReport("No logical sources...");
                        }
                        else
                        {
                            m_Source0 = logical_sources[0];
                            bLogged = true;
                        }
               
                    }
                    catch (Exception excp)
                    {
                         bLogged=false;
                        ErrorReport( excp.Message);
                    }
                 
                } while (!pStopServiceEvent.WaitOne(1000, false) && !bLogged);
             
                InfoReport("Reader Initialization OK");



                // соединение прошло успешно
                if (bLogged)
                {
                    do
                    {

                        try
                        {

                            m_RFIDTags = null;


                            m_RFIDTags = m_Source0.InventoryTag();
                            if (m_RFIDTags != null)
                            {
                                for (int i = 0; i < m_RFIDTags.Length; i++)
                                {
                                    string code;
                                    byte[] bb = m_RFIDTags[i].GetId();
                                    code = System.BitConverter.ToString(bb);

                                    code = code.Replace("-", "");

                                    if (lastcode != code)
                                    {
                                        for (i = 0; i < code.Length; i++)
                                        {
                                            KeysSender.PressTheKey(code.ToCharArray()[i]);
                                        }
                                        InfoReport(code);
                                        lastcode = code;
                                        //cansend = false;
                                    }

                                }
                                if (m_RFIDTags.Length != 0)
                                {
                                    //m_inventory_listBox.SelectedIndex = 0;
                                    //cansend = true;
                                }
                            }
                        }
                        catch (Exception excp)
                        {
                            ErrorReport(excp.Message);
                        }

                        //System.Threading.Thread.Sleep(1000);


                    } while (!pStopServiceEvent.WaitOne(RequestInterval, false));
                } 

            try
            {
                InfoReport("Closing CR2KService...");
                 m_RFIDReader.Disconnect();
        
                             base.OnStop();
                eventLog1.Dispose();
                return;
            }
            catch (Exception Ex)
            {
                ErrorReport("Closing CR2KService error:" + Ex.Message);
            }
            
            
        }
             

        protected override void OnStop()
        {
            pStopServiceEvent.Set();
        }
        protected override void OnContinue()
        {
            base.OnContinue();
   
        }

        protected override void OnPause()
        {
            base.OnPause();
          
        }
        
        

        #region log functions
        public void ErrorReport(string Message)
        {
            lock (thisLock)
            {
                LogReport(Message, EventLogEntryType.Error);
            }
        }
        public void InfoReport(string Message)
        {
            lock (thisLock)
            {
                LogReport(Message, EventLogEntryType.Information);
            }
        }
        public void WarningReport(string Message)
        {
            lock (thisLock)
            {
                LogReport(Message, EventLogEntryType.Warning);
            }
        }
        public void LogReport(string Message, System.Diagnostics.EventLogEntryType ELET)
        {
            if (pLogParams != null)
            {
                if (pLogParams.UseEventLog)
                {
                    //this.EventLog.WriteEntry(Message, ELET);
                    this.eventLog1.WriteEntry(Message, ELET);
                    System.Diagnostics.Trace.WriteLine(ELET.ToString() + " :" + Message);
                }
                if (pLogParams.UseFileLog && pLogParams.LogFile.ToString() != "")
                {
                    try
                    {
                        string FileName = "";//string FileName = pLogParams.LogFile;
                        if (FileName == string.Empty || FileName == "") FileName = System.IO.Path.GetDirectoryName(GetType().Assembly.Location)+"CR2KServiceLogFile.txt";
                        System.IO.TextWriter LogFile = new System.IO.StreamWriter(FileName, true);
                        if (ELET == System.Diagnostics.EventLogEntryType.Error)
                            LogFile.WriteLine(System.DateTime.Now.ToString() + " Error: " + Message);
                        else if (ELET == System.Diagnostics.EventLogEntryType.Warning)
                            LogFile.WriteLine(System.DateTime.Now.ToString() + " Warning: " + Message);
                        else
                            LogFile.WriteLine(System.DateTime.Now.ToString() + Message);
                        LogFile.Close();
                        LogFile = null;
                    }
                    catch { }
                }
            }
            else
            {
                this.eventLog1.WriteEntry(System.DateTime.Now.ToString() + Message, ELET);
                if (ELET == System.Diagnostics.EventLogEntryType.Error)
                    System.Diagnostics.Trace.WriteLine(System.DateTime.Now.ToString() + " Error: " + Message);
                else if (ELET == System.Diagnostics.EventLogEntryType.Warning)
                    System.Diagnostics.Trace.WriteLine(System.DateTime.Now.ToString() + " Warning: " + Message);
                else
                    System.Diagnostics.Trace.WriteLine(System.DateTime.Now.ToString() + Message);
            }
            return;
        }
        #endregion log functions

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}
