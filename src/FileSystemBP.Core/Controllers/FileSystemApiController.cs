using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileSystemBP.Core.Models.Dto;
using System.Threading;
using FileSystemBP.Core.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FileSystemBP.Core.Controllers
{
    [Route("api/filesystem")]
    public class FileSystemApiController : Controller
    {
        // GET: api/values


        [HttpPost]
        [Route("createfile")]
        public string CreateFile(File file)
        {
            return "";
        }

        [HttpPost]
        [Route("createfolder")]
        public string CreateFolder(File file)
        {
            return "";
        }

        [HttpPost]
        [Route("deletefile")]
        public string DeleteFile(File file)
        {
            return "";
        }

        [HttpPost]
        [Route("count")]
        public int PostCountFilesJob(CountSizeFilterDto filter)
        {
            var uri = Request.QueryString.Value.Replace("?", string.Empty);
           
            var cancelationToken = new CancellationTokenSource();
            var task = new Task<int>(() => {
                return 1;
            });

            return 3; 
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
