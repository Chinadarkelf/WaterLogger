using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Runtime.InteropServices;
using WaterLogger.Models;

namespace WaterLogger.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DrinkingWaterModel DrinkingWater { get; set; }

        public IActionResult OnPost()
        {
            // Validate form
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Basic SQL syntax to POST data to our database/table
            using (var conn = new SqliteConnection(_configuration.GetConnectionString("connectionString")))
            {
                conn.Open();

                using (var tableCmd = conn.CreateCommand())
                {
                    tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{DrinkingWater.Date}', {DrinkingWater.Quantity})";

                    tableCmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            // Sends user back to Index page
            return RedirectToPage("./Index");
        }
    }
}
