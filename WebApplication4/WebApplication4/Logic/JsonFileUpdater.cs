using System.Text.Json;
using WebApplication3.Model;

namespace WebApplication3.Logic
{
    public class JsonFileUpdater
    {
        private readonly string _filePath;

        public JsonFileUpdater(string filePath)
        {
            _filePath = filePath;
        }

        public void UpdateBooks(List<Book> books)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(new { Books = books }, options);
            File.WriteAllText(_filePath, json);
        }
    }
}
