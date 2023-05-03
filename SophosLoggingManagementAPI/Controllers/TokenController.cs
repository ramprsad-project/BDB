using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SophosLoggingManagementAPI.Business;
using SophosLoggingManagementAPI.Data;
using SophosLoggingManagementAPI.Dto;
using SophosLoggingManagementAPI.Models;

namespace SophosLoggingManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class TokenController : ControllerBase
    {
        public IConfiguration configuration;
        // this will hold the Access Token returned from the server.
        static string accessToken = null;
        private readonly ILogger<TokenController> _logger;

        public TokenController(ILogger<TokenController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetTokenResponse")]
        public string GetToken()
        {
            return accessToken = TokenGeneration.GetToken().Result;
        }

        [HttpGet("GetPartner")]
        public string GetPartner()
        {
            return TokenGeneration.GetPartnerID().Result;
        }

        [HttpGet("GetTenants")]
        public string GetTenants()
        {
            return TokenGeneration.GetTenants().Result;
        }

        [HttpGet("GetTenantsAPI")]
        public string GetTenantsAPI()
        {
            return TokenGeneration.GetTenantsAPI().Result;
        }

        [HttpGet("SaveEvents")]
        public string SaveEvents()
        {
            return TokenGeneration.SaveSystemEventsToDB().Result;
        }
    }
}