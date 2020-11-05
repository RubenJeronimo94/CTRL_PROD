using CTRL_PROD.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CTRL_PROD.VarsAuxiliares;

namespace CTRL_PROD
{
    public partial class MainForm : Form
    {
        public VarsAuxiliares VariaveisAuxiliares = new VarsAuxiliares();


        public bool modoSimulacaoLeitura = true;

        public MainForm()
        {
            InitializeComponent();

            this.notifyIcon1.Text = this.Text;
        }

        private void ThreadInsert()
        {
            Random random = new Random();

            DateTime startdt = new DateTime(2020, 1, 1);

            using (SqlConnection sqlConn = new SqlConnection(this.VariaveisAuxiliares.DB_CONNECTION_STRING))
                while (this.VariaveisAuxiliares.FLAG_WHILE_CYCLE && startdt.Year == 2020)
                    try
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("INSERT INTO TEST_ROWS (DIAMETRO, dtupload) VALUES (@VALUE, @DATAHORA)", sqlConn))
                        {
                            sqlCmd.Parameters.AddWithValue("@VALUE", Math.Round(random.Next(1600, 1800) / 1000.0, 3));
                            sqlCmd.Parameters.AddWithValue("@DATAHORA", startdt);

                            if (sqlConn.State != ConnectionState.Open) sqlConn.Open();

                            sqlCmd.ExecuteNonQuery();
                        }

                        startdt = startdt.AddMilliseconds(500);

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("ThreadInsert(): " + ex.Message);
                    }
                    finally
                    {
                        sqlConn.Close();
                    }
        }

        private void ThreadRefreshSomeInfos()
        {
            DateTime lastUpdate = DateTime.MinValue;

            while (this.VariaveisAuxiliares.FLAG_WHILE_CYCLE)
                try
                {
                    if (DateTime.Now > lastUpdate.AddMilliseconds(this.VariaveisAuxiliares.SETTINGS_UPDATE_RATE))
                    {
                        lastUpdate = DateTime.Now;

                        if (!this.VariaveisAuxiliares.UpdateSomeInfos())
                            throw new Exception("erro ao atualizar listagem de emails!");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ThreadRefreshSomeInfos(): " + ex.Message);
                }
                finally
                {
                    Thread.Sleep(1000);
                }
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            this.VariaveisAuxiliares.LAST_LOCATION = this.Location;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Deseja realmente desligar a aplicação? Os dados máquina deixarão de ser guardados em base de dados!", "Fechar Aplicação", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                VariaveisAuxiliares.Dispose();

                foreach (TabPage page in this.tabControl1.TabPages)
                    foreach (Control control in page.Controls)
                        if (control is CtrlMaquina)
                            (control as CtrlMaquina).Dispose();
            }
            else
                e.Cancel = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text += " [" + Convert.ToString(Assembly.GetEntryAssembly().GetName().Version).Replace(",", ".") + " R1" + "]";

            this.Location = VariaveisAuxiliares.LAST_LOCATION;

            #region Update view
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this.VariaveisAuxiliares.DB_CONNECTION_STRING))
                using (SqlCommand sqlCmd = new SqlCommand("SELECT ID, Nome FROM MAQUINAS WHERE Id > 0 ORDER BY ID", sqlConn))
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = sqlCmd.ExecuteReader())
                        while (dr.Read())
                        {
                            TabPage page = new TabPage();

                            page.Text = Convert.ToString(dr[1]);

                            page.Controls.Add(new CtrlMaquina(Convert.ToInt32(dr[0]), this.VariaveisAuxiliares.DB_CONNECTION_STRING, this.VariaveisAuxiliares.iniPath));

                            page.Controls[0].Dock = DockStyle.Fill;

                            tabControl1.TabPages.Add(page);
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Error: " + ex.Message);
                Environment.Exit(0);
            }

            #endregion

            new Thread(this.ThreadRefreshSomeInfos).Start();

            VariaveisAuxiliares.FORM_LOADED = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //try
            //{
            //    bool connected = modoSimulacaoLeitura || (this.PLC != null && this.PLC.EstadoLigacao);

            //    txtStatusPLC.Text = connected ? "Ligado" : "Desligado";
            //    txtStatusDB.Text = VariaveisAuxiliares.DB_CONNECTION_STATE ? "Ligado" : "Desligado";

            //    txtElapsedTime.Text = connected ? this.PlcReadTime.ToString() : "---";

            //    if (dgvListagem.RowCount == 0)
            //    {
            //        List<string> fieldNames = typeof(Variaveis).GetFields()
            //                .Select(field => field.Name)
            //                .ToList();

            //        foreach (string var in fieldNames)
            //            dgvListagem.Rows.Add(Resources.reject, var);
            //    }

            //    if (modoSimulacaoLeitura || this.PLC != null)
            //        for (int i = 0; i < dgvListagem.RowCount; i++)
            //        {
            //            dgvListagem.Rows[i].Cells[0].Value = connected ? Resources.check_dgv : Resources.reject;
            //            dgvListagem.Rows[i].Cells[2].Value = typeof(Variaveis).GetField(dgvListagem.Rows[i].Cells[1].Value.ToString()).GetValue(null);
            //            dgvListagem.Rows[i].Cells[3].Value = connected ? DateTime.Now.ToString("HH:mm:ss.fff") : "----";

            //        }
            //}
            //catch (Exception ex)
            //{
            //    this.AdicionaLog("timer1_Tick(): " + ex.Message);
            //}
        }

        private void fecharAplicaçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void abrirVisualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.MainForm.Visible = true;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Forms.MainForm.Visible = !Forms.MainForm.Visible;
        }

        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible) this.BringToFront();
        }

        private void esconderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.MainForm.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnviaEmail mail = new EnviaEmail(this.VariaveisAuxiliares.iniPath);


            Task.Factory.StartNew(() =>
            {
                mail.SendEmail("maquina 1", "test mensagem", Forms.MainForm.VariaveisAuxiliares.ListagemEmails);
            });
        }
    }
}