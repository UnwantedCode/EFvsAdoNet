using Bogus;
using EFvsAdoNet.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                    
                    command = new SqlCommand("DELETE FROM Pracowniks; DELETE FROM Dzials;", connection, transaction);
                    command.ExecuteNonQuery();
                    command = new SqlCommand(" DBCC CHECKIDENT ('Pracowniks', RESEED, 0);DBCC CHECKIDENT ('Dzials', RESEED, 0);", connection, transaction);
                    command.ExecuteNonQuery();

                    
                    StringBuilder departmentsQuery = new StringBuilder();
                    departmentsQuery.Append("INSERT INTO Dzials (Nazwa) VALUES ");
                    for (int i = 1; i <= numberOfDepartments; i++)
                    {
                        departmentsQuery.Append($"('Dział {i}'){(i < numberOfDepartments ? "," : ";")}");
                    }
                    command = new SqlCommand(departmentsQuery.ToString(), connection, transaction);
                    command.ExecuteNonQuery();

                    
                    List<int> departmentIds = new List<int>();
                    command = new SqlCommand("SELECT DzialId FROM Dzials", connection, transaction);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departmentIds.Add(reader.GetInt32(0));
                        }
                    }

                    
                    var rand = new Random();
                    for (int i = 0; i < numberOfEmployees; i += 1000)
                    {
                        StringBuilder employeesQuery = new StringBuilder();
                        employeesQuery.Append("INSERT INTO Pracowniks (Imie, Nazwisko, Stanowisko, DzialId) VALUES ");

                        for (int j = 0; j < 1000 && (i + j) < numberOfEmployees; j++)
                        {
                            string imie = new Faker().Name.FirstName().Replace("'", "''");
                            string nazwisko = new Faker().Name.LastName().Replace("'", "''");
                            string stanowisko = new Faker().Name.JobTitle().Replace("'", "''");
                            int dzialId = departmentIds[rand.Next(departmentIds.Count)];  

                            employeesQuery.Append($"('{imie}', '{nazwisko}', '{stanowisko}', {dzialId}){(j < 999 && (i + j) < numberOfEmployees - 1 ? "," : ";")}");
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

            
            var departmentIds = context.Dzialy.Select(d => d.DzialId).ToList();

            
            if (!departmentIds.Any())
            {
                throw new InvalidOperationException("Brak działów w bazie danych.");
            }

            
            var employeeGenerator = new Faker<Pracownik>()
                .RuleFor(u => u.Imie, f => f.Name.FirstName())
                .RuleFor(u => u.Nazwisko, f => f.Name.LastName())
                .RuleFor(u => u.Stanowisko, f => f.Name.JobTitle())
                .RuleFor(u => u.DzialId, f => f.PickRandom(departmentIds));

            
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
                    
                    List<int> departmentIds = new List<int>();
                    string selectDepartmentsQuery = "SELECT DzialId FROM Dzials";
                    command = new SqlCommand(selectDepartmentsQuery, connection, transaction);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departmentIds.Add(reader.GetInt32(0));
                        }
                    }

                    
                    if (departmentIds.Count == 0)
                    {
                        throw new Exception("Brak działów w bazie danych.");
                    }

                    
                    var employeeGenerator = new Faker<Pracownik>()
                        .RuleFor(u => u.Imie, f => f.Name.FirstName())
                        .RuleFor(u => u.Nazwisko, f => f.Name.LastName())
                        .RuleFor(u => u.Stanowisko, f => f.Name.JobTitle())
                        .RuleFor(u => u.DzialId, f => f.PickRandom(departmentIds)); 

                    
                    for (int i = 0; i < numberOfEmployees; i += 1000)
                    {
                        StringBuilder employeesQuery = new StringBuilder();
                        employeesQuery.Append("INSERT INTO Pracowniks (Imie, Nazwisko, Stanowisko, DzialId) VALUES ");

                        for (int j = 0; j < 1000 && (i + j) < numberOfEmployees; j++)
                        {
                            var employee = employeeGenerator.Generate();
                            string imie = employee.Imie.Replace("'", "''");       
                            string nazwisko = employee.Nazwisko.Replace("'", "''");
                            string stanowisko = employee.Stanowisko.Replace("'", "''");

                            employeesQuery.Append($"('{imie}', '{nazwisko}', '{stanowisko}', {employee.DzialId}){(j < 999 && (i + j) < numberOfEmployees - 1 ? "," : ";")}");
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



    }
}
