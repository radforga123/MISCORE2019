using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;

namespace MISCORE2019.Controllers
{
    public class GraphController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateGraph(string patientNumber)
        {
            // Define the path to the text files
            string filePath = Path.Combine("Data", $"{patientNumber}.txt");

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                ViewBag.Error = "File not found.";
                return View("Index");
            }

            // Read the data from the file
            var data = await ReadDataFromFile(filePath);

            // Generate the graph
            var imageBytes = GenerateGraph(data);

            // Convert image bytes to base64 string for display in the view
            ViewBag.ImageData = $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";

            return View("Index");
        }

        private async Task<List<(int Minutes, int Pressure)>> ReadDataFromFile(string filePath)
        {
            var lines = await System.IO.File.ReadAllLinesAsync(filePath);
            var data = new List<(int Minutes, int Pressure)>();

            foreach (var line in lines)
            {
                var parts = Regex.Split(line.Trim(), @"\s+");
                if (parts.Length == 2 && int.TryParse(parts[0], out int minutes) && int.TryParse(parts[1], out int pressure))
                {
                    data.Add((pressure, minutes));
                }
            }

            return data;
        }

        private byte[] GenerateGraph(List<(int Minutes, int Pressure)> data)
        {
            int width = 800;
            int height = 600;
            var bitmap = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);

            if (data.Count == 0)
                return BitmapToBytes(bitmap);

            int maxX = (data.Max(d => d.Pressure) / 10 + 1) * 10;
            int maxY = (data.Max(d => d.Minutes) / 10 + 1) * 10;
            int padding = 40;

            // Draw axes
            graphics.DrawLine(Pens.Black, padding, height - padding, padding, padding);
            graphics.DrawLine(Pens.Black, padding, height - padding, width - padding, height - padding);

            // Draw axis labels
            var font = new Font("Arial", 10);
            graphics.DrawString("t (мин)", font, Brushes.Black, width - padding, height - padding + 10);
            graphics.DrawString("P (мм рт. ст.)", font, Brushes.Black, padding - 40, padding - 20);

            // Draw axis divisions
            for (int i = 0; i <= maxX; i += 10)
            {
                int x = padding + i * (width - 2 * padding) / maxX;
                graphics.DrawLine(Pens.Gray, x, height - padding, x, padding);
                graphics.DrawString(i.ToString(), font, Brushes.Black, x - 10, height - padding + 5);
            }

            for (int i = 0; i <= maxY; i += 10)
            {
                int y = height - padding - i * (height - 2 * padding) / maxY;
                graphics.DrawLine(Pens.Gray, padding, y, width - padding, y);
                graphics.DrawString(i.ToString(), font, Brushes.Black, padding - 30, y - 10);
            }

            // Draw points and lines
            var points = data.Select(d => new PointF(
                padding + (float)(d.Pressure) / maxX * (width - 2 * padding),
                height - padding - (float)(d.Minutes) / maxY * (height - 2 * padding)
            )).ToList();

            for (int i = 0; i < points.Count - 1; i++)
            {
                graphics.DrawLine(Pens.Blue, points[i], points[i + 1]);
            }
            return BitmapToBytes(bitmap);
        }

        private byte[] BitmapToBytes(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
