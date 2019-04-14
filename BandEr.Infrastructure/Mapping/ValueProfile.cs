using AutoMapper;
using BandEr.DAL.DTO;
using BandEr.DAL.Entity;

namespace BandEr.Infrastructure.Mapping
{
    public class ValueProfile:Profile
    {
        public ValueProfile()
        {
            CreateMap<ValueEntity, ValueDetailDto>();
            CreateMap<ValueEntity, ValueListDto>();
            CreateMap<AddValueDto, ValueEntity>()
                .ForMember(x => x.OwnerId, c => c.Ignore())
                .ForMember(x => x.Owner, c => c.Ignore())
                .ForMember(x => x.Id, c => c.Ignore());
            CreateMap<UpdateValueDto, ValueEntity>()
                .ForMember(x => x.OwnerId, c => c.Ignore())
                .ForMember(x => x.Owner, c => c.Ignore())
                .ForMember(x => x.Id, c => c.Ignore());
        }
    }
}
