using AutoMapper;
using gamitude_backend.Dto.Authorization;
using gamitude_backend.Dto.Project;
using gamitude_backend.Dto.Rank;
using gamitude_backend.Dto.User;
using gamitude_backend.Models;

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

            //Project          
            CreateMap<CreateProjectDto, Project>();
            CreateMap<Project, GetProjectDto>();
            CreateMap<UpdateProjectDto, Project>();
            CreateMap<Project, Project>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //ProjectLog
            CreateMap<CreateProjectLogDto, ProjectLog>();
            CreateMap<ProjectLog, GetProjectLogDto>();

            //Rank
            CreateMap<Rank, GetRank>();
        }
    }
}