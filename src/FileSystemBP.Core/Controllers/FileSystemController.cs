using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileSystemBP.Core.BusinessLogic;
using FileSystemBP.Core.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FileSystemBP.Core.Controllers
{
    public class FileSystemController : Controller
    {
        IFileSystemManager _manager;

        public FileSystemController()
        {
            _manager = new FileSystemManager();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Files()
        {
            var files = _manager.GetAll();
            return PartialView("~/Views/Partial/FileList",files);
        }
       
        
    }
}
