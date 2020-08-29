using Microsoft.AspNetCore.Mvc;
using AdminPanel.Models;

namespace AdminPanel.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private   MetaData MetaList { get; }
        public AdminController(MetaData meta)
        {
            MetaList = meta;
        }


        [HttpGet]
        public IActionResult GetMeta()
        {
            return Ok(MetaList.GetList());
        }
    }
}
