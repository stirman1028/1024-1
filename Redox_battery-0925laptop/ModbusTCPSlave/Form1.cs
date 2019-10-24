using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Modbus;
using Modbus.Device;
using Modbus.Data;
using Modbus.Message;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;
using System.Drawing;
using System.Diagnostics;//debug msg在『輸出』視窗觀看
using System.IO.Ports;  //for serial port
namespace ModbusTCPSlave
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitialListView();
        }
        #region listview
        private void InitialListView()//初始化ListView的格式大小 
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.LabelEdit = false;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("address", 50);
            listView1.Columns.Add("value", 50);
            listView2.View = View.Details;
            listView2.GridLines = true;
            listView2.LabelEdit = false;
            listView2.FullRowSelect = true;
            listView2.Columns.Add("address", 50);
            listView2.Columns.Add("value", 50);
            listView3.View = View.Details;
            listView3.GridLines = true;
            listView3.LabelEdit = false;
            listView3.FullRowSelect = true;
            listView3.Columns.Add("address", 50);
            listView3.Columns.Add("value", 50);
            listView4.View = View.Details;
            listView4.GridLines = true;
            listView4.LabelEdit = false;
            listView4.FullRowSelect = true;
            listView4.Columns.Add("address", 50);
            listView4.Columns.Add("value", 50);
            listView5.View = View.Details;
            listView5.GridLines = true;
            listView5.LabelEdit = false;
            listView5.FullRowSelect = true;
            listView5.Columns.Add("address", 50);
            listView5.Columns.Add("value", 50);
            /// 雙緩衝
            listView3.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
| System.Reflection.BindingFlags.NonPublic).SetValue(listView3, true, null);
            listView1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
    | System.Reflection.BindingFlags.NonPublic).SetValue(listView1, true, null);
            listView2.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
    | System.Reflection.BindingFlags.NonPublic).SetValue(listView2, true, null);
            listView4.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
| System.Reflection.BindingFlags.NonPublic).SetValue(listView4, true, null);
            listView5.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
