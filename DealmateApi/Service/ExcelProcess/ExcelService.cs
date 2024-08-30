using DealmateApi.Domain.Aggregates;
using Microsoft.VisualBasic.FileIO;
using OfficeOpenXml;
using System.Globalization;
using System.Security.Claims;

namespace DealmateApi.Service.ExcelProcess;

public class ExcelService : IExcelService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExcelService(IHttpContextAccessor httpContextAccessor) 
    { 
        _httpContextAccessor = httpContextAccessor;
    }

    public List<Vehicle> VehicleProcess(IFormFile file)
    {

        // Retrieve user claims from the HTTP context
        var userClaims = _httpContextAccessor.HttpContext?.User.Claims;
        
        // Retrieve the user's name from the claims
        var name = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        // Validate and get the file extension
        var fileExtension = (file == null || file.Length == 0)
            ? throw new Exception("No file uploaded.")
            : Path.GetExtension(file.FileName).ToLower();

        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        memoryStream.Position = 0;
        var list = new List<Vehicle>();

        if (fileExtension == ".xlsx")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(memoryStream);
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension?.Rows ?? 0;

            var columnMapping = rowCount > 2 ? new Dictionary<string, int>()
                : throw new InvalidOperationException("No data found in the worksheet.");

            var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension!.Columns];

            GetCoumnMapping(1, worksheet.Dimension.Columns, columnMapping, worksheet: headerRow);
            list= VehiclesList(columnMapping, 2, rowCount, worksheet: worksheet);
        }
        else if (fileExtension == ".csv")
        {
            var columnMapping = new Dictionary<string, int>();
            var csvData = new List<string[]>();

            using (var reader = new StreamReader(memoryStream))
            {
                using var csv = new TextFieldParser(reader);
                csv.SetDelimiters(new string[] { "," });
                csv.HasFieldsEnclosedInQuotes = true;

                //Read header row to create column mappings
                if (!csv.EndOfData)
                {
                    string[]? headers = csv.ReadFields();
                    if (headers != null && headers.Length > 0)
                    {
                        for (int i = 0; i < headers.Length; i++)
                        {
                            columnMapping[headers[i].ToLower()] = i + 1;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("No data found in the CSV file.");
                    }
                }

                //Read data rows
                while (!csv.EndOfData)
                {
                    string[]? fields = csv.ReadFields();
                    if (fields != null)
                    {
                        csvData.Add(fields);
                    }
                }
            }

            //Process the CSV data into a list of vehicles
            int rowCount = csvData.Count;
            list = ParseVehicle(columnMapping, 1, rowCount, csvData);
        }
        list.ForEach(v =>
        {
            v.CreatedBy = name ?? "test";
            v.UpdatedBy = name ?? "test";
        });
        return list;
    }

    private static int? GetInt(string? value)
    {
        return int.TryParse(value, out int val) ? val : null;
    }
    private static bool GetBool(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;
        else
            return bool.TryParse(value, out bool val) ? val : false;
    }

    private static double? GetDouble(string? value)
    {
        return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double val) ? val : null;
    }
    private static DateTime? GetDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        else if (DateTime.TryParse(value.Trim(), out DateTime parsedDateTime))
            return DateTime.SpecifyKind(parsedDateTime, DateTimeKind.Utc);
        else
            return null;
    }

    private static List<Vehicle> VehiclesList(Dictionary<string, int> columnMapping, int rowIndex, int rowCount,
        ExcelWorksheet worksheet)
    {
        var vehicles = new List<Vehicle>();
        for (int i = rowIndex; i <= rowCount; i++)
        {
            rowIndex = i;
            vehicles.Add(new Vehicle
            {
                FrameNo = GetCellValue("frameno"),
                LoadNo = GetInt(GetCellValue("loadno"))??0,
                SG = GetBool(GetCellValue("sg")),
                Mirror = GetBool(GetCellValue("mirror")),
                Tools = GetBool(GetCellValue("tools")),
                ManualBook = GetBool(GetCellValue("manualbook")),
                CreatedBy = "",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = "",
                UpdatedOn = DateTime.UtcNow,
                Key = GetInt(GetCellValue("key")),
                Mileage = GetDouble(GetCellValue("mileage")),
                ManufactureDate = GetDate(GetCellValue("manufacturedate")),
                FuelType = GetCellValue("fueltype")
            });
        }
        return vehicles;

        string? GetCellValue(string columnName)
        {
            string cellText = worksheet.Cells[rowIndex, columnMapping[columnName]].Text;
            return string.IsNullOrWhiteSpace(cellText) ? null : cellText;
        }
    }

    private static void GetCoumnMapping(int init, int max, Dictionary<string, int> columnMapping, ExcelRange worksheet)
    {
        for (int i = init; i <= max; i++)
        {
            string s = worksheet[1, i].Text.ToLower().Trim();
            if (!string.IsNullOrEmpty(s))
            {
                columnMapping[s] = i;
            }
        }
    }

    private static List<Vehicle> ParseVehicle(Dictionary<string, int> columnMapping, int startRowIndex, int rowCount, List<string[]> csvData)
    {
        var vehicles = new List<Vehicle>();

        for (int i = startRowIndex; i <= rowCount; i++)
        {
            vehicles.Add(new Vehicle
            {
                FrameNo = GetCellValue("frameno",i),
                SG = GetBool(GetCellValue("sg",i)),
                LoadNo = GetInt(GetCellValue("loadno", i))??0,
                Mirror = GetBool(GetCellValue("mirror",i)),
                Tools = GetBool(GetCellValue("tools",i)),
                ManualBook = GetBool(GetCellValue("manualbook",i)),
                CreatedBy = "",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = "",
                UpdatedOn = DateTime.UtcNow,
                Key = GetInt(GetCellValue("key",i)),
                Mileage = GetDouble(GetCellValue("mileage",i)),
                ManufactureDate = DateTime.SpecifyKind(GetDate(GetCellValue("manufacturedate",i)) ?? DateTime.MinValue, DateTimeKind.Utc),
                FuelType = GetCellValue("fueltype",i)
            });
        }

        return vehicles;

        string? GetCellValue(string columnName, int rowIndex)
        {
            return string.IsNullOrEmpty(csvData[rowIndex - 1][columnMapping[columnName] - 1]) ? null :
                csvData[rowIndex - 1][columnMapping[columnName] - 1];
        }
    }

}
