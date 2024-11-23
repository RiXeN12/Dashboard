using AutoMapper;
using Dashboard.DAL.Models.Identity.NewsCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.BLL.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category mappings
            CreateMap<Category, CategoryVM>();
            CreateMap<CreateCategoryVM, Category>();
            CreateMap<UpdateCategoryVM, Category>();

            // News mappings
            CreateMap<News, NewsVM>();
            CreateMap<CreateNewsVM, News>();
            CreateMap<UpdateNewsVM, News>();
        }
    }
}
