using AutoMapper;
using BeastsGym.BLL.ViewModels.SessionViewModels;
using BeastsGym.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.BLL.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapSession();
        }

        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                                                  .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                                                  .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore()).ReverseMap();

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }
    }
}
