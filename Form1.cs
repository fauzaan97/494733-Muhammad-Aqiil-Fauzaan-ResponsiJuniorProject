using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace responsiAqiil
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private NpgsqlConnection conn;
        string connstring = "Host=localhost; Username=postgres;Password=informatika;Database=responsi2024;Include Error Detail=true";
        public DataTable dt;
        public static NpgsqlCommand cmd;
        private string sql = null;
        private DataGridView r;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {
            try
            { 
                conn.Open();
                dgvData.DataSource = null;
                sql = @"select * from st_select()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dgvData.DataSource = dt;
                conn.Close();

            } catch (Exception ex) 
            {
                MessageBox.Show("Error:" + ex.Message, "Fail!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from st_insert(:_id_karyawan, :_nama, :_nama_dep, :_nama_jabatan)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_nama", txtNama.Text);
                cmd.Parameters.AddWithValue("_id_dep", txtNamaDep.Text);
                if ((int)cmd.ExecuteScalar() == 1);
                {
                    MessageBox.Show("Data Berhasil Dimasukkan", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btn_Load.PerformClick();
                    txtNama.Text = txtNamaDep.Text = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Insert Fail!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
            {
                r = dgvData.Rows[e.RowIndex];
                txtNama.Text = r.SelectedCells[":_nama"].Value.ToString();
                txtNamaDep.Text = r.SelectedCells["_nama_dep"].Value.ToString();
            }
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (r ==  null)
            {
                MessageBox.Show("Pilih data yang akan diedit", "Good", MessageBoxButtons.OK,MessageBoxIcon.Information); 
                return;
            }
            try
            {
                conn.Open();
                sql = @"select * from st_insert(:_id_karyawan, :_nama, :_nama_dep, :_nama_jabatan)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.SelectedCells["_id_karyawan"].Value.ToString());
                cmd.Parameters.AddWithValue("_nama", txtNama.Text);
                cmd.Parameters.AddWithValue("_id_dep", txtNamaDep.Text);
                if ((int)cmd.ExecuteScalar() == 1) ;
                {
                    MessageBox.Show("Data Berhasil Dimasukkan", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btn_Load.PerformClick();
                    txtNama.Text = txtNamaDep.Text = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Edit Fail!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Pilih data yang akan dihapus", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                conn.Open();
                sql = @"select * from delete(_id_karyawan)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.SelectedCells["_id_karyawan"].Value.ToString());
                if ((int)cmd.ExecuteScalar() == 1) ;
                {
                    MessageBox.Show("Data Berhasil Dihapus", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btn_Load.PerformClick();
                    txtNama.Text = txtNamaDep.Text = null;
                    r = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Delete Fail!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
