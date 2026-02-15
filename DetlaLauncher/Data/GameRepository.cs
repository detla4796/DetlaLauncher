using DetlaLauncher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace DetlaLauncher.Data
{
    public static class GameRepository
    {
        private static string filePath = "games.json";

        public static List<Game> Load()
        {
            if (!File.Exists(filePath))
                return new List<Game>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Game>>(json) ?? new List<Game>();
        }

        public static void Save(List<Game> games)
        {
            string json = JsonSerializer.Serialize(games, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(filePath, json);
        }
    }
}
