using AutoMapper;
using gamitude_backend.Dto.Authorization;
using gamitude_backend.Dto.User;
using gamitude_backend.Model;

namespace gamitude_backend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //User          
            CreateMap<CreateUserDto, User>();
            CreateMap<User, GetUserDto>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, User>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //UserToken
            CreateMap<UserToken, GetUserTokenDto>();

        }
    }
}