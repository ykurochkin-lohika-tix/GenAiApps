using AutoMapper;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Models;

namespace AskGenAi.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DisciplineOnPremises, Discipline>();
        CreateMap<QuestionOnPremises, Question>();
        CreateMap<ResponseOnPremises, Response>();
    }
}