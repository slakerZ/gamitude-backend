using AutoMapper;
using gamitude_backend.Dto.Authorization;
using gamitude_backend.Dto.BulletJournal;
using gamitude_backend.Dto.Energy;
using gamitude_backend.Dto.Folder;
using gamitude_backend.Dto.Project;
using gamitude_backend.Dto.Rank;
using gamitude_backend.Dto.stats;
using gamitude_backend.Dto.Timer;
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
            CreateMap<UpdateProjectDto, Project>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            //ProjectLog
            CreateMap<CreateProjectLogDto, ProjectLog>();
            CreateMap<UpdateProjectLogDto, ProjectLog>();
            CreateMap<ProjectLog, GetProjectLogDto>();
            CreateMap<ProjectLog, ProjectLog>() // helper for update
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //Folder          
            CreateMap<CreateFolderDto, Folder>();
            CreateMap<Folder, GetFolderDto>();
            // CreateMap<UpdateFolderDto, Folder>();
            CreateMap<Folder, Folder>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateFolderDto, Folder>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //Rank
            CreateMap<Rank, GetRank>();

            //Stats
            CreateMap<Stats, GetStatsDto>();

            //DailyEnergy
            CreateMap<DailyEnergy, GetDailyEnergyDto>();

            //Timer          
            CreateMap<CreateTimerDto, Timer>();
            CreateMap<Timer, GetTimerDto>();
            // CreateMap<UpdateTimerDto, Timer>();
            CreateMap<Timer, Timer>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateTimerDto, Timer>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //CountDownInfo          
            CreateMap<CreateCountDownInfoDto, CountDownInfo>();
            CreateMap<CountDownInfo, GetCountDownInfoDto>();
            CreateMap<CountDownInfo, CountDownInfo>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateCountDownInfoDto, CountDownInfo>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //Journal          
            CreateMap<CreateJournalDto, Journal>();
            CreateMap<Journal, GetJournalDto>();
            // CreateMap<UpdateJournalDto, Journal>();
            CreateMap<Journal, Journal>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateJournalDto, Journal>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //Page          
            CreateMap<CreatePageDto, Page>();
            CreateMap<Page, GetPageDto>();
            // CreateMap<UpdatePageDto, Page>();
            CreateMap<Page, Page>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdatePageDto, Page>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //ProjectTask          
            CreateMap<CreateProjectTaskDto, ProjectTask>();
            CreateMap<ProjectTask, GetProjectTaskDto>();
            // CreateMap<UpdateProjectTaskDto, ProjectTask>();
            CreateMap<ProjectTask, ProjectTask>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateProjectTaskDto, ProjectTask>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //BeetwenDays          
            CreateMap<CreateBeetwenDaysDto, BeetwenDays>();
            CreateMap<BeetwenDays, GetBeetwenDaysDto>();
            CreateMap<BeetwenDays, BeetwenDays>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateBeetwenDaysDto, BeetwenDays>() // helper for update
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}