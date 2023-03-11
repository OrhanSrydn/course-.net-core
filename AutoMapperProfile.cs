using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using course_.net_core.Dtos.Fight;
using course_.net_core.Dtos.Skill;
using course_.net_core.Dtos.Weapon;

namespace course_.net_core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character,GetCharacterDto>();
            CreateMap<AddCharacterDto,Character>();
            CreateMap<UpdateCharacterDto,Character>();
            CreateMap<Weapon,GetWeaponDto>();
            CreateMap<Skill,GetSkillDto>();
            CreateMap<Character,HighscoreDto>();
        }
    }
}