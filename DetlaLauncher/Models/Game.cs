using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetlaLauncher.Models
{
    public class Game
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public int Rating { get; set; }
        public bool IsFavorite { get; set; }
        public string Description { get; set; }
        public List<GameSource> Sources { get; set; }

        public Game()
        {
            Sources = new List<GameSource>();
        }
    }
}

