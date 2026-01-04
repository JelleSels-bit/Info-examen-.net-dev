

using Interimkantoor.ViewModels.Job;

namespace Interimkantoor.Configuration
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            //Job
            CreateMap<Job, JobItemViewModel>();
            CreateMap<CreateJobViewModel, Job>();
            CreateMap<EditJobViewModel, Job>();

            //Klant
            CreateMap<Klant, SelectListItem>();
            CreateMap<KlantCreateViewModel, Klant>();
            CreateMap<KlantEditViewModel, Klant>();

            //Gebruiker
            CreateMap<GebruikerCreateViewModel, CustomUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.GebruikersNaam))
                .ForMember(dest => dest.Achternaam, opt => opt.MapFrom(src => src.Naam));
            CreateMap<CustomUser, GebruikerDeleteViewModel>()
                .ForMember(dest => dest.GebruikerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Naam, opt => opt.MapFrom(src => src.Achternaam));
            CreateMap<CustomUser, GebruikerEditViewModel>().ReverseMap();
            
        }
    }
}
