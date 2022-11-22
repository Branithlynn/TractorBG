using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using TractorBG.Entity;
using TractorBG.Model;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace TractorBG.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        //----------------------------------------------- View - HomePage -----------------------------------------------//
        
        /*
         * Index IActionResult returns the user into the Index page (Home Page) of the site
         */
        public IActionResult Index()
        {
            return View();
        }

        //---------------------------------------------------------------------------------------------------------------//


        //----------------------------------------------- View - Contact ------------------------------------------------//
        
        /*
         * This ActionResult returns the user to the Contact page where the user is 
         * able to send an email to contact the support
         */
        public IActionResult Contact()
        {
            return View();
        }


        /*
         * SendEmail is the method that sends the email.
         * The method gets a model with information about the email the user wants to send.
         */
        public IActionResult SendEmail(emailModel model)
        {
            //here are initialized sove variables that store the information of the model
            var fromAddress = new MailAddress(model.email);
            var toAddress = new MailAddress("danfan12@abv.bg");
            string subject = model.name +" : "+ model.subject;
            string body = "Phone to contact : " + model.phone + ". Message : " + model.message;

            /*
             * heres the initialization of the smtp Client
             * with filling the host,port,credentials and more
             */
            var smtp = new SmtpClient
            {
                Host = "smtp.mailtrap.io",
                Port = 2525,
                EnableSsl = true,
                Credentials = new NetworkCredential("d45020d30ffd67", "49afcac18d360b")
            };

            /*
             * This is the creation of the email and after creating it its being send to
             * the smtp mail service mailtrap
             */
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

            return RedirectToAction("Contact");
        }

        //---------------------------------------------------------------------------------------------------------------//


        //----------------------------------------------- View - Shop ---------------------------------------------------//
        
        /*
         * This is the result that sends the user to the shop page where he can see 
         * the catalogue with all the tractors
         */
        public IActionResult Shop()
        {
           /* 
            * before sending the user to the view the information 
            * about the tractors is being retrived from the database 
            * and sent into the view
            */
            Context context = new Context();
            shopModel model = new shopModel();
            model.tractors = context.Tractors.ToList();

            return View(model);
        }

        public IActionResult Details(int id)
        {
            Context context = new Context();
            itemModel model = new itemModel();
            model.item = context.Tractors.Find(id);

            if (context.Tractors.Count() <= 3)
            {
                model.releatedProducts = context.Tractors.Where(x => x.id != id).ToList();
            }
            else
            {
                model.releatedProducts = context.Tractors.Where(x => x.id != id).Take(3).ToList();
            }

            if (model.item != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Shop");
            }
        }

        public FileResult GetImage(int id)
        {
            Context context = new Context();
            Tractor tractor = context.Tractors.Find(id);

            string rootPath=  _webHostEnvironment.WebRootPath;
            var path = Path.Combine(rootPath+"\\img\\tractors\\",tractor.fileName+".png");

            byte[] imageByteData = System.IO.File.ReadAllBytes(path);
            return File(imageByteData, "image/png");
        }

        //---------------------------------------------------------------------------------------------------------------//

        //----------------------------------------------- View - Account ------------------------------------------------//

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(loginRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Context context = new Context();
            foreach (var item in context.Users)
            {
                if (item.username.Equals(model.username) && item.password.Equals(model.password))
                {
                    LoggedUser.user = item;
                }
            }

            if (LoggedUser.user == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(loginRegisterModel model)
        {
            if (!ModelState.IsValid)
                return View();


            Context context = new Context();
            User user = new User();

            user.username = model.username;
            user.password = model.password;

            context.Users.Add(user);
            context.SaveChanges();

            LoggedUser.user = user;

            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            LoggedUser.user = null;
            return RedirectToAction("Index");
        }

        //---------------------------------------------------------------------------------------------------------------//

        //----------------------------------------------- View - AdminPanel ---------------------------------------------//
        public IActionResult AdminPanel()
        {
            Context context = new Context();
            adminPanelModel model = new adminPanelModel();
            model.tractorList = context.Tractors.ToList();
            model.userList = context.Users.ToList();
            return View(model);
        }
        
        [HttpGet]
        public IActionResult CreateTractor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateTractor(createTractorModel model)
        {
            Context context = new Context();
            Tractor tractor = new Tractor();

            if (model.file != null)
            {
                tractor.fileName = model.file.FileName.Substring(0,model.file.FileName.Length-4);

                var path = Path.Combine(_webHostEnvironment.WebRootPath + "\\img\\tractors", model.file.FileName);

                using var fileStream = new FileStream(path,FileMode.Create);
                model.file.CopyTo(fileStream);
            }

            
            tractor.brand = model.brand;
            tractor.model = model.model;
            tractor.year = model.year;
            tractor.description = model.description;
            
            context.Tractors.Add(tractor);
            context.SaveChanges();
            return RedirectToAction("AdminPanel");
        }

        public IActionResult DeleteTractor(int id)
        {
            Context context = new Context();
            Tractor item = context.Tractors.Find(id);

            if (item == null)
            {
                return RedirectToAction("AdminPanel");
            }

            context.Tractors.Remove(item);
            context.SaveChanges();

            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        public IActionResult UpdateTractor(int id)
        {
            Context context = new Context();
            Tractor item = context.Tractors.Find(id);
            updateTractorModel model = new updateTractorModel();

            model.id = item.id;
            model.year = item.year;
            model.description = item.description;
            model.brand = item.brand;
            model.model = item.model;

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateTractor(updateTractorModel model)
        {
            Context context = new Context();
            Tractor tractor = context.Tractors.Find(model.id);

            tractor.year = model.year;
            tractor.description = model.description;
            tractor.brand = model.brand;
            tractor.model = model.model;

            context.Entry(tractor).State = EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("AdminPanel");
        }

        public IActionResult DeleteUser(int id)
        {
            Context context = new Context();
            User user = context.Users.Find(id);

            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(createUserModel model)
        {
            Context context = new Context();
            User user = new User();

            user.username = model.username;
            user.password = model.password;
            user.admin = model.admin;

            context.Users.Add(user);
            context.SaveChanges();

            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        public IActionResult UpdateUser(int id)
        {
            Context context = new Context();
            User user = context.Users.Find(id);

            updateUserModel model = new updateUserModel();
            model.id = user.id;
            model.username = user.username;
            model.password = user.password;
            model.admin = user.admin;

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateUser(updateUserModel model)
        {
            Context context = new Context();
            User user = context.Users.Find(model.id);

            user.username = model.username;
            user.password = model.password;
            user.admin = model.admin;

            
            context.SaveChanges();

            return RedirectToAction("AdminPanel");
        }

        //---------------------------------------------------------------------------------------------------------------//
    }
}