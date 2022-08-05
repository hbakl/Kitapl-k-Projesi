using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;


namespace Kitaplık_Projesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //access için bağlantı
        OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Hüseyin\Desktop\Udemy Projeler\Kitaplık Projesi\Kitaplık.mdb");


        void listele()
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("Select * From Kitaplar", baglanti);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            listele();
        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            listele();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut1 = new OleDbCommand("insert into Kitaplar (KitapAd,Yazar,Tur,Sayfa,Durum) values (@p1,@p2,@p3,@p4,@p5)", baglanti);
            komut1.Parameters.AddWithValue("@p1", TxtKitapAd.Text);
            komut1.Parameters.AddWithValue("@p2", TxtKitapYazar.Text);
            komut1.Parameters.AddWithValue("@p3", CmbTur.Text);
            komut1.Parameters.AddWithValue("@p4", TxtKitapSayfa.Text);


            //**********************************************************
            //Soruda bahsedilen türde if-else ile parametre atama.
            if(radioButton1.Checked)
            {
                komut1.Parameters.AddWithValue("@p5","1" );
            }
            else if(radioButton2.Checked)
            {
                komut1.Parameters.AddWithValue("@p5", "0" );
            }
            //***********************************************************

        
            komut1.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kitap başarıyla eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listele();

            //kayıt sonrası veri kutucuklarını silen kodlar
            TxtKitapAd.Text = "";
            TxtKitapId.Text = "";
            TxtKitapSayfa.Text = "";
            TxtKitapYazar.Text = "";
            CmbTur.Text = null;
            radioButton1.Checked = false;
            radioButton2.Checked = false;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //datagridviewe tıklayınca verileri kutucuklara çeken kodlar.
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            TxtKitapId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtKitapAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            TxtKitapYazar.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            CmbTur.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            TxtKitapSayfa.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            if (dataGridView1.Rows[secilen].Cells[5].Value.ToString()=="True")
            {
                radioButton1.Checked = true;
            }
            else if (dataGridView1.Rows[secilen].Cells[5].Value.ToString() == "False")
            {
                radioButton2.Checked = true;
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            //id alanı dolu ise kitabı silen kodlar
            if (TxtKitapId.Text != "")
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("delete from kitaplar where Kitapid=@p1", baglanti);
                komut.Parameters.AddWithValue("@p1", TxtKitapId.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Kitap başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listele();

                //silme sonrası veri kutucuklarını silen kodlar
                TxtKitapAd.Text = "";
                TxtKitapId.Text = "";
                TxtKitapSayfa.Text = "";
                TxtKitapYazar.Text = "";
                CmbTur.Text = null;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
            }
            else
            {
                MessageBox.Show("Lütfen listeden bir kitap seçiniz!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("update kitaplar set KitapAd=@p1, Yazar=@p2, Tur=@p3, Sayfa=@p4, Durum=@p5  where Kitapid=@p6", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtKitapAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtKitapYazar.Text);
            komut.Parameters.AddWithValue("@p3", CmbTur.Text);
            komut.Parameters.AddWithValue("@p4", TxtKitapSayfa.Text);



            if (radioButton1.Checked == true)
            {
                komut.Parameters.AddWithValue("@p5", "1");
            }
            else if(radioButton2.Checked == true)
            {
                komut.Parameters.AddWithValue("@p5", "0");
            }


            komut.Parameters.AddWithValue("@p6", TxtKitapId.Text);

            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kitap bilgisi başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listele();

            //silme sonrası veri kutucuklarını silen kodlar
            TxtKitapAd.Text = "";
            TxtKitapId.Text = "";
            TxtKitapSayfa.Text = "";
            TxtKitapYazar.Text = "";
            CmbTur.Text = null;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }

        private void BtnBul_Click(object sender, EventArgs e)
        {

            OleDbCommand komut = new OleDbCommand("select * from Kitaplar where KitapAd=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtKitapBul.Text);
            DataTable dt= new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(komut);
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }



        private void TxtAra_TextChanged(object sender, EventArgs e)
        {
            OleDbCommand komut = new OleDbCommand("select * from Kitaplar where KitapAd like '%" + TxtAra.Text + "%' ", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtKitapBul.Text);
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(komut);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
