using Bogus.DataSets;
using EFvsAdoNet.Models;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace EFvsAdoNet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pobierzEF_Click(object sender, EventArgs e)
        {
            PopulateDB populateDB = new PopulateDB();
            var (pracownicy, stopwatch) = populateDB.SelectEF();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = pracownicy;
            labelOutput.Text = $"Pobieranie EF: {stopwatch.ElapsedMilliseconds} ms";

        }

        private void pobierzAN_Click(object sender, EventArgs e)
        {
            PopulateDB populateDB = new PopulateDB();
            var (dataTable, stopwatch) = populateDB.SelectADO();

            dataGridView1.DataSource = dataTable;
            labelOutput.Text = $"Pobieranie ADO.NET: {stopwatch.ElapsedMilliseconds} ms";

        }

        private void btnSaveEF_Click(object sender, EventArgs e)
        {
            PopulateDB EF = new PopulateDB();
            Stopwatch stopwatch = EF.PopulateEmployeesEF((int)numberInput.Value);
            labelOutput.Text = $"Dodawanie EF: {stopwatch.ElapsedMilliseconds} ms";

        }

        private void btnSaveAdo_Click(object sender, EventArgs e)
        {
            PopulateDB EF = new PopulateDB();
            Stopwatch stopwatch = EF.PopulateEmployeesADO((int)numberInput.Value);
            labelOutput.Text = $"Dodawanie ADO.NET: {stopwatch.ElapsedMilliseconds} ms";

        }

        private void btnDeleteEF_Click(object sender, EventArgs e)
        {

            PopulateDB EF = new PopulateDB();
            Stopwatch stopwatch = EF.DeleteEF((int)numberInput.Value);
            labelOutput.Text = $"Usuwanie EF: {stopwatch.ElapsedMilliseconds} ms";
        }

        private void btnDeleteAdo_Click(object sender, EventArgs e)
        {

            PopulateDB EF = new PopulateDB();
            Stopwatch stopwatch = EF.DeleteADO((int)numberInput.Value);
            labelOutput.Text = $"Usuwanie ADO.NET: {stopwatch.ElapsedMilliseconds} ms";
        }

        private void btnUpdateEF_Click(object sender, EventArgs e)
        {

            PopulateDB EF = new PopulateDB();
            Stopwatch stopwatch = EF.UpdateEF((int)numberInput.Value);
            labelOutput.Text = $"Aktualizacja EF: {stopwatch.ElapsedMilliseconds} ms";

        }

        private void btnUpdateAdo_Click(object sender, EventArgs e)
        {

            PopulateDB EF = new PopulateDB();
            Stopwatch stopwatch = EF.UpdateADO((int)numberInput.Value);
            labelOutput.Text = $"Aktualizacja ADO.NET: {stopwatch.ElapsedMilliseconds} ms";

        }



        private void btnReset_Click(object sender, EventArgs e)
        {
            PopulateDB EF = new PopulateDB();
            EF.PopulateNewData(1000, 100);
        }

        private void btnSummaryExcel_Click(object sender, EventArgs e)
        {
            // create summary time excel with update, delete, insert and select
            // get 1000 with all times
            PopulateDB EF = new PopulateDB();
            EF.CreateSummaryExcel(10,10);
        }
    }
}
