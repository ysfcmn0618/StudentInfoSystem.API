using AutoMapper;
using StudentInfoSystem.Data.Entities;
using StudentInfoSystem.Models;
using System.Collections.Generic;

namespace StudentInfoSystem.Mapping
{
    public class MappingProfile:Profile
    {
       
        public MappingProfile()
        {
            CreateMap<AddStudentInfoModel, StudentEntity>().ReverseMap();
            CreateMap<AddStudentInfoModel, ContactEntity>().ReverseMap();
            CreateMap<AddStudentInfoModel, LessonEntity>().ReverseMap();
            CreateMap<StudentEntity,StudentListInfoModel>().ReverseMap();
        }
    }
}
