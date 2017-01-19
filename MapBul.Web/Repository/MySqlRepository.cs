using System;
using System.Collections.Generic;
using System.Linq;
using MapBul.DBContext;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;
using MapBul.Web.Controllers;
using MapBul.Web.Models;

namespace MapBul.Web.Repository
{
    public class MySqlRepository : IRepository
    {
        private readonly Context _db;

        public MySqlRepository()
        {
            _db = new Context();
        }

        public user GetUserByLoginAndPassword(string email, string password)
        {
            var md5Pass = StringTransformationProvider.Md5(password);
            email = StringTransformationProvider.TransformEmail(email);
            user user = _db.user.FirstOrDefault(u => u.Email == email && md5Pass == u.Password);
            if (user == null)
                throw new MyException(Errors.UserNotFound);
            return user;
        }

        public user GetUserByGuid(string guid)
        {
            user user = _db.user.FirstOrDefault(u => u.Guid == guid);
            if (user == null)
                throw new MyException(Errors.UserNotFound);
            return user;
        }

        public List<journalist> GetJournalists()
        {
            return _db.journalist.ToList();
        }

        public List<editor> GetEditors()
        {
            return _db.editor.ToList();
        }

        public int GetUserTypeByTag(string tag)
        {
            return _db.usertype.First(u => u.Tag == tag).Id;
        }

        public List<admin> GetAdmins()
        {
            return _db.admin.ToList();
        }

