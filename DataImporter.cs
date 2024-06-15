using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MISCORE2019.Models;
using System.IO;
using OfficeOpenXml;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using System.Data.Entity;
using System.Data.SqlClient;

namespace MISCORE2019
{

    public class DataImporter
    {
        private readonly PatientContext _context;
        public DataImporter(PatientContext context)
        {
            _context = context;
        }
        
        public async Task ImportDataAsync(string patientDataFilePath, string visitDataFilePath)
        {
            if (_context.Patients.Any())
            {
                return;
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var fileInfo = new FileInfo(patientDataFilePath);
            var patients = new List<Patient>();
            var analyzes = new List<Analyze>();
            var visits = new List<Visit>();

            try
            {
                Console.WriteLine("Reading patient data file...");
                using (var package = new ExcelPackage(new FileInfo(patientDataFilePath)))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; rowCount > 0 && row < 75; row++)
                    {
                        var patientID = ConvertSpecialID(worksheet.Cells[row, 1].Text);
                        var patientName = worksheet.Cells[row, 2].Text;
                        var weight = int.Parse(worksheet.Cells[row, 3].Text);
                        var diagnos = worksheet.Cells[row, 4].Text;

                        var patient = patients.FirstOrDefault(p => p.ID == patientID);
                        if (patient == null)
                        {
                            patient = new Patient {FIO = patientName};
                            patients.Add(patient);
                        }

                        var analyze = new Analyze {PatientID = patientID, weight = weight, diagnos = diagnos, Patient = patient };
                        analyzes.Add(analyze);
                    }
                }

                Console.WriteLine("Reading visit data file...");
                using (var package = new ExcelPackage(new FileInfo(visitDataFilePath)))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; rowCount > 0 && row < 250; row++)
                    {
                        int ID = ConvertSpecialID(worksheet.Cells[row, 1].Text);
                        var date = DateTime.ParseExact(worksheet.Cells[row, 2].Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                        var time = TimeSpan.Parse(worksheet.Cells[row, 3].Text);
                        var dateTime = date + time;
                        var patient = patients.FirstOrDefault(p => p.ID == ID);


                        var visit = new Visit {PatientID = ID, time = dateTime};
                        visits.Add(visit);
                    }
                }

                _context.Patients.AddRange(patients);

                _context.Analyzes.AddRange(analyzes);
                _context.Visits.AddRange(visits);
                await _context.SaveChangesAsync();
                Console.WriteLine("Data import completed successfully.");
            }
            catch (Exception ex)
            {
                Exception inEx = ex.InnerException;
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        
    }
        private int ConvertSpecialID(string specialID)
        {
            var numberPart = specialID.Substring(4);
            return int.Parse(numberPart);
        }
    }
}
