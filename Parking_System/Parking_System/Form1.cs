using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Parking_System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ///загрузка параметров устройств
            DevicesClass.Download();
            ///загрузка конфигураций системы
            ConfigsClass.Download();

            ///текущее кол-во свободных мест
            textBox1.Text = ConfigsClass.ParkingCount;

            ///запускаем форму подключения к БД
            var _cForm = new ConnectionForm();
            _cForm.ShowDialog();


            ///Создаем новый поток для прослушивания
            UDP.Start(ConfigsClass.LocalPort);
     
        }

        

        private void подключенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _sForrm = new SettingsForm();
            _sForrm.ShowDialog();
        }

        private void парковкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var set = new SettingsParking();
            set.ShowDialog();
            textBox1.Text = ConfigsClass.ParkingCount;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Xотите закрыть приложение? Поток, принимающий сообщения, будет остановлен!",
                "Закрыть приложение?", MessageBoxButtons.YesNo) == DialogResult.No)
                e.Cancel = true;
            else
                UDP.Stop();
        }

        private void добавитьГостяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var set = new AddGuest();
            set.ShowDialog();
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var set = new SettingsTariff();
            set.ShowDialog();
        }
    }
}
