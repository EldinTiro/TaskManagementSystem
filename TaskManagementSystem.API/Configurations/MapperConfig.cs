using AutoMapper;
using TaskManagementSystem.API.Data;
using TaskManagementSystem.API.Models.Tasks;

namespace TaskManagementSystem.API.Configurations;

public class MapperConfig: Profile
{
    public MapperConfig()
    {
        CreateMap<TaskEntity, CreateTaskDto>().ReverseMap();
        CreateMap<TaskEntity, GetTaskDto>().ReverseMap();
        CreateMap<TaskEntity, UpdateTaskDto>().ReverseMap();
    }
}