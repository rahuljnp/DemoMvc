using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoMvc.Models;

namespace DemoMvc.Controllers
{
    
    public class CategoryController : Controller
    {
        WebAppDBContext db = new WebAppDBContext();
        // GET: Category
        [Authorize(Roles = "V,A")]
        public ActionResult Index()
        {
            var category = db.tbl_Category.ToList();
            return View(category);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(tbl_Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.tbl_Category.Add(category);
                    db.SaveChanges();

                    TempData["msg"] = "Category added successfully";

                    return RedirectToAction("Create");
                }
                else
                {
                    return View();
                }
            }
            catch(Exception ex)
            {
                TempData["msg"] = "Category added failed"+ex.Message;
                return RedirectToAction("Create");
            }
            
        }
    }
}