using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace ứng_dụng_thông_tin_sinh_viên
{
    public partial class Form1 : Form
    {
        private StudentsManager manager;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnMoFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text file (*.txt)|*.txt|XML file (*.xml)|*.xml|JSON file (*.json)|*.json";
                ofd.Title = "Chọn file để mở";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string path = ofd.FileName;
                    List<Student> ds = null;
                    if (path.EndsWith(".txt"))
                        ds = DocFile.LoadFromText(path);
                    else if (path.EndsWith(".xml"))
                        ds = DocFile.ReadFromXml(path);
                    else if (path.EndsWith(".json"))
                        ds = DocFile.ReadFromJson(path);

                    // Gán lại manager với danh sách mới
                    //manager = new StudentManager(path);

                    if (ds != null)
                    {
                        lvSinhVien.Items.Clear();
                        foreach (var s in ds)
                        {
                            var item = new ListViewItem(s.MSSV);
                            item.SubItems.Add(s.HoLot);
                            item.SubItems.Add(s.Ten);
                            item.SubItems.Add(s.NgaySinh.ToString("dd/MM/yyyy"));
                            item.SubItems.Add(s.Lop);
                            item.SubItems.Add(s.CCCD);
                            item.SubItems.Add(s.DienThoai);
                            item.SubItems.Add(s.DiaChi);
                            item.Tag = s;
                            lvSinhVien.Items.Add(item);
                        }
                    }
                }
            }
        }

        private void btnLuuFile_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text file (*.txt)|*.txt|XML file (*.xml)|*.xml|JSON file (*.json)|*.json";
                sfd.Title = "Chọn nơi lưu file";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string path = sfd.FileName;
                    if (path.EndsWith(".txt"))
                        DocFile.WriteToText(path, manager.GetAll());
                    else if (path.EndsWith(".xml"))
                        DocFile.WriteToXml(path, manager.GetAll());
                    else if (path.EndsWith(".json"))
                        DocFile.WriteToJson(path, manager.GetAll());
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // Bước 1: Khởi tạo manager nếu nó null (tránh lỗi NullReferenceException)
            // Giả sử bạn đã khởi tạo manager ở nơi khác, nhưng thêm kiểm tra này là an toàn.
            if (manager == null)
            {
                manager = new StudentsManager();
            }

            // Bước 2: Lấy danh sách lớp duy nhất để truyền vào form tìm kiếm
            var dsLop = manager.GetAll().Select(sv => sv.Lop).Distinct().ToList();

            // Bước 3: Mở form tìm kiếm
            using (var frm = new frmTimKiem(dsLop))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Bước 4: Lấy danh sách đầy đủ từ manager để lọc
                    var allStudents = manager.GetAll();

                    // Bước 5: Thực hiện lọc dữ liệu dựa trên các tiêu chí tìm kiếm
                    var ketQua = allStudents.Where(sv =>
                        (string.IsNullOrEmpty(frm.MSSV) || sv.MSSV.Contains(frm.MSSV)) &&
                        (string.IsNullOrEmpty(frm.HoLot) || sv.HoLot.IndexOf(frm.HoLot, StringComparison.OrdinalIgnoreCase) >= 0) &&
                        (string.IsNullOrEmpty(frm.Ten) || sv.Ten.IndexOf(frm.Ten, StringComparison.OrdinalIgnoreCase) >= 0) &&
                        (string.IsNullOrEmpty(frm.Lop) || sv.Lop == frm.Lop) &&
                        (!frm.TimTheoNgaySinh || sv.NgaySinh.Date == frm.NgaySinh) &&
                        (string.IsNullOrEmpty(frm.GioiTinh) || sv.GioiTinh == frm.GioiTinh) &&
                        (string.IsNullOrEmpty(frm.CMND) || sv.CCCD.Contains(frm.CMND)) &&
                        (string.IsNullOrEmpty(frm.DienThoai) || sv.DienThoai.Contains(frm.DienThoai)) &&
                        (string.IsNullOrEmpty(frm.DiaChi) || sv.DiaChi.Contains(frm.DiaChi)) &&
                        (string.IsNullOrEmpty(frm.MonHoc) || sv.MonHoc.Contains(frm.MonHoc))
                    ).ToList();

                    // Bước 6: Hiển thị kết quả tìm kiếm
                    HienThiDanhSach(ketQua);
                }
                else
                {
                    // Nếu người dùng đóng form tìm kiếm, hiển thị lại toàn bộ danh sách
                    HienThiDanhSach(manager.GetAll());
                }
            }
        }
        private void HienThiDanhSach(List<Student> ds)
        {
            lvSinhVien.Items.Clear();
            foreach (var s in ds)
            {
                var item = new ListViewItem(s.MSSV);
                item.SubItems.Add(s.HoLot);
                item.SubItems.Add(s.Ten);
                item.SubItems.Add(s.NgaySinh.ToString("dd/MM/yyyy"));
                item.SubItems.Add(s.Lop);
                item.SubItems.Add(s.CCCD);
                item.SubItems.Add(s.DienThoai);
                item.SubItems.Add(s.DiaChi);
                item.Tag = s;
                lvSinhVien.Items.Add(item);
            }
        }

        private void LoadListView()
        {
            lvSinhVien.Items.Clear();
            foreach (var s in manager.GetAll())
            {
                var item = new ListViewItem(s.MSSV);
                item.SubItems.Add(s.HoLot);
                item.SubItems.Add(s.Ten);
                item.SubItems.Add(s.NgaySinh.ToString("dd/MM/yyyy"));
                item.SubItems.Add(s.Lop);
                item.SubItems.Add(s.CCCD);
                item.SubItems.Add(s.DienThoai);
                item.SubItems.Add(s.DiaChi);
                item.Tag = s; // Gán đối tượng Student vào Tag
                lvSinhVien.Items.Add(item);
            }
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            if (!KiemTraThongTin())
            {
                MessageBox.Show("Vui lòng nhập đầy đủ và đúng thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo đối tượng Student mới từ dữ liệu trên form
            var sv = new Student
            {
                MSSV = mtbMSV.Text,
                HoLot = txtHoVaTen.Text,
                Ten = txtTen.Text,
                NgaySinh = dtpNgaySinh.Value,
                Lop = cboLop.Text,
                GioiTinh = rdNam.Checked ? "Nam" : "Nữ",
                CCCD = mtbCMND.Text,
                DienThoai = new string(mtbSoDT.Text.Where(char.IsDigit).ToArray()),
                DiaChi = txtDiaChi.Text,
                MonHoc = clbMonHoc.CheckedItems.Cast<string>().ToList()
            };
            // Đảm bảo manager đã được khởi tạo
            if (manager == null)
            {
                manager = new StudentsManager();
            }

            manager.AddOrUpdate(sv);

            MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            HienThiDanhSach(manager.GetAll()); // Cập nhật ListView
        }

        private bool KiemTraThongTin()
        {

            // Kiểm tra MSSV: 7 chữ số
            if (string.IsNullOrWhiteSpace(mtbMSV.Text) || mtbMSV.Text.Length != 7 || !mtbMSV.Text.All(char.IsDigit))
                return false;

            // Kiểm tra định dạng MSSV: AABBCCC
            string mssv = mtbMSV.Text;
            string lop = cboLop.Text;
            if (lop.Length < 2)
                return false;

            // Lấy 2 số cuối năm nhập học từ lớp (giả sử lớp có dạng: "D20CQCN01" -> năm nhập học là 2020 -> AA = 20)
            string yearPart = "";
            foreach (char c in lop)
            {
                if (char.IsDigit(c))
                    yearPart += c;
                if (yearPart.Length == 4) break;
            }
            if (yearPart.Length < 4)
                return false;
            string aa = yearPart.Substring(2, 2); // 2 số cuối năm

            if (mssv.Substring(0, 2) != aa)
                return false;
            if (mssv.Substring(2, 2) != "10")
                return false;

            // Kiểm tra trùng MSSV
            var allStudents = manager.GetAll();
            if (allStudents.Any(sv => sv.MSSV == mssv))
                return false;

            // Họ lót và tên
            if (string.IsNullOrWhiteSpace(txtHoVaTen.Text) || string.IsNullOrWhiteSpace(txtTenSV.Text))
                return false;

            // Ngày sinh
            if (dtpNgaySinhSV.Value == null)
                return false;

            // Lớp
            if (string.IsNullOrWhiteSpace(cboLop.Text))
                return false;

            // Giới tính
            if (!rBnam.Checked && !rBnu.Checked)
                return false;

            // CMND: 9 chữ số
            if (string.IsNullOrWhiteSpace(mtbCMND.Text) || mtbCMND.Text.Length != 9 || !mtbCMND.Text.All(char.IsDigit))
                return false;

            // Kiểm tra số điện thoại: loại bỏ ký tự không phải số trước khi kiểm tra
            string soDT = new string(mtbSoDT.Text.Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(soDT) || soDT.Length != 10)
                return false;

            // Địa chỉ
            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
                return false;

            // Môn học (ít nhất 1 môn)
            if (clbMonHoc.Items.Count == 0)
                return false;

            return true;
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if(!KiemTraThongTin())
            {
                MessageBox.Show("Vui lòng nhập đầy đủ và đúng thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            LoadListView();

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void lvSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSinhVien.SelectedItems.Count > 0)
            {
                var sv = (Student)lvSinhVien.SelectedItems[0].Tag;
                HienThiThongTinLenForm(sv);
            }
        }

        private void HienThiThongTinLenForm(Student sv)
        {
            mtbMSV.Text = sv.MSSV;
            txtHoVaTen.Text = sv.HoLot;
            txtTenSV.Text = sv.Ten;
            dtpNgaySinhSV.Value = sv.NgaySinh;
            cboLop.Text = sv.Lop;
            mtbCMND.Text = sv.CCCD;
            rBnam.Checked = sv.GioiTinh == "Nam";
            rBnu.Checked = sv.GioiTinh == "Nữ";
            mtbSoDT.Text = sv.DienThoai;
            txtDiaChi.Text = sv.DiaChi;

            // Hiển thị các môn học
            clbMonHoc.Items.Clear();
            if (sv.MonHoc != null)
            {
                foreach (var mon in sv.MonHoc)
                {
                    clbMonHoc.Items.Add(mon, true);
                }
            }
        }

        private void cmsSinhVien_Opening(object sender, CancelEventArgs e)
        {
            // Lấy danh sách MSSV của các sinh viên được check
            var listMSSV = new List<string>();
            foreach (ListViewItem item in lvSinhVien.Items)
            {
                if (item.Checked)
                {
                    listMSSV.Add(item.Text); // item.Text là MSSV
                }
            }

            if (listMSSV.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sinh viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa các sinh viên đã chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                manager.Delete(listMSSV);
                LoadListView();
                MessageBox.Show("Đã xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}