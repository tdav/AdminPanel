using System;
using System.Threading.Tasks;
using AdminPanel.Database;
using AdminPanel.Database.Collections;
using AdminPanel.Database.Core;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    /// <summary>
    /// Rounte "admin/modelname"
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseController<T> : Controller where T : class
    {
        private Repository<T> db;

        public BaseController(Repository<T> _db) => db = _db;


        [HttpGet()]
        public async Task<IActionResult> GetAsync()
        {
            var it = await db.GetAllAsync();
            return Ok(it);
        }
        
        //[HttpGet("dyn/{fieldName}/{value}")]
        //public async Task<IActionResult> GetAsync(string fieldName, string value)
        //{
        //    var it = await db.GetAllAsync(fieldName, value);
        //    return Ok(it);
        //}


        [HttpGet("{id}")]
        public async Task<ActionResult<T>> GetAsyncById(string id)
        {
            var res = await db.GetAsync(id);
            return Ok(res);
        }


        [HttpGet("page/{index}/{page}")]
        public async Task<ActionResult<IPagedList<T>>> GetPageAsync(int index, int page)
        {
            var res = await db.ToPagedListAsync(PageIndex: index, PageSize: page);
            return Ok(res);
        }


        [HttpPost("ins")]
        public async Task<ActionResult<T>> PostInsAsync([FromBody] T value)
        {
            var res = await db.AddAsync(value);
            return Ok(res);
        }


        [HttpPost("upd")]
        public async Task PostUpdAsync([FromBody] T value)
        {
            await db.UpdateAsync(value);
        }


        [HttpPost("del/{id}")]
        public async Task PostDelAsync(Guid id)
        {
            await db.RemoveAsync(id);
        }
    }
}
