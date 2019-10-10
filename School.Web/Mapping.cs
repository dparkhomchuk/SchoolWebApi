using AutoMapper;
using School.Entity;
using School.Interface;
using School.Models;
using School.Web.Models;

namespace School.Web
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Student, StudentDTO>();
            CreateMap<StudentDTO, Student>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Group, GroupDTO>();
            CreateMap<GroupDTO, Group>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Group,GroupDTOWithFreePlaces>()
                .ForMember(x=>x.FreePlaces,opt=>opt.MapFrom(x=>x.Capacity-x.Students.Count));

            CreateMap<StudentDTO, StudentModel>();
            CreateMap<StudentModel, StudentDTO>();

            CreateMap<GroupDTO, GroupModel>().ReverseMap();

            CreateMap<GroupDTOWithFreePlaces, GroupModelWithFreePlaces>().ReverseMap();
        }
        public static IMapper GetMapper()
        {
            var mappingConfigure = new MapperConfiguration(con =>
            {
                con.AddProfiles(new Profile[] { new Mapping() });
            });

            return mappingConfigure.CreateMapper();
        }
    }
}
