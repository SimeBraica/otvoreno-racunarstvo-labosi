using API.Models;
using API.Models.DTO;
using AutoMapper;

namespace API.Repositories.Mapper {
    public class MappingProfileMovies : Profile {
        public MappingProfileMovies()
        {
            CreateMap<MovieDTO, Filmovi>();
        }
    }
}
