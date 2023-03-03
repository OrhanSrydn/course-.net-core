using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_.net_core.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; }
        public Character? Character { get; set; }
        public int CharacterId { get; set; }
    }
}