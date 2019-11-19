using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using com.caen.RFIDLibrary;

namespace CAENRFID2KEYtest
{
    class Program
    {
      

        static void Main(string[] args)
        {
            CAENRFIDReader m_RFIDReader = new CAENRFIDReader();
            com.caen.RFIDLibrary.CAENRFIDTag[] m_RFIDTags = new CAENRFIDTag[0];
            CAENRFIDLogicalSource m_Source0 = null;
            CAENRFIDPort connection_type;

            //switch (m_connection_type_comboBox.SelectedIndex)
            //{
            //    case 0:
                    connection_type = CAENRFIDPort.CAENRFID_RS232;
            //        break;
            //    case 1:		//10.0.7.85
            //        connection_type = CAENRFIDPort.CAENRFID_TCP;
            //        break;
            //    default:
            //        MessageBox.Show(this, "Unsupported connection", ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //}
            try
            {
                //m_inventory_listBox.Items.Clear();
                string address;
                address = "com22";
                m_RFIDReader.Connect(connection_type, address);
                CAENRFIDProtocol protocol = m_RFIDReader.GetProtocol();
              
                com.caen.RFIDLibrary.CAENRFIDLogicalSource[] logical_sources = m_RFIDReader.GetSources();
                if (logical_sources.Length == 0)
                    throw new Exception("No logical sources");
                m_Source0 = logical_sources[0];
               
            }
            catch (Exception excp)
            {
               // MessageBox.Show(this, excp.Message, ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
            int tcnt = 120;
            string lastcode = "";
            
            while (tcnt>0)
            {
                tcnt--;
                try
                {

                    //com.caen.RFIDLibrary.CAENRFIDLogicalSource[] logical_sources = m_RFIDReader.GetSources();
                   
                               
                    m_RFIDTags = null;
                    //System.Threading.Thread.Sleep(1000);

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
                                Console.WriteLine(code);
                                lastcode = code;
                                //cansend = false;
                            }

                        }
                        if (m_RFIDTags.Length != 0)
                        {
                            Console.Write(".");
                        }
                    }
                }
                catch (Exception excp)
                {
                    Console.WriteLine(excp.Message);
                }
                System.Threading.Thread.Sleep(1000);
            }
            m_RFIDReader.Disconnect();
        }
    }
}
