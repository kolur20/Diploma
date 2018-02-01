using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parking_System
{
    public partial class SettingsParking : Form
    {
        public SettingsParking()
        {
            InitializeComponent();
            textBox1.Text = ConfigsClass.ParkingCount;
            textBox2.Text = ConfigsClass.ParkingSize;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                ConfigsClass.SetParkingCount(textBox2.Text);
                ConfigsClass.SetParkingSize(textBox2.Text);
            }
            else
            if (Convert.ToInt32(ConfigsClass.ParkingSize) < Convert.ToInt32(textBox2.Text))
            {

                ConfigsClass.SetParkingCount(
                    (Convert.ToInt32(ConfigsClass.ParkingCount) + Convert.ToInt32(textBox2.Text) - Convert.ToInt32(ConfigsClass.ParkingSize)).ToString());
                ConfigsClass.SetParkingSize(textBox2.Text);
            }
            else
                ConfigsClass.SetParkingSize(textBox2.Text);

            Close();
            
        }
    }
}
