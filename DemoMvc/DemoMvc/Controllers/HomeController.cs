using DemoMvc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DemoMvc.Controllers
{
    public class HomeController : Controller
    {
        WebAppDBContext db = new WebAppDBContext();
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Home");
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(tbl_User user)
        {
            WebAppDBContext db = new WebAppDBContext();
            //var usrs= db.tbl_User.Where(x => x.Email == user.Email && x.Password == user.Password).Count();
            // if(usrs >0)
            var usrs = db.tbl_User.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
            if (usrs !=null)
            {
                FormsAuthentication.SetAuthCookie(usrs.UserName, false);
                return RedirectToAction("UserList");
            }
            else
            { 
                return View();
            }
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(tbl_User users)
        {
            WebAppDBContext db = new WebAppDBContext();
            db.tbl_User.Add(users);
            db.SaveChanges();
            return RedirectToAction("UserList");
        }

        //public ActionResult  UserList(string SortBy, string CurrentSort, int PageNumber=1)
        //{
        //    //SortOrder = (SortOrder == null) ? "Asc" : SortOrder;
        //    //ViewBag.SortOrder = SortOrder;
        //    //SortOrder = ViewBag.SortBy == SortBy?"Desc":"Asc";

        //    ViewBag.CurrentSort = SortBy;          
        //    SortBy = String.IsNullOrEmpty(SortBy) ? "UserName" : SortBy;
            
        //    var userlist = db.tbl_User.ToList();
           
        //    userlist = applyPaging(userlist, PageNumber);
        //    applySorting(SortBy, CurrentSort,userlist);

        //    return View(userlist);
        //}
        //[HttpPost]
        //public ActionResult UserList(string SearchText)
        //{
        //    var userlist = db.tbl_User.ToList();
        //    if(SearchText!=null)
        //    {
        //        userlist = db.tbl_User.Where(x => x.UserName.Contains(SearchText)|| x.Email.Contains(SearchText)|| x.Gender.Contains(SearchText)).ToList();
        //    }
             
        //    return View(userlist);
        //}
        [Authorize(Roles="V,A")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult UserList(string SearchText, string SortBy, string CurrentSort, int PageNumber = 1)
        {
            ViewBag.CurrentSort = SortBy;
            SortBy = String.IsNullOrEmpty(SortBy) ? "UserId" : SortBy;
            ViewBag.SearchText = SearchText == null ?"" : SearchText;
            var userlist = db.tbl_User.ToList();
            if(SearchText!=null)
            {
                userlist = db.tbl_User.Where(x => x.UserName.Contains(SearchText)|| x.Email.Contains(SearchText)|| x.Gender.Contains(SearchText)).ToList();
                userlist = applySorting(SortBy, CurrentSort, userlist);
                userlist = applyPaging(userlist, PageNumber);
            }
            else
            {
                userlist = applySorting(SortBy, CurrentSort, userlist);
                userlist = applyPaging(userlist, PageNumber);
            }
            return View(userlist);
        }
        [Authorize(Roles="A")]
        public ActionResult UserProfile(int id)
        {
            var user = db.tbl_User.Find(id);
            user.IsInterestedIncSharp = (user.IsInterestedIncSharp == null) ? false : user.IsInterestedIncSharp;
            user.IsInterestedInJava = (user.IsInterestedInJava == null) ? false : user.IsInterestedInJava;
            user.IsInterestedInRuby = (user.IsInterestedInRuby == null) ? false : user.IsInterestedInRuby;
            var citylist = db.tbl_City.ToList();
            //ViewBag.CityList = new SelectList(citylist, "CityId", "CityNamr");

            user.CityList = new SelectList(citylist, "CityId", "CityName");
            if (user.ImagePath == null)
            {
                user.ImagePath = "~/Images/no-image-available.png";
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult UserProfile(tbl_User users,string cSharp, string Java, string Ruby)
        {
            users.IsInterestedIncSharp = (cSharp == "true") ? true : false;
            users.IsInterestedInJava = (Java == "true") ? true : false;
            users.IsInterestedInRuby = (Ruby == "true") ? true : false;

            if(users.UserImageFile!=null)
            {
                string filename = Path.GetFileNameWithoutExtension(users.UserImageFile.FileName);
                string extension = Path.GetExtension(users.UserImageFile.FileName);
                filename = filename + DateTime.Now.ToString("yymmssff") + extension;
                users.ImagePath = "~/Images/" + filename;

                //save file in folder
                filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                users.UserImageFile.SaveAs(filename);
            }
            if(users.ImagePath== "/Images/no-image-available.png")
            {
                users.ImagePath = null;
            }
            db.Entry(users).State = System.Data.Entity.EntityState.Modified;           
            db.SaveChanges();

            var usrupdated = db.tbl_User.Find(users.UserId);
            //return RedirectToAction("UserList");
            return RedirectToAction("UserProfile", new { id=users.UserId});
        }
        

        public List<tbl_User> applySorting(string SortBy, string CurrentSort,List<tbl_User> userlist)
        {
            switch (SortBy)
            {
                case "UserName":
                    {
                        userlist = SortBy.Equals(CurrentSort) ? userlist.OrderByDescending(x => x.UserName).ToList() : userlist.OrderBy(x => x.UserName).ToList();
                        //if (SortOrder.Equals("Asc"))
                        //{ userlist = db.tbl_User.OrderBy(x => x.UserName).ToList(); }else { userlist = db.tbl_User.OrderByDescending(x => x.UserName).ToList(); }                        
                        break;
                    }
                case "Email":
                    {
                        userlist = SortBy.Equals(CurrentSort) ? userlist.OrderByDescending(x => x.Email).ToList() : userlist.OrderBy(x => x.Email).ToList();
                        //if (SortOrder.Equals("Asc"))
                        //{ userlist = db.tbl_User.OrderBy(x => x.Email).ToList(); }
                        //else { userlist = db.tbl_User.OrderByDescending(x => x.Email).ToList(); }
                        break;
                    }
                case "Password":
                    {
                        userlist = SortBy.Equals(CurrentSort) ? userlist.OrderByDescending(x => x.Password).ToList() : userlist.OrderBy(x => x.Password).ToList();
                        //if (SortOrder.Equals("Asc"))
                        //{ userlist = db.tbl_User.OrderBy(x => x.Password).ToList(); }
                        //else { userlist = db.tbl_User.OrderByDescending(x => x.Password).ToList(); }
                        break;
                    }
                case "Gender":
                    {
                        userlist = SortBy.Equals(CurrentSort) ? userlist.OrderByDescending(x => x.Gender).ToList() : userlist.OrderBy(x => x.Gender).ToList();
                        //if (SortOrder.Equals("Asc"))
                        //{ userlist = db.tbl_User.OrderBy(x => x.Gender).ToList(); }
                        //else { userlist = db.tbl_User.OrderByDescending(x => x.Gender).ToList(); }
                        break;
                    }
                default:
                    {
                        // if(SortOrder.Equals("Asc")) { userlist = db.tbl_User.OrderBy(x => x.UserId).ToList(); }else { userlist = db.tbl_User.OrderByDescending(x => x.UserId).ToList(); }
                        userlist = userlist.OrderByDescending(x => x.UserId).ToList();
                        break;
                    }
            }
            return userlist;
        }
        public List<tbl_User> applyPaging(List<tbl_User> userlist,int PageNumber)
        {
            int itemcount = 5;
            ViewBag.TotalPages = System.Math.Ceiling(userlist.Count() / Convert.ToDouble(itemcount));
            ViewBag.PageNumber = PageNumber;
            userlist = userlist.Skip((PageNumber - 1) * itemcount).Take(itemcount).ToList();
            return userlist;
        }
    }
}