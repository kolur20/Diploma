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
    public partial class ConnectionForm : Form
    {
        bool flagClose;
        public ConnectionForm()
        {
            InitializeComponent();
            flagClose = false;

            if (DevicesClass.ListDB.Count == 0) comboBox1.Text = "Не найдено ни одного подключения к базе";
            else
            {
                foreach (var i in DevicesClass.ListDB)
                    comboBox1.Items.Add(i.NameDB);
                if (comboBox1.Items.Contains(ConfigsClass.CurrentDatabaseName)) comboBox1.SelectedItem = ConfigsClass.CurrentDatabaseName;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            var temp = DevicesClass.ListDB.Find(x => x.NameDB == comboBox1.Text);
            ConfigsClass.SetConnectionDb(temp.NameDB, temp.UrlDB);
            flagClose = true;
            this.Close();
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConnectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!flagClose) Application.Exit(); 
        }

        /// <summary>
        /// Добавление нового подключения к бд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            
            DevicesClass.AddConnectionUrl(textBox1.Text, textBox2.Text);
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Items.Add(ConfigsClass.CurrentDatabaseName);
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;

        }
    }
}
