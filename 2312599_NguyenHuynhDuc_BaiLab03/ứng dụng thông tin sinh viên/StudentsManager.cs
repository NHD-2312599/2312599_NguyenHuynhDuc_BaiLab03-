using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ứng_dụng_thông_tin_sinh_viên
{
    public enum FileType { Text, Xml, Json }

    public class StudentsManager
    {
        private List<Student> students = new List<Student>();
        private string fileHandler;
        private FileType fileType;

        public StudentsManager(string path)
        {
            fileHandler = path;
            students = DocFile.LoadFromText(fileHandler);
        }

        public StudentsManager()
        {
        }

        public List<Student> GetAll() => students;

        // a. Thêm hoặc cập nhật (theo MSSV)
        public void AddOrUpdate(Student s)
        {
            var existing = students.FirstOrDefault(x => x.MSSV == s.MSSV);
            if (existing != null)
            {
                // Cập nhật
                existing.HoLot = s.HoLot;
                existing.Ten = s.Ten;
                existing.NgaySinh = s.NgaySinh;
                existing.Lop = s.Lop;
                existing.GioiTinh = s.GioiTinh;
                existing.CCCD = s.CCCD;
                existing.DienThoai = s.DienThoai;
                existing.DiaChi = s.DiaChi;
                existing.MonHoc = s.MonHoc;
            }
            else
            {
                // Thêm mới
                students.Add(s);
            }
            SaveChanges();
        }

        // b. Tìm kiếm
        public List<Student> Search(string keyword, string type)
        {
            keyword = keyword.ToLower();
            switch (type.ToLower())
            {
                case "mssv":
                    return students.Where(s => s.MSSV.ToLower().Contains(keyword)).ToList();
                case "hotlot":
                    return students.Where(s => s.HoLot.ToLower().Contains(keyword)).ToList();
                case "ten":
                    return students.Where(s => s.Ten.ToLower().Contains(keyword)).ToList();
                case "ngaysinh":
                    return students.Where(s => s.NgaySinh.ToString("dd/MM/yyyy").Contains(keyword)).ToList();
                case "lop":
                    return students.Where(s => s.Lop.ToLower().Contains(keyword)).ToList();
                case "gioitinh":
                    return students.Where(s => s.GioiTinh != null && s.GioiTinh.ToLower().Contains(keyword)).ToList();
                case "cmnd":
                    return students.Where(s => s.CCCD != null && s.CCCD.ToLower().Contains(keyword)).ToList();
                case "dienthoai":
                    return students.Where(s => s.DienThoai != null && s.DienThoai.ToLower().Contains(keyword)).ToList();
                case "diachi":
                    return students.Where(s => s.DiaChi != null && s.DiaChi.ToLower().Contains(keyword)).ToList();
                case "monhoc":
                    return students.Where(s => s.MonHoc != null && s.MonHoc.Any(m => m.ToLower().Contains(keyword))).ToList();
                default:
                    return new List<Student>();
            }
        }

        // c. Xóa (1 hoặc nhiều)
        public void Delete(List<string> listMSSV)
        {
            students.RemoveAll(s => listMSSV.Contains(s.MSSV));
            SaveChanges();
        }

        // Lưu lại file
        private void SaveChanges()
        {
            DocFile.SaveToText(fileHandler, students);
        }

        public List<Student> TimKiemTheoMSSV(string mssv)
        {
            return students.Where(s => s.MSSV.ToLower().Contains(mssv.ToLower())).ToList();
        }

        public List<Student> TimKiemTheoTen(string ten)
        {
            return students.Where(s => s.Ten.ToLower().Contains(ten.ToLower())).ToList();
        }

        public List<Student> TimKiemTheoLop(string lop)
        {
            return students.Where(s => s.Lop.ToLower().Contains(lop.ToLower())).ToList();
        }

        // a. Thêm và cập nhật thông tin sinh viên (có lưu vào file)
        
        public Student LaySinhVienTheoMSSV(string mssv)
        {
            return students.FirstOrDefault(s => s.MSSV == mssv);
        }

        public List<Student> LayTatCaSinhVien()
        {
            return students;
        }

        public int TongSoSinhVien()
        {
            return students.Count;
        }

        public List<string> LayDanhSachLop()
        {
            return students.Select(s => s.Lop).Distinct().ToList();
        }



        public List<Student> TimKiemNhieuTieuChi(string mssv, string hoTenLot, string ten, string lop)
        {
            var ketQua = students;

            if (!string.IsNullOrEmpty(mssv))
                ketQua = ketQua.Where(s => s.MSSV.ToLower().Contains(mssv.ToLower())).ToList();

            if (!string.IsNullOrEmpty(hoTenLot))
                ketQua = ketQua.Where(s => s.HoLot.ToLower().Contains(hoTenLot.ToLower())).ToList();

            if (!string.IsNullOrEmpty(ten))
                ketQua = ketQua.Where(s => s.Ten.ToLower().Contains(ten.ToLower())).ToList();

            if (!string.IsNullOrEmpty(lop))
                ketQua = ketQua.Where(s => s.Lop.ToLower().Contains(lop.ToLower())).ToList();

            return ketQua;
        }

    }
}

