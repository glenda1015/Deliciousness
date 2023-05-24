using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Web.Mvc;
using WebApplication.Models;
using System.Security.Cryptography;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private Database_Entities _db = new Database_Entities();

        // GET: Home
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //GET: the register
        public ActionResult Register()
        {
            return View();
        }

        //POST: the register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = _db.Users.FirstOrDefault(s => s.UserName == _user.UserName);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.Users.Add(_user);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Username already exists";
                    return View();
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User infoLogin)
        {

            var f_pw = GetMD5(infoLogin.Password);

            var data = _db.Users.Where(s => s.UserName.Equals(infoLogin.UserName) && s.Password.Equals(f_pw)).ToList();

            if (data.Count() > 0)
            {
                //add session
                Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                Session["Username"] = data.FirstOrDefault().UserName;
                Session["ID"] = data.FirstOrDefault().ID;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Notification = "Oh no! Login failed. Double-check your Username or Password";
                return View();
            }

        }
        

        //create a string "MD5"
        public static string GetMD5(string str)
        {
            if (str == null)
                return " ";

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2string = null;

            //converter function byte to string
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2string += targetData[i].ToString("x2");
            }
            return byte2string;
        }

        public ActionResult Recipes()
        {

            if (Session["ID"] != null)
            {
                using (_db)
                {
                    return View(_db.Posts.ToList());

                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult MyAccount()
        {
            if (Session["ID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //GET: the recipe to post
        [HttpGet]
        public ActionResult PostRecipe()
        {
            if (Session["ID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //POST: the recipe
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult PostRecipe(Post model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile == null)
                {
                    ViewBag.Notification = "Oh no! A recipe photo is missing";

                    return View();
                }
                else
                {
                    string filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string extension = Path.GetExtension(model.ImageFile.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                    model.PostPhoto = "../RecipeImg/" + filename;
                    filename = Path.Combine(Server.MapPath("../RecipeImg/"), filename);
                    model.ImageFile.SaveAs(filename);

                    using (_db)
                    {
                        var session = Session["ID"];
                        model.ID = Convert.ToInt32(session);
                        _db.Configuration.ValidateOnSaveEnabled = false;
                        _db.Posts.Add(model);
                        _db.SaveChanges();
                    }

                    ModelState.Clear();
                    return RedirectToAction("MyAccount");
                }
            }
            return View();
          
        }
       
        public ActionResult YourPosts()
        {
            var session = Session["ID"];
            int sess = Convert.ToInt32(session);
            if (Session["ID"] != null)
            {
                using (_db)
                {
                    List<Post> post = _db.Posts.Where(x => x.ID == sess).ToList();
                    return View(post);

                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult Delete(int id)
        {
            var post = _db.Posts.Find(id);
            _db.Posts.Remove(post);
            _db.SaveChanges();
           
            return RedirectToAction("YourPosts");

        }

        public ActionResult Fav(Favorite model, int id)
        {
            if (Session["ID"] != null)
            {
                using (_db)
                {
                    var session = Session["ID"];
                    int sess = Convert.ToInt32(session);

                    List<Favorite> post = _db.Favorites.Where(x => x.ID == sess && x.FavPostID == id).ToList();

                    if (post.Count() == 0)
                    {
                        model.ID = sess;
                        model.FavPostID = id;
                        _db.Configuration.ValidateOnSaveEnabled = false;
                        _db.Favorites.Add(model);
                        _db.SaveChanges();
                        return RedirectToAction("Recipes");

                    }
                    else
                    {
                        return RedirectToAction("Recipes");

                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public ActionResult AddToFav(Favorite model)
        {
            if (Session["ID"] != null)
            {
                using (_db)
                {
                    var session = Session["ID"];
                    int sess = Convert.ToInt32(session);
                    var data = (from p in _db.Posts join f in _db.Favorites on p.PostID equals f.FavPostID where f.ID == sess select new ViewModel { PostVM = p, FavoriteVM = f }).ToList();
                    return View(data);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult About()
        {
            if (Session["ID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon(); //removes the session
            return RedirectToAction("Login");
        }
    }
}