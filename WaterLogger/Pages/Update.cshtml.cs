using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;
using WaterLogger.Models;

namespace WaterLogger.Pages
{
    public class UpdateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public DrinkingWaterModel DrinkingWater { get; set; }

        public UpdateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet(int id)
        {
            DrinkingWater = GetById(id);

            return Page();
        }

        private DrinkingWaterModel GetById(int id)
        {
            var drinkingWaterRecord = new DrinkingWaterModel();
            using (var conn = new SqliteConnection(_configuration.GetConnectionString("connectionString")))
            {
                conn.Open();

                using (var tableCmd = conn.CreateCommand())
                {
                    tableCmd.CommandText =
                        $"SELECT * FROM drinking_water WHERE Id = {id}";

                    SqliteDataReader reader = tableCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        drinkingWaterRecord.Id = reader.GetInt32(0);
                        drinkingWaterRecord.Date = DateTime.Parse(reader.GetString(1), CultureInfo.CurrentUICulture.DateTimeFormat);
                        drinkingWaterRecord.Quantity = reader.GetInt32(2);
                    }

                    return drinkingWaterRecord;
                }
            }
        }

        public IActionResult OnPost()
        {
            using (var conn = new SqliteConnection(_configuration.GetConnectionString("connectionString")))
            {
                conn.Open();
                using (var tableCmd = conn.CreateCommand())
                {
                    tableCmd.CommandText = $@"UPDATE drinking_water
                                              SET Date = '{DrinkingWater.Date}',
                                                  Quantity = {DrinkingWater.Quantity}
                                              WHERE Id = {DrinkingWater.Id}";

                    tableCmd.ExecuteNonQuery();
                }

                return RedirectToPage("./Index");
            }
        }
    }
}
