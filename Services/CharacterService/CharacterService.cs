global using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace course_.net_core.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private string GetUserRole() => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role)!;


        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            character.Users = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters
            .Where(u => u.Users!.Id == GetUserId())
            .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == id && c.Users!.Id == GetUserId());
                if (character is null)
                {
                    throw new Exception($"Character with Id {id} not found.");
                }
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Characters
                    .Where(c => c.Users!.Id == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;

            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = 
                GetUserRole().Equals("Admin") ? 
                await _context.Characters.ToListAsync() :
                await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skill)
                .Where(x => x.Users!.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skill)
                .FirstOrDefaultAsync(x => x.Id == id && x.Users!.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                    .Include(c => c.Users)
                    .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                if (character is null || character.Users!.Id != GetUserId())
                {
                    throw new Exception($"Character with Id {updatedCharacter.Id} not found.");
                }

                _mapper.Map(updatedCharacter, character);

                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Class = updatedCharacter.Class;
                character.Defence = updatedCharacter.Defence;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Strength = updatedCharacter.Strength;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;

            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillsDto newCharacterSkill)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skill)
                    .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.Users!.Id == GetUserId());

                if (character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found.";
                    return response;
                }

                var skill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if (skill is null)
                {
                    response.Success = false;
                    response.Message = "Skill not found.";
                    return response;
                }

                character.Skill!.Add(skill);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}