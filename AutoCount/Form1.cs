using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using ComboBox = System.Windows.Forms.ComboBox;

namespace AutoCount
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            getAllPort();
        }

        int timeRight = 0;

        void getAllPort()
        {
            string[] s = SerialPort.GetPortNames();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(s);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            timeRight = 0;
            AutoCount.Start();
        }

        private void lblScreen_Click(object sender, EventArgs e)
        {

        }

        private void AutoCount_Tick(object sender, EventArgs e)
        {
            if (timeRight < 100)
            {
                timeRight = timeRight++;
                lblScreen.Text = timeRight + "";
                var TimeSpanVar = TimeSpan.FromSeconds(timeRight);//take timeleft from seconds to hours and put it timespanvar variable
                lblScreen.Text = TimeSpanVar.ToString();//display the value on the timespan variable on label1
            }

            if (lblScreen.Text == "100")
            {
                timeRight = 0;
            }

            lblScreen.Text = timeRight++.ToString();
            List<byte> PacketToSend = new List<byte>();//craete a list/packet to send to serialport
            PacketToSend.Add(0);//sign address that the communication is sent to 
            PacketToSend.Add((byte)'S');//sign address that the communication is sent to 
            PacketToSend.Add(1);//sign address that the communication is sent to 
            PacketToSend.Add(3);//end of header

            string val1 = lblScreen.Text;//take the information displayed on label1 and put in the string val1
            for (int i = 0; i < val1.Length; i++)
            {
                PacketToSend.Add((byte)(Char)val1[i]);//add the value on val1 to the list/packet that will be sent to the display
            }

            PacketToSend.Add(4);//end of text indiscation to the  display

            byte crc = 0;
            for (int i = 0; i < PacketToSend.Count; i++)
            {
                crc ^= PacketToSend[i];
            }
            PacketToSend.Add(crc);
            String StringToSend = "";
            for (int i = 0; i < PacketToSend.Count; i++)
            {
                StringToSend += (Char)PacketToSend[i];
            }
            serialPort1.Write(StringToSend);

        }
        private void psbutton_Click(object sender, EventArgs e)
        {
            timeRight = 100;
            AutoCount.Stop();
        }

        private void rstbutton_Click(object sender, EventArgs e)
        {
            AutoCount.Stop();
            timeRight = 100;
            lblScreen.Text = "000";
        }

        private void opnbutton1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Convert.ToInt32((comboBox2.Text));
                serialPort1.Open();
                progressBar1.Value = 100;
            }
            catch (Exception)
            {
                //Do nothing
            }


        }

        private void clsbutton_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            progressBar1.Value = 0;
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            getAllPort();
        }
    }
    }

