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
    public partial class SettingsForm : Form
    {
        bool _flagModifications;
        int _indexCombobox = 0;
        public SettingsForm()
        {
            InitializeComponent();
            //камера
            textBox1.Text = DevicesClass.Camera;
            //въезд
            comboBox1.Items.AddRange(DevicesClass.ListIpStancesIn.ToArray());
            //выезд
            comboBox2.Items.AddRange(DevicesClass.ListIpStancesOut.ToArray());
            //общие
            comboBox3.Items.AddRange(DevicesClass.ListIpStancesIn.ToArray());
            comboBox3.Items.AddRange(DevicesClass.ListIpStancesOut.ToArray());
            //подключение к бд
            textBox4.Text = ConfigsClass.CurrentDatabaseName + "     " + ConfigsClass.CurrentDatabaseUrl;
            UpdateListDb();
            //инициальзация точкек
            try { comboBox3.SelectedIndex = 0; }
            catch (Exception) { }
            try { comboBox2.SelectedIndex = 0; }
            catch (Exception) { }
            try { comboBox1.SelectedIndex = 0; }
            catch (Exception) { }
            //порт
            textBox6.Text = ConfigsClass.LocalPort.ToString();
            //флаг изменения данных на форме
            Modifications = false;
        }


        bool Modifications
        {
            set
            {
                button2.Enabled = value;
                _flagModifications = value;
            }
            get { return _flagModifications; }
        }



        private void UpdateListDb()
        {
            listBox1.Items.Clear();
            foreach (var i in DevicesClass.ListDB)
                listBox1.Items.Add(i.NameDB + "   " + i.UrlDB);
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox5.Text = DevicesClass.ListDB[listBox1.SelectedIndex].UrlDB;
        }

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DevicesClass.ReplaceDataXML();
            Close();
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DevicesClass.ListDB[listBox1.SelectedIndex].UrlDB = textBox5.Text;
                UpdateListDb();
            }
        }

        /// <summary>
        /// Выход
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            if (Modifications && MessageBox.Show("Вы уверены, что хотите закрыть настройки? Несохраненные данные будут утеряны!",
                "Закрыть?", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                DevicesClass.Download();
                Close();
            }
            else return;

        }
        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modifications)
            {
                if (MessageBox.Show("Вы уверены, что хотите закрыть настройки? Несохраненные данные будут утеряны!",
                "Закрыть?", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    DevicesClass.Download();
                    return;
                }
                else e.Cancel = true;
            }
            else e.Cancel = false;
        }

        /// <summary>
        /// Отправить команду на выбранный МК
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            string temp = UDP.Write(textBox2.Text, comboBox3.Text);
            switch (temp[0])
            {
                case 'n':
                    textBox3.Text = "Ошибка обработки команды";
                    break;
                case 'y':
                    textBox3.Text = "Соманда обработана успешно";
                    break;
                case 'I':
                    DevicesClass.ListIpStancesIn.Add(comboBox3.Text);
                    comboBox1.Items.Add(comboBox3.Text);
                    Modifications = true;
                    break;
                case 'O':
                    DevicesClass.ListIpStancesOut.Add(comboBox3.Text);
                    comboBox2.Items.Add(comboBox3.Text);
                    Modifications = true;
                    break;
                default:
                    textBox3.Text = temp;
                    break;

            }
            
            
               

        }

        /// <summary>
        /// Изменение порта приема данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("При изменении сетевых настроек возможна потеря соединения с переферийными устройствами! Убедитесь в безопасности операции!",
               "Изменение сетевого порта!", 
                 MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                textBox6.Text = ConfigsClass.LocalPort.ToString();
                return;
            }
            try
            {
                ConfigsClass.SetLocalPort(Convert.ToInt32(textBox6.Text));
                UDP.Stop();
                UDP.Start(ConfigsClass.LocalPort);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка при изменении сетевого порта!");
            }
            
        }


        /// <summary>
        /// Изменение доступа камеры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            DevicesClass.Camera = textBox1.Text.ToString();
            Modifications = true;
        }

        


        //BEGIN---------------------------------------------- IN -------------------
        /// <summary>
        /// Изменение стойки въезда (IN)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count == 0) return;
            DevicesClass.ListIpStancesIn[_indexCombobox] = comboBox1.Text.ToString();
            comboBox1.Items[_indexCombobox] = comboBox1.Text;
            Modifications = true;
        }
        /// <summary>
        /// Удаление стойки въеза (IN)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count == 0) return;
            DevicesClass.ListIpStancesIn.Remove(comboBox1.Items[_indexCombobox].ToString());
            comboBox1.Items.Remove(comboBox1.Items[_indexCombobox]);
            Modifications = true;
        }

        /// <summary>
        /// Отслеживание индекса стойки въезда (IN)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _indexCombobox = comboBox1.SelectedIndex;
        }
        //END----------------------------------------------- IN ------------------------- 



        //BEGIN----------------------------------------------- OUT ------------------------

        /// <summary>
        /// Изменение стойки выезда (OUT)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            if (comboBox2.Items.Count == 0) return;
            DevicesClass.ListIpStancesOut[_indexCombobox] = comboBox2.Text.ToString();
            comboBox2.Items[_indexCombobox] = comboBox2.Text;
            Modifications = true;
        }

        /// <summary>
        /// Отслеживание индекса стойки вызда (OUT)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox2_MouseDown(object sender, MouseEventArgs e)
        {
            _indexCombobox = comboBox2.SelectedIndex;
        }
        /// <summary>
        /// Удаление стойки выезда (OUT)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox2.Items.Count == 0) return;
            DevicesClass.ListIpStancesOut.Remove(comboBox2.Items[_indexCombobox].ToString());
            comboBox2.Items.Remove(comboBox2.Items[_indexCombobox]);
            Modifications = true;
        }

       
        //END----------------------------------------------- OUT
    }
}
