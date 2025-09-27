using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bài_tập_3
{
    public partial class FormĐọcFileXML : Form
    {
        public FormĐọcFileXML()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Luôn khởi tạo lại DataTable mỗi khi nhấn nút để tránh dữ liệu bị lặp lại
            var dataTable = new DataTable("Books");
            dataTable.Columns.Add("ISBN", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("Author", typeof(string));

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load("..\\..\\Book.xml");

                // Dòng quan trọng để kiểm tra
                var nodeList = xmlDoc.DocumentElement.SelectNodes("/catalog/book");

                if (nodeList.Count == 0)
                {
                    MessageBox.Show("Đọc file XML thành công nhưng không tìm thấy node '/catalog/book' nào. Vui lòng kiểm tra lại cấu trúc file XML.");
                }

                foreach (XmlNode node in nodeList)
                {
                    // ... (phần code lấy dữ liệu và Rows.Add như cũ)
                    var isbn = node.Attributes["ISBN"].Value;
                    var title = node.SelectSingleNode("title").InnerText;
                    var price = node.SelectSingleNode("price").InnerText;
                    var firstName = node.SelectSingleNode("author/first-name").InnerText;
                    var lastName = node.SelectSingleNode("author/last-name").InnerText;
                    dataTable.Rows.Add(isbn, title, "$" + price, $"{firstName} {lastName}");
                }

                dataGridView1.DataSource = dataTable;
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("LỖI: Không tìm thấy file 'Book.xml'.\n\nHãy kiểm tra lại bước thiết lập 'Copy to Output Directory' thành 'Copy if newer' cho file.");
            }
            catch (System.Xml.XmlException ex)
            {
                MessageBox.Show("LỖI: File 'books.xml' bị lỗi cấu trúc.\n\nChi tiết: " + ex.Message);
            }
        }
    }
}
