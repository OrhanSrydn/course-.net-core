using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using course_.net_core.Dtos.Weapon;

namespace course_.net_core.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}