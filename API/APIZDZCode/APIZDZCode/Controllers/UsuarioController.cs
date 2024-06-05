using APIZDZCode.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIZDZCode.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        
        public UsuarioController()
        {
         
        }

        [HttpPost()]
        public Usuario Get([FromBody] Usuario usuario)
        {
            return usuario;
        }
    }
}
