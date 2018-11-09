using System.Linq;
using System.Web.Mvc;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;
using MapBul.Web.Auth;
using MapBul.Web.Filters;
using MapBul.Web.Models;
using MapBul.Web.Repository;

namespace MapBul.Web.Controllers
{
    [Culture]
    public class UsersController : Controller
    {
        /// <summary>
        /// главная страница раздела пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public ActionResult Index()
        {
            return View();
        }

        #region partials

        /// <summary>
        /// частичное представление списка администраторов
        /// </summary>
        /// <returns></returns>
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _AdminsTablePartial()
        {
            var model = new AdminsListModel();
            return PartialView("Partial/_AdminsTablePartial", model);
        }

        /// <summary>
        /// частичное представление списка редакторов
        /// </summary>
        /// <returns></returns>
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _EditorsTablePartial()
        {
            var model = new EditorsListModel();
            return PartialView("Partial/_EditorsTablePartial", model);
        }

        /// <summary>
        /// частичное представление списка журналистов
        /// </summary>
        /// <returns></returns>
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public ActionResult _JournalistsTablePartial()
        {
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            var model = new JournalistsListModel(userGuid);
            return PartialView("Partial/_JournalistsTablePartial", model);
        }

        /// <summary>
        /// частичное представление модального окна добавления нового редактора
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _NewEditorPartial()
        {
            var model = new NewEditorModel();
            var repo = DependencyResolver.Current.GetService<IRepository>();
            ViewBag.Countries = repo.GetCountries().Where(i => i.Id != 0).ToList();
//            ViewBag.Regions = repo.GetRegions();
            ViewBag.Cities = repo.GetCities().Where(i=>i.Id!=0).ToList();
            return PartialView("Partial/_NewEditorPartial", model);
        }

        /// <summary>
        /// частичное представление модального окна внесения изменений в карточку редактора
        /// </summary>
        /// <param name="editorId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _EditorInformationPartial(int editorId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var model = new NewEditorModel(repo.GetEditor(editorId));
            ViewBag.Countries = repo.GetCountries().Where(i => i.Id != 0).ToList();
            //            ViewBag.Regions = repo.GetRegions();
            ViewBag.Cities = repo.GetCities().Where(i => i.Id != 0).ToList();

            return PartialView("Partial/_EditorInformationPartial", model);
        }

        /// <summary>
        /// частичное представление модального окна добавления нового журналиста
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public ActionResult _NewJournalistPartial()
        {
            var model = new NewJournalistModel();
            var repo = DependencyResolver.Current.GetService<IRepository>();
            ViewBag.Countries = repo.GetCountries().Where(i => i.Id != 0).ToList();
            //            ViewBag.Regions = repo.GetRegions();
            ViewBag.Cities = repo.GetCities().Where(i => i.Id != 0).ToList();
            ViewBag.Editors = repo.GetEditors();
            ViewBag.User = repo.GetUserByGuid(HttpContext.User.Identity.Name);
            return PartialView("Partial/_NewJournalistPartial", model);
        }