        public void AddNewEditor(NewEditorModel model)
        {
            model.Email = StringTransformationProvider.TransformEmail(model.Email);
            if (_db.user.Any(u => u.Email == model.Email))
                throw new MyException(Errors.UserExists);
            var trans = _db.Database.BeginTransaction();
            try
            {
                var newUser = new user
                {
                    Guid = Guid.NewGuid().ToString(),
                    Password = StringTransformationProvider.Md5(model.Password),
                    Email = model.Email,
                    UserTypeId = GetUserTypeByTag(UserTypes.Editor),
                    Deleted = model.Deleted
                };
                _db.user.Add(newUser);
                _db.SaveChanges();
                editor newEditor = new editor();
                model.CopyTo(ref newEditor);
                newEditor.UserId = newUser.Id;
                _db.editor.Add(newEditor);

                _db.country_permission.AddRange(
                    model.PermittedCountries.Select(c => new country_permission {CountryId = c, UserId = newUser.Id}));
                _db.city_permission.AddRange(
                    model.PermittedCities.Select(c => new city_permission {CityId = c, UserId = newUser.Id}));
               /* _db.region_permission.AddRange(
                    model.PermittedRegions.Select(c => new region_permission {RegionId = c, UserId = newUser.Id}));*/

                _db.SaveChanges();
                trans.Commit();


            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public editor GetEditor(int editorId)
        {
            return _db.editor.First(e => e.Id == editorId);
        }

        public void SaveEditorChanges(NewEditorModel model)
        {
            editor editor = _db.editor.FirstOrDefault(e => e.Id == model.Id);
            if (editor == null)
                throw new MyException(Errors.UserNotFound);

            model.CopyTo(ref editor);
            //editor.user.Password = StringTransformationProvider.Md5(model.Password);
            editor.user.Email = StringTransformationProvider.TransformEmail(model.Email);
            editor.user.Deleted = model.Deleted;

            //сохранение новых прав
            _db.country_permission.RemoveRange(editor.user.country_permission);
            _db.city_permission.RemoveRange(editor.user.city_permission);
//            _db.region_permission.RemoveRange(editor.user.region_permission);

            _db.country_permission.AddRange(
                model.PermittedCountries.Select(c => new country_permission {CountryId = c, UserId = editor.UserId}));
            _db.city_permission.AddRange(
                model.PermittedCities.Select(c => new city_permission {CityId = c, UserId = editor.UserId}));
//            _db.region_permission.AddRange(
//                model.PermittedRegions.Select(c => new region_permission {RegionId = c, UserId = editor.UserId}));


            _db.SaveChanges();
        }

        public List<country> GetCountries()
        {
            return _db.country.ToList();
        }

        /*public List<region> GetRegions()
        {
            return _db.region.ToList();
        }*/

        public List<city> GetCities()
        {
            return _db.city.ToList();
        }

        public void AddNewJournalist(NewJournalistModel model)
        {
            model.Email = StringTransformationProvider.TransformEmail(model.Email);
            if (_db.user.Any(u => u.Email == model.Email))
                throw new MyException(Errors.UserExists);
            var trans = _db.Database.BeginTransaction();
            try
            {
                var newUser = new user
                {
                    Guid = Guid.NewGuid().ToString(),
                    Password = StringTransformationProvider.Md5(model.Password),
                    Email = model.Email,
                    UserTypeId = GetUserTypeByTag(UserTypes.Journalist),
                    Deleted = model.Deleted
                };
                _db.user.Add(newUser);
                _db.SaveChanges();
                journalist newEditor = new journalist();
                model.CopyTo(ref newEditor);
                newEditor.UserId = newUser.Id;
                _db.journalist.Add(newEditor);
                _db.country_permission.AddRange(
                    model.PermittedCountries.Select(c => new country_permission {CountryId = c, UserId = newUser.Id}));
                _db.city_permission.AddRange(
                    model.PermittedCities.Select(c => new city_permission {CityId = c, UserId = newUser.Id}));
//                _db.region_permission.AddRange(
//                    model.PermittedRegions.Select(c => new region_permission {RegionId = c, UserId = newUser.Id}));

                _db.SaveChanges();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public journalist GetJournalist(int journalistId)
        {
            return _db.journalist.First(j => j.Id == journalistId);
        }

        public void SaveJournalistChanges(NewJournalistModel model)
        {
            journalist journalist = _db.journalist.FirstOrDefault(e => e.Id == model.Id);
            if (journalist == null)
                throw new MyException(Errors.UserNotFound);

            model.CopyTo(ref journalist);
            //editor.user.Password = StringTransformationProvider.Md5(model.Password);
            journalist.user.Email = StringTransformationProvider.TransformEmail(model.Email);
            journalist.user.Deleted = model.Deleted;

            //сохранение новых прав
            _db.country_permission.RemoveRange(journalist.user.country_permission);
            _db.city_permission.RemoveRange(journalist.user.city_permission);
            //_db.region_permission.RemoveRange(journalist.user.region_permission);

            _db.country_permission.AddRange(
                model.PermittedCountries.Select(c => new country_permission { CountryId = c, UserId = journalist.UserId }));
            _db.city_permission.AddRange(
                model.PermittedCities.Select(c => new city_permission { CityId = c, UserId = journalist.UserId }));
//            _db.region_permission.AddRange(
//                model.PermittedRegions.Select(c => new region_permission { RegionId = c, UserId = journalist.UserId }));


            _db.SaveChanges();
        }

        public void AddCountry(string name, string placeId, string code)
        {
            if(_db.country.Any(c=>c.PlaceId==placeId))
                return;
            _db.country.Add(new country {Name = name, PlaceId = placeId, Code = code});
            _db.SaveChanges();
        }

        /*public void AddRegion(string name, int countryId, string placeId)
        {
            if (_db.region.Any(c => c.PlaceId == placeId))
                return;
            _db.region.Add(new region {Name = name, CountryId = countryId,PlaceId = placeId});
            _db.SaveChanges();
        }*/

        /*public void AddCity(string name, int regionId,string placeId, float lat, float lng)
        {
            if (_db.city.Any(c => c.PlaceId == placeId))
                return;
            region region = GetRegion(regionId);
            var coordinates =
                ExternalRequest.ExternalRequestProvider.GetCoordinates(region.country.Name + ", " + region.Name + ", " +
                                                               name);
            _db.city.Add(new city { Name = name, RegionId = regionId, Lat = coordinates.Lat, Lng = coordinates.Lng, PlaceId = placeId });
            //_db.city.Add(new city {Name = name, RegionId = regionId, Lat = lat, Lng = lng, PlaceId = placeId});
            _db.SaveChanges();
        }*/

        public void AddCity(string name, int countryId, string placeId, float lat, float lng)
        {
            if (_db.city.Any(c => c.PlaceId == placeId))
                return;
            country country = GetCountry(countryId);
            /*var coordinates =
                ExternalRequest.ExternalRequestProvider.GetCoordinates(country.Name + ", " +
                                                               name);
            _db.city.Add(new city { Name = name, CountryId = countryId, Lat = coordinates.Lat, Lng = coordinates.Lng, PlaceId = placeId });*/
            _db.city.Add(new city { Name = name, CountryId = country.Id, Lat = lat, Lng = lng, PlaceId = placeId });
            _db.SaveChanges();
        }

        public void DeleteAdmin(int adminId)
        {
            _db.user.Remove(_db.admin.First(a => a.Id == adminId).user);
            _db.SaveChanges();
        }

        public void AddNewGuide(NewGuideModel model)
        {
            model.Email = StringTransformationProvider.TransformEmail(model.Email);
            if (_db.user.Any(u => u.Email == model.Email))
                throw new MyException(Errors.UserExists);
            var trans = _db.Database.BeginTransaction();
            try
            {
                var newUser = new user
                {
                    Guid = Guid.NewGuid().ToString(),
                    Password = StringTransformationProvider.Md5(model.Password),
                    Email = model.Email,
                    UserTypeId = GetUserTypeByTag(UserTypes.Guide),
                    Deleted = model.Deleted
                };
                _db.user.Add(newUser);
                _db.SaveChanges();
                guide newEditor = new guide();
                model.CopyTo(ref newEditor);
                newEditor.UserId = newUser.Id;
                _db.guide.Add(newEditor);
                _db.country_permission.AddRange(
                    model.PermittedCountries.Select(c => new country_permission { CountryId = c, UserId = newUser.Id }));
                _db.city_permission.AddRange(
                    model.PermittedCities.Select(c => new city_permission { CityId = c, UserId = newUser.Id }));
                //                _db.region_permission.AddRange(
                //                    model.PermittedRegions.Select(c => new region_permission {RegionId = c, UserId = newUser.Id}));

                _db.SaveChanges();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public void DeleteUser(int userId)
        {
            _db.user.Remove(_db.user.First(u => u.Id == userId));
            _db.SaveChanges();
        }

        private country GetCountry(int countryId)
        {
            return _db.country.First(c => c.Id == countryId);
        }


        public List<category> GetMarkerCategories()
        {
            return _db.category.Where(c => !c.ForArticle).ToList();
        }

        public List<category> GetArticleCategories()
        {
            return _db.category.Where(c=>c.ForArticle).ToList();
        }

        public void DeleteCategory(int categoryId)
        {
            _db.category.Remove(_db.category.First(c => c.Id == categoryId));
            _db.SaveChanges();
        }

        public void DeleteCountry(int countryId)
        {
            _db.country.Remove(_db.country.First(c => c.Id == countryId));
            _db.SaveChanges();
        }

        public void DeleteCity(int cityId)
        {
            _db.city.Remove(_db.city.First(c => c.Id == cityId));
            _db.SaveChanges();
        }

        public void DeleteMarker(int markerId)
        {
            _db.marker.Remove(_db.marker.First(m => m.Id == markerId));
            _db.SaveChanges();
        }

        public void DeleteArticle(int articleId)
        {
            _db.article.Remove(_db.article.First(a => a.Id == articleId));
            _db.SaveChanges();
        }

        private void UpdateCategoryParent(NestableElement structure, int? parrentId)
        {
            _db.category.First(c => c.Id == structure.id).ParentId = parrentId;
            if(structure.children==null)
                return;
            foreach (var element in structure.children)
            {
                UpdateCategoryParent(element, structure.id);
            }
        }

        public void SaveCategoriesStructure(List<NestableElement> structure)
        {
            foreach (var element in structure)
            {
                UpdateCategoryParent(element,null);
            }
            _db.SaveChanges();
        }

        public void AddNewCategory(category model)
        {
            _db.category.Add(model);
            _db.SaveChanges();
        }

        public category GetCategory(int categoryId)
        {
            return _db.category.First(c => c.Id == categoryId);
        }

        public void EditCategory(category model)
        {
            var existingCategory=_db.category.First(c => c.Id == model.Id);
            existingCategory.Icon = model.Icon;
            existingCategory.Name = model.Name;
            if (!string.IsNullOrEmpty(model.EnName))
            {
                existingCategory.EnName = model.EnName;
            }
            existingCategory.ParentId = model.ParentId;
            existingCategory.Color = model.Color;
            existingCategory.Pin = model.Pin;
            _db.SaveChanges();
        }

        public void AddNewAdmin(NewAdminModel model)
        {
            model.Email = StringTransformationProvider.TransformEmail(model.Email);
            if (_db.user.Any(u => u.Email == model.Email))
                throw new MyException(Errors.UserExists);
            var trans = _db.Database.BeginTransaction();
            try
            {
                var newUser = new user
                {
                    Guid = Guid.NewGuid().ToString(),
                    Password = StringTransformationProvider.Md5(model.Password),
                    Email = model.Email,
                    UserTypeId = GetUserTypeByTag(UserTypes.Admin),
                    Deleted = false
                };
                _db.user.Add(newUser);
                _db.SaveChanges();
                var admin = new admin();
                model.CopyTo(ref admin);
                admin.UserId = newUser.Id;
                _db.admin.Add(admin);
                _db.SaveChanges();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public List<marker> GetMarkers()
        {
            return _db.marker.OrderByDescending(m=>m.AddedDate).ToList();
        }

        public List<discount> GetDiscounts()
        {
            return _db.discount.ToList();
        }

        public List<status> GetStatuses()
        {
            return _db.status.ToList();
        }

        public List<status> GetStatuses(string userGuid)
        {
            var user = GetUserByGuid(userGuid);
            var statuses=_db.status.ToList();
            if (user.usertype.Tag != UserTypes.Admin && user.usertype.Tag != UserTypes.Editor)
                statuses.Remove(statuses.First(s => s.Tag == MarkerStatuses.Published));
            return statuses;
        }

        public List<weekday> GetWeekDays()
        {
            return _db.weekday.ToList();
        }

        public int AddMarker(NewMarkerModel model, List<WorkTimeDay> openTimes, List<WorkTimeDay> closeTimes,
            string userGuid)
        {
            var trans = _db.Database.BeginTransaction();
            try
            {
                user adder = GetUserByGuid(userGuid);
                marker newMarker = new marker();
                model.CopyTo(newMarker);
                newMarker.BaseCategoryId = model.BaseCategoryId;
                newMarker.CityId = model.CityId;
                newMarker.DiscountId = model.DiscountId;
                newMarker.StatusId = model.StatusId;
                newMarker.UserId = adder.Id;
                if (model.StatusId == GetStatusByTag(MarkerStatuses.Published).Id)
                    newMarker.PublishedDate = DateTime.Now;
                else if (model.StatusId == GetStatusByTag(MarkerStatuses.Checking).Id)
                {
                    newMarker.CheckDate = DateTime.Now;
                    newMarker.PublishedDate = null;
                }
                else
                {
                    newMarker.CheckDate = null;
                    newMarker.PublishedDate = null;
                }
                _db.marker.Add(newMarker);
                _db.SaveChanges();

                var subCategories = model.SubCategories != null
                    ? model.SubCategories.Select(sc => new subcategory {CategoryId = sc, MarkerId = newMarker.Id})
                        .ToList()
                    : new List<subcategory>();

                var phones =
                    model.Phones.Where(p=>p.Length!=0).Select(p => new phone {Number = p, MarkerId = newMarker.Id, Primary = false}).ToList();

                var firstOrDefault = phones.FirstOrDefault();
                if (firstOrDefault != null)
                    firstOrDefault.Primary = true;


                var workTimes =
                    openTimes.Join(closeTimes.Select(ct => new {ct.WeekDayId, CloseTime = ct.Time}),
                        ot => ot.WeekDayId, ct => ct.WeekDayId,
                        (ot, ct) => new {ot.WeekDayId, OpenTime = ot.Time, ct.CloseTime})
                        .Where(t => t.CloseTime != null && t.OpenTime != null)
                        .ToList(); //собираем из двух частей полное время работы

                _db.subcategory.AddRange(subCategories);
                _db.phone.AddRange(phones);
                _db.worktime.AddRange(
                    workTimes.Select(
                        t =>
                            new worktime
                            {
                                CloseTime = t.CloseTime.Value,
                                OpenTime = t.OpenTime.Value,
                                MarkerId = newMarker.Id,
                                WeekDayId = t.WeekDayId
                            }));
                _db.SaveChanges();
                trans.Commit();
                return newMarker.Id;
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public void AddMarkerPhotos(int markerId, string[] photos)
        {
            if(photos!=null && photos.Length > 0)
            {
                var trans = _db.Database.BeginTransaction();
                try
                {
                    _db.marker_photos.AddRange(photos.Select(i => new marker_photos
                    {
                        MarkerId = markerId,
                        Photo = i
                    }));
                    _db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public void ChangeMarkerStatus(int markerId, int statusId)
        {
            var marker = _db.marker.First(m => m.Id == markerId);

            if (statusId == GetStatusByTag(MarkerStatuses.Published).Id)
                marker.PublishedDate = DateTime.Now;
            else if (statusId == GetStatusByTag(MarkerStatuses.Checking).Id)
            {
                marker.CheckDate = DateTime.Now;
                marker.PublishedDate = null;
            }
            else
            {
                marker.CheckDate = null;
                marker.PublishedDate = null;
            }
            marker.StatusId = statusId;
            _db.SaveChanges();
        }

        public marker GetMarker(int markerId)
        {
            return _db.marker.First(m => m.Id == markerId);
        }

        public void EditMarker(NewMarkerModel model, List<WorkTimeDay> openTimes, List<WorkTimeDay> closeTimes, string userGuid)
        {
            var trans = _db.Database.BeginTransaction();
            try
            {
                GetUserByGuid(userGuid);
                marker newMarker = _db.marker.First(m => m.Id == model.Id);
                model.CopyTo(newMarker);
                newMarker.BaseCategoryId = model.BaseCategoryId;
                newMarker.CityId = model.CityId;
                newMarker.DiscountId = model.DiscountId;
                newMarker.StatusId = model.StatusId;
                if (model.StatusId == GetStatusByTag(MarkerStatuses.Published).Id)
                    newMarker.PublishedDate = DateTime.Now;
                else if (model.StatusId == GetStatusByTag(MarkerStatuses.Checking).Id)
                {
                    newMarker.CheckDate = DateTime.Now;
                    newMarker.PublishedDate = null;
                }
                else
                {
                    newMarker.CheckDate = null;
                    newMarker.PublishedDate = null;
                }

                _db.subcategory.RemoveRange(_db.subcategory.Where(s => s.MarkerId == newMarker.Id));
                _db.phone.RemoveRange(_db.phone.Where(s => s.MarkerId == newMarker.Id));
                _db.worktime.RemoveRange(_db.worktime.Where(s => s.MarkerId == newMarker.Id));




                var subCategories = model.SubCategories != null ?
                    model.SubCategories.Select(sc => new subcategory { CategoryId = sc, MarkerId = newMarker.Id }).ToList() : new List<subcategory>();

                var phones =
                    model.Phones.Where(p => p.Length != 0).Select(p => new phone { Number = p, MarkerId = newMarker.Id, Primary = false }).ToList();

                var firstOrDefault = phones.FirstOrDefault();
                if (firstOrDefault != null)
                    firstOrDefault.Primary = true;

                var workTimes =
                    openTimes.Join(closeTimes.Select(ct => new { ct.WeekDayId, CloseTime = ct.Time }),
                        ot => ot.WeekDayId, ct => ct.WeekDayId, (ot, ct) => new { ot.WeekDayId, OpenTime = ot.Time, ct.CloseTime }).Where(t => t.CloseTime != null && t.OpenTime != null).ToList();  //собираем из двух частей полное время работы

                _db.subcategory.AddRange(subCategories);
                _db.phone.AddRange(phones);
                _db.worktime.AddRange(workTimes.Select(t => new worktime { CloseTime = t.CloseTime.Value, OpenTime = t.OpenTime.Value, MarkerId = newMarker.Id, WeekDayId = t.WeekDayId }));
                _db.SaveChanges();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public List<article> GetArticles()
        {
            return _db.article.ToList();
        }

        public void AddNewArticle(NewArticleModel model, string userGuid)
        {
            var trans = _db.Database.BeginTransaction();
            try
            {
                user adder = GetUserByGuid(userGuid);
                var article = new article();
                model.CopyTo(article);
                article.BaseCategoryId = model.BaseCategoryId;
                article.AuthorId = adder.Id;
                article.MarkerId = model.MarkerId;
                article.CityId = model.CityId;
                if (model.StatusId == GetStatusByTag(MarkerStatuses.Published).Id)
                {
                    article.EditorId = adder.Id;
                }
                article.PublishedDate = DateTime.Now;
                article.StatusId = model.StatusId;
                _db.article.Add(article);
                _db.SaveChanges();

                if(model.SubCategories!=null)
                {
                    _db.articlesubcategory.AddRange(
                    model.SubCategories.Select(sc => new articlesubcategory {ArticleId = article.Id, CategoryId = sc}));
                }

                _db.SaveChanges();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                return;
            }

        }

        public void ChangeArticleStatus(int articleId, int statusId, string userGuid)
        {
            var user = GetUserByGuid(userGuid);
            var status = GetStatuses().First(s => s.Id == statusId);
            if(status.Tag==MarkerStatuses.Published&&(user.usertype.Tag!=UserTypes.Editor&&user.usertype.Tag!=UserTypes.Admin))
                throw new MyException(Errors.NotPermitted);
            var article = _db.article.First(m => m.Id == articleId);

            if (statusId == status.Id)
            {
                article.PublishedDate = DateTime.Now;
                article.EditorId = user.Id;
            }
            else 
            {
                article.PublishedDate = DateTime.Now;
                article.EditorId = null;
            }
            article.StatusId = statusId;
            _db.SaveChanges();
        }

        public article GetArticle(int articleId)
        {
            return _db.article.First(a => a.Id == articleId);
        }

        public void EditArticle(NewArticleModel model, string userGuid)
        {
            var trans = _db.Database.BeginTransaction();
            try
            {
                user adder = GetUserByGuid(userGuid);
                article article = _db.article.First(m => m.Id == model.Id);
                model.CopyTo(article);
                article.BaseCategoryId = model.BaseCategoryId;
                article.MarkerId = model.MarkerId;
                article.CityId = model.CityId;

                if (model.StatusId == GetStatusByTag(MarkerStatuses.Published).Id&&article.status!=GetStatusByTag(MarkerStatuses.Published))
                {
                    article.EditorId = adder.Id;
                    article.PublishedDate = DateTime.Now;
                }
                else
                {
                    article.EditorId = null;
                    article.PublishedDate = DateTime.Now;
                }
                article.StatusId = model.StatusId;

                _db.articlesubcategory.RemoveRange(_db.articlesubcategory.Where(s => s.ArticleId == article.Id));
                if (model.SubCategories != null)
                {
                    _db.articlesubcategory.AddRange(
                    model.SubCategories.Select(sc => new articlesubcategory { ArticleId = article.Id, CategoryId = sc }));
                }
                
                _db.SaveChanges();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public List<marker> GetMarkers(string userGuid)
        {
            var user = GetUserByGuid(userGuid);
            switch (user.usertype.Tag)
            {
                case UserTypes.Admin:
                    return _db.marker.ToList();
                case UserTypes.Journalist:
                    return user.marker.ToList();
                case UserTypes.Editor:
                    var journalists = user.editor.First().journalist;
                    var result = new List<marker>();
                    result.AddRange(user.marker);
                    foreach (var journalist in journalists)
                    {
                        result.AddRange(journalist.user.marker);
                    }
                    return result;
                default:
                    return null;
            }
        }

        public List<article> GetArticles(string userGuid)
        {
            var user = GetUserByGuid(userGuid);
            switch (user.usertype.Tag)
            {
                case UserTypes.Admin:
                    return _db.article.ToList();
                case UserTypes.Journalist:
                    return user.article.ToList();
                case UserTypes.Editor:
                    var journalists = user.editor.First().journalist;
                    var result = new List<article>();
                    result.AddRange(user.article);
                    foreach (var journalist in journalists)
                    {
                        result.AddRange(journalist.user.article);
                    }
                    return result;
                default:
                    return null;
            }
        }

        public List<guide> GetGuides()
        {
            return _db.guide.ToList();
        }

        public List<tenant> GetTenants()
        {
            return _db.tenant.ToList();
        }

        public tenant GetTenant(int tenantId)
        {
            return _db.tenant.First(t => t.Id == tenantId);
        }

        public guide GetGuide(int guideId)
        {
            return _db.guide.First(g => g.Id == guideId);
        }

        public void SaveGuideChanges(NewGuideModel model)
        {
            guide guide = _db.guide.FirstOrDefault(e => e.Id == model.Id);
            if (guide == null)
                throw new MyException(Errors.UserNotFound);

            model.CopyTo(ref guide);
            //editor.user.Password = StringTransformationProvider.Md5(model.Password);
            guide.user.Email = StringTransformationProvider.TransformEmail(model.Email);
            guide.user.Deleted = model.Deleted;

            //сохранение новых прав
            _db.country_permission.RemoveRange(guide.user.country_permission);
            _db.city_permission.RemoveRange(guide.user.city_permission);
            //_db.region_permission.RemoveRange(guide.user.region_permission);

            _db.country_permission.AddRange(
                model.PermittedCountries.Select(c => new country_permission { CountryId = c, UserId = guide.UserId }));
            _db.city_permission.AddRange(
                model.PermittedCities.Select(c => new city_permission { CityId = c, UserId = guide.UserId }));
           // _db.region_permission.AddRange(
           //     model.PermittedRegions.Select(c => new region_permission { RegionId = c, UserId = guide.UserId }));


            _db.SaveChanges();
        }

        public void SaveTenantChanges(NewTenantModel model)
        {
            tenant tenant = _db.tenant.FirstOrDefault(e => e.Id == model.Id);
            if (tenant == null)
                throw new MyException(Errors.UserNotFound);

            model.CopyTo(ref tenant);
            //editor.user.Password = StringTransformationProvider.Md5(model.Password);
            tenant.user.Email = StringTransformationProvider.TransformEmail(model.Email);
            tenant.user.Deleted = model.Deleted;

            _db.SaveChanges();
        }

        public List<journalist> GetJournalists(string userGuid)
        {
            var user = GetUserByGuid(userGuid);
            switch (user.usertype.Tag)
            {
                case UserTypes.Admin:
                    return _db.journalist.ToList();
                case UserTypes.Editor:
                    return user.editor.First().journalist.ToList();
                default:
                    return null;
            }
        }

        public List<guide> GetGuides(string userGuid)
        {
            var user = GetUserByGuid(userGuid);
            switch (user.usertype.Tag)
            {
                case UserTypes.Admin:
                    return _db.guide.ToList();
                case UserTypes.Editor:
                    return user.editor.First().guide.ToList();
                default:
                    return null;
            }
        }

        public status GetStatusByTag(string tag)
        {
            return _db.status.First(s => s.Tag == tag);
        }

        /*public region GetRegion(int regionId)
        {
            return _db.region.First(r => r.Id == regionId);
        }*/
    }
}