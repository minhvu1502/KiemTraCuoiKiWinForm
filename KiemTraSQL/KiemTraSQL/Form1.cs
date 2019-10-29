using System;
using System.Data;
using System.Windows.Forms;

namespace KiemTraSQL
{
    public partial class Form1 : Form
    {
        RunSQL db = new RunSQL();
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            DialogResult ret =
                MessageBox.Show("Bạn có muốn thoát không", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (ret == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.LoadDuLieu(" Select * from tbl_Sv");
            txt_MaMon.Text = dataGridView1.Rows[0].Cells[0].Value.ToString();
            txt_TenMon.Text = dataGridView1.Rows[0].Cells[1].Value.ToString();
            txt_SoTinChi.Text = dataGridView1.Rows[0].Cells[2].Value.ToString();
            txt_DiemThi.Text = dataGridView1.Rows[0].Cells[3].Value.ToString();
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            int check;
            double check1;
            if (txt_MaMon.Text == "")
            {
                MessageBox.Show("Hãy Nhập Mã Môn");
            }
            else if (txt_TenMon.Text == "")
            {
                MessageBox.Show("Hãy Nhập Tên Môn");
            }
            else if (txt_SoTinChi.Text == "")
            {
                MessageBox.Show("Hãy Nhập Số Tín Chỉ");
            }
            else if (int.TryParse(txt_SoTinChi.Text, out check) == false)
            {
                MessageBox.Show("Số tín Chỉ Phải là số");
            }
            else if (int.Parse(txt_SoTinChi.Text) <= 0)
            {
                MessageBox.Show("Số Tín Chỉ Phải Lớn Hơn 0");
            }
            else if (txt_DiemThi.Text == "")
            {
                MessageBox.Show("Hãy Nhập số điểm");
            }
            else if (double.TryParse(txt_DiemThi.Text, out check1) == false)
            {
                MessageBox.Show("Điểm phải là kiểu số");
            }
            else if (double.Parse(txt_DiemThi.Text) <= 0)
            {
                MessageBox.Show("Điểm thi phải lớn hơn 0");
            }
            else
            {
                string sql;
                sql = " SELECT MaMon FROM tbl_Sv WHERE MaMon=N'" + txt_MaMon.Text + "'";
                DataTable table = new DataTable();
                table = db.LoadDuLieu(sql);
                if (table.Rows.Count > 0)
                { MessageBox.Show("Mã Môn này đã tồn tại"); return; }
                //Thực hiện chèn thêm mới
                sql = " INSERT INTO tbl_Sv VALUES (N'" + txt_MaMon.Text + "',N'" + txt_TenMon.Text + "','" + txt_SoTinChi.Text + "','" + txt_DiemThi.Text + "')";
                db.RunSQL123(sql);
                dataGridView1.DataSource = db.LoadDuLieu("SELECT * FROM tbl_Sv");
                txt_MaMon.Text = "";
                txt_TenMon.Text = "";
                txt_SoTinChi.Text = "";
                txt_DiemThi.Text = "";
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int t = dataGridView1.CurrentRow.Index;
            txt_MaMon.Text = dataGridView1.Rows[t].Cells[0].Value.ToString();
            txt_TenMon.Text = dataGridView1.Rows[t].Cells[1].Value.ToString();
            txt_SoTinChi.Text = dataGridView1.Rows[t].Cells[2].Value.ToString();
            txt_DiemThi.Text = dataGridView1.Rows[t].Cells[3].Value.ToString();
        }

        private void btn_Tinh_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Danh sách đang trống. Bạn hãy nhập Môn học");
            }
            else if (dataGridView1.CurrentRow.Index == -1)
            {
                MessageBox.Show("Hãy chọn 1 môn học");
            }
            else
            {
                string sql;
                sql = " select sum(SoTinChi) from tbl_Sv";
                DataTable table = new DataTable();
                table = db.LoadDuLieu(sql);
                int TongSoTinChi = 0;
                double TongDiem = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    TongSoTinChi += Convert.ToInt32( dataGridView1.Rows[i].Cells[2].Value);
                    TongDiem += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value)* Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
                }
                txt_sumTinChi.Text = TongSoTinChi.ToString();
                txt_sumDiem.Text = TongDiem.ToString();
                txt_DTB.Text = (TongDiem / TongSoTinChi).ToString();
       

            }
        }
    }
}
