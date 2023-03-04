using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using course_.net_core.Dtos.Skill;
using course_.net_core.Dtos.Weapon;

namespace course_.net_core.Dtos.Character
{
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Frodo";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defence { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
        public GetWeaponDto? Weapon { get; set; }
        public List<GetSkillDto>? Skill { get; set; }
    }
}