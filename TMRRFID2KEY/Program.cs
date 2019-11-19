using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ThingMagic;
using System.Xml;
using System.Diagnostics ;


namespace WindowsFormsApplication1
{

   
    static class Program
    {

        static NotifyIcon mNotifyIcon;
        static ContextMenu mContextMenu;
        static bool Stopped=true;
        static Reader  m_RFIDReader =null;
        static TagReadData[] m_RFIDTags = null;
        //static TMRRFIDLogicalSource m_Source0 = null;
        //static TMRRFIDPort connection_type;
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
                mNotifyIcon.Icon = new System.Drawing.Icon("TmrPaused.ico");
                ReadConfig();
                if (AutoStart) Start_Click(null, null);
                Application.Run();
            }
          


        }

        static string address;
        static Int32 power=0;
        static Int32 codelength;
        static bool sendEnter=false;
        static bool AutoStart = false;

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
            xml.Load(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\TmrConfig.xml");
            XmlNode node ;
            node = xml.FirstChild;

            try 
	        {	        
		        address = "tmr:///" + node.Attributes.GetNamedItem("ComPort").Value ;
	        }
	        catch (Exception)
	        {
		
		       
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
       
        }
        static void CreateMenu(){
            MenuItem mi;
            mi=new MenuItem("Stop TMR RFID 2 KEY");
            mi.Click+=new EventHandler(Stop_Click);
            mContextMenu.MenuItems.Add(mi);

            mi = new MenuItem("Start TMR RFID 2 KEY");
            mi.Click+=new EventHandler(Start_Click);
            StartMenuItem = mi;
            mContextMenu.MenuItems.Add(mi);

            mi = new MenuItem("Exit TMR RFID 2 KEY");
            mi.Click+=new EventHandler(Exit_Click);
            mContextMenu.MenuItems.Add(mi);

          
        }

        static void Stop_Click(object sender, EventArgs e)
        {
           if (m_RFIDReader!=null){
                try 
	                {	        
		                m_RFIDReader.Destroy() ;
                     mNotifyIcon.Icon = new System.Drawing.Icon("TmrStopped.ico");
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
            ReadConfig(); 
           
            bool connected = false;


           // KeysSender.GetCurrentKeyboardLayout(); 

            
            try
            {
                m_RFIDReader = Reader.Create(address);

                m_RFIDReader.Connect();
                if (Reader.Region.UNSPEC == (Reader.Region)m_RFIDReader.ParamGet("/reader/region/id"))
                {
                    Reader.Region[] supportedRegions = (Reader.Region[])m_RFIDReader.ParamGet("/reader/region/supportedRegions");
                    if (supportedRegions.Length < 1)
                    {
                        throw new FAULT_INVALID_REGION_Exception();
                    }
                    else
                    {
                        m_RFIDReader.ParamSet("/reader/region/id", supportedRegions[0]);
                    }
                }

                connected = true;
               
                if (power != 0)
                {
                    try
                    {
                        m_RFIDReader.ParamSet("/reader/radio/readPower",power);
                    }
                    catch (Exception)
                    {
                            
                            
                    }
                }
                //MessageBox.Show(null,"Current Power is: " + m_RFIDReader.ParamGet("/reader/radio/readPower").ToString() , Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); 
                
               
            }
            catch (Exception excp)
            {
               MessageBox.Show(null, excp.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

             if (connected)
             {
                 mNotifyIcon.Icon = new System.Drawing.Icon("TmrRunning.ico");
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
                             m_RFIDTags = m_RFIDReader.Read(500);
                           
                            
                         }
                         catch (Exception )
                         {
                             
                             Stopped = true;
                             Stop_Click(sender, e);
                             return;
                            
                         }
                        
                         if (m_RFIDTags != null)
                         {
                             for (int i = 0; i < m_RFIDTags.Length; i++)
                             {
                                 string code;
                                 byte[] bb = m_RFIDTags[i].Epc ;
                                 code = System.BitConverter.ToString(bb);

                                 code = code.Replace("-", "").ToLower() ;
                                 if (codelength < code.Length)
                                 {
                                     code = code.Substring(code.Length - codelength, codelength);
                                 }

                                 if (lastcode != code)
                                 {
                                     for (i = 0; i < code.Length; i++)
                                     {
                                         //KeysSender.PressTheKey(code.ToCharArray()[i]);
                                         KeysSender.SendCharUnicode((char)code.ToCharArray()[i]);
                                     }
                                     if (sendEnter)
                                     {
                                         //KeysSender.PressTheKey(Convert.ToChar(13));
                                         KeysSender.SendCharUnicode(Convert.ToChar(13));
                                     }
                                     //Console.WriteLine(code);
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
                     }
                     System.Threading.Thread.Sleep(1000);
                 }
             }
             else
             {
                 MessageBox.Show(null, "TMR RFID Connection error", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
             }

        }
        static void Exit_Click(object sender, EventArgs e)
        {
            if (m_RFIDReader!=null){
                try 
	                {

                    Stopped = true;
                    for(int i=0;i<100;i++) Application.DoEvents();  
                    if(m_RFIDReader !=null)
                       
		                m_RFIDReader.Destroy() ;
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
