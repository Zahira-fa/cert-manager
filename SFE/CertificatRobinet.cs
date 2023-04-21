﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace SFE
{
    public partial class CertificatRobinet : UserControl
    {
        //--------------------------------------------------------------------------------------------------------------
        MySqlConnection connexion = new MySqlConnection("Server=localhost;Port=3306;Database=technitar;Uid=root;");
        MySqlCommand cmd;
        DataTable dt = new DataTable();
        private string data = "Select certificat_robinet.id as ID ,certificat_robinet.Ref as 'Réf',certificat_robinet.Date, certificat_robinet.Num_Commande as 'N° Commande',certificat_robinet.Client , certificat_robinet.Num_TechniTar as 'N° Technitar' , "
                    + "appareil.Marque , appareil.Type , appareil.Num_serie as 'N° Série' , appareil.Reference as 'Référence', "
                    + "conditions_service_c2.Nature_Produit as 'Nature Produit'"
                    + " From certificat_robinet Inner join appareil on certificat_robinet.id = appareil.id Inner join conditions_service_c2 on appareil.id = conditions_service_c2.id ";
        //--------------------------------------------------------------------------------------------------------------
        public void IsEmpty()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                Dupliquer.Enabled = false;
                Supprimer.Enabled = false;
                Modifier.Enabled = false;
                V_Rapport.Enabled = false;
                contextMenuStrip1.Enabled = false;
            }
            else
            {
                Dupliquer.Enabled = true;
                Supprimer.Enabled = true;
                Modifier.Enabled = true;
                V_Rapport.Enabled = true;
                contextMenuStrip1.Enabled = true;

            }
        }
        public CertificatRobinet()
        {
            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
            // Fill the SqlDataSource asynchronously
            sqlDataSource1.FillAsync();
            IsEmpty();
        }
        //---------------------------------------------------------------------------------------------------------------
        public void UpLoad()
        {
            try
            {

                connexion.Open();
                MySqlDataAdapter adapt;
                adapt = new MySqlDataAdapter(data, connexion);
                dt = new DataTable();
                adapt.FillAsync(dt);
                dataGridView1.DataSource = dt;
                connexion.Close();
                dataGridView1.Refresh();
                IsEmpty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connexion.Close();
            }
        }
        public void GetSelectedRow(int a )
        {
            try
            {

                FormCertf2 form1 = new FormCertf2();
                form1.Show();
                if (a == 0)
                {
                    form1.Enregistrer.Enabled = false;

                }
                else if (a == 1)
                {
                    form1.Add.Enabled = false;
                }
                string selectedIndexS = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
                form1.id.Text = selectedIndexS;
                int index = Int32.Parse(form1.id.Text);
                string dataR = "SELECT certificat_robinet.Num_Commande , certificat_robinet.Client , certificat_robinet.Num_TechniTar , certificat_robinet.Date ," 
                    + "conditions_service_c2.Nature_Produit , conditions_service_c2.temp_Service ,conditions_service_c2.Contre_P , conditions_service_c2.P_essai , conditions_service_c2.Diametre_entre , conditions_service_c2.Diametre_Sortie , conditions_service_c2.PN , conditions_service_c2.Essais_S , conditions_service_c2.C_Essais_S ,"
                    + " appareil.Marque , appareil.Type , appareil.Num_serie"
                    + " From certificat_robinet Inner join appareil on certificat_robinet.id = appareil.id Inner join conditions_service_c2 on appareil.id = conditions_service_c2.id "
                    + "Where certificat_robinet.id = "+index+ " AND appareil.id = " + index + " AND conditions_service_c2.id = " + index;
                //------------------------
                connexion.Open();
                cmd = new MySqlCommand(dataR, connexion);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetValue(11).ToString() == "Oui" && reader.GetValue(12).ToString() == "Non")
                    {
                        form1.Oui.Checked = true;
                        form1.Non.Checked = false;
                    }
                    else if (reader.GetValue(11).ToString() == "Non" && reader.GetValue(12).ToString() == "Oui")
                    {
                        form1.Oui.Checked = false;
                        form1.Non.Checked = true;
                    }
                    form1.Num_Commande.Text = reader.GetValue(0).ToString();
                    form1.Client.Text = reader.GetValue(1).ToString();
                    form1.Num_TechniTar.Text = reader.GetValue(2).ToString();
                    form1.dateTimePicker1.Text = reader.GetValue(3).ToString(); 
                    form1.Marque.Text = reader.GetValue(13).ToString();
                    form1.Type.Text = reader.GetValue(15).ToString();
                    form1.Num_Serie.Text = reader.GetValue(15).ToString();
                    form1.Nature_produit.Text = reader.GetValue(4).ToString();
                    form1.TService.Text = reader.GetValue(5).ToString();
                    form1.Contre_p.Text = reader.GetValue(6).ToString();
                    form1.P_essai.Text = reader.GetValue(7).ToString();
                    form1.Diametre_entre.Text = reader.GetValue(8).ToString();
                    form1.Diametre_Sortie.Text = reader.GetValue(9).ToString();
                    form1.PN.Text = reader.GetValue(10).ToString();
                }
                reader.Close();
                connexion.Close();
                //------------------------


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connexion.Close();

            }
        }

        private void Add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormCertf2 form1 = new FormCertf2();
            form1.Show();
            form1.Enregistrer.Enabled = false;

        }

        private void Dupliquer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetSelectedRow(0);
        }

        private void Modifier_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetSelectedRow(1);
        }

        private void Refresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                connexion.Open();
                MySqlDataAdapter adapt;
                adapt = new MySqlDataAdapter(data, connexion);
                dt = new DataTable();
                adapt.Fill(dt);
                dataGridView1.DataSource = dt;
                connexion.Close();
                dataGridView1.Refresh();
                IsEmpty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connexion.Close();

            }
        }

        private void CertificatRobinet_Load(object sender, EventArgs e)
        {
            UpLoad();

        }
        public void Delete()
        {
            
            string selectedIndexS = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
            int selectedIndex = Int32.Parse(selectedIndexS);
            string d = "DELETE certificat_robinet , appareil , conditions_service_c2 "
                     + "From certificat_robinet Inner join appareil on certificat_robinet.id = appareil.id "
                     + "Inner join conditions_service_c2 on conditions_service_c2.id = appareil.id "
                     + "WHERE certificat_robinet.id = "+selectedIndex+ " AND appareil.id = " + selectedIndex + " AND conditions_service_c2.id = " + selectedIndex+" ;";
            try
            {
                int selectIndex = dataGridView1.CurrentCell.RowIndex;
                if (selectIndex > -1)
                {

                    MySqlCommand MyCommand2 = new MySqlCommand(d, connexion);
                    MySqlDataReader MyReader2;
                    connexion.Open();
                    MyReader2 = MyCommand2.ExecuteReader();
                    dataGridView1.Rows.RemoveAt(selectIndex);
                    MessageBox.Show("Suppression bien effectué ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    while (MyReader2.Read())
                    {
                    }
                    connexion.Close();

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connexion.Close();
            }

        }
        
        private void Supprimer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult result = MessageBox.Show("Est-ce que vous êtes sure ? ", "Supprimer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Delete();
            }
            IsEmpty();
        }


        private void V_Rapport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string selectedIndexS = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
                int selectedIndex = Int32.Parse(selectedIndexS);
                string data_Query = "INSERT IGNORE INTO query_c2 ( id , Num_Commande , Client , Num_TechniTar , Ref , Date , Marque , Type , Num_serie , Reference , Nature_Produit , temp_Service , Contre_P , P_essai , Diametre_entre , Diametre_Sortie , PN , Essais_S , C_Essais_S) " 
                    + "Select certificat_robinet.id , certificat_robinet.Num_Commande , certificat_robinet.Client , certificat_robinet.Num_TechniTar  , certificat_robinet.Ref , certificat_robinet.Date ,appareil.Marque , appareil.Type , appareil.Num_serie , appareil.Reference , conditions_service_c2.Nature_Produit , conditions_service_c2.temp_Service , conditions_service_c2.Contre_P , "
                    + "conditions_service_c2.P_essai , conditions_service_c2.Diametre_entre , conditions_service_c2.Diametre_Sortie , conditions_service_c2.PN , conditions_service_c2.Essais_S , conditions_service_c2.C_Essais_S " + "From certificat_robinet" + " Inner join appareil on certificat_robinet.id = appareil.id" + " Inner join conditions_service_c2 on conditions_service_c2.id = appareil.id" 
                    + " where certificat_robinet.id = " + selectedIndex + ";";
                string data_Query_delete = "Delete From query_c2 where id !=" + selectedIndex;
                //----------------------
                MySqlCommand MyCommand = new MySqlCommand(data_Query_delete, connexion);
                MySqlDataReader MyReader;
                connexion.Open();
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                }
                MyReader.Close();
                connexion.Close();
                //------------------------
                connexion.Open();
                cmd = new MySqlCommand(data_Query, connexion);
                dt.Load(cmd.ExecuteReader());
                connexion.Close();
                ReportFormCertif2 formCertif2 = new ReportFormCertif2();
                formCertif2.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connexion.Close();
            }
        }

        private void ImporterTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ReportCertif2 reportCertif2 = new ReportCertif2();
            reportCertif2.Show();
        }

        private void dupliquerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetSelectedRow(0);

        }

        private void modifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetSelectedRow(1);
        }

        private void visualiserSurRapportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                string selectedIndexS = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
                int selectedIndex = Int32.Parse(selectedIndexS);
                string data_Query = "INSERT IGNORE INTO query_c2 ( id , Num_Commande , Client , Num_TechniTar , Ref , Date , Marque , Type , Num_serie , Reference , Nature_Produit , temp_Service , Contre_P , P_essai , Diametre_entre , Diametre_Sortie , PN , Essais_S , C_Essais_S) "
                    + "Select certificat_robinet.id , certificat_robinet.Num_Commande , certificat_robinet.Client , certificat_robinet.Num_TechniTar  , certificat_robinet.Ref , certificat_robinet.Date ,appareil.Marque , appareil.Type , appareil.Num_serie , appareil.Reference , conditions_service_c2.Nature_Produit , conditions_service_c2.temp_Service , conditions_service_c2.Contre_P , "
                    + "conditions_service_c2.P_essai , conditions_service_c2.Diametre_entre , conditions_service_c2.Diametre_Sortie , conditions_service_c2.PN , conditions_service_c2.Essais_S , conditions_service_c2.C_Essais_S " + "From certificat_robinet" + " Inner join appareil on certificat_robinet.id = appareil.id" + " Inner join conditions_service_c2 on conditions_service_c2.id = appareil.id"
                    + " where certificat_robinet.id = " + selectedIndex + ";";
                string data_Query_delete = "Delete From query_c2 ";
                connexion.Open();
                cmd = new MySqlCommand(data_Query, connexion);
                dt.Load(cmd.ExecuteReader());
                connexion.Close();
                //----------------------
                MySqlCommand MyCommand = new MySqlCommand(data_Query_delete, connexion);
                MySqlDataReader MyReader;
                connexion.Open();
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                }
                MyReader.Close();
                connexion.Close();
                ReportFormCertif2 formCertif2 = new ReportFormCertif2();
                formCertif2.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connexion.Close();
            }

        }

        private void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Est-ce que vous êtes sure ? ", "Supprimer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Delete();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            connexion.Open();
            MySqlDataAdapter adapt;
            string data1 = "Select certificat_robinet.id , certificat_robinet.Ref , certificat_robinet.Date , certificat_robinet.Num_Commande , certificat_robinet.Client , certificat_robinet.Num_TechniTar , "
                    + "appareil.Marque , appareil.Type , appareil.Num_serie , appareil.Reference , "
                    + "conditions_service_c2.Nature_Produit "
                    + " From certificat_robinet Inner join appareil on certificat_robinet.id = appareil.id Inner join conditions_service_c2 on appareil.id = conditions_service_c2.id " 
                    + " where certificat_robinet.Num_Commande   like '%" + textBox1.Text + "%'"
                    + " OR certificat_robinet.Client like'%" + textBox1.Text + "%'"
                    + " OR certificat_robinet.Ref like'%" + textBox1.Text + "%'"
                    + " OR certificat_robinet.Num_TechniTar like'%" + textBox1.Text + "%'"
                    + " OR certificat_robinet.Date like'%" + textBox1.Text + "%'"
                    + " OR conditions_service_c2.Nature_Produit like'%" + textBox1.Text + "%'"
                    + " OR appareil.Marque like'%" + textBox1.Text + "%'"
                    + " OR appareil.Type like'%" + textBox1.Text + "%'"
                    + " OR appareil.Num_serie like'%" + textBox1.Text + "%'";

            adapt = new MySqlDataAdapter(data1, connexion);
            dt = new DataTable();
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            connexion.Close();
        }
    }
}
