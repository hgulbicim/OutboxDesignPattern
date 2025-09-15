using AutoMapper;
using Outbox.Events.V1;
using OutboxDesignPattern.Data.Entity;

namespace OutboxDesignPattern.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<UserCreated, UserEntity>();
        }
    }
}