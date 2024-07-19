using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xceed.Words.NET;

namespace Des
{
    public partial class Form1 : Form
    {
        private RichTextBox rtbOutput;
        private RichTextBox rtbBanro_DES;
        private RichTextBox rtbBanro_AES;
        private RichTextBox rtbBanma_DES;
        private RichTextBox rtbBanma_AES;
        private TextBox tbTocdomahoa_DES;
        private TextBox tbTocdomahoa_AES;
        private TextBox tbTocdogiaima_DES;
        private TextBox tbTocdogiaima_AES;
        private const long MaxFileSizeInBytes = 100 * 1024; // 100 KB
        bool showCompareForm = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ucOutput uc = new ucOutput();
            rtbOutput = uc.RtbOutput;
            setUserControl(uc);
            pnOutput.Tag = "output";
        }

        private void setUserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            pnOutput.Controls.Clear();
            pnOutput.Controls.Add(uc);
            uc.BringToFront();
        }

        private void btnBangsosanh_Click(object sender, EventArgs e)
        {
            if(showCompareForm)
            {
                ucOutput uc = new ucOutput();
                rtbOutput = uc.RtbOutput;
                setUserControl(uc);
                rbAes.Enabled = true;
                rbDes.Enabled = true;
                rtbOutput = uc.RtbOutput;
                showCompareForm = false;
                btnBangsosanh.Text = "Hiện bảng so sánh";
            } else
            {
                ucCompare uc = new ucCompare();
                rtbBanro_DES = uc.RtbBanro_DES;
                rtbBanro_AES = uc.RtbBanro_AES;
                rtbBanma_DES = uc.RtbBanma_DES;
                rtbBanma_AES = uc.RtbBanma_AES;
                tbTocdomahoa_DES = uc.TbTocdomahoa_DES;
                tbTocdomahoa_AES = uc.TbTocdomahoa_AES;
                tbTocdogiaima_DES = uc.TbTocdogiaima_DES;
                tbTocdogiaima_AES = uc.TbTocdogiaima_AES;
                setUserControl(uc);
                rbAes.Checked = false;
                rbDes.Checked = false;
                rbAes.Enabled = false;
                rbDes.Enabled = false;
                showCompareForm = true;
                btnBangsosanh.Text = "Ẩn bảng so sánh";
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private bool checkVietnamese(string input)
        {
            Regex regex = new Regex(@"[ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặ
                                        ẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]");

            if (regex.IsMatch(input))
            {
                return true;
            }
            return false;
        }

        private void btnMahoa_Click(object sender, EventArgs e)
        {
            string banro = rtbBanro.Text;
            string khoa = rtbKhoa.Text;
            if(!showCompareForm)
            {
                if (rbDes.Checked)
                {
                    if (banro == "")
                    {
                        MessageBox.Show("Bạn chưa nhập bản rõ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbBanro.Focus();
                        return;
                    }
                    if (khoa == "")
                    {
                        MessageBox.Show("Bạn chưa nhập khóa K", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbKhoa.Focus();
                        return;
                    }
                    if (khoa.Length != 8)
                    {
                        MessageBox.Show("Khóa K phải là chuỗi độ dài 8 ký tự, độ dài hiện tại " + khoa.Length, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbKhoa.Focus();
                        return;
                    }
                    if (checkVietnamese(khoa))
                    {
                        MessageBox.Show("Khóa K không được chứa tiếng Việt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbKhoa.Focus();
                        return;
                    }
                    try
                    {
                        string encryptedText = EncryptDES(banro, khoa);
                        rtbBanma.Text = encryptedText;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Xảy ra lỗi trong quá tình mã hóa: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } 
                else if(rbAes.Checked)
                {
                    if (banro == "")
                    {
                        MessageBox.Show("Bạn chưa nhập bản rõ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbBanro.Focus();
                        return;
                    }
                    if (khoa == "")
                    {
                        MessageBox.Show("Bạn chưa nhập khóa K", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbKhoa.Focus();
                        return;
                    }
                    if (khoa.Length != 16)
                    {
                        MessageBox.Show("Khóa K phải là chuỗi độ dài 16 ký tự, độ dài hiện tại " + khoa.Length, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbKhoa.Focus();
                        return;
                    }
                    if (checkVietnamese(khoa))
                    {
                        MessageBox.Show("Khóa K không được chứa tiếng Việt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbKhoa.Focus();
                        return;
                    }
                    try
                    {
                        string encryptedText = EncryptAES(banro, khoa);
                        rtbBanma.Text = encryptedText;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Xảy ra lỗi trong quá tình mã hóa: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else
                {
                    MessageBox.Show("Bạn chưa chọn thuật toán mã hóa");
                }
            } else
            {
                if (banro == "")
                {
                    MessageBox.Show("Bạn chưa nhập bản rõ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtbBanro.Focus();
                    return;
                }
                if (khoa == "")
                {
                    MessageBox.Show("Bạn chưa nhập khóa K", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtbKhoa.Focus();
                    return;
                }
                if (khoa.Length != 8)
                {
                    MessageBox.Show("Khóa K phải là chuỗi độ dài 8 ký tự, độ dài hiện tại " + khoa.Length, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtbKhoa.Focus();
                    return;
                }
                if (checkVietnamese(khoa))
                {
                    MessageBox.Show("Khóa K không được chứa tiếng Việt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtbKhoa.Focus();
                    return;
                }
                try
                {
                    // DES
                    rtbBanro_DES.Text = banro;
                    Stopwatch desStopwatch = Stopwatch.StartNew();
                    string encryptedText = EncryptDES(banro, khoa);
                    desStopwatch.Stop();
                    rtbBanma_DES.Text = encryptedText;
                    tbTocdomahoa_DES.Text = desStopwatch.ElapsedMilliseconds.ToString();

                    // AES
                    rtbBanro_AES.Text = banro;
                    Stopwatch aesStopwatch = Stopwatch.StartNew();
                    encryptedText = EncryptAES(banro, khoa + khoa);
                    aesStopwatch.Stop();
                    rtbBanma_AES.Text = encryptedText;
                    tbTocdomahoa_AES.Text = aesStopwatch.ElapsedMilliseconds.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xảy ra lỗi trong quá tình mã hóa: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnGiaima_Click(object sender, EventArgs e)
        {
            if (!showCompareForm)
            {
                string banma = rtbBanma.Text;
                string khoa = rtbKhoa.Text;
                if (rbDes.Checked)
                {
                    if (banma == "")
                    {
                        MessageBox.Show("Bạn chưa nhập bản mã", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbBanma.Focus();
                        return;
                    }
                    if (khoa == "")
                    {
                        MessageBox.Show("Bạn chưa nhập khóa K", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbKhoa.Focus();
                        return;
                    }
                    if (khoa.Length != 8)
                    {
                        MessageBox.Show("Khóa K phải là chuỗi độ dài 8 ký tự, độ dài hiện tại " + khoa.Length, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbKhoa.Focus();
                        return;
                    }
                    if (checkVietnamese(khoa))
                    {
                        MessageBox.Show("Khóa K không được chứa tiếng Việt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbKhoa.Focus();
                        return;
                    }
                    try
                    {
                        string decryptedText = DecryptDES(banma, khoa);
                        rtbBanro.Text = decryptedText;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Xảy ra lỗi trong quá tình giải mã: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (rbAes.Checked)
                {
                    if (banma == "")
                    {
                        MessageBox.Show("Bạn chưa nhập bản mã", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbBanma.Focus();
                        return;
                    }
                    if (khoa == "")
                    {
                        MessageBox.Show("Bạn chưa nhập khóa K", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbKhoa.Focus();
                        return;
                    }
                    if (khoa.Length != 16)
                    {
                        MessageBox.Show("Khóa K phải là chuỗi độ dài 16 ký tự, độ dài hiện tại " + khoa.Length, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbKhoa.Focus();
                        return;
                    }
                    if (checkVietnamese(khoa))
                    {
                        MessageBox.Show("Khóa K không được chứa tiếng Việt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbKhoa.Focus();
                        return;
                    }
                    try
                    {
                        string decryptedText = DecryptAES(banma, khoa);
                        rtbBanro.Text = decryptedText;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Xảy ra lỗi trong quá tình giải mã: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Bạn chưa chọn thuật toán giải mã");
                }
            }
            else
            {
                string khoa = rtbKhoa.Text;
                string banma_des = rtbBanma_DES.Text;
                string banma_aes = rtbBanma_AES.Text;
                if (banma_des == "")
                {
                    MessageBox.Show("Bạn chưa nhập bản mã DES", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtbBanma_DES.Focus();
                    return;
                }
                if (banma_aes == "")
                {
                    MessageBox.Show("Bạn chưa nhập bản mã AES", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtbBanma_AES.Focus();
                    return;
                }
                if (khoa == "")
                {
                    MessageBox.Show("Bạn chưa nhập khóa K", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtbKhoa.Focus();
                    return;
                }
                if (khoa.Length != 8)
                {
                    MessageBox.Show("Khóa K phải là chuỗi độ dài 8 ký tự, độ dài hiện tại " + khoa.Length, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtbKhoa.Focus();
                    return;
                }
                if (checkVietnamese(khoa))
                {
                    MessageBox.Show("Khóa K không được chứa tiếng Việt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtbKhoa.Focus();
                    return;
                }
                try
                {
                    // DES
                    Stopwatch desStopwatch = Stopwatch.StartNew();
                    string decryptedText = DecryptDES(banma_des, khoa);
                    desStopwatch.Stop();
                    rtbBanro_DES.Text = decryptedText;
                    tbTocdogiaima_DES.Text = desStopwatch.ElapsedMilliseconds.ToString();

                    // AES
                    Stopwatch aesStopwatch = Stopwatch.StartNew();
                    decryptedText = DecryptAES(banma_aes, khoa + khoa);
                    aesStopwatch.Stop();
                    rtbBanro_AES.Text = decryptedText;
                    tbTocdogiaima_AES.Text = aesStopwatch.ElapsedMilliseconds.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xảy ra lỗi trong quá tình giải mã: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }    
        }

        private string EncryptDES(string plainText, string key)
        {
            DESCryptoServiceProvider desCrypto = new DESCryptoServiceProvider();
            desCrypto.Key = Encoding.UTF8.GetBytes(key);
            desCrypto.IV = Encoding.UTF8.GetBytes(key);

            rtbOutput.Text = "";
            rtbOutput.AppendText("Bản rõ: " + plainText + Environment.NewLine);
            byte[] plantextBytes = Encoding.UTF8.GetBytes(plainText);

            rtbOutput.AppendText("Bản rõ (UTF8): ");

            // Chuyển đổi từng cặp byte thành giá trị UTF8
            for (int i = 0; i < plantextBytes.Length; i++)
            {
                int unicodeValue = plantextBytes[i];
                rtbOutput.AppendText(unicodeValue.ToString() + "  ");
            }

            rtbOutput.AppendText(Environment.NewLine + "Bản rõ (Binary): ");
            for (int i = 0; i < plantextBytes.Length; i++)
            {
                int unicodeValue = plantextBytes[i];
                rtbOutput.AppendText(Convert.ToString(unicodeValue, 2).PadLeft(8, '0') + "  ");
            }

            //hiển thị tt khóa
            rtbOutput.AppendText(Environment.NewLine + Environment.NewLine + "Khóa: " + key);
            rtbOutput.AppendText(Environment.NewLine + "Khóa (UTF8): ");
            foreach (byte b in desCrypto.Key)
            {
                rtbOutput.AppendText(b.ToString() + "  ");
            }
            rtbOutput.AppendText(Environment.NewLine + "Khóa (Binary): ");
            foreach (byte b in desCrypto.Key)
            {
                rtbOutput.AppendText(Convert.ToString(b, 2).PadLeft(8, '0') + "  ");
            }
            using(MemoryStream ms = new MemoryStream()) 
            {
                using (CryptoStream cs = new CryptoStream(ms, desCrypto.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                }
                byte[] encryptedBytes = ms.ToArray();
                string encryptedBase64 = Convert.ToBase64String(encryptedBytes);

                rtbOutput.AppendText(Environment.NewLine + Environment.NewLine + "Bản mã (Binary): ");
                // In các bit của từng byte
                foreach (byte b in encryptedBytes)
                {
                    rtbOutput.AppendText(Convert.ToString(b, 2).PadLeft(8, '0') + "  ");
                }
                rtbOutput.AppendText(Environment.NewLine + "Bản mã (UTF8): ");
                foreach (byte b in encryptedBytes)
                {
                    rtbOutput.AppendText(b.ToString() + "  ");
                }
                rtbOutput.AppendText(Environment.NewLine + "Bản mã: " + encryptedBase64);
                return encryptedBase64;
            }
        }

        private string DecryptDES(string encryptedText, string key)
        {
            DESCryptoServiceProvider desCrypto = new DESCryptoServiceProvider();
            desCrypto.Key = Encoding.UTF8.GetBytes(key);
            desCrypto.IV = Encoding.UTF8.GetBytes(key);

            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            rtbOutput.Text = "";
            // In chi tiết từng bước vào rtbOutput
            rtbOutput.AppendText("Bản mã (Base64): " + encryptedText + Environment.NewLine);
            rtbOutput.AppendText("Bản mã (UTF8): ");
            foreach (byte b in encryptedBytes)
            {
                rtbOutput.AppendText(b.ToString() + "  ");
            }
            rtbOutput.AppendText(Environment.NewLine + "Bản mã (Binary): ");
            foreach (byte b in encryptedBytes)
            {
                rtbOutput.AppendText(Convert.ToString(b, 2).PadLeft(8, '0') + "  ");
            }
            rtbOutput.AppendText(Environment.NewLine + Environment.NewLine + "Khóa: " + key);
            rtbOutput.AppendText(Environment.NewLine + "Khóa (UTF8): ");
            foreach (byte b in desCrypto.Key)
            {
                rtbOutput.AppendText(b.ToString() + "  ");
            }
            rtbOutput.AppendText(Environment.NewLine + "Khóa (Binary): ");
            foreach (byte b in desCrypto.Key)
            {
                rtbOutput.AppendText(Convert.ToString(b, 2).PadLeft(8, '0') + "  ");
            }
            
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
            {
                using (CryptoStream cs = new CryptoStream(ms, desCrypto.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (MemoryStream decryptedMs = new MemoryStream())
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead;
                        while ((bytesRead = cs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            decryptedMs.Write(buffer, 0, bytesRead);
                        }

                        byte[] decryptedBytes = decryptedMs.ToArray();
                        rtbOutput.AppendText(Environment.NewLine + Environment.NewLine + "Bản rõ (Binary): ");
                        // In các bit của từng byte
                        foreach (byte b in decryptedBytes)
                        {
                            rtbOutput.AppendText(Convert.ToString(b, 2).PadLeft(8, '0') + "  ");
                        }
                        rtbOutput.AppendText(Environment.NewLine + "Bản rõ (UTF8): ");
                        foreach (byte b in decryptedBytes)
                        {
                            rtbOutput.AppendText(b.ToString() + "  ");
                        }
                        
                        using (StreamReader sr = new StreamReader(new MemoryStream(decryptedBytes)))
                        {
                            string decryptedText = sr.ReadToEnd();
                            rtbOutput.AppendText(Environment.NewLine + "Bản rõ: " + decryptedText);
                            return decryptedText;
                        }
                    }
                }
            }
        }

        private string EncryptAES(string plainText, string key)
        {
            using (AesCryptoServiceProvider aesCrypto = new AesCryptoServiceProvider())
            {
                aesCrypto.Key = Encoding.UTF8.GetBytes(key);
                aesCrypto.IV = Encoding.UTF8.GetBytes(key);

                rtbOutput.Text = "";
                rtbOutput.AppendText("Bản rõ: " + plainText + Environment.NewLine);
                byte[] plantextBytes = Encoding.UTF8.GetBytes(plainText);

                rtbOutput.AppendText("Bản rõ (UTF8): ");

                // Chuyển đổi từng cặp byte thành giá trị UTF8
                for (int i = 0; i < plantextBytes.Length; i++)
                {
                    int unicodeValue = plantextBytes[i];
                    rtbOutput.AppendText(unicodeValue.ToString() + "  ");
                }

                rtbOutput.AppendText(Environment.NewLine + "Bản rõ (Binary): ");
                for (int i = 0; i < plantextBytes.Length; i++)
                {
                    int unicodeValue = plantextBytes[i];
                    rtbOutput.AppendText(Convert.ToString(unicodeValue, 2).PadLeft(8, '0') + "  ");
                }

                rtbOutput.AppendText(Environment.NewLine + Environment.NewLine + "Khóa: " + key);
                rtbOutput.AppendText(Environment.NewLine + "Khóa (UTF8): ");
                foreach (byte b in aesCrypto.Key)
                {
                    rtbOutput.AppendText(b.ToString() + "  ");
                }
                rtbOutput.AppendText(Environment.NewLine + "Khóa (Binary): ");
                foreach (byte b in aesCrypto.Key)
                {
                    rtbOutput.AppendText(Convert.ToString(b, 2).PadLeft(8, '0') + "  ");
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aesCrypto.CreateEncryptor(), CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    byte[] encryptedBytes = ms.ToArray();
                    string encryptedBase64 = Convert.ToBase64String(encryptedBytes);
                    rtbOutput.AppendText(Environment.NewLine + Environment.NewLine + "Bản mã (Binary): ");
                    // In các bit của từng byte
                    foreach (byte b in encryptedBytes)
                    {
                        rtbOutput.AppendText(Convert.ToString(b, 2).PadLeft(8, '0') + "  ");
                    }
                    rtbOutput.AppendText(Environment.NewLine + "Bản mã (UTF8): ");
                    foreach (byte b in encryptedBytes)
                    {
                        rtbOutput.AppendText(b.ToString() + "  ");
                    }
                    rtbOutput.AppendText(Environment.NewLine + "Bản mã: " + encryptedBase64);
                    return encryptedBase64;
                }
            }
        }

        private string DecryptAES(string encryptedText, string key)
        {
            using (AesCryptoServiceProvider aesCrypto = new AesCryptoServiceProvider())
            {
                aesCrypto.Key = Encoding.UTF8.GetBytes(key);
                aesCrypto.IV = Encoding.UTF8.GetBytes(key);

                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

                rtbOutput.Text = "";
                // In chi tiết từng bước vào rtbOutput
                rtbOutput.AppendText("Bản mã (Base64): " + encryptedText + Environment.NewLine);
                rtbOutput.AppendText("Bản mã (UTF8): ");
                foreach (byte b in encryptedBytes)
                {
                    rtbOutput.AppendText(b.ToString() + "  ");
                }
                rtbOutput.AppendText(Environment.NewLine + "Bản mã (Binary): ");
                foreach (byte b in encryptedBytes)
                {
                    rtbOutput.AppendText(Convert.ToString(b, 2).PadLeft(8, '0') + "  ");
                }
                rtbOutput.AppendText(Environment.NewLine + Environment.NewLine + "Khóa: " + key);
                rtbOutput.AppendText(Environment.NewLine + "Khóa (UTF8): ");
                foreach (byte b in aesCrypto.Key)
                {
                    rtbOutput.AppendText(b.ToString() + "  ");
                }
                rtbOutput.AppendText(Environment.NewLine + "Khóa (Binary): ");
                foreach (byte b in aesCrypto.Key)
                {
                    rtbOutput.AppendText(Convert.ToString(b, 2).PadLeft(8, '0') + "  ");
                }

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, aesCrypto.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (MemoryStream decryptedMs = new MemoryStream())
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead;
                            while ((bytesRead = cs.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                decryptedMs.Write(buffer, 0, bytesRead);
                            }

                            byte[] decryptedBytes = decryptedMs.ToArray();
                            rtbOutput.AppendText(Environment.NewLine + Environment.NewLine + "Bản rõ (Binary): ");
                            // In các bit của từng byte
                            foreach (byte b in decryptedBytes)
                            {
                                rtbOutput.AppendText(Convert.ToString(b, 2).PadLeft(8, '0') + "  ");
                            }
                            rtbOutput.AppendText(Environment.NewLine + "Bản rõ (UTF8): ");
                            foreach (byte b in decryptedBytes)
                            {
                                rtbOutput.AppendText(b.ToString() + "  ");
                            }

                            using (StreamReader sr = new StreamReader(new MemoryStream(decryptedBytes)))
                            {
                                string decryptedText = sr.ReadToEnd();
                                rtbOutput.AppendText(Environment.NewLine + "Bản rõ: " + decryptedText);
                                return decryptedText;
                            }
                        }
                    }
                }
            }
        }

        private void mãHóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt|Word Documents|*.docx|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                FileInfo fileInfo = new FileInfo(filePath);
                long fileSizeInBytes = fileInfo.Length;

                if (fileSizeInBytes > MaxFileSizeInBytes)
                {
                    MessageBox.Show($"Kích thước của file vượt quá giới hạn cho phép ({MaxFileSizeInBytes / 1024} KB).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (Path.GetExtension(filePath).Equals(".docx", StringComparison.OrdinalIgnoreCase))
                {
                    using (DocX document = DocX.Load(filePath))
                    {
                        foreach (var paragraph in document.Paragraphs)
                        {
                            rtbBanro.AppendText(paragraph.Text);

                            if (paragraph != document.Paragraphs.Last())
                            {
                                rtbBanro.AppendText(Environment.NewLine);
                            }
                        }
                    }
                }
                else if (Path.GetExtension(filePath).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    string txtContent = File.ReadAllText(filePath);
                    rtbBanro.Text = txtContent;
                }
                else
                {
                    MessageBox.Show("Định dạng file không được hỗ trợ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void giảiMãToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt|Word Documents|*.docx|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                if (Path.GetExtension(filePath).Equals(".docx", StringComparison.OrdinalIgnoreCase))
                {
                    using (DocX document = DocX.Load(filePath))
                    {
                        string docxText = document.Text;
                        rtbBanma.Text = docxText;
                    }
                }
                else if (Path.GetExtension(filePath).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    string txtContent = File.ReadAllText(filePath);
                    rtbBanma.Text = txtContent;
                }
                else
                {
                    MessageBox.Show("Định dạng file không được hỗ trợ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bảnRõToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files|*.txt|Word Documents|*.docx|All Files|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                if (!string.IsNullOrEmpty(filePath))
                {
                    if (Path.GetExtension(filePath).Equals(".docx", StringComparison.OrdinalIgnoreCase))
                    {
                        using (DocX document = DocX.Create(filePath))
                        {
                            document.InsertParagraph(rtbBanro.Text);
                            document.Save();    
                        }

                        MessageBox.Show("Lưu bản rõ thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (Path.GetExtension(filePath).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        File.WriteAllText(filePath, rtbBanro.Text);

                        MessageBox.Show("Lưu bản rõ thành công");
                    }
                    else
                    {
                        MessageBox.Show("Định dạng file không được hỗ trợ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void bảnMãToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files|*.txt|Word Documents|*.docx|All Files|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                if (!string.IsNullOrEmpty(filePath))
                {
                    if (Path.GetExtension(filePath).Equals(".docx", StringComparison.OrdinalIgnoreCase))
                    {
                        using (DocX document = DocX.Create(filePath))
                        {
                            document.InsertParagraph(rtbBanma.Text);
                            document.Save();
                        }

                        MessageBox.Show("Lưu bản rõ thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (Path.GetExtension(filePath).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        // Lưu vào file văn bản (txt)
                        File.WriteAllText(filePath, rtbBanma.Text);

                        MessageBox.Show("Lưu bản mã thành công");
                    }
                    else
                    {
                        MessageBox.Show("Định dạng file không được hỗ trợ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                groupBox1.Width = 600;
            } else
            {
                groupBox1.Width = 380;
            }
        }
    }
}
