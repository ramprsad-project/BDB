using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using SophosLoggingManagementAPI.Dto;
using SophosLoggingManagementAPI.Models;
using AutoMapper;

namespace SophosLoggingManagementAPI.Mapping
{
    public class TenantProfile
    {
        public TenantProfile()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Items, ItemsDto>();
            });

            IMapper mapper = config.CreateMapper();
            var source = new Items();
            var dest = mapper.Map<Items, ItemsDto>(source);
        }
    }
}