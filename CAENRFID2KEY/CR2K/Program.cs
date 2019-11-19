using System;
using System.Collections.Generic;
using System.Windows.Forms;
using com.caen.RFIDLibrary;
using System.Xml;
using System.Diagnostics ;


namespace WindowsFormsApplication1
{

   
    static class Program
    {

        static NotifyIcon mNotifyIcon;
        static ContextMenu mContextMenu;
        static bool Stopped=true;
        static CAENRFIDReader m_RFIDReader =null;
        static com.caen.RFIDLibrary.CAENRFIDTag[] m_RFIDTags =null;
        static CAENRFIDLogicalSource m_Source0 = null;
        static CAENRFIDPort connection_type;
        static MenuItem StartMenuItem = null;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Process p = checkInstance();
            if (p == null)
            {
                mNotifyIcon = new NotifyIcon();
                mNotifyIcon.Visible = false;
                mContextMenu = new ContextMenu();
                CreateMenu();
                mNotifyIcon.ContextMenu = mContextMenu;

                mNotifyIcon.Visible = true;
                mNotifyIcon.Icon = new System.Drawing.Icon("Paused.ico");
                ReadConfig();
                if (AutoStart) Start_Click(null, null);
                Application.Run();
            }
          


        }

        static string address;
        static string testkeys = "";
        static string logfile="";
        static Int32 power=0;
        static Int32 codelength;
        static bool sendEnter=false;
        static bool AutoStart = false;

        static void LogData(string s)
        {
            if (logfile == "") return;
            DateTime dt;
            dt = DateTime.Now;
            try
            {
                System.IO.File.AppendAllText(logfile, dt.ToString("G") + ": " + s+"\r\n");
            }
            catch (Exception)
            {
                
                
            }   


           

        }

        static Process checkInstance()
        {
            Process cProcess = Process.GetCurrentProcess();
            Process[] aProcesses = Process.GetProcessesByName(cProcess.ProcessName);

            foreach (var process in aProcesses)
            {


                if (process.Id != cProcess.Id)
                {

                    if (System.Reflection.Assembly.GetExecutingAssembly().Location == cProcess.MainModule.FileName)
                    {

                        return process;
                    }
                }
            }

            return null;
        }