        /// <summary>
        /// частичное представление изменения карточки журналиста
        /// </summary>
        /// <param name="journalistId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public ActionResult _JournalistInformationPartial(int journalistId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var model = new NewJournalistModel(repo.GetJournalist(journalistId));
            ViewBag.Countries = repo.GetCountries().Where(i => i.Id != 0).ToList();
            //            ViewBag.Regions = repo.GetRegions();
            ViewBag.Cities = repo.GetCities().Where(i => i.Id != 0).ToList();
            ViewBag.Editors = repo.GetEditors();
            return PartialView("Partial/_JournalistInformationPartial", model);
        }

        /// <summary>
        /// частичное представление модального окна добавления администратора 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _NewAdminModalPartial()
        {
            var model = new NewAdminModel();
            return PartialView("Partial/_NewAdminModalPartial", model);
        }

        /// <summary>
        /// частичное представление списка гидов
        /// </summary>
        /// <returns></returns>
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public ActionResult _GuidesTablePartial()
        {
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            var model = new GuidesListModel(userGuid);
            return PartialView("Partial/_GuidesTablePartial", model);
        }

        /// <summary>
        /// частичное представление списка жителей
        /// </summary>
        /// <returns></returns>
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _TenantsTablePartial()
        {
            var model = new TenantsListModel();
            return PartialView("Partial/_TenantsTablePartial", model);
        }

        /// <summary>
        /// частичное представление внесения изменений в карточку жителя
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _TenantInformationPartial(int tenantId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var model = new NewTenantModel(repo.GetTenant(tenantId));
            return PartialView("Partial/_TenantInformationPartial", model);
        }

        /// <summary>
        /// Частичное представление внесения изменений в карточку гида
        /// </summary>
        /// <param name="guideId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public ActionResult _GuideInformationPartial(int guideId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var model = new NewGuideModel(repo.GetGuide(guideId));
            ViewBag.Countries = repo.GetCountries().Where(i => i.Id != 0).ToList();
            //            ViewBag.Regions = repo.GetRegions();
            ViewBag.Cities = repo.GetCities().Where(i => i.Id != 0).ToList();
            ViewBag.Editors = repo.GetEditors();
            ViewBag.User = repo.GetUserByGuid(HttpContext.User.Identity.Name);
            return PartialView("Partial/_GuideInformationPartial", model);
        }

        /// <summary>
        /// Частичное представление модального окна добавления гида
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public ActionResult _NewGuideModalPartial()
        {
            var model = new NewGuideModel();
            var repo = DependencyResolver.Current.GetService<IRepository>();
            ViewBag.Countries = repo.GetCountries().Where(i => i.Id != 0).ToList();
            //            ViewBag.Regions = repo.GetRegions();
            ViewBag.Cities = repo.GetCities().Where(i => i.Id != 0).ToList();
            ViewBag.Editors = repo.GetEditors();
            ViewBag.User = repo.GetUserByGuid(HttpContext.User.Identity.Name);
            return PartialView("Partial/_NewGuideModalPartial", model);
        }

        #endregion

        #region actions

        /// <summary>
        /// метод добовления нового редактора
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult AddNewEditor(NewEditorModel model)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                model.Password = StringTransformationProvider.GeneratePassword();
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

        /// <summary>
        /// метод сохранения изменений редактора
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult EditEditor(NewEditorModel model)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                repo.SaveEditorChanges(model);
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

        /// <summary>
        /// метод добавления нового журналиста
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public ActionResult AddNewJournalist(NewJournalistModel model)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                model.Password = StringTransformationProvider.GeneratePassword();
                repo.AddNewJournalist(model);
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

        /// <summary>
        /// Метод сохранения изменений журналиста
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult EditJournalist(NewJournalistModel model)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                //model.Password = StringTransformationProvider.GeneratePassword();
                repo.SaveJournalistChanges(model);
                //MailProvider.SendMailWithCredintails(model.Password,model.FirstName,model.MiddleName,model.Email);
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

        /// <summary>
        /// Метод добавления нового администратора
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult AddNewAdmin(NewAdminModel model)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                model.Password = StringTransformationProvider.GeneratePassword();
                repo.AddNewAdmin(model);
                MailProvider.SendMailWithCredintails(model.Password, "Администратор", "", model.Email);
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

        /// <summary>
        /// Метод сохранения изменений гида
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin+", "+UserTypes.Editor)]
        public ActionResult EditGuide(NewGuideModel model)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                repo.SaveGuideChanges(model);
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

        /// <summary>
        /// метод сохранения изменений жителя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult EditTenant(NewTenantModel model)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                repo.SaveTenantChanges(model);
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

        /// <summary>
        /// метод удаления администратора
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult DeleteAdmin(int adminId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            repo.DeleteAdmin(adminId);
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                Data = new { success=true }
            };
        }

        /// <summary>
        /// метод добавления нового гида
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin+", "+UserTypes.Editor)]
        public ActionResult AddNewGuide(NewGuideModel model)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                model.Password = StringTransformationProvider.GeneratePassword();
                repo.AddNewGuide(model);
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

        /// <summary>
        /// метод удаления пользователей
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public ActionResult DeleteUser(int userId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            repo.DeleteUser(userId);
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                Data = new
                {
                    success = true
                }
            };
        }

        #endregion


    }
}