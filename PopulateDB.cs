using Bogus;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using EFvsAdoNet.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EFvsAdoNet
{
    internal class PopulateDB
    {
        public PopulateDB()
        {
            
        }

        public void PopulateNewData(int numberOfEmployees, int numberOfDepartments)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=EFvsAdoNet.DatabaseEFContext;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command;
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    
                    command = new SqlCommand("DELETE FROM Pracowniks;", connection, transaction);
                    command.ExecuteNonQuery();
                    command = new SqlCommand(" DBCC CHECKIDENT ('Pracowniks', RESEED, 0);", connection, transaction);
                    command.ExecuteNonQuery();

                    
                   
                    var rand = new Random();
                    for (int i = 0; i < numberOfEmployees; i += 1000)
                    {
                        StringBuilder employeesQuery = new StringBuilder();
                        employeesQuery.Append("INSERT INTO Pracowniks (Imie, Nazwisko, Stanowisko) VALUES ");

                        for (int j = 0; j < 1000 && (i + j) < numberOfEmployees; j++)
                        {
                            string imie = new Faker().Name.FirstName().Replace("'", "''");
                            string nazwisko = new Faker().Name.LastName().Replace("'", "''");
                            string stanowisko = new Faker().Name.JobTitle().Replace("'", "''");

                            employeesQuery.Append($"('{imie}', '{nazwisko}', '{stanowisko}'){(j < 999 && (i + j) < numberOfEmployees - 1 ? "," : ";")}");
                        }

                        command = new SqlCommand(employeesQuery.ToString(), connection, transaction);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public Stopwatch PopulateEmployeesEF(int numberOfEmployees)
        {
            var context = new DatabaseEFContext();
            var employeeGenerator = new Faker<Pracownik>()
                .RuleFor(u => u.Imie, f => f.Name.FirstName())
                .RuleFor(u => u.Nazwisko, f => f.Name.LastName())
                .RuleFor(u => u.Stanowisko, f => f.Name.JobTitle());
            Stopwatch stopwatch = Stopwatch.StartNew(); 
            var employeesToAdd = new List<Pracownik>();
            for (int i = 0; i < numberOfEmployees; i++)
            {
                employeesToAdd.Add(employeeGenerator.Generate());
            }
            context.Pracownicy.AddRange(employeesToAdd);
            context.SaveChanges();
            stopwatch.Stop(); 
            return stopwatch;
        }

        public Stopwatch PopulateEmployeesADO(int numberOfEmployees)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=EFvsAdoNet.DatabaseEFContext;Trusted_Connection=True;";
            Stopwatch stopwatch = Stopwatch.StartNew(); 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command;
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    var employeeGenerator = new Faker<Pracownik>()
                        .RuleFor(u => u.Imie, f => f.Name.FirstName())
                        .RuleFor(u => u.Nazwisko, f => f.Name.LastName())
                        .RuleFor(u => u.Stanowisko, f => f.Name.JobTitle());
                    StringBuilder employeesQuery = new StringBuilder();
                    employeesQuery.Append("INSERT INTO Pracowniks (Imie, Nazwisko, Stanowisko) VALUES ");
                    for (int i = 0; i < numberOfEmployees; i++)
                    {
                        var employee = employeeGenerator.Generate();
                        string imie = employee.Imie.Replace("'", "''");       
                        string nazwisko = employee.Nazwisko.Replace("'", "''");
                        string stanowisko = employee.Stanowisko.Replace("'", "''");
                        employeesQuery.Append($"('{imie}', '{nazwisko}', '{stanowisko}'){((i < (numberOfEmployees - 1)) ? "," : ";")}");
                    }
                    command = new SqlCommand(employeesQuery.ToString(), connection, transaction);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            stopwatch.Stop(); 
            return stopwatch;
        }


        public Stopwatch DeleteEF(int numberOfEmployees)
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); 
            using (var context = new DatabaseEFContext())
            {
                var employees = context.Pracownicy.Take(numberOfEmployees);
                context.Pracownicy.RemoveRange(employees);
                context.SaveChanges();
            }
            stopwatch.Stop(); 
            return stopwatch;
        }
        public Stopwatch DeleteADO(int numberOfEmployees)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=EFvsAdoNet.DatabaseEFContext;Trusted_Connection=True;";
            Stopwatch stopwatch = Stopwatch.StartNew(); 

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    List<int> employeeIds = new List<int>();
                    string selectQuery = $"SELECT TOP (@numberOfEmployees) PracownikId FROM Pracowniks ORDER BY PracownikId";
                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection, transaction))
                    {
                        selectCommand.Parameters.AddWithValue("@numberOfEmployees", numberOfEmployees);
                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employeeIds.Add(reader.GetInt32(0)); 
                            }
                        }
                    }
                    if (employeeIds.Count > 0)
                    {
                        string deleteQuery = $"DELETE FROM Pracowniks WHERE PracownikId IN ({string.Join(",", employeeIds)})";
                        using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection, transaction))
                        {
                            deleteCommand.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            stopwatch.Stop(); 
            return stopwatch;
        }


        public Stopwatch UpdateEF(int numberOfEmployees)
        {
            string[] firstnames = new string[numberOfEmployees];
            string[] lastnames = new string[numberOfEmployees];

            for (int i = 0; i < numberOfEmployees; i++)
            {
                firstnames[i] = new Faker().Name.FirstName();
                lastnames[i] = new Faker().Name.LastName();
            }
            Stopwatch stopwatch = Stopwatch.StartNew(); 

            using (var context = new DatabaseEFContext())
            {
                
                var employees = context.Pracownicy.Take(numberOfEmployees).ToList();

                for (int i = 0; i < employees.Count; i++)
                {
                    employees[i].Imie = firstnames[i];
                    employees[i].Nazwisko = lastnames[i];
                }
                context.SaveChanges();
            }
            stopwatch.Stop(); 
            return stopwatch;
        }

        public Stopwatch UpdateADO(int numberOfEmployees)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=EFvsAdoNet.DatabaseEFContext;Trusted_Connection=True;";
            string[] firstnames = new string[numberOfEmployees];
            string[] lastnames = new string[numberOfEmployees];
            for (int i = 0; i < numberOfEmployees; i++)
            {
                firstnames[i] = new Faker().Name.FirstName();
                lastnames[i] = new Faker().Name.LastName();
            }
            Stopwatch stopwatch = Stopwatch.StartNew(); 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                List<int> employeeIds = new List<int>();
                string selectQuery = $"SELECT TOP {numberOfEmployees} PracownikId FROM Pracowniks ORDER BY PracownikId";
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeeIds.Add(reader.GetInt32(0)); 
                        }
                    }
                }                
                for (int i = 0; i < employeeIds.Count; i++)
                {
                    string updateQuery = "UPDATE Pracowniks SET Imie = @Imie, Nazwisko = @Nazwisko WHERE PracownikId = @PracownikId";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@Imie", firstnames[i]);
                        updateCommand.Parameters.AddWithValue("@Nazwisko", lastnames[i]);
                        updateCommand.Parameters.AddWithValue("@PracownikId", employeeIds[i]);
                        updateCommand.ExecuteNonQuery();
                    }
                }
            }
            stopwatch.Stop(); 
            return stopwatch;
        }
        public (List<Pracownik>, Stopwatch) SelectEF()
        {
            Stopwatch stopwatch = new Stopwatch();
            var pracownicy = new List<Pracownik>();
            using (var context = new DatabaseEFContext())
            {
                stopwatch = Stopwatch.StartNew();
                pracownicy = context.Pracownicy.ToList();
                stopwatch.Stop();
            }
            return (pracownicy, stopwatch);
        }
        public (DataTable, Stopwatch) SelectADO()
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=EFvsAdoNet.DatabaseEFContext;Trusted_Connection=True;";
            string query = "SELECT * FROM Pracowniks";
            DataTable dataTable = new DataTable();
            Stopwatch stopwatch = new Stopwatch();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                stopwatch.Start();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                stopwatch.Stop();
            }

            return (dataTable, stopwatch);
        }
        public void CreateSummaryExcel()
        {
            // generate 1000 times for each method
            int numberOfEmployees = 10;
            int numberIteration = 10;
            // create list of stopwatches selected methods
            List<Stopwatch> stopwatchesSelectEF = new List<Stopwatch>();
            List<Stopwatch> stopwatchesSelectAN = new List<Stopwatch>();
            List<Stopwatch> stopwatchesInsertEF = new List<Stopwatch>();
            List<Stopwatch> stopwatchesInsertAN = new List<Stopwatch>();
            List<Stopwatch> stopwatchesUpdateEF = new List<Stopwatch>();
            List<Stopwatch> stopwatchesUpdateAN = new List<Stopwatch>();
            List<Stopwatch> stopwatchesDeleteEF = new List<Stopwatch>();
            List<Stopwatch> stopwatchesDeleteAN = new List<Stopwatch>();
            // create list of stopwatches selected methods
            for (int i = 0; i < numberIteration; i++)
            {
                stopwatchesInsertEF.Add(PopulateEmployeesEF(numberOfEmployees));
                stopwatchesInsertAN.Add(PopulateEmployeesADO(numberOfEmployees));
                stopwatchesUpdateEF.Add(UpdateEF(numberOfEmployees));
                stopwatchesUpdateAN.Add(UpdateADO(numberOfEmployees));
                stopwatchesDeleteEF.Add(DeleteEF(numberOfEmployees));
                stopwatchesDeleteAN.Add(DeleteADO(numberOfEmployees));
                stopwatchesSelectEF.Add(SelectEF().Item2);
                stopwatchesSelectAN.Add(SelectADO().Item2);
            }
            // create excel file
            // create excel file
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Performance Summary");

                // Nagłówki kolumn
                worksheet.Cell(1, 1).Value = "Operation";
                worksheet.Cell(1, 2).Value = "Index";
                worksheet.Cell(1, 3).Value = "Entity Framework (ms)";
                worksheet.Cell(1, 4).Value = "ADO.NET (ms)";   
                worksheet.Cell(1, 5).Value = "Entity Framework (ms)";
                worksheet.Cell(1, 6).Value = "ADO.NET (ms)";
                worksheet.Cell(1, 7).Value = "Entity Framework (ms)";
                worksheet.Cell(1, 8).Value = "ADO.NET (ms)";

                // Zapisywanie czasów
                int row = 2; // Początek danych
                for (int i = 0; i < numberIteration; i++)
                {
                    worksheet.Cell(row + i, 1).Value = "Insert";
                    worksheet.Cell(row + i, 2).Value = i + 1; // Indeks pomiaru
                    worksheet.Cell(row + i, 3).Value = stopwatchesInsertEF[i].ElapsedMilliseconds;
                    worksheet.Cell(row + i, 4).Value = stopwatchesInsertAN[i].ElapsedMilliseconds;
                    worksheet.Cell(row + i, 5).Value = stopwatchesUpdateEF[i].ElapsedMilliseconds;
                    worksheet.Cell(row + i, 6).Value = stopwatchesUpdateAN[i].ElapsedMilliseconds;
                    worksheet.Cell(row + i, 7).Value = stopwatchesDeleteEF[i].ElapsedMilliseconds;
                    worksheet.Cell(row + i, 8).Value = stopwatchesDeleteAN[i].ElapsedMilliseconds;
                    worksheet.Cell(row + i, 9).Value = stopwatchesSelectEF[i].ElapsedMilliseconds;
                    worksheet.Cell(row + i, 10).Value = stopwatchesSelectAN[i].ElapsedMilliseconds;
                }

                // Tutaj dodaj dane dla Update, Delete itp. w podobny sposób, pamiętając o aktualizacji wartości `row`

                // Ajustowanie szerokości kolumn do zawartości
                worksheet.Columns().AdjustToContents();

                // Zapis do pliku
                workbook.SaveAs("PerformanceSummary.xlsx");
            }
        }




    }
}
