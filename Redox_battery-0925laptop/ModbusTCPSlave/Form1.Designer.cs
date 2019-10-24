using System.Reflection;

namespace ModbusTCPSlave
{
    public class Control
    {

        protected virtual bool DoubleBuffered { get; set; }
    }
    public static class Extensions
    {
        public static void DoubleBuffered(this Control control, bool enabled)
        {
            var prop = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            prop.SetValue(control, enabled, null);
        }
    }


    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code
        ///???????
        //protected override bool DoubleBuffered { get; set; }



        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labServerName = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.listView2 = new System.Windows.Forms.ListView();
            this.textBox_addr = new System.Windows.Forms.TextBox();
            this.textBox_value = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Button_write = new System.Windows.Forms.Button();
            this.listView3 = new System.Windows.Forms.ListView();
            this.listView4 = new System.Windows.Forms.ListView();
            this.listView1 = new System.Windows.Forms.ListView();
            this.timer_display_flag = new System.Windows.Forms.Timer(this.components);
            this.label_flag = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lb41241b0 = new System.Windows.Forms.Label();
            this.lb41241b1 = new System.Windows.Forms.Label();
            this.lb41241b2 = new System.Windows.Forms.Label();
            this.lb41241b3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lb41201b3 = new System.Windows.Forms.Label();
            this.lb41201b2 = new System.Windows.Forms.Label();
            this.lb41201b1 = new System.Windows.Forms.Label();
            this.lb41201b0 = new System.Windows.Forms.Label();
            this.lb41201b7 = new System.Windows.Forms.Label();
            this.lb41201b6 = new System.Windows.Forms.Label();
            this.lb41201b5 = new System.Windows.Forms.Label();
            this.lb41201b4 = new System.Windows.Forms.Label();
            this.lb41201b8 = new System.Windows.Forms.Label();
            this.timer_status = new System.Windows.Forms.Timer(this.components);
            this.label_status = new System.Windows.Forms.Label();
            this.listView5 = new System.Windows.Forms.ListView();
            this.bt_EMS_stop = new System.Windows.Forms.Button();
            this.bt_EMS_start = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.timer_rtu = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lb_mode = new System.Windows.Forms.Label();
            this.lb_trip = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label_41001 = new System.Windows.Forms.Label();
            this.bt_test = new System.Windows.Forms.Button();
            this.lb_test = new System.Windows.Forms.Label();
            this.bt_PCSreset = new System.Windows.Forms.Button();
            this.bt_reset_clr = new System.Windows.Forms.Button();
            this.bt_BlackStart = new System.Windows.Forms.Button();
            this.timer_wdt = new System.Windows.Forms.Timer(this.components);
            this.lb_tt3 = new System.Windows.Forms.Label();
            this.timer_comm = new System.Windows.Forms.Timer(this.components);
            this.lb_check_wdt = new System.Windows.Forms.Label();
            this.lb_Program_error = new System.Windows.Forms.Label();
            this.lb41001b1 = new System.Windows.Forms.Label();
            this.label41001 = new System.Windows.Forms.Label();
            this.lb41001b2 = new System.Windows.Forms.Label();
            this.lb41001b3 = new System.Windows.Forms.Label();
            this.lb41001b4 = new System.Windows.Forms.Label();
            this.lb41001b5 = new System.Windows.Forms.Label();
            this.lb41001b6 = new System.Windows.Forms.Label();
            this.lb41001b7 = new System.Windows.Forms.Label();
            this.lb41001b8 = new System.Windows.Forms.Label();
            this.lb41001b9 = new System.Windows.Forms.Label();
            this.lb41001b10 = new System.Windows.Forms.Label();
            this.lb41001b11 = new System.Windows.Forms.Label();
            this.lb41001b12 = new System.Windows.Forms.Label();
            this.lb41001b13 = new System.Windows.Forms.Label();
            this.label41002 = new System.Windows.Forms.Label();
            this.lb41002b2 = new System.Windows.Forms.Label();
            this.lb41002b5 = new System.Windows.Forms.Label();
            this.lb41002b6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labServerName
            // 
            this.labServerName.Location = new System.Drawing.Point(167, 9);
            this.labServerName.Name = "labServerName";
            this.labServerName.Size = new System.Drawing.Size(161, 28);
            this.labServerName.TabIndex = 41;
            this.labServerName.Text = "label6";
            this.labServerName.Click += new System.EventHandler(this.LabServerName_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("新細明體", 14F);
            this.btnStop.Location = new System.Drawing.Point(437, 9);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(98, 28);
            this.btnStop.TabIndex = 59;
            this.btnStop.Text = "stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("新細明體", 14F);
            this.btnStart.Location = new System.Drawing.Point(333, 9);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(98, 28);
            this.btnStart.TabIndex = 58;
            this.btnStart.Text = "start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // listView2
            // 
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(1059, 8);
            this.listView2.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(110, 496);
            this.listView2.TabIndex = 66;
            this.listView2.UseCompatibleStateImageBehavior = false;
            // 
            // textBox_addr
            // 
            this.textBox_addr.Location = new System.Drawing.Point(70, 49);
            this.textBox_addr.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBox_addr.Name = "textBox_addr";
            this.textBox_addr.Size = new System.Drawing.Size(68, 22);
            this.textBox_addr.TabIndex = 67;
            this.textBox_addr.Text = "41201";
            this.textBox_addr.TextChanged += new System.EventHandler(this.TextBox_addr_TextChanged);
            // 
            // textBox_value
            // 
            this.textBox_value.Location = new System.Drawing.Point(182, 49);
            this.textBox_value.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBox_value.Name = "textBox_value";
            this.textBox_value.Size = new System.Drawing.Size(37, 22);
            this.textBox_value.TabIndex = 68;
            this.textBox_value.Text = "48";
            this.textBox_value.TextChanged += new System.EventHandler(this.TextBox_value_TextChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(21, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 21);
            this.label6.TabIndex = 69;
            this.label6.Text = "address";
            this.label6.Click += new System.EventHandler(this.Label6_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(142, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 21);
            this.label7.TabIndex = 70;
            this.label7.Text = "value";
            this.label7.Click += new System.EventHandler(this.Label7_Click);
            // 
            // Button_write
            // 
            this.Button_write.Location = new System.Drawing.Point(223, 45);
            this.Button_write.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.Button_write.Name = "Button_write";
            this.Button_write.Size = new System.Drawing.Size(63, 28);
            this.Button_write.TabIndex = 71;
            this.Button_write.Text = "write";
            this.Button_write.Click += new System.EventHandler(this.Button_write_Click);
            // 
            // listView3
            // 
            this.listView3.HideSelection = false;
            this.listView3.Location = new System.Drawing.Point(941, 8);
            this.listView3.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(114, 496);
            this.listView3.TabIndex = 72;
            this.listView3.UseCompatibleStateImageBehavior = false;
            // 
            // listView4
            // 
            this.listView4.HideSelection = false;
            this.listView4.Location = new System.Drawing.Point(1173, 8);
            this.listView4.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.listView4.Name = "listView4";
            this.listView4.Size = new System.Drawing.Size(110, 496);
            this.listView4.TabIndex = 73;
            this.listView4.UseCompatibleStateImageBehavior = false;
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(822, 8);
            this.listView1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(115, 496);
            this.listView1.TabIndex = 65;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.ListView1_SelectedIndexChanged);
            // 
            // timer_display_flag
            // 
            this.timer_display_flag.Enabled = true;
            this.timer_display_flag.Interval = 800;
            this.timer_display_flag.Tick += new System.EventHandler(this.timer_display_flag_Tick);
            // 
            // label_flag
            // 
            this.label_flag.AutoSize = true;
            this.label_flag.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_flag.Font = new System.Drawing.Font("新細明體", 18F);
            this.label_flag.Location = new System.Drawing.Point(21, 112);
            this.label_flag.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_flag.Name = "label_flag";
            this.label_flag.Size = new System.Drawing.Size(47, 26);
            this.label_flag.TabIndex = 76;
            this.label_flag.Text = "flag";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(313, 112);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 14);
            this.label1.TabIndex = 77;
            this.label1.Text = "Project specific status 41241";
            // 
            // lb41241b0
            // 
            this.lb41241b0.AutoSize = true;
            this.lb41241b0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41241b0.Location = new System.Drawing.Point(321, 133);
            this.lb41241b0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41241b0.Name = "lb41241b0";
            this.lb41241b0.Size = new System.Drawing.Size(93, 14);
            this.lb41241b0.TabIndex = 78;
            this.lb41241b0.Text = "b0: BMS mode on";
            // 
            // lb41241b1
            // 
            this.lb41241b1.AutoSize = true;
            this.lb41241b1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41241b1.Location = new System.Drawing.Point(321, 152);
            this.lb41241b1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41241b1.Name = "lb41241b1";
            this.lb41241b1.Size = new System.Drawing.Size(146, 14);
            this.lb41241b1.TabIndex = 79;
            this.lb41241b1.Text = "b1: Initial charge sequence on";
            // 
            // lb41241b2
            // 
            this.lb41241b2.AutoSize = true;
            this.lb41241b2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41241b2.Location = new System.Drawing.Point(321, 173);
            this.lb41241b2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41241b2.Name = "lb41241b2";
            this.lb41241b2.Size = new System.Drawing.Size(75, 14);
            this.lb41241b2.TabIndex = 80;
            this.lb41241b2.Text = "b2: Pump Run";
            // 
            // lb41241b3
            // 
            this.lb41241b3.AutoSize = true;
            this.lb41241b3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41241b3.Location = new System.Drawing.Point(321, 197);
            this.lb41241b3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41241b3.Name = "lb41241b3";
            this.lb41241b3.Size = new System.Drawing.Size(73, 14);
            this.lb41241b3.TabIndex = 81;
            this.lb41241b3.Text = "b3: BMS reset";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(317, 221);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 14);
            this.label2.TabIndex = 83;
            this.label2.Text = "BMS_Status word 41201";
            // 
            // lb41201b3
            // 
            this.lb41201b3.AutoSize = true;
            this.lb41201b3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41201b3.Location = new System.Drawing.Point(321, 304);
            this.lb41201b3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41201b3.Name = "lb41201b3";
            this.lb41201b3.Size = new System.Drawing.Size(128, 14);
            this.lb41201b3.TabIndex = 87;
            this.lb41201b3.Text = "b3: Alarm (Battery alarm)";
            // 
            // lb41201b2
            // 
            this.lb41201b2.AutoSize = true;
            this.lb41201b2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41201b2.Location = new System.Drawing.Point(321, 283);
            this.lb41201b2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41201b2.Name = "lb41201b2";
            this.lb41201b2.Size = new System.Drawing.Size(114, 14);
            this.lb41201b2.TabIndex = 86;
            this.lb41201b2.Text = "b2: Trip (Battery fault)";
            this.lb41201b2.Click += new System.EventHandler(this.Lb41201b2_Click);
            // 
            // lb41201b1
            // 
            this.lb41201b1.AutoSize = true;
            this.lb41201b1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41201b1.Location = new System.Drawing.Point(321, 261);
            this.lb41201b1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41201b1.Name = "lb41201b1";
            this.lb41201b1.Size = new System.Drawing.Size(120, 14);
            this.lb41201b1.TabIndex = 85;
            this.lb41201b1.Text = "b1: BMS mode (Online)";
            this.lb41201b1.Click += new System.EventHandler(this.Lb41201b1_Click);
            // 
            // lb41201b0
            // 
            this.lb41201b0.AutoSize = true;
            this.lb41201b0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41201b0.Location = new System.Drawing.Point(321, 241);
            this.lb41201b0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41201b0.Name = "lb41201b0";
            this.lb41201b0.Size = new System.Drawing.Size(220, 14);
            this.lb41201b0.TabIndex = 84;
            this.lb41201b0.Text = "b0: EMS or BC or PCS mode Ready (Online)";
            this.lb41201b0.Click += new System.EventHandler(this.Lb41201b0_Click);
            // 
            // lb41201b7
            // 
            this.lb41201b7.AutoSize = true;
            this.lb41201b7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41201b7.Location = new System.Drawing.Point(321, 389);
            this.lb41201b7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41201b7.Name = "lb41201b7";
            this.lb41201b7.Size = new System.Drawing.Size(223, 14);
            this.lb41201b7.TabIndex = 91;
            this.lb41201b7.Text = "b7: Discharge completion (Can not Discharge)";
            this.lb41201b7.Click += new System.EventHandler(this.Lb41201b7_Click);
            // 
            // lb41201b6
            // 
            this.lb41201b6.AutoSize = true;
            this.lb41201b6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41201b6.Location = new System.Drawing.Point(321, 368);
            this.lb41201b6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41201b6.Name = "lb41201b6";
            this.lb41201b6.Size = new System.Drawing.Size(199, 14);
            this.lb41201b6.TabIndex = 90;
            this.lb41201b6.Text = "b6: Charge completion (Can not Charge)";
            // 
            // lb41201b5
            // 
            this.lb41201b5.AutoSize = true;
            this.lb41201b5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41201b5.Location = new System.Drawing.Point(321, 345);
            this.lb41201b5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41201b5.Name = "lb41201b5";
            this.lb41201b5.Size = new System.Drawing.Size(134, 14);
            this.lb41201b5.TabIndex = 89;
            this.lb41201b5.Text = "b5: Permission to operation";
            // 
            // lb41201b4
            // 
            this.lb41201b4.AutoSize = true;
            this.lb41201b4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41201b4.Location = new System.Drawing.Point(321, 325);
            this.lb41201b4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41201b4.Name = "lb41201b4";
            this.lb41201b4.Size = new System.Drawing.Size(92, 14);
            this.lb41201b4.TabIndex = 88;
            this.lb41201b4.Text = "b4: Pump running";
            // 
            // lb41201b8
            // 
            this.lb41201b8.AutoSize = true;
            this.lb41201b8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41201b8.Location = new System.Drawing.Point(321, 409);
            this.lb41201b8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41201b8.Name = "lb41201b8";
            this.lb41201b8.Size = new System.Drawing.Size(187, 14);
            this.lb41201b8.TabIndex = 92;
            this.lb41201b8.Text = "b8:  [Reserved] Initial precharge ready";
            // 
            // timer_status
            // 
            this.timer_status.Interval = 1000;
            this.timer_status.Tick += new System.EventHandler(this.timer_status_Tick);
            // 
            // label_status
            // 
            this.label_status.Font = new System.Drawing.Font("新細明體", 18F);
            this.label_status.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label_status.Location = new System.Drawing.Point(20, 13);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(140, 33);
            this.label_status.TabIndex = 93;
            this.label_status.Text = "status";
            // 
            // listView5
            // 
            this.listView5.HideSelection = false;
            this.listView5.Location = new System.Drawing.Point(1287, 8);
            this.listView5.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.listView5.Name = "listView5";
            this.listView5.Size = new System.Drawing.Size(110, 496);
            this.listView5.TabIndex = 94;
            this.listView5.UseCompatibleStateImageBehavior = false;
            // 
            // bt_EMS_stop
            // 
            this.bt_EMS_stop.BackColor = System.Drawing.Color.Salmon;
            this.bt_EMS_stop.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bt_EMS_stop.Location = new System.Drawing.Point(295, 49);
            this.bt_EMS_stop.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.bt_EMS_stop.Name = "bt_EMS_stop";
            this.bt_EMS_stop.Size = new System.Drawing.Size(87, 28);
            this.bt_EMS_stop.TabIndex = 95;
            this.bt_EMS_stop.Text = "EMS_PCSstop";
            this.bt_EMS_stop.UseVisualStyleBackColor = false;
            this.bt_EMS_stop.Click += new System.EventHandler(this.bt_EMS_stop_Click);
            // 
            // bt_EMS_start
            // 
            this.bt_EMS_start.BackColor = System.Drawing.Color.Salmon;
            this.bt_EMS_start.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bt_EMS_start.Location = new System.Drawing.Point(388, 49);
            this.bt_EMS_start.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.bt_EMS_start.Name = "bt_EMS_start";
            this.bt_EMS_start.Size = new System.Drawing.Size(85, 28);
            this.bt_EMS_start.TabIndex = 96;
            this.bt_EMS_start.Text = "EMS_PCSstart";
            this.bt_EMS_start.UseVisualStyleBackColor = false;
            this.bt_EMS_start.Click += new System.EventHandler(this.bt_EMS_start_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Orange;
            this.button3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button3.Location = new System.Drawing.Point(295, 81);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(58, 19);
            this.button3.TabIndex = 97;
            this.button3.Text = "grid fault";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // timer_rtu
            // 
            this.timer_rtu.Tick += new System.EventHandler(this.Timer_rtu_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(346, 489);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(88, 22);
            this.textBox1.TabIndex = 98;
            this.textBox1.Text = "rtu_write_2_55";
            this.textBox1.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // lb_mode
            // 
            this.lb_mode.AutoSize = true;
            this.lb_mode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_mode.Location = new System.Drawing.Point(8, 469);
            this.lb_mode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_mode.Name = "lb_mode";
            this.lb_mode.Size = new System.Drawing.Size(69, 14);
            this.lb_mode.TabIndex = 99;
            this.lb_mode.Text = "control mode";
            // 
            // lb_trip
            // 
            this.lb_trip.AutoSize = true;
            this.lb_trip.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_trip.Font = new System.Drawing.Font("新細明體", 14F);
            this.lb_trip.Location = new System.Drawing.Point(8, 489);
            this.lb_trip.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_trip.Name = "lb_trip";
            this.lb_trip.Size = new System.Drawing.Size(88, 21);
            this.lb_trip.TabIndex = 100;
            this.lb_trip.Text = "trip reason";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Orange;
            this.button4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button4.Location = new System.Drawing.Point(356, 81);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(58, 19);
            this.button4.TabIndex = 101;
            this.button4.Text = "pcs fault";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // label_41001
            // 
            this.label_41001.AutoSize = true;
            this.label_41001.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_41001.Font = new System.Drawing.Font("新細明體", 12F);
            this.label_41001.Location = new System.Drawing.Point(8, 421);
            this.label_41001.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_41001.Name = "label_41001";
            this.label_41001.Size = new System.Drawing.Size(34, 18);
            this.label_41001.TabIndex = 102;
            this.label_41001.Text = "flag";
            // 
            // bt_test
            // 
            this.bt_test.Location = new System.Drawing.Point(277, 489);
            this.bt_test.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.bt_test.Name = "bt_test";
            this.bt_test.Size = new System.Drawing.Size(63, 28);
            this.bt_test.TabIndex = 103;
            this.bt_test.Text = "test";
            this.bt_test.Click += new System.EventHandler(this.bt_test_Click);
            // 
            // lb_test
            // 
            this.lb_test.AutoSize = true;
            this.lb_test.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_test.Font = new System.Drawing.Font("新細明體", 14F);
            this.lb_test.Location = new System.Drawing.Point(346, 465);
            this.lb_test.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_test.Name = "lb_test";
            this.lb_test.Size = new System.Drawing.Size(59, 21);
            this.lb_test.TabIndex = 104;
            this.lb_test.Text = "lb_test";
            // 
            // bt_PCSreset
            // 
            this.bt_PCSreset.BackColor = System.Drawing.Color.Salmon;
            this.bt_PCSreset.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bt_PCSreset.Location = new System.Drawing.Point(479, 49);
            this.bt_PCSreset.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.bt_PCSreset.Name = "bt_PCSreset";
            this.bt_PCSreset.Size = new System.Drawing.Size(58, 28);
            this.bt_PCSreset.TabIndex = 105;
            this.bt_PCSreset.Text = "PCSreset";
            this.bt_PCSreset.UseVisualStyleBackColor = false;
            this.bt_PCSreset.Click += new System.EventHandler(this.Bt_PCSreset_Click);
            // 
            // bt_reset_clr
            // 
            this.bt_reset_clr.BackColor = System.Drawing.Color.Salmon;
            this.bt_reset_clr.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bt_reset_clr.Location = new System.Drawing.Point(479, 81);
            this.bt_reset_clr.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.bt_reset_clr.Name = "bt_reset_clr";
            this.bt_reset_clr.Size = new System.Drawing.Size(58, 28);
            this.bt_reset_clr.TabIndex = 106;
            this.bt_reset_clr.Text = "reset_clr";
            this.bt_reset_clr.UseVisualStyleBackColor = false;
            this.bt_reset_clr.Click += new System.EventHandler(this.bt_reset_clr_Click);
            // 
            // bt_BlackStart
            // 
            this.bt_BlackStart.BackColor = System.Drawing.Color.Salmon;
            this.bt_BlackStart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bt_BlackStart.Location = new System.Drawing.Point(479, 119);
            this.bt_BlackStart.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.bt_BlackStart.Name = "bt_BlackStart";
            this.bt_BlackStart.Size = new System.Drawing.Size(94, 19);
            this.bt_BlackStart.TabIndex = 107;
            this.bt_BlackStart.Text = "BlackStart_iso";
            this.bt_BlackStart.UseVisualStyleBackColor = false;
            this.bt_BlackStart.Click += new System.EventHandler(this.Bt_BlackStart_Click);
            // 
            // timer_wdt
            // 
            this.timer_wdt.Interval = 1000;
            this.timer_wdt.Tick += new System.EventHandler(this.Timer_wdt_Tick);
            // 
            // lb_tt3
            // 
            this.lb_tt3.AutoSize = true;
            this.lb_tt3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_tt3.Font = new System.Drawing.Font("新細明體", 14F);
            this.lb_tt3.Location = new System.Drawing.Point(503, 463);
            this.lb_tt3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_tt3.Name = "lb_tt3";
            this.lb_tt3.Size = new System.Drawing.Size(53, 21);
            this.lb_tt3.TabIndex = 108;
            this.lb_tt3.Text = "lb_tt3";
            // 
            // timer_comm
            // 
            this.timer_comm.Interval = 5000;
            this.timer_comm.Tick += new System.EventHandler(this.Timer_comm_Tick);
            // 
            // lb_check_wdt
            // 
            this.lb_check_wdt.AutoSize = true;
            this.lb_check_wdt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_check_wdt.Font = new System.Drawing.Font("新細明體", 14F);
            this.lb_check_wdt.Location = new System.Drawing.Point(491, 487);
            this.lb_check_wdt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_check_wdt.Name = "lb_check_wdt";
            this.lb_check_wdt.Size = new System.Drawing.Size(89, 21);
            this.lb_check_wdt.TabIndex = 109;
            this.lb_check_wdt.Text = "check_wdt";
            this.lb_check_wdt.Click += new System.EventHandler(this.Lb_check_wdt_Click);
            // 
            // lb_Program_error
            // 
            this.lb_Program_error.AutoSize = true;
            this.lb_Program_error.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_Program_error.Font = new System.Drawing.Font("新細明體", 14F);
            this.lb_Program_error.Location = new System.Drawing.Point(631, 509);
            this.lb_Program_error.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_Program_error.Name = "lb_Program_error";
            this.lb_Program_error.Size = new System.Drawing.Size(117, 21);
            this.lb_Program_error.TabIndex = 110;
            this.lb_Program_error.Text = "Program_error";
            // 
            // lb41001b1
            // 
            this.lb41001b1.AutoSize = true;
            this.lb41001b1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b1.Location = new System.Drawing.Point(617, 122);
            this.lb41001b1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b1.Name = "lb41001b1";
            this.lb41001b1.Size = new System.Drawing.Size(90, 14);
            this.lb41001b1.TabIndex = 111;
            this.lb41001b1.Text = "b1:Local_control ";
            this.lb41001b1.Click += new System.EventHandler(this.Lb40001b1_Click);
            // 
            // label41001
            // 
            this.label41001.AutoSize = true;
            this.label41001.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label41001.Location = new System.Drawing.Point(610, 108);
            this.label41001.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label41001.Name = "label41001";
            this.label41001.Size = new System.Drawing.Size(138, 14);
            this.label41001.TabIndex = 112;
            this.label41001.Text = "Project specific status 41001";
            this.label41001.Click += new System.EventHandler(this.Label3_Click);
            // 
            // lb41001b2
            // 
            this.lb41001b2.AutoSize = true;
            this.lb41001b2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b2.Location = new System.Drawing.Point(617, 136);
            this.lb41001b2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b2.Name = "lb41001b2";
            this.lb41001b2.Size = new System.Drawing.Size(100, 14);
            this.lb41001b2.TabIndex = 113;
            this.lb41001b2.Text = "b2:Remote_control ";
            // 
            // lb41001b3
            // 
            this.lb41001b3.AutoSize = true;
            this.lb41001b3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b3.Location = new System.Drawing.Point(617, 152);
            this.lb41001b3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b3.Name = "lb41001b3";
            this.lb41001b3.Size = new System.Drawing.Size(101, 14);
            this.lb41001b3.TabIndex = 114;
            this.lb41001b3.Text = "b3:Off_site_control ";
            // 
            // lb41001b4
            // 
            this.lb41001b4.AutoSize = true;
            this.lb41001b4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b4.Location = new System.Drawing.Point(617, 166);
            this.lb41001b4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b4.Name = "lb41001b4";
            this.lb41001b4.Size = new System.Drawing.Size(137, 14);
            this.lb41001b4.TabIndex = 115;
            this.lb41001b4.Text = "b4:Commissioning_control ";
            // 
            // lb41001b5
            // 
            this.lb41001b5.AutoSize = true;
            this.lb41001b5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b5.Location = new System.Drawing.Point(617, 180);
            this.lb41001b5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b5.Name = "lb41001b5";
            this.lb41001b5.Size = new System.Drawing.Size(52, 14);
            this.lb41001b5.TabIndex = 116;
            this.lb41001b5.Text = "b5:Ready";
            this.lb41001b5.Click += new System.EventHandler(this.Label5_Click);
            // 
            // lb41001b6
            // 
            this.lb41001b6.AutoSize = true;
            this.lb41001b6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b6.Location = new System.Drawing.Point(617, 194);
            this.lb41001b6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b6.Name = "lb41001b6";
            this.lb41001b6.Size = new System.Drawing.Size(116, 14);
            this.lb41001b6.TabIndex = 117;
            this.lb41001b6.Text = "b6:AC_Breaker_closed";
            // 
            // lb41001b7
            // 
            this.lb41001b7.AutoSize = true;
            this.lb41001b7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b7.Location = new System.Drawing.Point(617, 208);
            this.lb41001b7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b7.Name = "lb41001b7";
            this.lb41001b7.Size = new System.Drawing.Size(53, 14);
            this.lb41001b7.TabIndex = 118;
            this.lb41001b7.Text = "b7:Online";
            // 
            // lb41001b8
            // 
            this.lb41001b8.AutoSize = true;
            this.lb41001b8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b8.Location = new System.Drawing.Point(617, 222);
            this.lb41001b8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b8.Name = "lb41001b8";
            this.lb41001b8.Size = new System.Drawing.Size(60, 14);
            this.lb41001b8.TabIndex = 119;
            this.lb41001b8.Text = "b8:Standby";
            this.lb41001b8.Click += new System.EventHandler(this.Label10_Click);
            // 
            // lb41001b9
            // 
            this.lb41001b9.AutoSize = true;
            this.lb41001b9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b9.Location = new System.Drawing.Point(617, 236);
            this.lb41001b9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b9.Name = "lb41001b9";
            this.lb41001b9.Size = new System.Drawing.Size(51, 14);
            this.lb41001b9.TabIndex = 120;
            this.lb41001b9.Text = "b9:Alarm";
            // 
            // lb41001b10
            // 
            this.lb41001b10.AutoSize = true;
            this.lb41001b10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b10.Location = new System.Drawing.Point(617, 250);
            this.lb41001b10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b10.Name = "lb41001b10";
            this.lb41001b10.Size = new System.Drawing.Size(117, 14);
            this.lb41001b10.TabIndex = 121;
            this.lb41001b10.Text = "b10:Partial_battery_trip";
            // 
            // lb41001b11
            // 
            this.lb41001b11.AutoSize = true;
            this.lb41001b11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b11.Location = new System.Drawing.Point(617, 264);
            this.lb41001b11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b11.Name = "lb41001b11";
            this.lb41001b11.Size = new System.Drawing.Size(70, 14);
            this.lb41001b11.TabIndex = 122;
            this.lb41001b11.Text = "b11:PCS_trip";
            // 
            // lb41001b12
            // 
            this.lb41001b12.AutoSize = true;
            this.lb41001b12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b12.Location = new System.Drawing.Point(617, 278);
            this.lb41001b12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b12.Name = "lb41001b12";
            this.lb41001b12.Size = new System.Drawing.Size(48, 14);
            this.lb41001b12.TabIndex = 123;
            this.lb41001b12.Text = "b12:Trip";
            // 
            // lb41001b13
            // 
            this.lb41001b13.AutoSize = true;
            this.lb41001b13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b13.Location = new System.Drawing.Point(617, 292);
            this.lb41001b13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b13.Name = "lb41001b13";
            this.lb41001b13.Size = new System.Drawing.Size(76, 14);
            this.lb41001b13.TabIndex = 124;
            this.lb41001b13.Text = "b13:Grid_fault";
            // 
            // label41002
            // 
            this.label41002.AutoSize = true;
            this.label41002.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label41002.Location = new System.Drawing.Point(617, 325);
            this.label41002.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label41002.Name = "label41002";
            this.label41002.Size = new System.Drawing.Size(138, 14);
            this.label41002.TabIndex = 125;
            this.label41002.Text = "Project specific status 41002\r\n";
            this.label41002.Click += new System.EventHandler(this.Label3_Click_1);
            // 
            // lb41002b2
            // 
            this.lb41002b2.AutoSize = true;
            this.lb41002b2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41002b2.Location = new System.Drawing.Point(617, 339);
            this.lb41002b2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41002b2.Name = "lb41002b2";
            this.lb41002b2.Size = new System.Drawing.Size(43, 14);
            this.lb41002b2.TabIndex = 126;
            this.lb41002b2.Text = "b2:CSI ";
            // 
            // lb41002b5
            // 
            this.lb41002b5.AutoSize = true;
            this.lb41002b5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41002b5.Location = new System.Drawing.Point(617, 353);
            this.lb41002b5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41002b5.Name = "lb41002b5";
            this.lb41002b5.Size = new System.Drawing.Size(166, 14);
            this.lb41002b5.TabIndex = 127;
            this.lb41002b5.Text = "b5:Operation_islanding_condition";
            // 
            // lb41002b6
            // 
            this.lb41002b6.AutoSize = true;
            this.lb41002b6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41002b6.Location = new System.Drawing.Point(617, 368);
            this.lb41002b6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41002b6.Name = "lb41002b6";
            this.lb41002b6.Size = new System.Drawing.Size(116, 14);
            this.lb41002b6.TabIndex = 128;
            this.lb41002b6.Text = "b6:Black_start_enabled";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1403, 564);
            this.Controls.Add(this.lb41002b6);
            this.Controls.Add(this.lb41002b5);
            this.Controls.Add(this.lb41002b2);
            this.Controls.Add(this.label41002);
            this.Controls.Add(this.lb41001b13);
            this.Controls.Add(this.lb41001b12);
            this.Controls.Add(this.lb41001b11);
            this.Controls.Add(this.lb41001b10);
            this.Controls.Add(this.lb41001b9);
            this.Controls.Add(this.lb41001b8);
            this.Controls.Add(this.lb41001b7);
            this.Controls.Add(this.lb41001b6);
            this.Controls.Add(this.lb41001b5);
            this.Controls.Add(this.lb41001b4);
            this.Controls.Add(this.lb41001b3);
            this.Controls.Add(this.lb41001b2);
            this.Controls.Add(this.label41001);
            this.Controls.Add(this.lb41001b1);
            this.Controls.Add(this.lb_Program_error);
            this.Controls.Add(this.lb_check_wdt);
            this.Controls.Add(this.lb_tt3);
            this.Controls.Add(this.bt_BlackStart);
            this.Controls.Add(this.bt_reset_clr);
            this.Controls.Add(this.bt_PCSreset);
            this.Controls.Add(this.lb_test);
            this.Controls.Add(this.bt_test);
            this.Controls.Add(this.label_41001);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.lb_trip);
            this.Controls.Add(this.lb_mode);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.bt_EMS_start);
            this.Controls.Add(this.bt_EMS_stop);
            this.Controls.Add(this.listView5);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.lb41201b8);
            this.Controls.Add(this.lb41201b7);
            this.Controls.Add(this.lb41201b6);
            this.Controls.Add(this.lb41201b5);
            this.Controls.Add(this.lb41201b4);
            this.Controls.Add(this.lb41201b3);
            this.Controls.Add(this.lb41201b2);
            this.Controls.Add(this.lb41201b1);
            this.Controls.Add(this.lb41201b0);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lb41241b3);
            this.Controls.Add(this.lb41241b2);
            this.Controls.Add(this.lb41241b1);
            this.Controls.Add(this.lb41241b0);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_flag);
            this.Controls.Add(this.listView4);
            this.Controls.Add(this.listView3);
            this.Controls.Add(this.Button_write);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_value);
            this.Controls.Add(this.textBox_addr);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.labServerName);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.Name = "Form1";
            this.Text = " ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labServerName;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.TextBox textBox_addr;
        private System.Windows.Forms.TextBox textBox_value;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button Button_write;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ListView listView4;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label_flag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer_display_flag;
        private System.Windows.Forms.Label lb41241b0;
        private System.Windows.Forms.Label lb41241b1;
        private System.Windows.Forms.Label lb41241b2;
        private System.Windows.Forms.Label lb41241b3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb41201b3;
        private System.Windows.Forms.Label lb41201b2;
        private System.Windows.Forms.Label lb41201b1;
        private System.Windows.Forms.Label lb41201b0;
        private System.Windows.Forms.Label lb41201b7;
        private System.Windows.Forms.Label lb41201b6;
        private System.Windows.Forms.Label lb41201b5;
        private System.Windows.Forms.Label lb41201b4;
        private System.Windows.Forms.Label lb41201b8;
        private System.Windows.Forms.Timer timer_status;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.ListView listView5;
        private System.Windows.Forms.Button bt_EMS_stop;
        private System.Windows.Forms.Button bt_EMS_start;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Timer timer_rtu;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lb_mode;
        private System.Windows.Forms.Label lb_trip;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label_41001;
        private System.Windows.Forms.Button bt_test;
        private System.Windows.Forms.Label lb_test;
        private System.Windows.Forms.Button bt_PCSreset;
        private System.Windows.Forms.Button bt_reset_clr;
        private System.Windows.Forms.Button bt_BlackStart;
        private System.Windows.Forms.Timer timer_wdt;
        private System.Windows.Forms.Label lb_tt3;
        private System.Windows.Forms.Timer timer_comm;
        private System.Windows.Forms.Label lb_check_wdt;
        private System.Windows.Forms.Label lb_Program_error;
        private System.Windows.Forms.Label lb41001b1;
        private System.Windows.Forms.Label label41001;
        private System.Windows.Forms.Label lb41001b2;
        private System.Windows.Forms.Label lb41001b3;
        private System.Windows.Forms.Label lb41001b4;
        private System.Windows.Forms.Label lb41001b5;
        private System.Windows.Forms.Label lb41001b6;
        private System.Windows.Forms.Label lb41001b7;
        private System.Windows.Forms.Label lb41001b8;
        private System.Windows.Forms.Label lb41001b9;
        private System.Windows.Forms.Label lb41001b10;
        private System.Windows.Forms.Label lb41001b11;
        private System.Windows.Forms.Label lb41001b12;
        private System.Windows.Forms.Label lb41001b13;
        private System.Windows.Forms.Label label41002;
        private System.Windows.Forms.Label lb41002b2;
        private System.Windows.Forms.Label lb41002b5;
        private System.Windows.Forms.Label lb41002b6;
    }
}

