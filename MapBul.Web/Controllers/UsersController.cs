using System.Web.Mvc;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;
using MapBul.Web.Auth;
using MapBul.Web.Models;
using MapBul.Web.Repository;

namespace MapBul.Web.Controllers
{
    public class UsersController : Controller
    {
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult Index()
        {
            return View();
        }

#region partials
        
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _AdminsTablePartial()
        {
            AdminsListModel model=new AdminsListModel();
            return PartialView("Partial/_AdminsTablePartial", model);
        }

        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _EditorsTablePartial()
        {
            EditorsListModel model = new EditorsListModel();
            return PartialView("Partial/_EditorsTablePartial", model);
        }

        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _JournalistsTablePartial()
        {
            JournalistsListModel model = new JournalistsListModel();
            return PartialView("Partial/_JournalistsTablePartial", model);
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _NewEditorPartial()
        {
            var model=new NewEditorModel();
            var repo = DependencyResolver.Current.GetService<IRepository>();
            ViewBag.Countries = repo.GetCountries();
            ViewBag.Regions = repo.GetRegions();
            ViewBag.Cities = repo.GetCities();
            return PartialView("Partial/_NewEditorPartial", model);
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _EditorInformationPartial(int editorId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            NewEditorModel model=new NewEditorModel(repo.GetEditor(editorId));
            ViewBag.Countries = repo.GetCountries();
            ViewBag.Regions = repo.GetRegions();
            ViewBag.Cities = repo.GetCities();

            return PartialView("Partial/_EditorInformationPartial",model);
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _NewJournalistPartial()
        {
            var model = new NewJournalistModel();
            var repo = DependencyResolver.Current.GetService<IRepository>();
            ViewBag.Countries = repo.GetCountries();
            ViewBag.Regions = repo.GetRegions();
            ViewBag.Cities = repo.GetCities();
            ViewBag.Editors = repo.GetEditors();
            return PartialView("Partial/_NewJournalistPartial", model);
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _JournalistInformationPartial(int journalistId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            NewJournalistModel model = new NewJournalistModel(repo.GetJournalist(journalistId));
            ViewBag.Countries = repo.GetCountries();
            ViewBag.Regions = repo.GetRegions();
            ViewBag.Cities = repo.GetCities();
            ViewBag.Editors = repo.GetEditors();
            return PartialView("Partial/_JournalistInformationPartial", model);
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _NewAdminModalPartial()
        {
            NewAdminModel model = new NewAdminModel();
            return PartialView("Partial/_NewAdminModalPartial", model);
        }

#endregion

#region actions
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult AddNewEditor(NewEditorModel model)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                model.Password = TransformationProvider.GeneratePassword();
                repo.AddNewEditor(model);
                MailProvider.SendMailWithCredintails(model.Password, model.FirstName, model.MiddleName, model.Email);
            }
            catch (MyException e)
            {
                    return new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                        Data = new {success = false, errorReason = e.Error.Message}
                    };
            }
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                Data = new {success = true}
            };
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult EditEditor(NewEditorModel model)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                //model.Password = TransformationProvider.GeneratePassword();
                repo.SaveEditorChanges(model);
                //MailProvider.SendMailWithCredintails(model.Password,model.FirstName,model.MiddleName,model.Email);
            }
            catch (MyException e)
            {
                    return new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                        Data = new { success = false, errorReason = e.Error.Message}
                    };
            }
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                Data = new { success = true }
            };
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult AddNewJournalist(NewJournalistModel model)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                model.Password = TransformationProvider.GeneratePassword();
                repo.AddNewJournalist(model);
                MailProvider.SendMailWithCredintails(model.Password, model.FirstName, model.MiddleName, model.Email);
            }
            catch (MyException e)
            {
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                    Data = new { success = false, errorReason = e.Error.Message }
                };
            }
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                Data = new { success = true }
            };
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult EditJournalist(NewJournalistModel model)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                //model.Password = TransformationProvider.GeneratePassword();
                repo.SaveJournalistChanges(model);
                //MailProvider.SendMailWithCredintails(model.Password,model.FirstName,model.MiddleName,model.Email);
            }
            catch (MyException e)
            {
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                    Data = new { success = false, errorReason = e.Error.Message }
                };
            }
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                Data = new { success = true }
            };
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult AddNewAdmin(NewAdminModel model)
        {
            
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                model.Password = TransformationProvider.GeneratePassword();
                repo.AddNewAdmin(model);
                MailProvider.SendMailWithCredintails(model.Password, "Администратор", "", model.Email);
            }
            catch (MyException e)
            {
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                    Data = new { success = false, errorReason = e.Error.Message }
                };
            }
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                Data = new { success = true }
            };
        }

#endregion

    }
}