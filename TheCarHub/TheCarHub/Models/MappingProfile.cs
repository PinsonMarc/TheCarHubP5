using AutoMapper;
namespace TheCarHub.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarViewModel>();
            CreateMap<CarViewModel, Car>();
        }
    }
}
