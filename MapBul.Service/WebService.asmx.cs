using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Services;
using MapBul.DBContext;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;
using Newtonsoft.Json;

namespace MapBul.Service
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://MapBul.ru/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        #region private

        private List<int> GetCategoriesBranch(category category)
        {
            var result = new List<int>();
            var currentCategory = category;
            while (currentCategory != null)
            {
                result.Add(currentCategory.Id);
                currentCategory = currentCategory.category2;
            }
            return result;
        }

        private string MapUrl(string filePath)
        {
            return "http://" + HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"] + "/" + filePath;
        }

        private object GetUserDescriptor(user user)
        {
            switch (user.usertype.Tag)
            {
                case UserTypes.Editor:
                    return user.editor.First();
                case UserTypes.Journalist:
                    return user.journalist.First();
                case UserTypes.Tenant:
                    return user.tenant.First();
                case UserTypes.Guide:
                    return user.guide.First();
                default:
                    throw new MyException(Errors.NotFound);
            }
        }

        #endregion

        #region webMethods

        [WebMethod]
        public string Authorize(string email, string password)
        {
            try
            {
                MySqlRepository repo = new MySqlRepository();
                var user = repo.GetUserByEmailAndPassword(email, password); //вытащили пользователя

                dynamic userDescriptor = GetUserDescriptor(user);

                var userType = repo.GetMobileUserTypeById(user.UserTypeId); //вытащили его тип
                var result =
                    new JsonResult(new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            {"FirstName", userDescriptor.FirstName},
                            {"UserTypeTag", userType.Tag},
                            {"UserTypeDescription", userType.Description}
                        }
                    });
                result.AddObjectToResult(user, 0);
                return JsonConvert.SerializeObject(result);
            }
            catch (MyException e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Error.Message));
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Message));
            }
        }

        [WebMethod]
        public string GetUserTypeById(int id, string userGuid)
        {
            try
            {
                MySqlRepository repo = new MySqlRepository();
                usertype userType = repo.GetMobileUserTypeById(id);
                var result = new JsonResult(new List<Dictionary<string, object>>());
                result.AddObjectToResult(userType, 0);
                return JsonConvert.SerializeObject(result);
            }
            catch (MyException e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Error.Message));
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Message));
            }
        }


        [WebMethod]
        public string GetMarkers(double p1Lat, double p1Lng, double p2Lat, double p2Lng)
        {
            try
            {
                MySqlRepository repo = new MySqlRepository();
                var markers = repo.GetMarkersInSquare(p1Lat, p1Lng, p2Lat, p2Lng);

                JsonResult result = new JsonResult(new List<Dictionary<string, object>>());

                int i = 0;

                foreach (var marker in markers)
                {
                    result.AddObjectToResult(
                        new
                        {
                            marker.Id,
                            marker.Name,
                            marker.Lat,
                            marker.Lng,
                            Icon = MapUrl(marker.category.Pin),
                            Logo = MapUrl(marker.Logo),
                            WorkTime = marker.worktime.Select(wt => new
                            {
                                wt.OpenTime,
                                wt.CloseTime,
                                wt.weekday.Id
                            }).ToList(),
                            CategoriesBranch = GetCategoriesBranch(marker.category),
                            SubCategories = marker.subcategory.Select(s => s.category.Name).ToList(),
                            marker.Wifi
                        }, i);
                    i++;
                }

                return JsonConvert.SerializeObject(result);
            }
            catch (MyException e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Error.Message));
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Message));
            }
        }


        [WebMethod]
        public string GetSessionMarkers(double p1Lat, double p1Lng, double p2Lat, double p2Lng,string sessionId)
        {
            try
            {
                MySqlRepository repo = new MySqlRepository();
                var markers = repo.GetMarkersInSquare(p1Lat, p1Lng, p2Lat, p2Lng, sessionId);

                JsonResult result = new JsonResult(new List<Dictionary<string, object>>());

                int i = 0;

                foreach (var marker in markers)
                {
                    result.AddObjectToResult(
                        new
                        {
                            marker.Id,
                            marker.Name,
                            marker.Lat,
                            marker.Lng,
                            Icon = MapUrl(marker.category.Pin),
                            Logo = MapUrl(marker.Logo),
                            WorkTime = marker.worktime.Select(wt => new
                            {
                                wt.OpenTime,
                                wt.CloseTime,
                                wt.weekday.Id
                            }).ToList(),
                            CategoriesBranch = GetCategoriesBranch(marker.category),
                            SubCategories = marker.subcategory.Select(s => s.category.Name).ToList(),
                            marker.Wifi
                        }, i);
                    i++;
                }

                return JsonConvert.SerializeObject(result);
            }
            catch (MyException e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Error.Message));
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Message));
            }
        }

        [WebMethod]
        public string RemoveRequestSession(string sessionId)
        {
            try
            {
                MySqlRepository repo = new MySqlRepository();
                repo.RemoveRequestSession(sessionId);

                JsonResult result = new JsonResult(new List<Dictionary<string, object>>());
                return JsonConvert.SerializeObject(result);
            }
            catch (MyException e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Error.Message));
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Message));
            }
        }


        [WebMethod]
        public string GetMarkerDescription(int markerId)
        {
            var repo = new MySqlRepository();
            marker marker = repo.GetMarker(markerId);
            JsonResult result = new JsonResult(new List<Dictionary<string, object>>());

            var categories = repo.GetMarkerCategories();

            marker.Photo = MapUrl(marker.Photo);
            marker.Logo = MapUrl(marker.Logo);
            result.AddObjectToResult(marker, 0);
            result.AddObjectToResult(new
            {
                Phones = marker.phone.Select(p => new {p.Primary, p.Number}).ToList(),
                Discount = marker.discount.Value,
                Subcategories = marker.subcategory.Select(s => new {s.category.Id, s.category.Name}).ToList(),
                CityName = marker.city.Name,
                Pin = MapUrl(marker.category.Pin),
                Icon = MapUrl(marker.category.Icon),
                WorkTime = marker.worktime.Select(wt => new
                {
                    wt.OpenTime,
                    wt.CloseTime,
                    wt.weekday.Id
                }).ToList(),
                repo.GetMarkerCategories().First(c=>c.Id==GetCategoriesBranch(marker.category).Last()).Color,
                CategoriesBranch =
                    GetCategoriesBranch(marker.category)
                        .Select(
                            number =>
                                new
                                {
                                    categories.First(c => c.Id == number).Id,
                                    categories.First(c => c.Id == number).Name
                                })
                        .ToList()
            }, 0);
            return JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string GetRootCategories()
        {
            MySqlRepository repo = new MySqlRepository();
            List<category> rootCategories = repo.GetRootMarkerCategories();
            int index = 0;
            var result = new JsonResult(new List<Dictionary<string, object>>());
            foreach (var rootCategory in rootCategories)
            {
                rootCategory.Icon = MapUrl(rootCategory.Icon);
                rootCategory.Pin = MapUrl(rootCategory.Pin);
                List<category> childCategories = repo.GetChildCategories(rootCategory.Id);
                result.AddObjectToResult(rootCategory, index);
                result.AddObjectToResult(
                    new
                    {
                        ChildCategories =
                            childCategories.Select(
                                c =>
                                    new
                                    {
                                        c.Id,
                                        c.ParentId,
                                        c.AddedDate,
                                        Pin = MapUrl(c.Pin),
                                        Icon = MapUrl(c.Icon),
                                        c.Name,
                                        c.Color
                                    }).ToList()
                    }, index);
                index++;
            }
            return JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string GetRecentArticles(bool refresh = false, DateTime? existingDateTime = null)
        {
            MySqlRepository repo = new MySqlRepository();
            List<article> articles = repo.GetArticles();
            var result = new JsonResult(new List<Dictionary<string, object>>());
            int i = 0;
            List<article> filteredArticles;

            if (existingDateTime != null && refresh)
                filteredArticles =
                    articles.Where(a => a.PublishedDate > existingDateTime)
                        .OrderByDescending(a => a.PublishedDate)
                        .ToList();
            else if (existingDateTime != null)
                filteredArticles =
                    articles.Where(a => a.PublishedDate < existingDateTime)
                        .OrderByDescending(a => a.PublishedDate)
                        .Take(ServiceSettings.ArticleOrderingCount)
                        .ToList();
            else
                filteredArticles = articles.OrderByDescending(a => a.PublishedDate).Take(ServiceSettings.ArticleOrderingCount).ToList();

            foreach (var article in filteredArticles)
            {
                article.Photo = MapUrl(article.Photo);
                article.TitlePhoto = MapUrl(article.TitlePhoto);

                object authorName;

                switch (article.user.usertype.Tag)
                {
                    case UserTypes.Editor:
                        authorName = new
                        {
                            article.user.editor.First().FirstName,
                            article.user.editor.First().MiddleName,
                            article.user.editor.First().LastName
                        };
                        break;
                    case UserTypes.Journalist:
                        authorName = new
                        {
                            article.user.journalist.First().FirstName,
                            article.user.journalist.First().MiddleName,
                            article.user.journalist.First().LastName
                        };
                        break;
                    default:
                        authorName = new
                        {
                            FirstName = "Администратор",
                            MiddleName = "Администратор",
                            LastName = "Администратор"
                        };
                        break;
                }

                result.AddObjectToResult(article, i);
                string markerAddress=null;
                if (article.marker != null)
                    markerAddress = article.marker.city.country.Name + ", " + article.marker.city.Name + ", " +
                                    article.marker.Street + " " + article.marker.House + " " + article.marker.Buliding;

                result.AddObjectToResult(new { AuthorName = authorName, MarkerAddress = markerAddress}, i);
                result.AddObjectToResult(new{Subcategories = article.articlesubcategory.Select(a=>a.category.Name).ToList()},i);

                i++;
            }
            return JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string GetRecentEvents(bool refresh = false, DateTime? existingDateTime = null)
        {
            MySqlRepository repo = new MySqlRepository();
            List<article> articles = repo.GetEvents();
            var result = new JsonResult(new List<Dictionary<string, object>>());
            int i = 0; List<article> filteredArticles;

            if (existingDateTime != null && refresh)
                filteredArticles =
                    articles.Where(a => a.PublishedDate > existingDateTime)
                        .OrderByDescending(a => a.PublishedDate)
                        .ToList();
            else if (existingDateTime != null)
                filteredArticles =
                    articles.Where(a => a.PublishedDate < existingDateTime)
                        .OrderByDescending(a => a.PublishedDate)
                        .Take(ServiceSettings.ArticleOrderingCount)
                        .ToList();
            else
                filteredArticles = articles.OrderByDescending(a => a.PublishedDate).Take(ServiceSettings.ArticleOrderingCount).ToList();

            foreach (var article in filteredArticles)
            {
                article.Photo = MapUrl(article.Photo);
                article.TitlePhoto = MapUrl(article.TitlePhoto);

                object authorName;

                switch (article.user.usertype.Tag)
                {
                    case UserTypes.Editor:
                        authorName = new
                        {
                            article.user.editor.First().FirstName,
                            article.user.editor.First().MiddleName,
                            article.user.editor.First().LastName
                        };
                        break;
                    case UserTypes.Journalist:
                        authorName = new
                        {
                            article.user.journalist.First().FirstName,
                            article.user.journalist.First().MiddleName,
                            article.user.journalist.First().LastName
                        };
                        break;
                    default:
                        authorName = new
                        {
                            FirstName = "Администратор",
                            MiddleName = "Администратор",
                            LastName = "Администратор"
                        };
                        break;
                }

                result.AddObjectToResult(article, i); string markerAddress = null;
                if (article.marker != null)
                    markerAddress = article.marker.city.country.Name + ", " + article.marker.city.Name + ", " +
                                    article.marker.Street + " " + article.marker.House + " " + article.marker.Buliding;

                result.AddObjectToResult(new { AuthorName = authorName, MarkerAddress = markerAddress }, i);
                result.AddObjectToResult(new { Subcategories = article.articlesubcategory.Select(a => a.category.Name).ToList(), StopDate=article.EndDate }, i);

                i++;
            }
            return JsonConvert.SerializeObject(result);
        }


        [WebMethod]
        public string CreateMarker(string userGuid, string name, string introduction, string description, int cityId,
            int baseCategoryId, double lat, double lng, string entryTicket, int discount, string street, string house,
            string building, string floor, string site, string email, string photoBase64, int[] subCategoryIds,
            string[] phones, List<KeyValue<int, int>> openTimes, List<KeyValue<int, int>> closeTimes)
        {
            try
            {


                if (string.IsNullOrEmpty(entryTicket))
                    entryTicket = "Нет";

                var repo = new MySqlRepository();
                repo.CheckUser(userGuid);
                var user = repo.GetUser(userGuid);
                var userType = repo.GetMobileUserTypeById(user.UserTypeId);
                var permittedUserTypes = new[] {UserTypes.Guide};


                if (!permittedUserTypes.Contains(userType.Tag))
                    throw new MyException(Errors.NotPermitted);
                if (!repo.HavePermissions(user.Guid, cityId))
                    throw new MyException(Errors.NotPermitted);



                var bytes = Convert.FromBase64String(photoBase64);

                var photoPath = FileProvider.SaveMarkerPhoto(bytes);
                var logoPath = FileProvider.SaveMarkerLogo(bytes);

                marker newMarker = new marker
                {
                    Name = name,
                    Introduction = introduction,
                    Description = description,
                    CityId = cityId,
                    BaseCategoryId = baseCategoryId,
                    EntryTicket = entryTicket,
                    Buliding = building,
                    DiscountId = repo.GetDiscountId(discount),
                    Email = email,
                    Floor = floor,
                    House = house,
                    Lat = (float) lat,
                    Lng = (float) lng,
                    Site = site,
                    Street = street,
                    UserId = user.Id,
                    StatusId = repo.GetStatuses().First(s => s.Tag == MarkerStatuses.Checking).Id,
                    Photo = photoPath,
                    Logo = logoPath,
                };

                repo.AddMarker(newMarker,
                    openTimes.Select(o => new WorkTimeDay {WeekDayId = o.Key, Time = TimeSpan.FromMinutes(o.Value)})
                        .ToList(),
                    closeTimes.Select(o => new WorkTimeDay {WeekDayId = o.Key, Time = TimeSpan.FromMinutes(o.Value)})
                        .ToList(), user.Id, subCategoryIds, phones);

            }
            catch (MyException e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Error.Message));
            }
            catch (DbEntityValidationException e)
            {
                return
                    JsonConvert.SerializeObject(
                        new JsonResult(e.EntityValidationErrors.First().ValidationErrors.First().PropertyName + " " +
                                       e.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage));
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.ToString()));
            }

            return JsonConvert.SerializeObject(new JsonResult(new List<Dictionary<string, object>>()));
        }

        [WebMethod]
        public string GetPermittedCities(string userGuid)
        {
            try
            {
                var repo = new MySqlRepository();
                var result = new JsonResult(new List<Dictionary<string, object>>());


                var permittedCities = repo.GetPermittedCities(userGuid);

                var i = 0;
                foreach (var permittedCity in permittedCities)
                {
                    result.AddObjectToResult(new {permittedCity.PlaceId, permittedCity.Id, permittedCity.Name}, i);
                    i++;
                }

                return JsonConvert.SerializeObject(result);
            }
            catch (MyException e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Error.Message));
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.ToString()));
            }

        }

        [WebMethod]
        public string RegisterTenant(string email, string firstName, string middleName, string lastName,
            DateTime birthDate, string gender, string phone, string address)
        {
            try
            {
                var repo = new MySqlRepository();
                repo.AddNewTenant(email, firstName, middleName, lastName, birthDate, gender, phone, address);
                return JsonConvert.SerializeObject(new JsonResult(new List<Dictionary<string, object>>()));
            }
            catch (MyException e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Error.Message));
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.ToString()));
            }
        }

        [WebMethod]
        public string RecoverPassword(string email)
        {
            try
            {
                var repo=new MySqlRepository();
                repo.RecoverPassword(email);
                return JsonConvert.SerializeObject(new JsonResult(new List<Dictionary<string, object>>()));
            }
            catch (MyException e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.Error.Message));
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new JsonResult(e.ToString()));
            }
        }

    #endregion

        
    }
}