| System.Reflection.BindingFlags.NonPublic).SetValue(listView5, true, null);
        }
        public delegate void Listview_Print(ListView list, string time, string type);//time type 沒改

        #region 設定雙緩衝listview 為了解決listview閃爍問題 
        //protected override bool DoubleBuffered { get; set; }
        //定義他的class並且擁有繼承然後創造一個新的物件 
        public class DoubleBufferListView : ListView
        {
            public DoubleBufferListView()
            {
                SetStyle(ControlStyles.DoubleBuffer |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint, true);
                UpdateStyles();
            }
        }
        DoubleBufferListView doubleBufferListView1 = new DoubleBufferListView();
        public void DoubleBufferListView_add_content()
        {

            string[] listViewData = new string[4] { "Action", "1", "Action", "1" };

            ListViewItem lvItem = new ListViewItem(listViewData, 0);
            doubleBufferListView1.Items.Add(lvItem);
        }


        //listView1.DoubleBuffered(true);
        #endregion
        public static void lv_Print(ListView list, string time, string type)// 輸入listview ,兩個str
        {
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (list.InvokeRequired)
            {
                Listview_Print ph = new Listview_Print(lv_Print);
                list.Invoke(ph, list, time, type);
            }
            else
            {
                String[] row = { time, type };
                ListViewItem item = new ListViewItem(row);
                //ADD ITEMS
                list.Items.Add(item);
                if (list.Items.Count > 1000)
                {
                    list.Items.RemoveAt(1);
                }
            }
        }
        #endregion
        #region 定義物件  Listener slave id port 儲存暫存器數值的矩陣 
        TcpListener slaveTcpListener;
        Modbus.Device.ModbusSlave slave;

        private byte slaveID = 1;
        private int port = 502;

        public int[] reg_400 = new int[21];//儲存暫存器 40000-41020 矩陣index映射暫存器位置  : index+41001=addr
        public int[] reg_410 = new int[62];//儲存暫存器 41000-41061 矩陣index映射暫存器位置  : index+41001=addr
        public int[] reg_412 = new int[70];//儲存暫存器 41200-41055 矩陣index映射暫存器位置  : index+41001=addr
        //1201--1225  bms write 超過25不需要寫入  1241-1255 class
        bool full_flag = false, no_power = false ,comm_fault=false; //為了實現 電池soc 上下限緩衝  與偵測通訊故障 
        int check_wdt=0; //為了 偵測通訊故障   儲存上一次wdt
        #region 初始狀態 
        ushort initial = 1; //不一定會用到 ，原本以為每次啟動都需要初始充電 
        //class分成3個  ems(包含EMS寫入暫存器) pcs(包含給雙方讀取的暫存器) bms (Bms寫入暫存器)
        Register GC = new Register(); //建立暫存器物件
        
        #endregion

        #endregion
        #region master rtu
        SerialPort serialPort = new SerialPort(); //create a new SerialPort object with default settings.
        ModbusSerialMaster master;  //創建物件  master serailport 

        #endregion
        private void Form1_Load(object sender, EventArgs e)//取得這一台電腦的IP並且顯示出來 
        {
            //Get host IP
            //IPAddress addr;    
            //addr = new IPAddress(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].Address);
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;

            labServerName.Text = "Host IP=" + addr[1].ToString() + " slaveID=" + slaveID.ToString();  //顯示設定的master IP 
            //txtServerName.Text = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        }
        private void Modbus_Request_Event(object sender, Modbus.Device.ModbusSlaveRequestEventArgs e)
        //把request裡面的開始位置長度取出來
        {
            //request from master//disassemble packet from master
            byte fc = e.Message.FunctionCode;
            byte[] data = e.Message.MessageFrame;
            byte[] byteStartAddress = new byte[] { data[3], data[2] };
            byte[] byteNum = new byte[] { data[5], data[4] };
            Int16 StartAddress = BitConverter.ToInt16(byteStartAddress, 0);
            Int16 NumOfPoint = BitConverter.ToInt16(byteNum, 0);

            //Console.WriteLine(fc.ToString() + "," + StartAddress.ToString() + "," + NumOfPoint.ToString());
        }

        private void Modbus_DataStoreWriteTo(object sender, Modbus.Data.DataStoreEventArgs e)
        //寫入資料 ，依照封包要求一個bit一個bit依序寫入資料
        {
            //this.Text = "DataType=" + e.ModbusDataType.ToString() + "  StartAdress=" + e.StartAddress;
            int iAddress = e.StartAddress;//e.StartAddress;

            switch (e.ModbusDataType)
            {
                case ModbusDataType.HoldingRegister:

                    for (int i = 0; i < e.Data.B.Count; i++)
                    {
                        //Set AO
                        //e.Data.B[i] already write to slave.DataStore.HoldingRegisters[e.StartAddress + i + 1]
                        //e.StartAddress starts from 0
                        //You can set AO value to hardware here
                        //Debug.Print("寫入暫存器得到的地址 iAddress:" + iAddress.ToString());
                        switch (iAddress) //地址會減1
                        {  //單純的暫存器映射 
                            case (1206): //地址會減1 41207>41041
                                reg_410[41] = e.Data.B[i];
                                break;
                            case (1207):
                                reg_410[42] = e.Data.B[i];
                                break;
                            case (1202): 
                                reg_410[15] = e.Data.B[i];
                                break;

                        }//寫入暫存器映射到讀取暫存器，Bms要告訴EMS有故障 

                        if (iAddress > 999 && iAddress < 1070)
                        {
                            reg_410[iAddress - 999] = e.Data.B[i]; //iAddress 1000是41001
                        }
                        if (iAddress > 1199 && iAddress < 1256)
                        {
                            reg_412[iAddress - 1199] = e.Data.B[i]; //iAddress 1200是41201 addr 會少1
                        }
                        DoAOUpdate(iAddress, e.Data.B[i].ToString());
                        iAddress++;
                    }
                    break;

                case ModbusDataType.Coil:
                    for (int i = 0; i < e.Data.A.Count; i++)
                    {
                        //Set DO
                        //e.Data.A[i] already write to slave.DataStore.CoilDiscretes[e.StartAddress + i + 1]
                        //e.StartAddress starts from 0
                        //You can set DO value to hardware here

                        DoDOUpdate(iAddress, e.Data.A[i]);
                        iAddress++;
                        if (e.Data.A.Count == 1)
                        {
                            break;
                        }
                    }
                    break;
            }
        }


        #region "設定暫存器  AO"
        private delegate void UpdateAOStatusDelegate(int index, String message);
        private void DoAOUpdate(int index, String message)
        {
            if (this.InvokeRequired)
            {
                // we were called on a worker thread
                // marshal the call to the user interface thread
                this.Invoke(new UpdateAOStatusDelegate(DoAOUpdate),
                            new object[] { index, message });
                return;
            }


        }
        #endregion

        #region "Set DO"
        private delegate void UpdateDOStatusDelegate(int index, bool value);
        private void DoDOUpdate(int index, bool value)
        {
            if (this.InvokeRequired)
            {
                // we were called on a worker thread
                // marshal the call to the user interface thread
                this.Invoke(new UpdateDOStatusDelegate(DoDOUpdate),
                            new object[] { index, value });
                return;
            }

            // this code can only be reached
            // by the user interface thread


        }
        #endregion
        string status_flag = "NormalStop"; //定義變數 
        private void btnStart_Click(object sender, EventArgs e)
        // 按下開始按鈕之後，創造啟動 slave
        {
            //流程 : 取得主機IP >創造 listener>啟動 listener>創造slave>


            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;
            slaveTcpListener = new TcpListener(addr[1], port);
            slaveTcpListener.Start();

            slave = Modbus.Device.ModbusTcpSlave.CreateTcp(slaveID, slaveTcpListener);
            slave.ModbusSlaveRequestReceived += new EventHandler<ModbusSlaveRequestEventArgs>(Modbus_Request_Event);// 收到request分析request 
            slave.DataStore = Modbus.Data.DataStoreFactory.CreateDefaultDataStore();//(ushort coilsCount,ushort inputsCount,ushort holdingRegistersCount,ushort inputRegistersCount); //建立儲存暫存器資料的東西
            slave.DataStore.DataStoreWrittenTo += new EventHandler<DataStoreEventArgs>(Modbus_DataStoreWriteTo);//收到request之後寫入暫存器 
            slave.Listen(); //Slave 開始監聽要求


            timer_comm.Enabled = true;
            timer1.Enabled = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            timer_display_flag.Enabled = true;
            timer_status.Enabled = true;
            timer_wdt.Enabled = true;
            #region master  取得rtu連線方式 參數 
            #region  設定串列連結通訊方式 
            serialPort.PortName = "COM2";
            serialPort.BaudRate = 9600;
            serialPort.DataBits = 8;
            //get parity  Odd
            serialPort.Parity = Parity.Odd;
            //get stopBit  one
            serialPort.StopBits = StopBits.One;
            #endregion
            try
            {
                serialPort.Open();//打開串列 
                master = ModbusSerialMaster.CreateRtu(serialPort); //串列物件送入master 
                master.Transport.Retries = 0;   //don't have to do retries 設定連線參數 
                master.Transport.ReadTimeout = 300; //milliseconds
                Debug.Print(DateTime.Now.ToString() + " =>Open " + serialPort.PortName + " sucessfully!");
                timer1.Enabled = true;
            }
            catch (Exception ex) //
            {
                Debug.Print("Error: " + ex.Message);
            }
            #endregion

            //reg_400[1] = 1; //EMS啟動指令
            //預設暫存器 
            reg_412[1] = writeBit(reg_412[1], 8, '1');//b8 還沒有初始充電預設值是1 
            slave.DataStore.HoldingRegisters[1201] = Convert.ToUInt16(reg_412[1]);
            reg_400[4] = 1; //grid connect mode
            reg_412[41] = 2;//剛開啟的時候要進入初始充電狀態 
            reg_412[42] =1500;
            reg_412[43] =3000;
        }
        #region 暫存器翻譯  41200
        private int bms_Watch_dog() { return reg_412[2]; }
        private int soc() { return reg_412[3]; }
        private int BMS_output_p() { return reg_412[4]; }
        private int BMS_output_q() { return reg_412[5]; }
        private int MaximumChargingCurrent() { return reg_412[9]; }
        private int MaximumDischargingCurrent() { return reg_412[10]; } //MaximumDischargingCurrent 1210
        private int MaximumVoltageCharging() { return reg_412[11]; } //1211
        private int MaximumVoltageDischarging() { return reg_412[12]; }//1212
        private int Stop_Charging_Current() { return reg_412[13]; }//到1225
        private int Stop_Discharging_Voltage() { return reg_412[14]; }
        private int Open_Cell_Voltage() { return reg_412[15]; }
        private int Electrolyte_Temperature() { return reg_412[16]; }
        private int MaxSOC() { return reg_412[17]; }
        private int MinSOC() { return reg_412[18]; }
        private int Hysteresis_SOCHigh() { return reg_412[19]; }
        private int Hysteresis_SOCLow() { return reg_412[20]; }
        private int Maxcharging_Power() { return reg_412[21]; }
        private int Max_DischargingPower() { return reg_412[22]; }
        private int Battery_Discharge_Capacity() { return reg_412[23]; }
        private int Battery_ChargeCapacity() { return reg_412[24]; }
        private int num_of_batt_cell() { return reg_412[25]; }
        private int battery_voltage() { return reg_412[42]; }
        private int battery_current() { return reg_412[43]; }
        private int DC_voltage() { return reg_412[44]; }
        private int pcs_Watch_dog() { return reg_410[43]; }

        //40005 -6
        private int EMS_P_set_point() { return reg_400[5]; }
        private int EMS_Q_set_point() { return reg_400[6]; }
        //40015-18
        private int EMS_Maximum_SOC() { return reg_400[15]; }
        private int EMS_Min_SOC() { return reg_400[16]; }
        private int EMS_Hysteresis_high() { return reg_400[17]; }
        private int EMS_Hysteresis_low() { return reg_400[18]; }
        //
        //旗標轉換 40001 -40003 40004
        private string EMS_Start_Command() { return ggetbit(reg_400[1],0); }
        private string EMS_Reset() { return ggetbit(reg_400[3], 1); }
        private string EMS_Black_Start() { return ggetbit(reg_400[3], 2); }
        private string EMS_in_grid() { return ggetbit(reg_400[4], 0); } //csi operation in grid connection
        private string EMS_islanding() { return ggetbit(reg_400[4], 3); }//vsi iso operation in islanding condition
        //41001-2 (2 5) 
        private string PCS_BMS_Mode() { return ggetbit(reg_410[1], 1); }//Local control
        private string PCS_EMS_Mode() { return ggetbit(reg_410[1], 2); }//Remote control
        private string PCS_Ready() { return ggetbit(reg_410[1], 5); }//Ready
        private string PCS_AC_Breaker_closed() { return ggetbit(reg_410[1], 6); }//PCS_AC_Breaker_closed
        private string Online() { return ggetbit(reg_410[1], 7); }//online
        private string standby() { return ggetbit(reg_410[1], 8); }//standby
        private string alarm() { return ggetbit(reg_410[1], 9); }//alarm
        private string Battery_trip() { return ggetbit(reg_410[1], 10); }//Battery_trip
        private string PCS_trip() { return ggetbit(reg_410[1], 11); }//PCS_trip
        private string trip() { return ggetbit(reg_410[1], 12); }//trip
        private string Grid_Fault() { return ggetbit(reg_410[1], 13); }//Grid_Fault
        private string PCS_in_grid() { return ggetbit(reg_410[2], 2); }//PCS_in_grid operation in grid connection
        private string PCS_islanding() { return ggetbit(reg_410[2], 5); }//PCS_islanding
        //41026
        private string Trip_grid_over_voltage() { return ggetbit(reg_410[26], 0); }//Trip_grid_over_voltage
        private string Trip_grid_under_voltage() { return ggetbit(reg_410[26], 1); }//Trip_grid_under_voltage
        private string Trip_grid_over_frequency() { return ggetbit(reg_410[26], 2); }//Trip_grid_over_frequency
        private string Trip_grid_under_frequency() { return ggetbit(reg_410[26], 3); }//Trip_grid_under_frequency
        private string Trip_grid_unbalanced_voltage() { return ggetbit(reg_410[26], 4); }//Trip_grid_unbalanced_voltage
        private string Trip_grid_unbalanced_current() { return ggetbit(reg_410[26], 5); }//Trip_grid_unbalanced_current
        private string Trip_grid_overcurrent() { return ggetbit(reg_410[26], 6); }//Trip_grid_overcurren
        private string Trip_ground_fault() { return ggetbit(reg_410[26], 7); }// Trip_ground_fault
        
        //41241
        private string PCS_BMS_mode() { return ggetbit(reg_412[41], 0); }// PCS_BMS_mode
        private string PCS_Initial_charge() { return ggetbit(reg_412[41], 1); }// PCS_Initial_charge
        private string Pump_Run() { return ggetbit(reg_412[41], 2); }// Pump_Run
        private string BMS_reset() { return ggetbit(reg_412[41], 3); }// BMS_reset
     
        #endregion
        private void btnStop_Click(object sender, EventArgs e)
        {
            timer_comm.Enabled = false;
            timer_wdt.Enabled = false;
            timer1.Enabled = false;
            timer_display_flag.Enabled = false;
            timer_status.Enabled = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            //slave
            try
            {
                slaveTcpListener.Stop();
                slaveTcpListener = null;
                slave.Stop();
                slave.Dispose();

                serialPort.Close();
            }
            catch (Exception ex)
            {

               MessageBox.Show(DateTime.Now.ToString() + "stop click catch  Exception" + ex.Message );
            }

            Debug.Print(DateTime.Now.ToString() + " =>Disconnect " + serialPort.PortName);
        }

        private void timer_status_Tick(object sender, EventArgs e)//專門用來改變狀態 
        {
            //try{


            switch (status_flag) //故障跳閘其實可以寫一個 副程式 
            {
                case ("NormalStop"):
                    label_status.Text = "NormalStop";
                    set_flag_normal();
                    //檢查是否有故障發生，假如有故障發生直接跳出去  
                    status_flag = checkif_trip_alarm_Fault(ref status_flag);
                    if (status_flag != "NormalStop") { break; }

                    //判斷目前是誰控制 bms Control mode 
                    if (bms_control_mode() == false) { }
                    GC.Pump_Run = 0;//預設狀態 幫浦關閉

                    //ems_control下 on-grid下 Pcs啟動指令 就啟動幫浦 
                    if (bms_control_mode() == false && on_grid_mode()==true && EMS_Start_Command() == "1")
                    {
                        //label3.Text = "bms_control_mode()==false&& on_grid_mode() ...";
                        GC.Pump_Run = 1; //啟動幫浦
                    }
                    //ems_control下 island下  全黑啟動=1 才能啟動幫浦
                    if (bms_control_mode() == false && on_grid_mode() == false && ggetbit(reg_410[2], 6) == "1")
                    {
                        //label3.Text = "bms_control_mode() == false && on_grid_mode()";
                        GC.Pump_Run = 1; //啟動幫浦
                    }
                    //ems_control下 pcs 停止指令 直接停止幫浦 
                    if (bms_control_mode() == false && EMS_Start_Command() == "0")
                    {
                        GC.Pump_Run = 0;//預設狀態 幫浦關閉
                    }
                    //顯示初始充電狀態 
                    if (ggetbit(reg_412[1], 8) == "1" && ggetbit(reg_412[1], 5) == "1"
                        && ggetbit(reg_412[6], 0) == "1")
                    {
                        GC.Initial_charge_sequence_on = 1;//顯示初始充電狀態 
                        
                    }
                    //bms mode 進入online  //  幫浦啟動  許可=1 Pcs啟動 
                    if (bms_control_mode() && ggetbit(reg_412[1], 4) == "1" && ggetbit(reg_412[1], 5) == "1" && ggetbit(reg_412[6], 0) == "1" )//&& ggetbit(reg_412[6], 1) == "0")
                    {
                        status_flag = "Online";
                    }
                    //ems mode 進入online  //  幫浦啟動  許可=1 Pcs啟動 
                    if (bms_control_mode() == false && on_grid_mode() == true && EMS_Start_Command() == "1" && ggetbit(reg_412[1], 4) == "1" && ggetbit(reg_412[1], 5) == "1")
                    {
                        status_flag = "Online";
                    }

                    //10-2 假如全黑啟動 =1改變旗標 
                    if (EMS_Black_Start() == "1")
                    {
                        GC.Black_start_enabled = 1;//顯示初始充電狀態 
                    }
                    //10-2 電網故障 若pcs trip =1 grid fault=1  全黑啟動 =1 vsi iso mode=1  可以 清除pcs trip
                    if (GC.Trip == 1 && GC.Grid_Fault == 1
                        && EMS_Black_Start() == "1" && EMS_islanding() == "1")
                    {
                        GC.Trip = 0;//清除pcs trip
                    }


                    if (EMS_islanding() == "1" && EMS_in_grid() == "0"
                        && ggetbit(reg_412[1], 5) == "1" && EMS_Start_Command() == "1")
                    // 孤島運行許可 2bit  bms運轉許可  pcs啟動  都通過  就可以進入孤島模式 
                    {
                        status_flag = "IslandingOnline";
                    }
                    Debug.Print(status_flag);
                    break;
                case ("Online"):
                    set_flag_Online();
                    //檢查是否有故障發生 
                    status_flag = checkif_trip_alarm_Fault(ref status_flag);
                    if (status_flag != "Online") { break; }

                    if (ggetbit(reg_412[1], 8) == "1" && ggetbit(reg_412[1], 7) == "0")
                    {
                        GC.Initial_charge_sequence_on = 1;//進入初始充電旗標
                        status_flag = "Online";
                    }
                    if(ggetbit(reg_412[1],5)=="0")
                    {
                        status_flag = "NormalStop";
                    }
                    //假如這時候幫浦啟動改變幫浦狀態暫存器
                    if (ggetbit(reg_412[1], 4) == "1")
                    {
                        GC.Pump_Run = 1;//改變幫浦狀態暫存器
                    }

                    //1Bms模式之下 充飽電了 進入standby模式 ， 1201-8=0離開初始充電狀態
                    if (bms_control_mode() && ggetbit(reg_412[1], 8) == "0" && ggetbit(reg_412[41], 1) == "0" && initial == 1)
                    {

                        status_flag = "NormalStop";
                        initial = 0;
                    }
                    //5-1 待機模式
                    if (bms_control_mode() == false && ggetbit(reg_400[2], 0) == "1")//ems告訴pcs待機 
                    {
                        lb_test.Text = "//5-1 待機模式";
                        status_flag = "Standby";
                    }
                    //5-2 待機模式 
                    if (bms_control_mode() && ggetbit(reg_412[6], 2) == "1")//Bms告訴pcs待機 
                    {//EMS_Start_Command() == "0" || ggetbit(reg_412[1], 5) == "0")
                        lb_test.Text = "/5-2 待機模式 ";
                        status_flag = "Standby";
                    }
                    //電壓限制
                    if (battery_voltage()> MaximumVoltageCharging() || battery_voltage() < Stop_Discharging_Voltage())
                    {
                        lb_test.Text = "電壓限制 ";
                        //假如電壓超過最大電壓或是電壓小於最低電壓都要切換到待機模式
                        status_flag = "Standby";
                    }
                    //soc限制
                    if (soc() > MaxSOC()*10 || soc() < MinSOC()*10)
                    {
                        lb_test.Text = "soc限制 ";
                        status_flag = "Standby";
                    }
                    //旗標限制
                    if (full_flag == true && BMS_output_p() < 0)
                    {
                        lb_test.Text = "full_flag 旗標限制";
                        status_flag = "Standby";
                    }
                    //旗標限制
                    if (no_power == true && BMS_output_p() > 0)
                    {
                        lb_test.Text = "no_power 旗標限制";
                        status_flag = "Standby";
                    }
                    if (ggetbit(reg_412[1],6) == "1" && BMS_output_p() < 0)
                    {
                        lb_test.Text = "full charge";
                        status_flag = "Standby";
                    }
                    if (ggetbit(reg_412[1], 7) == "1" && BMS_output_p() > 0)
                    {
                        lb_test.Text = "zero charge";
                        status_flag = "Standby";
                    }
                    #region 緩衝區旗標
                    if (soc() >= MaxSOC() * 10)
                    {
                        full_flag = true;
                    }
                    if (soc() <= (MaxSOC() - Hysteresis_SOCHigh()) * 10)
                    {
                        full_flag = false;
                    }
                    if (soc() <= MinSOC() * 10)
                    {
                        no_power = true;
                    }
                    if (soc() >= (MinSOC() + Hysteresis_SOCLow()) * 10)
                    {
                        no_power = false;
                    }
                    #endregion
                    #region 緩衝 輸出功率
                    if (full_flag == false && no_power == false)
                    {
                        textBox1.Text = "out" + BMS_output_p().ToString() + "W";
                    }
                    else if (full_flag == true && no_power == false && BMS_output_p() > 0)
                    {
                        textBox1.Text = "fullflag out" + BMS_output_p().ToString() + "W";
                    }
                    else if (full_flag == false && no_power == true && BMS_output_p() < 0)
                    {
                        textBox1.Text = "nopower  out" + BMS_output_p().ToString() + "W";
                    }
                    #endregion
                    //回到Online 改變410 旗標暫存器 
                    set_flag_Online();
                    Debug.Print(status_flag);
                    break;
                case ("Standby"):
                    set_flag_Standby();
                    //檢查是否有故障發生，假如有故障發生直接跳出去  
                    status_flag = checkif_trip_alarm_Fault(ref status_flag);
                    if (status_flag != "Standby") { break; }
                    bms_control_mode();
                    //回到Standby 改變410 旗標暫存器 
                    if (ggetbit(reg_412[1], 5) == "0")
                    {
                        status_flag = "NormalStop";
                    }
                    #region 緩衝 判斷是不是要輸出功率 
                    
                    if (bms_control_mode() && ggetbit(reg_412[6], 2) == "0" && ggetbit(reg_412[6], 0) == "1")
                    {//bMS下指令  STAND BY =0 RUN =1要回到線上模式(輸出功率 )//Bms告訴pcs待機 

                        if (full_flag == false && no_power == false )                   
                            {
                            if (soc() < MaxSOC()*10 && soc()/10 > MinSOC()*10)
                            {//假如soc在正常範圍內就輸出 
                                if (ggetbit(reg_412[1], 7) == "0" && ggetbit(reg_412[1], 6) == "0")
                                {
                                    lb_test.Text = "b6 b7 =0";
                                    status_flag = "Online";
                                }
                                else if (ggetbit(reg_412[1], 6) == "1" && BMS_output_p() > 0)
                                {
                                        lb_test.Text = "stb to online b6=1 ";
                                        status_flag = "Online";
                                }
                                else if (ggetbit(reg_412[1], 7) == "1" && BMS_output_p() < 0)
                                {
                                    lb_test.Text = " stb to online b7=1 ";
                                    status_flag = "Online";
                                }
                            }
                            else
                            {
                                textBox1.Text = "false :false but soc can't";
                            }
                        }
                        if (full_flag == true&& no_power == false &&BMS_output_p() > 0)
                        {//假如充飽電了只能放電 
                            status_flag = "Online";
                        }
                        else if(full_flag == true && no_power == false && BMS_output_p() < 0)
                        {
                            status_flag = "Standby";
                        }
                        if (full_flag == false && no_power == true  && BMS_output_p() < 0)
                        {//假如沒電了只能充電 
                            status_flag = "Online";
                        }
                        else if (full_flag == false && no_power == true && BMS_output_p() > 0)
                        {
                            status_flag = "Standby";
                        }
                        #endregion
                    }
                    textBox1.Text = "output: standby";
                    #region 緩衝區旗標
                    if (soc()  >= MaxSOC()*10)
                    {
                        full_flag = true;
                    }
                    if (soc()  <= (MaxSOC() - Hysteresis_SOCHigh()) * 10)
                    {
                        full_flag = false;
                    }
                    if (soc()  <= MinSOC() * 10)
                    {
                        no_power = true;
                    }
                    if (reg_412[3]  >= (MinSOC() + Hysteresis_SOCLow()) * 10)
                    {
                        no_power = false;
                    }
                    #endregion

                    break;
                case ("IslandingOnline"): //假如EMS下達停止指令和bms沒有運轉許可 
                    status_flag = checkif_trip_alarm_Fault(ref status_flag); //檢查是否有故障發生 
                    if (status_flag != "IslandingOnline") { break; }

                    label_status.Text = "IslandingOnline";
                    GC.AC_Breaker_closed = 1;//ACB=1
                    GC.Online = 1;//Online=1
                    GC.operation_in_islanding_condition = 1;//vsi iso=1
                    //接收到暫停指令 或許可=0  清除所有指標 回到正常模式 
                    if (EMS_Start_Command() == "0" || ggetbit(reg_412[1], 5) == "0")
                    {
                        reset_everthing();
                        status_flag = "NormalStop";
                        break;
                    }
                    Debug.Print(status_flag);
                    break;
                case ("TripStop"):
                    //Pcs故障會寫入 41001狀態暫存器 通知其他兩個設備
                    //應該Bms會自動停止幫浦 所以這個狀態只要寫如何復歸
                    GC.Pump_Run = 0; //停止幫浦
                    label_status.Text = "TripStop";
                    //TripStop的時候 遇到重置指令 都是全部清除 
                    if (bms_control_mode() == false && EMS_Reset() == "1")//EMS下達reset指令 
                    {
                        reset_everthing();
                        //reg_410[1] = writeBit(reg_410[1], 11, '0');//復歸pcs trip
                        //reg_410[1] = writeBit(reg_410[1], 12, '0');//復歸trip
                        //reg_410[1] = writeBit(reg_410[1], 13, '0');//復歸grid fault trip
                        status_flag = "NormalStop";
                        break;
                    }
                    if (bms_control_mode()==true &&ggetbit(reg_412[6], 3) == "1")//bMS下達reset指令 
                    {
                        reset_everthing();
                        //reg_410[1] = writeBit(reg_410[1], 10, '0');//復歸ems trip
                        //reg_410[1] = writeBit(reg_410[1], 11, '0');//復歸pcs trip
                        //reg_410[1] = writeBit(reg_410[1], 12, '0');//復歸trip
                        status_flag = "NormalStop";
                        break;
                    }
                    GC.Online = 0;
                    GC.Standby = 0;
                    Debug.Print(status_flag);
                    break;
            }
            /*}
            catch (Exception)
            {
                status_flag = "TripStop";
                lb_trip.Text = "try catch trip";
                throw;
            }*/
        }
        #region 設定狀態
        private void set_flag_normal()
        {
            label_status.Text = "NormalStop";
            GC.Online = 0;
            GC.Standby = 0;
        }
        private void set_flag_Online()
        {
            label_status.Text = "Online";
            GC.Online = 1;
            GC.Standby = 0;
        }

        private void set_flag_Standby()
        {
            label_status.Text = "Standby";
            GC.Online = 1;
            GC.Standby = 1;
        }
        private void set_flag_Grid_Fault()
        {
            GC.Trip = 1;
            GC.Grid_Fault = 1;
        }

        private void reset_everthing()//重設旗標
        {
            comm_fault = false;
            reg_412[41] = 0;//向bMS顯示狀態暫存器1
            reg_410[1] = 0;//向EMS顯示狀態暫存器1
            reg_410[2] = 0;//向EMS顯示狀態暫存器2 
            reg_410[26] = 0;//電網故障暫存器 
        }
        #endregion
        private bool on_grid_mode()
        { //並網 回傳true 離網 false
            if (EMS_in_grid() == "1" && EMS_islanding() == "0")
            {
                return true;
            }
            if (EMS_in_grid() == "0" && EMS_islanding() == "1")
            {
                return false;
            }
            else
            {
                lb_trip.Text = "need input on-grid or island //default on-grid";
                return true;
            }

        }
        public bool bms_control_mode() //假如是bms控制回傳1 
        {
            if (ggetbit(reg_412[1], 0) == "0" && ggetbit(reg_412[1], 1) == "1")
            {
                GC.Local_control_f = 0;//bms Control  mode
                GC.Remote_control_f = 1;//bms Control  mode
                GC.Off_site_control = 0;//bms Control  mode
                slave.DataStore.HoldingRegisters[1001] = Convert.ToUInt16(reg_410[1]);
                lb_mode.Text = "bms Control  mode";
                return true;
            }
            else if (ggetbit(reg_412[1], 0) == "1" && ggetbit(reg_412[1], 1) == "0")
            {
                GC.Local_control_f = 0;//bms Control  mode
                GC.Remote_control_f = 0;//bms Control  mode
                GC.Off_site_control = 1;//bms Control  mode
                slave.DataStore.HoldingRegisters[1001] = Convert.ToUInt16(reg_410[1]);
                lb_mode.Text = "ems Control  mode";
                return false;
            }
            else
            {
                lb_mode.Text = "need input control mode //default bms mode";
                return true;
            }
        }

        private string checkif_trip_alarm_Fault(ref string status_name)
        {
            //Debug.Print("into checkif_trip_alarm_Fault");
            if (status_name == "TripStop")//|| status_name=="PCSalarm" || status_name == "BMSalarm" 
            {//假如已經故障了就直接回傳原本故障狀態 
                return status_name;
            }
            if (comm_fault==true)
            {
                GC.Partial_PCS_trip = 1;
                GC.Trip = 1;
                lb_trip.Text = "comm_fault  " + comm_fault.ToString();
                status_name = "TripStop";
                return status_name;
            }
            else if (comm_fault == false)
            {
                GC.Partial_PCS_trip = 0;
                GC.Trip = 0;
            } 
            if (bms_control_mode() == true)
            {//功率抄寫
                GC.PCS_Real_Power_Demand = BMS_output_p();
                GC.PCS_Reactive_Power_Demand = BMS_output_q();
            }
            if (bms_control_mode() == false)
            {
                GC.PCS_Real_Power_Demand = EMS_P_set_point();
                GC.PCS_Reactive_Power_Demand = EMS_Q_set_point();
            }
            //開路電壓過高或過低 
            

            //6-1 ems _pcs trip  10-2 gird fault 
            if (GC.Partial_PCS_trip==1 || GC.Trip== 1 || GC.Grid_Fault == 1)
            {//假如pcs跳閘
                lb_trip.Text = "pcs trip";
                status_name = "TripStop";
                return status_name;
            }

            //7 好像根本不用更改程式碼 ，PCS警告就會自己更改暫存器 ，然後自動恢復 
            // 8 
            if ((ggetbit(reg_412[1], 2) == "1"))
            {//假如BMS TRIP
                lb_trip.Text = "bms trip";
                status_name = "TripStop";
                GC.Partial_Battery_trip= 1 ;//BMS trip = 1
                GC.Trip = 1; //trip =1
                return status_name;
            }
            //9-1
            if ((ggetbit(reg_412[1], 3) == "1"))// 假如 BMS Alarm
            {
                GC.Alarm = 1;// 設定 BMS Alarm 
                slave.DataStore.HoldingRegisters[1001] = Convert.ToUInt16(reg_410[1]);
            }
            //9-2
            if ((ggetbit(reg_412[1], 3) == "0"))
            {
                GC.Alarm = 0;// 清除BMS Alarm
                slave.DataStore.HoldingRegisters[1001] = Convert.ToUInt16(reg_410[1]);
            }
            // 9-3
            if ((EMS_Reset() == "1"))//EMS下指令重置
            {
                GC.BMS_reset = 1;// 告訴 BMS reset
            }
            if ((EMS_Reset() == "0"))//EMS清除 指令重置
            {
                GC.BMS_reset = 0;// 清除 BMS reset
            }
            //10-2 10-3
            if(GC.Trip ==1 && GC.Grid_Fault== 1 )
            {

                lb_trip.Text = "bms trip";
                status_name = "TripStop";
                return status_name;
            }

            //4-1 STOP
            if (bms_control_mode() == false)
            {   //EMS控制模式  假如EMS說暫停 或Bms沒有許可 直接回到暫停狀態 
                if (EMS_Start_Command() == "0" || ggetbit(reg_412[1], 5) == "0")
                {
                    GC.Pump_Run= 0 ;//pump run=0
                    lb_trip.Text = "go 4-1emsNormalStop";
                    status_name = "NormalStop";
                    GC.Online = 0; //'#41001-b7 (Online)=0
                    GC.Standby = 0; //'#41001-b8 (standby)=0
                    return status_name;
                }
            }

            //4-2 bms stop
            if (bms_control_mode() == true)
            {
                if (ggetbit(reg_412[6], 0) == "0" || ggetbit(reg_412[6], 1) == "1")
                {//假如bms下停止指令 
                    lb_trip.Text = "go 4-2bmsNormalStop";
                    status_name = "NormalStop";
                    GC.Online = 0; //'#41001-b7 (Online)=0
                    GC.Standby = 0; //'#41001-b8 (standby)=0
                    return status_name;
                }
                if ( ggetbit(reg_412[1], 4) == "0" || ggetbit(reg_412[1], 5) == "0" )
                {
                    status_name = "NormalStop";
                    lb_trip.Text = " Pump on run or permit no NormalStop";
                }
            }


            lb_trip.Text = "pass check fault";
            return status_name;
        }
        private void timer1_Tick(object sender, EventArgs e)
        //每隔特定時間區段會執行一次迴圈 ，會刷新 listview
        {

            //error_display.Text = "reg_410[1] =" + reg_410[1].ToString();
            //視窗1,3負責410開頭的暫存器

            listView1.Items.Clear();
            listView3.Items.Clear();
            listView2.Items.Clear();
            listView4.Items.Clear();
            listView5.Items.Clear();
            for (int i = 0; i < GC.reg10.Length; i++)
            {
                if (i == 20 || i == 21 || i == 22 || i == 23)  //因為pq 可能負數 補數轉換
                { lv_Print(listView1, (i + 41000).ToString(), to_2complement(GC.reg10[i]).ToString()); }
                if (i < 29 && i!=20 && i != 21 && i != 22 && i != 23)
                {lv_Print(listView1, (i + 41000).ToString(), GC.reg10[i].ToString());} // listView1.BeginUpdate(); listView1.EndUpdate();
                if (i > 28)
                { lv_Print(listView3, (i + 41000).ToString(), GC.reg10[i].ToString()); }
                
                slave.DataStore.HoldingRegisters[i + 1000] = Convert.ToUInt16(GC.reg10[i]);

                //把陣列的數字放入暫存器 
            }//顯示410系列暫存器並且把數值放入slave暫存器 
            for (int i = 0; i < reg_412.Length; i++)
            {
                //if i(41-70) 迴圈 41241 -41270 放入新暫存器
                if (i > 40 && i < 71)
                {
                    reg_412[i] = GC.reg12[i];
                }
                if (i < 29)
                { lv_Print(listView2, (i + 41200).ToString(), reg_412[i].ToString()); }
                if (i > 28)
                { lv_Print(listView4, (i + 41200).ToString(), reg_412[i].ToString()); }

                slave.DataStore.HoldingRegisters[i + 1200] = Convert.ToUInt16(reg_412[i]);
            }//顯示412系列暫存器並且把數值放入slave暫存器 
            for (int i = 1; i < reg_400.Length; i++)
            {
                lv_Print(listView5, (i + 40000).ToString(), reg_400[i].ToString());
                slave.DataStore.HoldingRegisters[i +1] = Convert.ToUInt16(reg_400[i]);
            }//顯示400系列暫存器並且把數值放入slave暫存器 

            label_flag.Text = "addr   76543210" +
                 "\r\n" + "41206 " + Convert.ToString(reg_412[6], 2).PadLeft(8, '0')
                + "\r\n" + "          76543210"
                + "\r\n" + "41207 " + Convert.ToString(reg_412[7], 2).PadLeft(8, '0') + "\r\n" + "41208 " + Convert.ToString(reg_412[8], 2).PadLeft(8, '0')
                + "\r\n" + "          76543210"
                 + "\r\n" + "41002 " + Convert.ToString(GC.reg10[2], 2).PadLeft(8, '0')
                + "\r\n" + "          76543210"
                + "\r\n" + "41043 " + Convert.ToString(GC.reg10[43], 2).PadLeft(8, '0')
                + "\r\n" + "          76543210"
                + "\r\n" + "40001 " + Convert.ToString(reg_400[1], 2).PadLeft(8, '0') + "\r\n" + "40002 " + Convert.ToString(reg_400[2], 2).PadLeft(8, '0')
                ;//+ "41241 " + Convert.ToString(reg_412[41], 2).PadLeft(8, '0') + "\r\n" 
            label_41001.Text = "41001 " + Convert.ToString(GC.reg10[1], 2).PadLeft(16, '0') + "\r\n" + "bit       5432109876543210";
        }
        private string ggetbit(int value, int bit_number)//取出16位元的幾個bit，輸入 數值  第幾個bit 
        {
            string flag;
            int Substring_bit = 15 - bit_number;
            flag = Convert.ToString(value, 2).PadLeft(16, '0');
            //Debug.Print("flag "+flag);
            //Debug.Print("return " + flag.Substring(Substring_bit, 1));
            return flag.Substring(Substring_bit, 1);
        }
        private int writeBit(int value, int bit_number, char bit_value) //輸入數值 要更改的第幾個bit  要更改的數值 回傳整個暫存器 更改好的數值  
        {
            string flag;
            int Substring_bit = 15 - bit_number;
            flag = Convert.ToString(value, 2).PadLeft(16, '0');
            var c = flag.ToCharArray();//分開成 char array
            c[Substring_bit] = bit_value; //修改
            string str = new string(c); //合併 char array to string 
            //slave.DataStore.HoldingRegisters[1000 + i] = Convert.ToUInt16(reg_410[i]);
            return Convert.ToInt32(str, 2);
        }
        private void ref_writeBit(ref int value, int bit_number, char bit_value) //輸入數值 要更改的第幾個bit  要更改的數值 回傳整個暫存器 更改好的數值  
        {
            string flag;
            int Substring_bit = 15 - bit_number;
            flag = Convert.ToString(value, 2).PadLeft(16, '0');
            var c = flag.ToCharArray();//分開成 char array
            c[Substring_bit] = bit_value; //修改
            string str = new string(c); //合併 char array to string 
            value = Convert.ToInt32(str, 2);
        }
        private void timer_display_flag_Tick(object sender, EventArgs e)
        {
            #region Project specific status
            label1.Text = "Project specific status41241";
            string reg41241 = Convert.ToString(reg_412[41], 2).PadLeft(8, '0');
            switch (reg41241.Substring(7, 1))
            {
                case "1":
                    lb41241b0.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41241b0.ForeColor = Color.Black;
                    break;
            }
            switch (reg41241.Substring(6, 1))
            {
                case "1":
                    lb41241b1.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41241b1.ForeColor = Color.Black;
                    break;
            }
            switch (reg41241.Substring(5, 1))
            {
                case "1":
                    lb41241b2.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41241b2.ForeColor = Color.Black;
                    break;
            }
            switch (reg41241.Substring(4, 1))
            {
                case "1":
                    lb41241b3.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41241b3.ForeColor = Color.Black;
                    break;
            }
  
            label41002.Text = "Project specific status41002";
            string reg41002 = Convert.ToString(reg_410[2], 2).PadLeft(16, '0');
            switch (reg41002.Substring(13, 2))
            {
                case "1":
                    lb41001b2.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b2.ForeColor = Color.Black;
                    break;
            }
            switch (reg41002.Substring(10, 5))
            {
                case "1":
                    lb41001b5.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b5.ForeColor = Color.Black;
                    break;
            }
            switch (reg41002.Substring(9, 6))
            {
                case "1":
                    lb41001b6.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b6.ForeColor = Color.Black;
                    break;
            }


            label41001.Text = "Project specific status41001";
            string reg41001 = Convert.ToString(reg_410[1], 2).PadLeft(16, '0');
            switch (reg41001.Substring(14, 1))
            {
                case "1":
                    lb41001b1.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b1.ForeColor = Color.Black;
                    break;
            }
            switch (reg41001.Substring(13, 1))
            {
                case "1":
                    lb41001b2.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b2.ForeColor = Color.Black;
                    break;
            }
            switch (reg41001.Substring(12, 1))
            {
                case "1":
                    lb41001b3.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b3.ForeColor = Color.Black;
                    break;
            }
            switch (reg41001.Substring(11, 1))
            {
                case "1":
                    lb41001b4.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b4.ForeColor = Color.Black;
                    break;
            }
            switch (reg41001.Substring(10, 1))
            {
                case "1":
                    lb41001b5.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b5.ForeColor = Color.Black;
                    break;
            }
            switch (reg41001.Substring(9, 1))
            {
                case "1":
                    lb41001b6.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b6.ForeColor = Color.Black;
                    break;
            }
            switch (reg41001.Substring(8, 1))
            {
                case "1":
                    lb41001b7.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b7.ForeColor = Color.Black;
                    break;
            }
            switch (reg41001.Substring(7, 1))
            {
                case "1":
                    lb41001b8.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b8.ForeColor = Color.Black;
                    break;
            }
            switch (reg41001.Substring(6, 1))
            {
                case "1":
                    lb41001b9.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b9.ForeColor = Color.Black;
                    break;
            }
            switch (reg41001.Substring(5, 1))
            {
                case "1":
                    lb41001b10.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b10.ForeColor = Color.Black;
                    break;
            }
            switch (reg41001.Substring(4, 1))
            {
                case "1":
                    lb41001b11.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b11.ForeColor = Color.Black;
                    break;
            }
            switch (reg41001.Substring(3, 1))
            {
                case "1":
                    lb41001b12.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b12.ForeColor = Color.Black;
                    break;
            }
            switch (reg41001.Substring(2, 1))
            {
                case "1":
                    lb41001b13.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41001b13.ForeColor = Color.Black;
                    break;
            }

            string reg41201 = Convert.ToString(reg_412[1], 2).PadLeft(16, '0');
            switch (reg41201.Substring(15, 1))
            {
                case "1":
                    lb41201b0.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41201b0.ForeColor = Color.Black;
                    break;
            }
            switch (reg41201.Substring(14, 1))
            {
                case "1":
                    lb41201b1.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41201b1.ForeColor = Color.Black;
                    break;
            }
            switch (reg41201.Substring(13, 1))
            {
                case "1":
                    lb41201b2.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41201b2.ForeColor = Color.Black;
                    break;
            }
            switch (reg41201.Substring(12, 1))
            {
                case "1":
                    lb41201b3.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41201b3.ForeColor = Color.Black;
                    break;
            }
            switch (reg41201.Substring(11, 1))
            {
                case "1":
                    lb41201b4.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41201b4.ForeColor = Color.Black;
                    break;
            }
            switch (reg41201.Substring(10, 1))
            {
                case "1":
                    lb41201b5.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41201b5.ForeColor = Color.Black;
                    break;
            }
            switch (reg41201.Substring(9, 1))
            {
                case "1":
                    lb41201b6.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41201b6.ForeColor = Color.Black;
                    break;
            }
            switch (reg41201.Substring(8, 1))
            {
                case "1":
                    lb41201b7.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41201b7.ForeColor = Color.Black;
                    break;
            }
            switch (reg41201.Substring(7, 1))
            {
                case "1":
                    lb41201b8.ForeColor = Color.Red;
                    break;
                case "0":
                    lb41201b8.ForeColor = Color.Black;
                    break;
            }
            #endregion

        }
        private void Button_write_Click(object sender, EventArgs e) //按鈕按下去就要把文字視窗裡面的數值寫入到特定暫存器 
        {
            try
            {
                if (Convert.ToUInt16(textBox_addr.Text) > 0)
                {
                    slave.DataStore.HoldingRegisters[Convert.ToUInt16(textBox_addr.Text) - 40000] = Convert.ToUInt16(textBox_value.Text);
                    if (Convert.ToUInt16(textBox_addr.Text) - 40000 > 1000 && Convert.ToUInt16(textBox_addr.Text) - 40000 < 1062)
                    {
                        reg_410[Convert.ToUInt16(textBox_addr.Text) - 41000] = Convert.ToUInt16(textBox_value.Text); //iAddress 0是40001
                    }
                    if (Convert.ToUInt16(textBox_addr.Text) - 40000 > 1200 && Convert.ToUInt16(textBox_addr.Text) - 40000 < 1256)
                    {
                        reg_412[Convert.ToUInt16(textBox_addr.Text) - 41200] = Convert.ToUInt16(textBox_value.Text); //iAddress 0是40001
                    }
                    if (Convert.ToUInt16(textBox_addr.Text) - 40000 > 0 && Convert.ToUInt16(textBox_addr.Text) - 40000 < 999)
                    {
                        reg_400[Convert.ToUInt16(textBox_addr.Text) - 40000] = Convert.ToUInt16(textBox_value.Text); //iAddress 0是40001
                    }
                }
            }
            catch (Exception ex)
            {
                lb_Program_error.Text = "error: " + ex.Message;
            }
           

        }
        private void Timer_rtu_Tick(object sender, EventArgs e)
        {
            try //讀取前4個類比輸出暫存器 
            {
                byte slaveID = 1;
                ushort startAddress = 0;
                ushort numofPoints = 4;

                //read AO(4xxxx) 讀Ao 輸入id 起點 長度 
                ushort[] holdingregister = master.ReadHoldingRegisters(slaveID, startAddress, numofPoints);
                for (int i = 0; i < numofPoints; i++)
                {

                }

            }
            catch (Exception exception)
            {
                //Connection exception
                //No response from server.
                //The server maybe close the com port, or response timeout.
                if (exception.Source.Equals("System"))
                {
                    Console.WriteLine(DateTime.Now.ToString() + " " + exception.Message);
                }
                //The server return error code.
                //You can get the function code and exception code.
                if (exception.Source.Equals("nModbusPC"))
                {
                    string str = exception.Message;
                    int FunctionCode;
                    string ExceptionCode;

                    str = str.Remove(0, str.IndexOf("\r\n") + 17);
                    FunctionCode = Convert.ToInt16(str.Remove(str.IndexOf("\r\n")));
                    Console.WriteLine("Function Code: " + FunctionCode.ToString("X"));

                    str = str.Remove(0, str.IndexOf("\r\n") + 17);
                    ExceptionCode = str.Remove(str.IndexOf("-"));
                    switch (ExceptionCode.Trim())
                    {
                        case "1":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal function!");
                            break;
                        case "2":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal data address!");
                            break;
                        case "3":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal data value!");
                            break;
                        case "4":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Slave device failure!");
                            break;
                    }
                    /*
                       //Modbus exception codes definition                            
                       * Code   * Name                                      * Meaning
                         01       ILLEGAL FUNCTION                            The function code received in the query is not an allowable action for the server.
                         
                         02       ILLEGAL DATA ADDRESS                        The data addrdss received in the query is not an allowable address for the server.
                         
                         03       ILLEGAL DATA VALUE                          A value contained in the query data field is not an allowable value for the server.
                           
                         04       SLAVE DEVICE FAILURE                        An unrecoverable error occurred while the server attempting to perform the requested action.
                             
                         05       ACKNOWLEDGE                                 This response is returned to prevent a timeout error from occurring in the client (or master)
                                                                              when the server (or slave) needs a long duration of time to process accepted request.
                          
                         06       SLAVE DEVICE BUSY                           The server (or slave) is engaged in processing a long–duration program command , and the
                                                                              client (or master) should retransmit the message later when the server (or slave) is free.
                             
                         08       MEMORY PARITY ERROR                         The server (or slave) attempted to read record file, but detected a parity error in the memory.
                             
                         0A       GATEWAY PATH UNAVAILABLE                    The gateway is misconfigured or overloaded.
                             
                         0B       GATEWAY TARGET DEVICE FAILED TO RESPOND     No response was obtained from the target device. Usually means that the device is not present on the network.
                     */
                }
            }
        }
        private void bt_EMS_stop_Click(object sender, EventArgs e)
        {

            ref_writeBit(ref reg_400[1], 0, '0');
            slave.DataStore.HoldingRegisters[1001] = Convert.ToUInt16(reg_400[1]);

        }
        private void bt_EMS_start_Click(object sender, EventArgs e)
        {
            ref_writeBit(ref reg_400[1], 0, '1');
        }

        private void Button3_Click(object sender, EventArgs e)
        { //grid fault
            reg_410[1] = writeBit(reg_410[1], 12, '1');// trip
            reg_410[1] = writeBit(reg_410[1], 13, '1');//grid fault
            slave.DataStore.HoldingRegisters[1001] = Convert.ToUInt16(reg_410[1]);
        }
        private void Button4_Click(object sender, EventArgs e)
        { //pcs fault
            reg_410[1] = writeBit(reg_410[1], 11, '1');// pcs trip
            reg_410[1] = writeBit(reg_410[1], 12, '1');//trip
            slave.DataStore.HoldingRegisters[1001] = Convert.ToUInt16(reg_410[1]);
        }
        /// <summary>
        ///  寫入的流程  會在write to 把value 放入 array   在timer 顯示 並且放入暫存器
        /// </summary>
        #region 多餘的程式碼 
        private void TxtAI1_TextChanged(object sender, EventArgs e)
        {

        }



        private void Label6_Click(object sender, EventArgs e)
        {

        }

        private void Label7_Click(object sender, EventArgs e)
        {

        }

        private void TextBox_value_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            ushort index = 2;
            ushort aovalue = 55;
            
        }

        private void LabServerName_Click(object sender, EventArgs e)
        {

        }

        private void bt_test_Click(object sender, EventArgs e)
        {
            ref_writeBit(ref reg_410[1], 0, '1');

            GC.Local_control_f = 1;
            GC.Remote_control_f = 1;
            lb_test.Text = GC.Local_control_f.ToString()+"  "+ GC.Remote_control_f.ToString();//to_2complement(100).ToString();
        }
        private int to_2complement(int value)
        {//把功率轉換成2的補數 
            if (value > 32768)
            {
                
                return value- 65536;
            }
            else
            {
                return value;
            }
            //Now value is -100
        }
        private void Bt_PCSreset_Click(object sender, EventArgs e)
        {
            ref_writeBit(ref reg_400[3], 1, '1');
        }

        private void bt_reset_clr_Click(object sender, EventArgs e)
        {
            ref_writeBit(ref reg_400[3], 1, '0');
        }

        private void Bt_BlackStart_Click(object sender, EventArgs e)
        {
            ref_writeBit(ref reg_400[3], 2, '1');
            ref_writeBit(ref reg_400[4], 3, '1');
            ref_writeBit(ref reg_400[4], 0, '0');
            ref_writeBit(ref reg_400[4], 4, '0');
        }

        private void Lb_check_wdt_Click(object sender, EventArgs e)
        {

        }

        private void TextBox_addr_TextChanged(object sender, EventArgs e)
        {

        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Lb41201b0_Click(object sender, EventArgs e)
        {

        }

        private void Lb41201b1_Click(object sender, EventArgs e)
        {

        }

        private void Lb41201b2_Click(object sender, EventArgs e)
        {

        }

        private void Lb41201b7_Click(object sender, EventArgs e)
        {

        }

        private void Lb40001b1_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void Label10_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click_1(object sender, EventArgs e)
        {

        }

        private void Timer_comm_Tick(object sender, EventArgs e)
        {//  newcheck_wdt現在這個時刻的計時  check_wdt上一個時刻的計時  假如他們兩個一樣代表 通訊故障 
            int newcheck_wdt= bms_Watch_dog(); 
            lb_check_wdt.Text = check_wdt.ToString();
            if (newcheck_wdt != check_wdt)
            {
                comm_fault = false;
            }
            else if (newcheck_wdt == check_wdt)
            {
                comm_fault = true;
            }
            check_wdt = bms_Watch_dog();
        }

        private void Timer_wdt_Tick(object sender, EventArgs e)
        {//每秒鐘計時器增加1 快要爆表的時候歸零 

            GC.Watch_dog += 1;
            lb_tt3.Text = GC.Watch_dog.ToString();
            Debug.Print("Timer_wdt_Tick"+ GC.Watch_dog.ToString());
            slave.DataStore.HoldingRegisters[1043] = Convert.ToUInt16(GC.Watch_dog);
            if (Convert.ToUInt16(GC.Watch_dog) == 65530)
            {
                GC.Watch_dog = 0;
                Debug.Print("into if " + GC.Watch_dog.ToString());
            }
        }
    }
}