        static void ReadConfig()
        {
             XmlDocument xml ;
            xml = new XmlDocument();
            xml.Load(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\Config.xml");
            XmlNode node ;
            node = xml.FirstChild;

            try 
	        {	        
		        address = node.Attributes.GetNamedItem("ComPort").Value;
	        }
	        catch (Exception)
	        {
		
		       
	        }

            try
            {
                testkeys = node.Attributes.GetNamedItem("Test").Value;
            }
            catch (Exception)
            {

                testkeys = "";
            }


            try
            {
                codelength = Int32.Parse (  node.Attributes.GetNamedItem("CodeLength").Value.ToString() );
            }
            catch (Exception)
            {

                codelength = 12;
            }

            try
            {
                string se;
                se = node.Attributes.GetNamedItem("SendEnter").Value;
                if (se.ToLower() == "false" )
                {
                    sendEnter = false;
                }
                else
                {
                    sendEnter = true;
                }
            }
            catch (Exception)
            {

                sendEnter = false;
            }

            try
            {
                string se;
                se = node.Attributes.GetNamedItem("AutoStart").Value;
                if (se.ToLower() == "false")
                {
                    AutoStart = false;
                }
                else
                {
                    AutoStart = true;
                }
            }
            catch (Exception)
            {

                AutoStart = false;
            }

            try
            {
                power = Int32.Parse(node.Attributes.GetNamedItem("Power").Value.ToString());
            }
            catch (Exception)
            {
                power = 0;

            }

             try 
	        {
                logfile = node.Attributes.GetNamedItem("LogFile").Value;
	        }
	        catch (Exception)
	        {
		
		       
	        }

             LogData("Config Loaded");
      
        }
        static void CreateMenu(){
            MenuItem mi;
            mi=new MenuItem("Stop CAEN RFID 2 KEY");
            mi.Click+=new EventHandler(Stop_Click);
            mContextMenu.MenuItems.Add(mi);

            mi = new MenuItem("Start CAEN RFID 2 KEY");
            mi.Click+=new EventHandler(Start_Click);
            StartMenuItem = mi;
            mContextMenu.MenuItems.Add(mi);

            mi = new MenuItem("Exit CAEN RFID 2 KEY");
            mi.Click+=new EventHandler(Exit_Click);
            mContextMenu.MenuItems.Add(mi);

          
        }

        static void Stop_Click(object sender, EventArgs e)
        {
           if (m_RFIDReader!=null){
                try 
	                {	        
		            m_RFIDReader.Disconnect();
                     mNotifyIcon.Icon = new System.Drawing.Icon("Stopped.ico");
                     Stopped = true;
	                }
	                catch (Exception)
	                {
		
		                
	                }
            }
            m_RFIDReader=null;
            StartMenuItem.Enabled = true;
        }

        static void Start_Click(object sender, EventArgs e)
        {
            LogData("Start menu click");
            ReadConfig();

           
            m_RFIDReader = new CAENRFIDReader();
            bool connected = false;
            try
            {
                
                m_RFIDReader.Connect(connection_type, address);
                CAENRFIDProtocol protocol = m_RFIDReader.GetProtocol();
              
                com.caen.RFIDLibrary.CAENRFIDLogicalSource[] logical_sources = m_RFIDReader.GetSources();
                if (logical_sources.Length == 0)
                {
                    connected = false;
                }
                else
                {
                    m_Source0 = logical_sources[0];
                    connected = true;
                    if (power != 0)
                    {
                        try
                        {
                            m_RFIDReader.SetPower(power); 
                        }
                        catch (Exception)
                        {
                            
                            
                        }
                    }
                }
               
            }
            catch (Exception excp)
            {
                LogData(excp.Message);
               MessageBox.Show(null, excp.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

             if (connected)
             {
                 mNotifyIcon.Icon = new System.Drawing.Icon("Running.ico");
                 Stopped = false;
                 string lastcode = "";
                 StartMenuItem.Enabled = false;


                 while (!Stopped)
                 {
                     Application.DoEvents();
                     try
                     {

                         m_RFIDTags = null;
                      

                         try
                         {
                             m_RFIDTags = m_Source0.InventoryTag();
                         }
                         catch (Exception )
                         {
                             
                             Stopped = true;
                             Stop_Click(sender, e);
                             return;
                            
                         }
                        
                         if (m_RFIDTags != null)
                         {
                             LogData("Get " + m_RFIDTags.Length.ToString() + " tag(s)"); 

                             for (int i = 0; i < m_RFIDTags.Length; i++)
                             {
                                 string code;
                                 byte[] bb = m_RFIDTags[i].GetId();
                                 code = System.BitConverter.ToString(bb);
                                 LogData("Tag("+ i.ToString()+")=" + code);

                                 code = code.Replace("-", "").ToLower() ;
                                 if (codelength < code.Length)
                                 {
                                     code = code.Substring(code.Length - codelength, codelength);
                                     LogData("code after cut " + codelength.ToString() + " bytes :"  + code);
                                 }

                                 if (lastcode != code)
                                 {
                                     //for (i = 0; i < code.Length; i++)
                                     //{
                                     //    LogData("Send To keyboard:" + code.ToCharArray()[i].ToString());
                                     //    KeysSender.PressTheKey(code.ToCharArray()[i]);
                                     //}
                                     //if (sendEnter)
                                     //{
                                     //    LogData("Send To keyboard: Enter");
                                     //    KeysSender.PressTheKey(Convert.ToChar(13));
                                     //}
                                     //Console.WriteLine(code);


                                     for (i = 0; i < code.Length; i++)
                                     {
                                         LogData("Send To keyboard:" + code.ToCharArray()[i].ToString());
                                         //KeysSender.PressTheKey(code.ToCharArray()[i]);
                                         KeysSender.SendCharUnicode((char)code.ToCharArray()[i]);
                                     }
                                     if (sendEnter)
                                     {
                                         LogData("Send To keyboard: Enter");
                                         //KeysSender.PressTheKey(Convert.ToChar(13));
                                         KeysSender.SendCharUnicode(Convert.ToChar(13));
                                     }
                                     lastcode = code;
                                     //cansend = false;
                                 }

                             }
                             if (m_RFIDTags.Length != 0)
                             {
                                 //Console.Write(".");
                             }
                         }

                     }
                     catch (Exception excp)
                     {
                         //Console.WriteLine(excp.Message);
                         LogData(excp.Message);
                     }
                     System.Threading.Thread.Sleep(1000);
                 }
             }
             else
             {
                 LogData("CAEN RFID Connection error");
                 MessageBox.Show(null, "CAEN RFID Connection error", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
             }

             System.Threading.Thread.Sleep(1000); 
             if (testkeys.Length > 0)
             {
                 for (int i = 0; i < testkeys.Length; i++)
                 {
                     LogData("Send Test To keyboard:" + testkeys.ToCharArray()[i].ToString());
                     KeysSender.PressTheKey(testkeys.ToCharArray()[i]);
                 }
             }
           

        }
        static void Exit_Click(object sender, EventArgs e)
        {
            LogData("Exit click");
            if (m_RFIDReader!=null){
                try 
	                {

                    Stopped = true;
                    for(int i=0;i<100;i++) Application.DoEvents();  
                    if(m_RFIDReader !=null)
                       
		                m_RFIDReader.Disconnect();
	                }
	                catch (Exception)
	                {
		
		               
	                }
            }
            m_RFIDReader = null;
            mNotifyIcon.Visible = false;
            mNotifyIcon.Dispose();
            Application.Exit(); 

        }



    }
}
