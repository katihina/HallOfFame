using HallOfFame.Dtos;

namespace HallOfFame.Services
{
    public interface IPersonService
    {
        Task<List<PersonDto>> GetAllAsync();
        Task<PersonDto?> GetByIdAsync(long id);
        Task<PersonDto> CreateAsync(PersonCreateDto dto);
        Task<PersonDto?> UpdateAsync(long id, PersonCreateDto dto);
        Task<bool> DeleteAsync(long id);
    }
}