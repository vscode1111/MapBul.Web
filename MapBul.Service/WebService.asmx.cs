using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
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
            return "http://"+HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"]+"/" + filePath;
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
                var user = repo.GetUserByEmailAndPassword(email, password);  //вытащили пользователя

                dynamic userDescriptor = GetUserDescriptor(user);

                var userType = repo.GetMobileUserTypeById(user.UserTypeId); //вытащили его тип
                var result = new JsonResult(new List<Dictionary<string, object>>{new Dictionary<string, object>{{"FirstName",userDescriptor.FirstName},{"UserTypeTag",userType.Tag},{"UserTypeDescription",userType.Description}}});
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
                MySqlRepository repo=new MySqlRepository();
                usertype userType = repo.GetMobileUserTypeById(id);
                var result = new JsonResult(new List<Dictionary<string, object>>());
                result.AddObjectToResult(userType,0);
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
                List<marker> markers = repo.GetMarkersInSquare(p1Lat, p1Lng, p2Lat, p2Lng);

                JsonResult result = new JsonResult(new List<Dictionary<string, object>>());

                foreach (var marker in markers)
                {
                    result.AddObjectToResult(
                        new
                        {
                            marker.Id,
                            marker.Name,
                            marker.Lat,
                            marker.Lng,
                            Icon=MapUrl(marker.category.Pin),
                            Logo=MapUrl(marker.Logo),
                            Photo=MapUrl(marker.Photo),
                            marker.phone.First(p => p.Primary).Number,
                            marker.Site,
                            marker.Introduction,
                            WorkTime=marker.worktime.Select(wt=>new
                            {
                                wt.OpenTime,
                                wt.CloseTime,
                                wt.weekday.Id
                            }).ToList(),
                            CategoriesBranch=GetCategoriesBranch(marker.category)
                        }, markers.IndexOf(marker));
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
        public string GetMarkerDescription(int markerId)
        {
            var repo=new MySqlRepository();
            marker marker = repo.GetMarker(markerId);
            JsonResult result=new JsonResult(new List<Dictionary<string, object>>());

            marker.Photo = MapUrl(marker.Photo);
            result.AddObjectToResult(marker,0);
            result.AddObjectToResult(new
            {
                Phones = marker.phone.Select(p=>new {p.Primary,p.Number}).ToList(),
                Discount = marker.discount.Value,
                Subcategories = marker.subcategory.Select(s=>s.category.Id).ToList()
            }, 0);
            return JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string GetRootCategories() 
        {
            MySqlRepository repo=new MySqlRepository();
            List<category> rootCategories = repo.GetRootCategories();
            int index = 0;
            var result=new JsonResult(new List<Dictionary<string, object>>());
            foreach (var rootCategory in rootCategories)
            {
                rootCategory.Icon = MapUrl(rootCategory.Icon);
                List<category> childCategories = repo.GetChildCategories(rootCategory.Id);
                result.AddObjectToResult(rootCategory,index);
                result.AddObjectToResult(new { ChildCategories = childCategories.Select(c => new { c.Id, c.ParentId, c.AddedDate, Icon = MapUrl(c.Icon), c.Name, c.Color }).ToList() }, index);
                index++;
            }
            return JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string GetRecentArticles()
        {
            MySqlRepository repo = new MySqlRepository();
            List<article> articles = repo.GetArticles();
            var result = new JsonResult(new List<Dictionary<string, object>>());
            int i = 0;
            foreach (var article in articles.OrderByDescending(a=>a.PublishedDate).Take(10))
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
                        default :
                        authorName = new
                        {
                            FirstName = "Администратор",
                            MiddleName = "Администратор",
                            LastName = "Администратор"
                        };
                        break;
                }

                result.AddObjectToResult(article,i);
                result.AddObjectToResult(new{AuthorName=authorName}, i);
                i++;
            }
            return JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string GetRecentEvents()
        {
            MySqlRepository repo = new MySqlRepository();
            List<article> articles = repo.GetEvents();
            var result = new JsonResult(new List<Dictionary<string, object>>());
            int i = 0;
            foreach (var article in articles.OrderByDescending(a => a.PublishedDate).Take(10))
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
                result.AddObjectToResult(new { AuthorName = authorName }, i);
                i++;
            }
            return JsonConvert.SerializeObject(result);
        }


        [WebMethod]
        public string CreateMarker(string userGuid, string name, string introduction, string description, int cityId,
            int baseCategoryId, float lat, float lng, string entryTicket, int discount, string street, string house,
            string building, string floor, string site, string email, string photoBase64, string logoBase64, int[] subCategoryIds, string[] phones)
        {
            try
            {
                var repo = new MySqlRepository();
                repo.CheckUser(userGuid);
                var user = repo.GetUser(userGuid);
                var userType = repo.GetMobileUserTypeById(user.Id);
                var permittedUserTypes = new[] {UserTypes.Guide};
                if (!permittedUserTypes.Contains(userType.Tag))
                    throw new MyException(Errors.NotPermitted);

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
                    Lat = lat,
                    Lng = lng,
                    Site = site,
                    Street = street,
                    UserId = user.Id,
                    StatusId = repo.GetStatuses().First(s => s.Tag == MarkerStatuses.Checking).Id
                };

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

        #endregion

        
    }
}
