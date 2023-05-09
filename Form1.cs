using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mysql
{
    public partial class Form1 : Form
    {
        ConnexionSql conn = ConnexionSql.getInstance("localhost", "biblio", "root", "");
        
        public Form1()
        {
            InitializeComponent();

            conn.openConnection();
            DataTable dt = new DataTable();
            dt = conn.getData("select * from auteur");
            dataGridView1.DataSource = dt;



            conn.closeConnection();
        }

        private void Insert_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            conn.openConnection();

            string recupTexte = textBox3.Text;

            conn.reqExec("delete from pret where numLivre like '" + recupTexte + "'");

            conn.reqExec("delete from livre where num like '" + recupTexte +"'");

            conn.closeConnection();

            RefreshDt2();
        }

        public void RefreshDt2()
        {
            dataGridView2.DataSource = null;
            conn.openConnection();
            DataTable dt = new DataTable();
            String i = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);
            dt = conn.getData("select * from livre where numAuteur like '" + i + "'");
            dataGridView2.DataSource = dt;



            conn.closeConnection();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            RefreshDt2();
        }
    }
}
