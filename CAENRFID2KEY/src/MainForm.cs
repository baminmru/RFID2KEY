using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using com.caen.RFIDLibrary;

namespace CAENRFIDCsDemo
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button m_connect_button;
		private System.Windows.Forms.Button m_inventory_button;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ListBox m_inventory_listBox;
		private System.Windows.Forms.Button m_read_button;
		private System.Windows.Forms.Button m_write_button;
		private System.Windows.Forms.Button m_disconnect_button;
		private System.Windows.Forms.TextBox m_rw_textBox;
		private System.Windows.Forms.Panel m_command_panel;
		private System.Windows.Forms.Panel m_view_panel;
		private System.Windows.Forms.ComboBox m_connection_type_comboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox m_connection_address_textBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private CAENRFIDReader m_RFIDReader = new CAENRFIDReader();
		private System.Windows.Forms.CheckBox m_asynch_checkBox;
		private com.caen.RFIDLibrary.CAENRFIDTag[] m_RFIDTags= new CAENRFIDTag[0];
		private CAENRFIDReceiver m_RFIDReceiver= null;
		private CAENRFIDLogicalSource m_Source0= null;
		private CAENRFIDTrigger m_CurrentReadTrigger= null;
		private CAENRFIDChannel m_CurrentChannel= null;
		private CAENRFIDTrigger m_CurrentNotifyTrigger= null;
		private System.Windows.Forms.ComboBox m_tag_type_comboBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox m_len_textBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox m_add_textBox;
		private System.Windows.Forms.Label label5;
		private bool m_b_connected= false;
		private bool m_use_event_mode= false;
        private delegate void UpdateDelegate( CAENRFIDEventArgs e);
        private UpdateDelegate update_delegate;
        public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            this.update_delegate = new UpdateDelegate( this.do_update_delegate);
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_connect_button = new System.Windows.Forms.Button();
			this.m_inventory_button = new System.Windows.Forms.Button();
			this.m_command_panel = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.m_tag_type_comboBox = new System.Windows.Forms.ComboBox();
			this.m_asynch_checkBox = new System.Windows.Forms.CheckBox();
			this.m_connection_address_textBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.m_connection_type_comboBox = new System.Windows.Forms.ComboBox();
			this.m_disconnect_button = new System.Windows.Forms.Button();
			this.m_write_button = new System.Windows.Forms.Button();
			this.m_read_button = new System.Windows.Forms.Button();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.m_view_panel = new System.Windows.Forms.Panel();
			this.m_rw_textBox = new System.Windows.Forms.TextBox();
			this.m_inventory_listBox = new System.Windows.Forms.ListBox();
			this.m_len_textBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.m_add_textBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.m_command_panel.SuspendLayout();
			this.m_view_panel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_connect_button
			// 
			this.m_connect_button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_connect_button.Cursor = System.Windows.Forms.Cursors.Hand;
			this.m_connect_button.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_connect_button.Location = new System.Drawing.Point(8, 176);
			this.m_connect_button.Name = "m_connect_button";
			this.m_connect_button.Size = new System.Drawing.Size(120, 23);
			this.m_connect_button.TabIndex = 0;
			this.m_connect_button.Text = "Connect";
			this.m_connect_button.Click += new System.EventHandler(this.m_connect_button_Click);
			// 
			// m_inventory_button
			// 
			this.m_inventory_button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_inventory_button.Cursor = System.Windows.Forms.Cursors.Hand;
			this.m_inventory_button.Enabled = false;
			this.m_inventory_button.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_inventory_button.Location = new System.Drawing.Point(8, 208);
			this.m_inventory_button.Name = "m_inventory_button";
			this.m_inventory_button.Size = new System.Drawing.Size(120, 23);
			this.m_inventory_button.TabIndex = 1;
			this.m_inventory_button.Text = "Inventory";
			this.m_inventory_button.Click += new System.EventHandler(this.m_inventory_button_Click);
			// 
			// m_command_panel
			// 
			this.m_command_panel.AutoScroll = true;
			this.m_command_panel.AutoScrollMinSize = new System.Drawing.Size(128, 270);
			this.m_command_panel.BackColor = System.Drawing.Color.Gainsboro;
			this.m_command_panel.Controls.Add(this.m_len_textBox);
			this.m_command_panel.Controls.Add(this.label4);
			this.m_command_panel.Controls.Add(this.m_add_textBox);
			this.m_command_panel.Controls.Add(this.label5);
			this.m_command_panel.Controls.Add(this.label3);
			this.m_command_panel.Controls.Add(this.m_tag_type_comboBox);
			this.m_command_panel.Controls.Add(this.m_asynch_checkBox);
			this.m_command_panel.Controls.Add(this.m_connection_address_textBox);
			this.m_command_panel.Controls.Add(this.label2);
			this.m_command_panel.Controls.Add(this.label1);
			this.m_command_panel.Controls.Add(this.m_connection_type_comboBox);
			this.m_command_panel.Controls.Add(this.m_disconnect_button);
			this.m_command_panel.Controls.Add(this.m_write_button);
			this.m_command_panel.Controls.Add(this.m_read_button);
			this.m_command_panel.Controls.Add(this.m_inventory_button);
			this.m_command_panel.Controls.Add(this.m_connect_button);
			this.m_command_panel.Dock = System.Windows.Forms.DockStyle.Left;
			this.m_command_panel.Location = new System.Drawing.Point(0, 0);
			this.m_command_panel.Name = "m_command_panel";
			this.m_command_panel.Size = new System.Drawing.Size(136, 398);
			this.m_command_panel.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(8, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 16);
			this.label3.TabIndex = 13;
			this.label3.Text = "Tag type:";
			// 
			// m_tag_type_comboBox
			// 
			this.m_tag_type_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_tag_type_comboBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.m_tag_type_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_tag_type_comboBox.Items.AddRange(new object[] {
																	 "ISO USER",
																	 "G2 RESERVED",
																	 "G2 EPC",
																	 "G2 TID",
																	 "G2 USER"});
			this.m_tag_type_comboBox.Location = new System.Drawing.Point(8, 104);
			this.m_tag_type_comboBox.Name = "m_tag_type_comboBox";
			this.m_tag_type_comboBox.Size = new System.Drawing.Size(120, 21);
			this.m_tag_type_comboBox.TabIndex = 12;
			// 
			// m_asynch_checkBox
			// 
			this.m_asynch_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_asynch_checkBox.Appearance = System.Windows.Forms.Appearance.Button;
			this.m_asynch_checkBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.m_asynch_checkBox.Enabled = false;
			this.m_asynch_checkBox.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_asynch_checkBox.Location = new System.Drawing.Point(8, 320);
			this.m_asynch_checkBox.Name = "m_asynch_checkBox";
			this.m_asynch_checkBox.Size = new System.Drawing.Size(120, 24);
			this.m_asynch_checkBox.TabIndex = 10;
			this.m_asynch_checkBox.Text = "Asynch";
			this.m_asynch_checkBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.m_asynch_checkBox.CheckedChanged += new System.EventHandler(this.m_asynch_checkBox_CheckedChanged);
			// 
			// m_connection_address_textBox
			// 
			this.m_connection_address_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_connection_address_textBox.Location = new System.Drawing.Point(8, 64);
			this.m_connection_address_textBox.Name = "m_connection_address_textBox";
			this.m_connection_address_textBox.Size = new System.Drawing.Size(120, 21);
			this.m_connection_address_textBox.TabIndex = 8;
			this.m_connection_address_textBox.Text = "";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "Address:";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Connection type:";
			// 
			// m_connection_type_comboBox
			// 
			this.m_connection_type_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_connection_type_comboBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.m_connection_type_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_connection_type_comboBox.Items.AddRange(new object[] {
																			"RS232",
																			"TCP/IP",
																			"USB"});
			this.m_connection_type_comboBox.Location = new System.Drawing.Point(8, 24);
			this.m_connection_type_comboBox.Name = "m_connection_type_comboBox";
			this.m_connection_type_comboBox.Size = new System.Drawing.Size(120, 21);
			this.m_connection_type_comboBox.TabIndex = 5;
			// 
			// m_disconnect_button
			// 
			this.m_disconnect_button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_disconnect_button.Cursor = System.Windows.Forms.Cursors.Hand;
			this.m_disconnect_button.Enabled = false;
			this.m_disconnect_button.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_disconnect_button.Location = new System.Drawing.Point(8, 360);
			this.m_disconnect_button.Name = "m_disconnect_button";
			this.m_disconnect_button.Size = new System.Drawing.Size(120, 23);
			this.m_disconnect_button.TabIndex = 4;
			this.m_disconnect_button.Text = "Disconnect";
			this.m_disconnect_button.Click += new System.EventHandler(this.m_disconnect_button_Click);
			// 
			// m_write_button
			// 
			this.m_write_button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_write_button.Cursor = System.Windows.Forms.Cursors.Hand;
			this.m_write_button.Enabled = false;
			this.m_write_button.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_write_button.Location = new System.Drawing.Point(8, 272);
			this.m_write_button.Name = "m_write_button";
			this.m_write_button.Size = new System.Drawing.Size(120, 23);
			this.m_write_button.TabIndex = 3;
			this.m_write_button.Text = "Write";
			this.m_write_button.Click += new System.EventHandler(this.m_write_button_Click);
			// 
			// m_read_button
			// 
			this.m_read_button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_read_button.Cursor = System.Windows.Forms.Cursors.Hand;
			this.m_read_button.Enabled = false;
			this.m_read_button.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_read_button.Location = new System.Drawing.Point(8, 240);
			this.m_read_button.Name = "m_read_button";
			this.m_read_button.Size = new System.Drawing.Size(120, 23);
			this.m_read_button.TabIndex = 2;
			this.m_read_button.Text = "Read";
			this.m_read_button.Click += new System.EventHandler(this.m_read_button_Click);
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.Color.Silver;
			this.splitter1.Location = new System.Drawing.Point(136, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 398);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// m_view_panel
			// 
			this.m_view_panel.Controls.Add(this.m_rw_textBox);
			this.m_view_panel.Controls.Add(this.m_inventory_listBox);
			this.m_view_panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_view_panel.Location = new System.Drawing.Point(139, 0);
			this.m_view_panel.Name = "m_view_panel";
			this.m_view_panel.Size = new System.Drawing.Size(413, 398);
			this.m_view_panel.TabIndex = 4;
			// 
			// m_rw_textBox
			// 
			this.m_rw_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_rw_textBox.Enabled = false;
			this.m_rw_textBox.Location = new System.Drawing.Point(8, 368);
			this.m_rw_textBox.Name = "m_rw_textBox";
			this.m_rw_textBox.Size = new System.Drawing.Size(400, 21);
			this.m_rw_textBox.TabIndex = 1;
			this.m_rw_textBox.Text = "";
			// 
			// m_inventory_listBox
			// 
			this.m_inventory_listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_inventory_listBox.Enabled = false;
			this.m_inventory_listBox.IntegralHeight = false;
			this.m_inventory_listBox.Location = new System.Drawing.Point(8, 8);
			this.m_inventory_listBox.Name = "m_inventory_listBox";
			this.m_inventory_listBox.Size = new System.Drawing.Size(400, 352);
			this.m_inventory_listBox.TabIndex = 0;
			// 
			// m_len_textBox
			// 
			this.m_len_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_len_textBox.Location = new System.Drawing.Point(72, 144);
			this.m_len_textBox.Name = "m_len_textBox";
			this.m_len_textBox.Size = new System.Drawing.Size(56, 21);
			this.m_len_textBox.TabIndex = 18;
			this.m_len_textBox.Text = "4";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(72, 128);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 20);
			this.label4.TabIndex = 19;
			this.label4.Text = "Length:";
			// 
			// m_add_textBox
			// 
			this.m_add_textBox.Location = new System.Drawing.Point(8, 144);
			this.m_add_textBox.Name = "m_add_textBox";
			this.m_add_textBox.Size = new System.Drawing.Size(56, 21);
			this.m_add_textBox.TabIndex = 20;
			this.m_add_textBox.Text = "4";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 128);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 20);
			this.label5.TabIndex = 21;
			this.label5.Text = "Address:";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.LightGray;
			this.ClientSize = new System.Drawing.Size(552, 398);
			this.Controls.Add(this.m_view_panel);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.m_command_panel);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.MinimumSize = new System.Drawing.Size(560, 432);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CAEN RFID C# Library demo application";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.m_command_panel.ResumeLayout(false);
			this.m_view_panel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		private void m_connect_button_Click(object sender, System.EventArgs e)
		{
			CAENRFIDPort connection_type;
			switch( this.m_connection_type_comboBox.SelectedIndex)
			{
				case 0:
					connection_type= CAENRFIDPort.CAENRFID_RS232;
					break;
				case 1:		//10.0.7.85
					connection_type= CAENRFIDPort.CAENRFID_TCP;
					break;
				default:
					MessageBox.Show( this, "Unsupported connection", this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
			}
			try
			{
				this.m_inventory_listBox.Items.Clear();
                string address;
                address=this.m_connection_address_textBox.Text.Trim();
				this.m_RFIDReader.Connect( connection_type, address);
				CAENRFIDProtocol protocol= this.m_RFIDReader.GetProtocol();
				//com.caen.RFIDLibrary.CAENRFIDEventMode mode= this.m_RFIDReader.GetEventMode();
				this.m_use_event_mode= true;
				/*switch( mode)
				{
					case com.caen.RFIDLibrary.CAENRFIDEventMode.READCYCLE_MODE:
					case com.caen.RFIDLibrary.CAENRFIDEventMode.TIME_MODE:
						this.m_use_event_mode= true;
						break;
				}*/
				com.caen.RFIDLibrary.CAENRFIDLogicalSource[] logical_sources= this.m_RFIDReader.GetSources(); 
				if( logical_sources.Length== 0)
					throw new Exception( "No logical sources");
				this.m_Source0= logical_sources[0];
				MessageBox.Show( this, "Connected", this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.m_b_connected= true;
				this.SetControls();
			}
			catch( Exception excp)
			{
				MessageBox.Show( this, excp.Message, this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void SetControls()
		{
			this.m_connect_button.Enabled= !this.m_b_connected;
			this.m_disconnect_button.Enabled= this.m_b_connected;
			this.m_asynch_checkBox.Enabled= this.m_b_connected;
			this.m_inventory_button.Enabled= this.m_b_connected;
			this.m_read_button.Enabled= this.m_b_connected;
			this.m_write_button.Enabled= this.m_b_connected;
			this.m_rw_textBox.Enabled= this.m_b_connected;
			this.m_inventory_listBox.Enabled= this.m_b_connected;

		}

		private void m_disconnect_button_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.m_RFIDReader.Disconnect();
				this.m_b_connected= false;
				this.SetControls();
			}
			catch( Exception excp)
			{
				MessageBox.Show( this, excp.Message, this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			this.m_connection_type_comboBox.SelectedIndex= 0;
			this.m_tag_type_comboBox.SelectedIndex= 0;
		}

		private void m_inventory_button_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.m_inventory_listBox.Items.Clear();
				com.caen.RFIDLibrary.CAENRFIDLogicalSource[] logical_sources= this.m_RFIDReader.GetSources(); 
				if( logical_sources.Length== 0)
					throw new Exception( "No logical sources");
				this.m_Source0= logical_sources[0];
                //logical_sources[0].SetSelected_EPC_C1G2(CAENRFIDLogicalSourceConstants.EPC_C1G2_SELECTED_YES);
				// this.m_RFIDTags= logical_sources[0].Inventory();
                int tt=0;
                this.m_RFIDTags = null;
                System.Threading.Thread.Sleep(5000);
                while (tt < 100 && this.m_RFIDTags == null)
                {
                    this.m_RFIDTags = logical_sources[0].InventoryTag();
                    System.Threading.Thread.Sleep(1000);
                    tt++;
                }
				//Dbg_end

				if( this.m_RFIDTags== null)
					return;
				for( int i= 0; i< this.m_RFIDTags.Length; i++)
				{
                    string code;
                    code = System.BitConverter.ToString(this.m_RFIDTags[i].GetId());
					this.m_inventory_listBox.Items.Add(  code);
                    for (i = 0; i < code.Length; i++)
                    {
                        KeysSender.PressTheKey(code.ToCharArray()[i]);  
                    }



				}
				if( this.m_RFIDTags.Length!= 0)
				{
					this.m_inventory_listBox.SelectedIndex= 0;
				}
			}
			catch( Exception excp)
			{
				MessageBox.Show( this, excp.Message, this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
//		// DbgOnly: prova multithread
//		com.caen.RFIDLibrary.CAENRFIDTag m_dbg_tag= null;
//		private void InventoryThread()
//		{
//			for( int i= 0; i< 10; i++)
//			{
//				com.caen.RFIDLibrary.CAENRFIDTag[] tags= null;
//				try
//				{
//					tags= this.m_Source0.Inventory();
//					if( tags== null)
//						continue;
//					for( int tag= 0; tag< tags.Length; tag++)
//					{
//						System.Diagnostics.Trace.WriteLine( i.ToString()+ " "+ System.BitConverter.ToString( tags[ tag].GetId()));
//					}
//				}
//				catch( Exception excp)
//				{
//					System.Diagnostics.Trace.WriteLine( "Inventory "+ i.ToString()+ " "+ excp.Message);
//				}
//
////				System.Threading.Thread.Sleep( 0);
//			}
//		}
//		private void ReadThread()
//		{
//			for( int i= 0; i< 10; i++)
//			{
//				string rel= null;
//				try
//				{
//					rel= this.m_RFIDReader.GetFirmwareRelease();
//					if( rel== null)
//						continue;
//					System.Diagnostics.Trace.WriteLine( i.ToString()+ " "+ rel);
//				}
//				catch( Exception excp)
//				{
//					System.Diagnostics.Trace.WriteLine( "Read "+ i.ToString()+ " "+ excp.Message);
//				}
//
////				System.Threading.Thread.Sleep( 0);
//			}
//		}
		private void m_read_button_Click(object sender, System.EventArgs e)
		{
			if( this.m_inventory_listBox.SelectedIndex< 0)
			{
				MessageBox.Show( this, "Select a tag to read!", this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			try
			{
				int add= int.Parse( this.m_add_textBox.Text);
				int len= int.Parse( this.m_len_textBox.Text);
				byte[] read= null;
				if( this.m_tag_type_comboBox.SelectedIndex== 0)
				{
					read= this.m_RFIDTags[ this.m_inventory_listBox.SelectedIndex].GetSource().ReadTagData( this.m_RFIDTags[ this.m_inventory_listBox.SelectedIndex], (short)add, (short)len);
				}
				else
				{
					read= this.m_RFIDTags[ this.m_inventory_listBox.SelectedIndex].GetSource().ReadTagData_EPC_C1G2( this.m_RFIDTags[ this.m_inventory_listBox.SelectedIndex], (short)( this.m_tag_type_comboBox.SelectedIndex- 1), (short)add, (short)len);
				}
				this.m_rw_textBox.Text= System.BitConverter.ToString( read);

//				// DbgOnly: starta thread per test Reader
//				this.m_dbg_tag= this.m_RFIDTags[ this.m_inventory_listBox.SelectedIndex];
//				System.Threading.Thread t_inv= new System.Threading.Thread( new System.Threading.ThreadStart( this.InventoryThread));
//				System.Threading.Thread t_inv2= new System.Threading.Thread( new System.Threading.ThreadStart( this.InventoryThread));
//				System.Threading.Thread t_read= new System.Threading.Thread( new System.Threading.ThreadStart( this.ReadThread));
//				t_read.Start();
//				t_inv.Start();
//				t_inv2.Start();
//				// attendi fine threads
//				t_read.Join();
//				t_inv.Join();
//				t_inv2.Join();
			}
			catch( Exception excp)
			{
				MessageBox.Show( this, excp.Message, this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.do_stop_asynch();
			this.m_RFIDReader.Disconnect();
		}

		private void m_write_button_Click(object sender, System.EventArgs e)
		{
			try
			{
				if( this.m_inventory_listBox.SelectedIndex< 0)
				{
					MessageBox.Show( this, "Select a tag to write into!", this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				string string_to_send= this.m_rw_textBox.Text.Trim();
				string[] tokens= string_to_send.Split( new char[] {' ', '-'});
				if( tokens.Length== 0)
					return;
				int add= int.Parse( this.m_add_textBox.Text);
				byte[] byte_to_send= new byte[ tokens.Length];
				for( int i= 0; i< byte_to_send.Length; i++)
				{
					byte_to_send[ i]= byte.Parse( tokens[i], System.Globalization.NumberStyles.HexNumber);
				}
				if( this.m_tag_type_comboBox.SelectedIndex== 0)
				{
					this.m_RFIDTags[ this.m_inventory_listBox.SelectedIndex].GetSource().WriteTagData( this.m_RFIDTags[ this.m_inventory_listBox.SelectedIndex], (short)add, (short)byte_to_send.Length, byte_to_send);
				}
				else
				{
					this.m_RFIDTags[ this.m_inventory_listBox.SelectedIndex].GetSource().WriteTagData_EPC_C1G2( this.m_RFIDTags[ this.m_inventory_listBox.SelectedIndex], (short)( this.m_tag_type_comboBox.SelectedIndex- 1), (short)add, (short)byte_to_send.Length, byte_to_send);
				}
			}
			catch( Exception excp)
			{
				MessageBox.Show( this, excp.Message, this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void m_asynch_checkBox_CheckedChanged(object sender, System.EventArgs e)
		{
			if( m_asynch_checkBox.Checked)
			{
				try
				{
					if( this.m_RFIDReceiver!= null)
					{
						this.m_RFIDReceiver.Dispose();
						m_RFIDReceiver= null;
					}
					if( this.m_Source0== null)
					{
						MessageBox.Show( this, "Connect, please!", this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
						this.m_asynch_checkBox.Checked= false;
						return;
					}
					this.m_RFIDReceiver= new CAENRFIDReceiver( 3000);
					System.Threading.Thread.Sleep( 100);
					this.m_RFIDReceiver.CAENRFIDEvent+=new CAENRFIDEventHandler(m_RFIDReceiver_CAENRFIDEvent);

					string host = "";
					try 
					{
						this.m_CurrentNotifyTrigger = this.m_RFIDReader.CreateTrigger( "NotifyTrigger1", 100);
					}
					catch (CAENRFIDException e1) 
					{
						if (e1.getError().Equals("RFID: Trigger name exist")) 
						{
							// problably someone kill demo without close connection!!!
							// It's wrong but one could continue.   
						}
						else 
						{
							MessageBox.Show( this, e1.getError(), this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
					}
					// create ReadTrigger
					try 
					{
						this.m_CurrentReadTrigger = this.m_RFIDReader.CreateTrigger( "ReadTrigger1", 1000);
					}
					catch (CAENRFIDException e2) 
					{
						if (e2.getError().Equals("RFID: Trigger name exist")) 
						{
							// problably someone kill demo without close connection!!!
							// It's wrong but one could continue.   
						}
						else 
						{
							try 
							{
								this.m_RFIDReader.RemoveTrigger(this.m_CurrentNotifyTrigger);
							}
							catch (CAENRFIDException e3) 
							{
								MessageBox.Show( this, e3.getError(), this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							return;
						}
					}        
					//create Asynchronous Channel
					try 
					{
						System.Net.IPAddress[] ipAddresses = System.Net.Dns.Resolve(System.Net.Dns.GetHostName()).AddressList;
						if( ipAddresses.Length== 0)
						{
							MessageBox.Show( this, "No address IP found", this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
							try 
							{
								this.m_RFIDReader.RemoveTrigger( this.m_CurrentNotifyTrigger);
							}
							catch (CAENRFIDException e4) 
							{
								MessageBox.Show( this, e4.getError(), this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							try 
							{
								this.m_RFIDReader.RemoveTrigger( this.m_CurrentReadTrigger);
							}
							catch (CAENRFIDException e5) 
							{
								MessageBox.Show( this, e5.getError(), this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							return;
						}
						host= ipAddresses[0].ToString();
						m_CurrentChannel = this.m_RFIDReader.CreateChannel( "NotifyChannel", host, 3000);
					}
					catch (CAENRFIDException e3) 
					{
						if (e3.getError().Equals("RFID: Channel Name exist")) 
						{
							// problably someone kill demo without close connection!!!
							// It's wrong but one could continue.   
						}
						else 
						{
							MessageBox.Show( this, e3.getError(), this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

							try 
							{
								this.m_RFIDReader.RemoveTrigger( this.m_CurrentNotifyTrigger);
							}
							catch (CAENRFIDException e4) 
							{
								MessageBox.Show( this, e4.getError(), this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							try 
							{
								this.m_RFIDReader.RemoveTrigger( this.m_CurrentReadTrigger);
							}
							catch (CAENRFIDException e5) 
							{
								MessageBox.Show( this, e5.getError(), this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							return;
						}
					}
					try 
					{
						this.m_CurrentChannel.AddSource( this.m_Source0);
						this.m_CurrentChannel.AddTrigger(this.m_CurrentNotifyTrigger);
						this.m_Source0.AddTrigger(this.m_CurrentReadTrigger);
					}
					catch (CAENRFIDException e5) 
					{
						MessageBox.Show( this, e5.getError(), this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				catch( Exception excp)
				{
					MessageBox.Show( this, excp.Message, this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				this.do_stop_asynch();
			}
		}
		private void do_stop_asynch( )
		{
			if( this.m_RFIDReceiver!= null)
			{
				try 
				{
					this.m_Source0.RemoveTrigger(this.m_CurrentReadTrigger);
					this.m_CurrentChannel.RemoveTrigger(this.m_CurrentNotifyTrigger);
					this.m_CurrentChannel.RemoveSource(this.m_Source0);
					this.m_RFIDReader.RemoveChannel(this.m_CurrentChannel);
					this.m_RFIDReader.RemoveTrigger(this.m_CurrentNotifyTrigger);
					this.m_RFIDReader.RemoveTrigger(this.m_CurrentReadTrigger);
				}
				catch (CAENRFIDException e3) 
				{
					MessageBox.Show( this, e3.getError(), this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				System.Threading.Thread.Sleep( 2000);
				this.m_RFIDReceiver.Dispose();
				this.m_RFIDReceiver= null;
			}
		}

		private void m_RFIDReceiver_CAENRFIDEvent(object sender, CAENRFIDEventArgs e)
		{
            this.BeginInvoke(this.update_delegate, new object[] { e });
        }
        private void do_update_delegate(CAENRFIDEventArgs e)
        {
            try
			{
				if( this.m_use_event_mode)
				{
					this.ManageEvent( e);
				}
				else
				{
					this.m_inventory_listBox.Items.Clear();
					for( int i= 0; i< e.Data.Length; i++)
					{
						this.m_inventory_listBox.Items.Add( System.BitConverter.ToString( e.Data[ i].TagID));
					}
				}
			}
			catch( Exception excp)
			{
				MessageBox.Show( this, excp.Message, this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void ManageEvent( CAENRFIDEventArgs e) 
		{
			ArrayList p= new ArrayList();
			p.AddRange(  e.Data);
			CAENRFIDNotify noti;
			int length;
			byte [] tag;
			CAENRFIDTagEventType status;
//			string t,t1;
			string t1;
			int total = p.Count;
			for ( int i=0; i< total; i++) 
			{
				t1 = "";
				noti = (CAENRFIDNotify) p[0];
				p.Remove(0);
				tag = noti.TagID;
				status = noti.Status;
				length = noti.TagLength;
				for ( int j = 0; j< length; j++) 
				{
					t1+= ((int)tag[j]).ToString( "X").PadLeft( 2, '0');
//					t = Integer.toHexString((int)tag[j]);
//					if (t.length() > 2) 
//					{
//						t1 += t.substring(t.length()-2, t.length());
//					}
//					else 
//					{
//						if (t.length() == 1) t1 += "0" + t; else t1 += t;
//					}
				}
				if (status == CAENRFIDTagEventType.TAG_GLIMPSED) 
				{
					t1 = t1 + " (GLIMPSED)\n";
					this.m_inventory_listBox.Items.Add( t1.ToUpper());
				}
				if (status == CAENRFIDTagEventType.TAG_OBSERVED) 
				{
					this.m_inventory_listBox.Items.Remove( ((string)(t1 + " (GLIMPSED)\n")).ToUpper());
					t1 = t1 + " (OBSERVED)\n";
					this.m_inventory_listBox.Items.Add( t1.ToUpper());
				}
				if (status == CAENRFIDTagEventType.TAG_LOST) 
				{
					this.m_inventory_listBox.Items.Remove( ((string)(t1 + " (OBSERVED)\n")).ToUpper());
					this.m_inventory_listBox.Items.Remove( ((string)(t1 + " (GLIMPSED)\n")).ToUpper());
					t1 = t1 + " (LOST)\n";
				}
			}
		}
	}
}
