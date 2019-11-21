using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestKiemTra
{
    public partial class frm_TestKiemTra : Form
    {
        Database db = new Database();
        public frm_TestKiemTra()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            if(txt_MCV.Text.Trim() == "")
            {
                MessageBox.Show("Hãy Nhập Mã Công Việc", "Thông Báo");
                txt_MCV.Focus();
            }
            else if (txt_TenCV.Text.Trim() == "")
            {
                MessageBox.Show("Hãy Nhập Tên Công Việc", "Thông Báo");
                txt_TenCV.Focus();
            }
            else if (txt_Luong.Text.Trim() == "")
            {
                MessageBox.Show("Hãy Nhập Mức Lương", "Thông Báo");
                txt_Luong.Focus();
            }
            else
            {
                string sql = " select * from CongViec where MCV = '" + txt_MCV.Text + "' ";
                DataTable table = new DataTable();
                table = db.LoadDuLieu(sql);
                if(table.Rows.Count > 0)
                {
                    MessageBox.Show("Mã Công Việc Này Đã Tồn Tại");
                    txt_MCV.Focus();
                    return;
                }
                string sql1 = " insert into CongViec VALUES ('" + txt_MCV.Text + "',N'" + txt_TenCV.Text + "','" + txt_Luong.Text + "')";
                db.RunSQL123(sql1);
                Fill();
                MessageBox.Show("Thêm Thành Công");
                ClearData();

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Fill();
            txt_MCV.Enabled = false;
            txt_MCV.Text = dataGridView1.Rows[0].Cells[0].Value.ToString();
            txt_TenCV.Text = dataGridView1.Rows[0].Cells[1].Value.ToString();
            txt_Luong.Text = dataGridView1.Rows[0].Cells[2].Value.ToString();
        }
        private void Fill()
        {
            string sql = "select * from CongViec";
            
            dataGridView1.DataSource = db.LoadDuLieu(sql);
            dataGridView1.Columns[0].HeaderText = "Mã Công Việc";
            dataGridView1.Columns[1].HeaderText = "Tên Công Việc";
            dataGridView1.Columns[2].HeaderText = "Mức Lương";
        }
        private void ClearData()
        {
            txt_MCV.Enabled = true;
            txt_MCV.Text = "";
            txt_TenCV.Text = "";
            txt_Luong.Text = "";
        }

        private void btn_dong_Click(object sender, EventArgs e)
        {
            DialogResult ret =
                MessageBox.Show("Bạn có muốn thoát không", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if(ret == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            txt_MCV.Enabled = false;
            int t = dataGridView1.CurrentRow.Index;
            txt_MCV.Text = dataGridView1.Rows[t].Cells[0].Value.ToString();
            txt_TenCV.Text = dataGridView1.Rows[t].Cells[1].Value.ToString();
            txt_Luong.Text = dataGridView1.Rows[t].Cells[2].Value.ToString();
        }

        private void txt_Luong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btn_excel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0) //TH có dữ liệu để ghi
            {
                //Khai báo và khởi tạo các đối tượng
                Excel.Application exApp = new Excel.Application();
                Excel.Workbook exBook = exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet exSheet = (Excel.Worksheet)exBook.Worksheets[1];

                //Định dạng chung
                Excel.Range tenCuaHang = (Excel.Range)exSheet.Cells[1, 1];
                tenCuaHang.Font.Size = 14;
                tenCuaHang.Font.Bold = true;
                tenCuaHang.Font.Color = Color.Blue;
                tenCuaHang.Value = "DANH SÁCH CÔNG VIỆC ";

                Excel.Range dcCuaHang = (Excel.Range)exSheet.Cells[2, 1];
                dcCuaHang.Font.Size = 14;
                dcCuaHang.Font.Bold = true;
                dcCuaHang.Font.Color = Color.Blue;
                dcCuaHang.Value = "Copyright-Vũ Quang Minh";

                Excel.Range dtCuaHang = (Excel.Range)exSheet.Cells[3, 1];
                dtCuaHang.Font.Size = 14;
                dtCuaHang.Font.Bold = true;
                dtCuaHang.Font.Color = Color.Blue;
                dtCuaHang.Value = "Điện thoại: 1800 8198";

                //Định dạng tiêu đề bảng

                exSheet.get_Range("A7:D7").Font.Bold = true;
                exSheet.get_Range("A7:D7").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                exSheet.get_Range("A7").Value = "STT";
                exSheet.get_Range("B7").Value = "Mã Công Việc";
                exSheet.get_Range("C7").Value = "Tên Công Việc";
                exSheet.get_Range("C7").ColumnWidth = 20;
                exSheet.get_Range("D7").Value = "Mức Lương";

                //In dữ liệu
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    exSheet.get_Range("A" + (i + 8).ToString() + ":G" + (i + 8).ToString()).Font.Bold = false;
                    exSheet.get_Range("A" + (i + 8).ToString()).Value = (i + 1).ToString();
                    exSheet.get_Range("B" + (i + 8).ToString()).Value =
                        dataGridView1.Rows[i].Cells[0].Value;
                    exSheet.get_Range("C" + (i + 8).ToString()).Value = dataGridView1.Rows[i].Cells[1].Value;
                    exSheet.get_Range("D" + (i + 8).ToString()).Value = dataGridView1.Rows[i].Cells[2].Value;
                }
                exSheet.Name = "Danh sách";
                exBook.Activate(); //Kích hoạt file Excel
                //Thiết lập các thuộc tính của SaveFileDialog
                SaveFileDialog dlgSave = new SaveFileDialog();
                dlgSave.Filter = "Excel Document(*.xlsx)|*.xlsx  |Word Document(*.docx) |*.docx|All files(*.*)|*.*";
                dlgSave.FilterIndex = 1;
                dlgSave.AddExtension = true;
                dlgSave.DefaultExt = ".xlsx";
                if (dlgSave.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    exBook.SaveAs(dlgSave.FileName.ToString());//Lưu file Excel
                exApp.Quit();//Thoát khỏi ứng dụng
            }
            else
                MessageBox.Show("Không có danh sách hàng để in");
        }

        private void btn_boqua_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow.Index == -1)
            {
                MessageBox.Show("Hãy chọn một công việc để xóa", "Thông Báo");
            }
            else
            {
                DialogResult ret =
                    MessageBox.Show("Bạn có muốn xóa không", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                if (ret == DialogResult.Yes)
                {
                    int t = dataGridView1.CurrentRow.Index;
                    string sql = " select MCV from NhanVien where MCV = '" + dataGridView1.Rows[t].Cells[0].Value.ToString() + "'";
                    DataTable tb = new DataTable();
                    tb = db.LoadDuLieu(sql);
                    if(tb.Rows.Count > 0)
                    {
                        MessageBox.Show("Công việc này đã có nhân viên làm rồi không thể xóa.");
                        return;
                    }
                    sql = "delete from CongViec where MCV = '" + txt_MCV.Text + "'";
                    db.RunSQL123(sql);
                    MessageBox.Show("Xóa Thành Công");
                    Fill();
                    ClearData();
                }
            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            string sql;
            int t = dataGridView1.CurrentRow.Index;
            sql = "update CongViec SET MCV = '" + txt_MCV.Text + "', TenCV = N'" + txt_TenCV.Text + "', MucLuong = '"+txt_Luong.Text+"' where MCV = '"+ dataGridView1.Rows[t].Cells[0].Value.ToString() + "'" ;
            if (txt_TenCV.Text.Trim() == "")
            {
                MessageBox.Show("Hãy Nhập Tên Công Việc", "Thông Báo");
                txt_TenCV.Focus();
            }
            else if (txt_Luong.Text.Trim() == "")
            {
                MessageBox.Show("Hãy Nhập Mức Lương", "Thông Báo");
                txt_Luong.Focus();
            }
            else
            {
                db.RunSQL123(sql);
                MessageBox.Show("Sửa Thành Công");
                Fill();
                ClearData();
            }
        }
    }
}
