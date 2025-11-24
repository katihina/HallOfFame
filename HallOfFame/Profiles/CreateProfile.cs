using AutoMapper;
using HallOfFame.Dtos;
using HallOfFame.Models;

namespace HallOfFame.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>().ReverseMap();
            CreateMap<PersonCreateDto, Person>();
            CreateMap<Skill, SkillDto>().ReverseMap();
        }
    }
}