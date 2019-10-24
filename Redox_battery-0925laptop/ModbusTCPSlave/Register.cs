using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ModbusTCPSlave
{
    /// <summary>
    /// class 格式盡量都用整數 
    /// 在程式裡面會需要寫入的暫存器 ，舊式文件裡面的 read Register ，
    ///給EMS讀取41001-41061 給bms讀取41241-41269
    /// </summary>
    class Register
    {
        public int[] reg10 = new int[62];//儲存暫存器 41000-41061  矩陣index映射暫存器位置  : index+41001=addr
        public int[] reg12 = new int[70];//儲存暫存器 41241-41255  矩陣index映射暫存器位置  : index+41001=addr
        private int START,RESET;
        public int reset
        {
            get { return RESET; }
            set { RESET = value; }
        }
        public int Black_Start { get; set; }
        public int Watch_dog { get { return reg10[43]; } set { reg10[43] = value; }  }
        public int Average_SOC { get { return reg10[15]; } set { reg10[15] = value; } }
        public int PCS_Real_Power_Demand { get { return reg10[20]; } set { reg10[20] = value; } }
        public int PCS_Reactive_Power_Demand { get { return reg10[21]; } set { reg10[21] = value; } }
        public int PCS_Output_Real_Power { get { return reg10[22]; } set { reg10[22] = value; } }
        public int PCS_Output_Reactive_Power { get { return reg10[23]; } set { reg10[23] = value; } }
        public int PCS_ouput_Vavg { get { return reg10[44]; } set { reg10[44] = value; } }
        public int PCS_ouput_Vab { get { return reg10[45]; } set { reg10[45] = value; } }
        public int PCS_ouput_Vbc { get { return reg10[46]; } set { reg10[46] = value; } }
        public int PCS_ouput_Vca { get { return reg10[47]; } set { reg10[47] = value; } }
        public int PCS_ouput_Iavg { get { return reg10[48]; } set { reg10[48] = value; } }
        public int PCS_ouput_Ia { get { return reg10[49]; } set { reg10[49] = value; } }
        public int PCS_ouput_Ib { get { return reg10[50]; } set { reg10[50] = value; } }
        public int PCS_ouput_Ic { get { return reg10[51]; } set { reg10[51] = value; } }
        public int PCS_ouput_kVA { get { return reg10[52]; } set { reg10[52] = value; } }
        public int PCS_ouput_PF { get { return reg10[53]; } set { reg10[53] = value; } }
        public int PCS_ouput_Frequency { get { return reg10[54]; } set { reg10[54] = value; } }
        public int PCS_ouput_DC_voltage { get { return reg10[55]; } set { reg10[55] = value; } }
        public int PCS_ouput_DC_Current { get { return reg10[56]; } set { reg10[56] = value; } }
        public int PCS_ouput_DC_Power { get { return reg10[57]; } set { reg10[57] = value; } }
        public int Measured_battery_voltage { get { return reg12[42]; } set { reg12[42] = value; } }
        public int Measured_battery_current { get { return reg12[43]; } set { reg12[43] = value; } }
        public int Measured_DC_power { get { return reg12[44]; } set { reg12[44] = value; } }

        //****英文名稱的空格都用底線連結 (重要)
        //value 41015  1020-1023  1044-1057  1242-1244
        public int Local_control_f { get { return ggetbit(reg10[1], 1); } set { ref_writeBit(ref reg10[1],1, value ) ; } }
        public int Remote_control_f { get { return ggetbit(reg10[1], 2); } set { ref_writeBit(ref reg10[1], 2, value); } }
        public int Off_site_control { get { return ggetbit(reg10[1], 3); } set { ref_writeBit(ref reg10[1], 3, value); } }
        public int Commissioning_control { get { return ggetbit(reg10[1], 4); } set { ref_writeBit(ref reg10[1], 4, value); } }
        public int Ready { get { return ggetbit(reg10[1], 5); } set { ref_writeBit(ref reg10[1], 5, value); } }
        public int AC_Breaker_closed { get { return ggetbit(reg10[1], 6); } set { ref_writeBit(ref reg10[1], 6, value); } }
        public int Online { get { return ggetbit(reg10[1], 7); } set { ref_writeBit(ref reg10[1], 7, value); } }
        public int Standby { get { return ggetbit(reg10[1], 8); } set { ref_writeBit(ref reg10[1], 8, value); } }
        public int Alarm { get { return ggetbit(reg10[1], 9); } set { ref_writeBit(ref reg10[1], 9, value); } }
        public int Partial_Battery_trip { get { return ggetbit(reg10[1], 10); } set { ref_writeBit(ref reg10[1], 10, value); } }
        public int Partial_PCS_trip { get { return ggetbit(reg10[1], 11); } set { ref_writeBit(ref reg10[1], 11, value); } }
        public int Trip { get { return ggetbit(reg10[1], 12); } set { ref_writeBit(ref reg10[1], 12, value); } }
        public int Grid_Fault { get { return ggetbit(reg10[1], 13); } set { ref_writeBit(ref reg10[1], 13, value); } }

        //41002
        public int operation_in_grid_connection { get { return ggetbit(reg10[2], 2); } set { ref_writeBit(ref reg10[2], 2, value); } }
        public int operation_in_islanding_condition { get { return ggetbit(reg10[2], 5); } set { ref_writeBit(ref reg10[2], 5, value); } }
        public int Black_start_enabled { get { return ggetbit(reg10[2], 6); } set { ref_writeBit(ref reg10[2], 6, value); } }
        public int Synchronized_to_grid { get { return ggetbit(reg10[2], 7); } set { ref_writeBit(ref reg10[2], 7, value); } }
        public int Output_power_limited { get { return ggetbit(reg10[2], 8); } set { ref_writeBit(ref reg10[2], 8, value); } }

        //41241 
        public int BMS_mode_on { get { return ggetbit(reg12[41], 0); } set { ref_writeBit(ref reg12[41], 0, value); } }
        public int Initial_charge_sequence_on { get { return ggetbit(reg12[41], 1); } set { ref_writeBit(ref reg12[41], 1, value); } }
        public int Pump_Run { get { return ggetbit(reg12[41], 2); } set { ref_writeBit(ref reg12[41], 2, value); } }
        public int BMS_reset { get { return ggetbit(reg12[41], 3); } set { ref_writeBit(ref reg12[41], 3, value); } }
        
        //旗標
        //****英文名稱的空格都用底線連結 (重要)
        //41001 b0-b13 41002 b2 5 6 7 8 
        //41241 b0-b3

        //41026 1041 1042 先不用
        //取得旗標 
        private int ggetbit(int value, int bit_number)//取出16位元的幾個bit，輸入 數值  第幾個bit 
        {
            string flag;
            int Substring_bit = 15 - bit_number , flag_int; //b1 =reg[14]
            flag = Convert.ToString(value, 2).PadLeft(16, '0'); //把輸入的暫存器轉換成二進制16 bit (ToString)
            //Debug.Print("flag "+flag);
            //Debug.Print("return " + flag.Substring(Substring_bit, 1));
            flag_int = Convert.ToInt16(flag.Substring(Substring_bit, 1),2);// 就可以呼叫這個string ，第幾個位元，長度
            return flag_int;
        }
        //修改旗標 
        private void ref_writeBit(ref int reg_value, int bit_number, int bit_value) //輸入數值 要更改的第幾個bit  要更改的數值 回傳整個暫存器 更改好的數值  
        {
            string flag , bit_value_char;
            int Substring_bit = 15 - bit_number; //b1 =reg[14]
            flag = Convert.ToString(reg_value, 2).PadLeft(16, '0');
            var c = flag.ToCharArray();//分開成 char array 0000> 0 0 0 0 
            bit_value_char = Convert.ToString(bit_value, 2).PadLeft(8, '0'); //1會變成 0000 0001 
            c[Substring_bit] = bit_value_char[7]; //修改
            string str = new string(c); //合併 char array to string 0 0 0 0 >0000
            reg_value = Convert.ToUInt16(str,2);
        }
    }
}
