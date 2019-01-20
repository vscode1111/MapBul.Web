using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using MapBul.DBContext;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;

namespace MapBul.Service
{
    public class MySqlRepository
    {
        private readonly Context _db=new Context();


        public user GetUserByEmailAndPassword(string email, string password)
        {
            email = StringTransformationProvider.TransformEmail(email);
            var md5Pass = StringTransformationProvider.Md5(password);
            var user =
                _db.user.FirstOrDefault(
                    u =>
                        u.Email == email &&
                        u.Password == md5Pass);
            if(user==null)
                throw new MyException(Errors.UserNotFound);
            if(user.Deleted)
                throw new MyException(Errors.UserBlocked);
            return user;
        }

        public usertype GetMobileUserTypeById(int id)
        {
            var userType = _db.usertype.FirstOrDefault(u => u.Id == id);
            if(userType==null)
                throw new MyException(Errors.NotFound);
            usertype userTypeMobile;
            if (userType.Tag == UserTypes.Admin || userType.Tag == UserTypes.Editor || userType.Tag == UserTypes.Guide || userType.Tag==UserTypes.Journalist)
                userTypeMobile = _db.usertype.First(u=>u.Tag==UserTypes.Guide);
            else
            {
                userTypeMobile = _db.usertype.First(u => u.Tag == UserTypes.Tenant);
            }
            return userTypeMobile;
        }


        public void CheckUser(string userGuid)
        {
            var user = _db.user.FirstOrDefault(u => u.Guid == userGuid);
            if(user==null)
                throw new MyException(Errors.NotFound);
        }


        public IEnumerable<marker> GetMarkersInSquare(double p1Lat, double p1Lng, double p2Lat, double p2Lng)
        {
            return _db.marker.ToList().Where(m => (m.Lat < Math.Max(p1Lat,p2Lat) && m.Lat > Math.Min(p1Lat,p2Lat)) && (m.Lng <  Math.Max(p1Lng,p2Lng) && m.Lng > Math.Min(p1Lng,p2Lng))&&m.status.Tag==MarkerStatuses.Published);
        }

        public marker GetMarker(int markerId)
        {
            return _db.marker.First(m => m.Id == markerId);
        }

        public string[] GetArrayOfPathsMarkerPhotos(int markerId)
        {
            return _db.marker_photos.Where(mp => mp.MarkerId == markerId).Select(mp => mp.Photo).ToArray();
        }
        public string[] GetArrayOfPathsMarkerPhotosMini(int markerId)
        {
            return _db.marker_photos.Where(mp => mp.MarkerId == markerId).Select(mp => mp.PhotoMini).ToArray();
        }

        public void AddMarkerPhotos(int markerId, string[] photos, string[] photosMini)
        {
            if (photos != null && photos.Length > 0)
            {
                var trans = _db.Database.BeginTransaction();
                try
                {
                    if (photos.Length == photosMini.Length)
                    {
                        for (var i = 0; i < photos.Length; i++)
                        {
                            _db.marker_photos.Add(new marker_photos
                            {
                                MarkerId = markerId,
                                Photo = photos[i],
                                PhotoMini = photosMini[i]
                            });
                        }
                    }
                    else
                    {
                        _db.marker_photos.AddRange(photos.Select(i => new marker_photos
                        {
                            MarkerId = markerId,
                            Photo = i
                        }));
                    }
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

        public void RemoveMarkerPhoto(int markerId, string[] pathsNotToDelete)
        {
            var trans = _db.Database.BeginTransaction();
            try
            {
                var listToDelete = new List<marker_photos>();
                var markersPhoto = _db.marker_photos.Where(i => i.MarkerId == markerId);
                if (markersPhoto.Any())
                {
                    foreach (var item in markersPhoto)
                    {
                        if (pathsNotToDelete.All(i => i != item.PhotoMini))
                        {
                            listToDelete.Add(item);
                        }
                    }
                    _db.marker_photos.RemoveRange(listToDelete);
                    _db.SaveChanges();
                    trans.Commit();
                }
                //var photoToDelete = _db.marker_photos.Where(i => pathsNotToDalete.Any(i.PhotoMini == pathsNotToDalete));
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public IEnumerable<marker> GetFavoriteMarkers(string userGuid)
        {
            var user = _db.user.FirstOrDefault(u => u.Guid == userGuid);
            if (user == null)
                throw new MyException(Errors.UserNotFound);
            var tempListOfFavoritsMarkerId =
                _db.favorites_marker.Where(m => m.userId == user.Id).Select(m => m.markerId).ToList();
            return _db.marker.Where(m => tempListOfFavoritsMarkerId.Any(tm => tm == m.Id));
        }

        public List<category> GetRootMarkerCategories()
        {
            return _db.category.Where(c => c.ParentId == null && !c.ForArticle).ToList();
        }

        private void FindChildCategoriesIteration(ref List<category> categories, int parentId )
        {
            var childs = _db.category.Where(c => c.ParentId == parentId).ToList();
            categories.AddRange(childs);
            foreach (var child in childs)
            {
                FindChildCategoriesIteration(ref categories, child.Id);
            }
        } 

        public List<category> GetChildCategories(int id)
        {
            var childCategories=new List<category>();
            FindChildCategoriesIteration(ref childCategories, id);
            return childCategories;
        }

        public List<article> GetArticles()
        {
            //return _db.article.Where(a => a.status.Tag == MarkerStatuses.Published && a.StartDate == null).ToList();
            return _db.article.Where(a => a.status.Tag == MarkerStatuses.Published).ToList();
            //return _db.article.ToList();
        }

        public List<article> GetEvents()
        {
            return _db.article.Where(a => a.status.Tag == MarkerStatuses.Published && a.StartDate != null).ToList();
        }

        public List<int> GetIdFavoritsArticleAndEvent(string userGuid)
        {
            var tempUser = GetUser(userGuid);
            if (tempUser == null)
                return null;
            return _db.favorites_article.Where(i => i.userId == tempUser.Id).Select(i=>i.articleId).ToList();
        }         

        public user GetUser(string userGuid)
        {
            var user = _db.user.FirstOrDefault(u => u.Guid == userGuid);
            if(user==null)
                throw new MyException(Errors.UserNotFound);
            return user;
        }

        public int GetDiscountId(int discount)
        {
            return _db.discount.First(d => d.Value == discount).Id;
        }

        public List<status> GetStatuses()
        {
            return _db.status.ToList();
        }

        public void AddMarker(marker marker, List<WorkTimeDay> openTimes, List<WorkTimeDay> closeTimes, int userId, int[] subCategoryIds, string[] phonesStrings)
        {
            var trans = _db.Database.BeginTransaction();
            try
            {
                _db.marker.Add(marker);
                _db.SaveChanges();

                var subCategories =
                    subCategoryIds.Select(sc => new subcategory {CategoryId = sc, MarkerId = marker.Id}).ToList();

                var phones = phonesStrings.Select(p => new phone { Number = p, MarkerId = marker.Id, Primary = false }).ToList();
                if(phones.Any())
                    phones.First().Primary = true;

                var workTimes =
                    openTimes.Join(closeTimes.Select(ct => new { ct.WeekDayId, CloseTime = ct.Time }),
                        ot => ot.WeekDayId, ct => ct.WeekDayId, (ot, ct) => new { ot.WeekDayId, OpenTime = ot.Time, ct.CloseTime }).Where(t => t.CloseTime != null && t.OpenTime != null).ToList(); 

                _db.subcategory.AddRange(subCategories);
                _db.phone.AddRange(phones);
                _db.worktime.AddRange(workTimes.Select(t => new worktime { CloseTime = t.CloseTime.Value, OpenTime = t.OpenTime.Value, MarkerId = marker.Id, WeekDayId = t.WeekDayId }));
                _db.SaveChanges();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public void UpdateMarker(marker marker, List<WorkTimeDay> openTimes, List<WorkTimeDay> closeTimes, int userId, int[] subCategoryIds, string[] phonesStrings)
        {
            var trans = _db.Database.BeginTransaction();
            try
            {
                _db.marker.AddOrUpdate(marker);
                _db.SaveChanges();

                var subCategories =
                    subCategoryIds.Select(sc => new subcategory { CategoryId = sc, MarkerId = marker.Id }).ToList();

                var phones = phonesStrings.Select(p => new phone { Number = p, MarkerId = marker.Id, Primary = false }).ToList();
                if (phones.Any())
                    phones.First().Primary = true;

                var workTimes =
                    openTimes.Join(closeTimes.Select(ct => new { ct.WeekDayId, CloseTime = ct.Time }),
                        ot => ot.WeekDayId, ct => ct.WeekDayId, (ot, ct) => new { ot.WeekDayId, OpenTime = ot.Time, ct.CloseTime }).Where(t => t.CloseTime != null && t.OpenTime != null).ToList();

                var tempLastSubCategory = _db.subcategory.Where(sc => sc.MarkerId == marker.Id).ToList();
                if (tempLastSubCategory.Count > 0)
                {
                    foreach (var subCategory in tempLastSubCategory)
                    {
                        _db.subcategory.Remove(subCategory);
                    }
                }
                _db.subcategory.AddRange(subCategories);
                var tempLastPhone = _db.phone.Where(sc => sc.MarkerId == marker.Id).ToList();
                if (tempLastPhone.Count > 0)
                {
                    foreach (var phone in tempLastPhone)
                    {
                        _db.phone.Remove(phone);
                    }
                }
                _db.phone.AddRange(phones);
                var tempLastWorkTime = _db.worktime.Where(sc => sc.MarkerId == marker.Id).ToList();
                if (tempLastWorkTime.Count > 0)
                {
                    foreach (var workTime in tempLastWorkTime)
                    {
                        _db.worktime.Remove(workTime);
                    }
                }
                _db.worktime.AddRange(workTimes.Select(t => new worktime { CloseTime = t.CloseTime.Value, OpenTime = t.OpenTime.Value, MarkerId = marker.Id, WeekDayId = t.WeekDayId }));
                _db.SaveChanges();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public List<city> GetPermittedCities(string userGuid, bool isPersonalMarker)
        {
            if (!isPersonalMarker)
            {
                var user = _db.user.First(u => u.Guid == userGuid);
                var permittedCities = user.city_permission.Select(cp => cp).Select(city => city.city).ToList();
                if (permittedCities?.Count > 0)
                {
                    foreach (
                        var city in
                            from country in
                                user.country_permission.Select(countryPermission => countryPermission.country)
                            from city in country.city
                            where !permittedCities.Contains(city)
                            select city)
                    {
                        permittedCities.Add(city);
                    }
                }
                else
                {
                    foreach (
                           var city in
                               from country in
                                   user.country_permission.Select(countryPermission => countryPermission.country)
                               from city in country.city
                               where !permittedCities.Contains(city)
                               select city)
                    {
                        permittedCities.Add(city);
                    }
                    permittedCities.Add(_db.city.FirstOrDefault(i => i.Id == 0));
                }
                return permittedCities;
            }
            else
            {
                return _db.city.ToList();
            }
        }

        public List<country> GetPermittedCountries(string userGuid)
        {
            var user = _db.user.First(u => u.Guid == userGuid);
            var permittedCountries = user.country_permission.Select(cp => cp).Select(country => country.country).ToList();
            return permittedCountries;
        }

        public bool HavePermissions(string guid, int cityId)
        {
            var user = _db.user.First(u => u.Guid == guid);
            if (user.city_permission?.Count > 0)//если у пользователя установлены права на города, то проверяем города
            {
                var permittedCities = GetPermittedCities(guid, false);
                return permittedCities.Any(p => p.Id == cityId);
            }
            else//у пользователя не устоновлено прав на города - роверяем только страну
            {
                return true;
                var country = _db.city.First(c => c.Id == cityId).country;
                return country.country_permission.Any(p => p.user == user);
            }

        }

        public List<category> GetMarkerCategories()
        {
            return _db.category.Where(c=>!c.ForArticle).ToList();
        }

        public void AddNewTenant(string email, string firstName, string middleName, string lastName, DateTime birthDate, string gender, string phone, string address, string appLang)
        {
            if(_db.user.Any(u=>u.Email==email))
                throw new MyException(Errors.UserExists);
            var trans = _db.Database.BeginTransaction();
            var password = StringTransformationProvider.GeneratePassword();
            try
            {
                var newUser = new user
                {
                    Email = email,
                    Deleted = false,
                    Guid = Guid.NewGuid().ToString(),
                    Password = StringTransformationProvider.Md5(password),
                    UserTypeId = _db.usertype.First(t => t.Tag == UserTypes.Tenant).Id
                };
                _db.user.Add(newUser);
                _db.SaveChanges();
                _db.tenant.Add(new tenant
                {
                    Address = address,
                    BirthDate = birthDate,
                    UserId = newUser.Id,
                    FirstName = firstName,
                    LastName = lastName,
                    MiddleName = middleName,
                    Gender = gender,
                    Phone = phone
                });
                _db.SaveChanges();
                trans.Commit();
                MailProvider.SendMailWithCredintails(password,firstName,middleName,email, appLang);
            }
            catch
            {
                trans.Rollback();
                throw;
            }

        }

        public void RecoverPassword(string email, string appLang)
        {
            var user = _db.user.FirstOrDefault(t => t.Email == email);
            if(user == null)
                throw new MyException(Errors.UserNotFound);
            var password = StringTransformationProvider.GeneratePassword();
            user.Password = StringTransformationProvider.Md5(password);
            _db.SaveChanges();
            var tenant = user.tenant.FirstOrDefault();
            if (tenant != null)
            {
                MailProvider.SendMailRecoveryPassword(password, tenant.FirstName,
                    tenant.MiddleName, user.Email, appLang);
                return;
            }
            else
            {
                var guide = user.guide.FirstOrDefault();
                if (guide != null)
                {
                    MailProvider.SendMailRecoveryPassword(password, guide.FirstName,
                        guide.MiddleName, user.Email, appLang);
                    return;
                }
                else
                {
                    var journalist = user.journalist.FirstOrDefault();
                    if (journalist != null)
                    {
                        MailProvider.SendMailRecoveryPassword(password, journalist.FirstName,
                            journalist.MiddleName, user.Email, appLang);
                        return;
                    }
                    else
                    {
                        var editor = user.editor.FirstOrDefault();
                        if (editor != null)
                        {
                            MailProvider.SendMailRecoveryPassword(password, editor.FirstName,
                                editor.MiddleName, user.Email, appLang);
                            return;
                        }
                        else
                        {
                            MailProvider.SendMailRecoveryPassword(password, "",
                                "", user.Email, appLang);
                            return;
                        }
                    }
                }
            }
        }

        public IEnumerable<marker> GetMarkersInSquare(double p1Lat, double p1Lng, double p2Lat, double p2Lng, string sessionId)
        {
            var allMarkers = GetMarkersInSquare(p1Lat, p1Lng, p2Lat, p2Lng).ToList();
            var requestSession = _db.marker_request_session.Where(s=>s.SessionId==sessionId);
            var filteredMarkers = allMarkers.Where(m => requestSession.All(s => m.Id != s.MarkerId) && !m.Personal).ToList();
            filteredMarkers.AddRange(allMarkers.Where(m => m.Personal).ToList());
            var sessionRecords =
                filteredMarkers.Select(f => new marker_request_session {MarkerId = f.Id, SessionId = sessionId});
            _db.marker_request_session.AddRange(sessionRecords);
            _db.SaveChanges();
            return filteredMarkers;

        }

        public void RemoveRequestSession(string sessionId)
        {
            _db.marker_request_session.RemoveRange(_db.marker_request_session.Where(s => s.SessionId == sessionId));
            _db.SaveChanges();
        }

        public void SaveFavoriteArticleEvent(string userGuid, int articleEventId)
        {
            var user = _db.user.FirstOrDefault(u => u.Guid == userGuid);
            if (user == null)
                throw new MyException(Errors.UserNotFound);
            if (_db.favorites_article.Any(i => i.userId == user.Id && i.articleId == articleEventId))
                return;
            var trans = _db.Database.BeginTransaction();
            try
            {
                var newFavoriteArticleEvent = new favorites_article
                {
                    userId = user.Id,
                    articleId = articleEventId
                };
                _db.favorites_article.Add(newFavoriteArticleEvent);
                _db.SaveChanges();
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        public void SaveFavoriteMarker(string userGuid, int markerId)
        {
            var user = _db.user.FirstOrDefault(u => u.Guid == userGuid);
            if (user == null)
                throw new MyException(Errors.UserNotFound);
            if (_db.favorites_marker.Any(i => i.userId == user.Id && i.markerId == markerId))
                return;
            var trans = _db.Database.BeginTransaction();
            try
            {
                var newFavoriteMarker = new favorites_marker
                {
                    userId = user.Id,
                    markerId = markerId
                };
                _db.favorites_marker.Add(newFavoriteMarker);
                _db.SaveChanges();
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        public void RemoveFavoriteArticleEvent(string userGuid, int articleEventId)
        {
            var user = _db.user.FirstOrDefault(u => u.Guid == userGuid);
            if (user == null)
                throw new MyException(Errors.UserNotFound);
            if (_db.favorites_article.All(i => i.userId != user.Id && i.articleId != articleEventId))
                return;
            var trans = _db.Database.BeginTransaction();
            try
            {
                var tempNote =
                    _db.favorites_article.FirstOrDefault(i => i.userId == user.Id && i.articleId == articleEventId);
                if (tempNote == null)
                    return;
                _db.favorites_article.Remove(tempNote);
                _db.SaveChanges();
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        public void RemoveFavoriteMarker(string userGuid, int markerId)
        {
            var user = _db.user.FirstOrDefault(u => u.Guid == userGuid);
            if (user == null)
                throw new MyException(Errors.UserNotFound);
            if (_db.favorites_marker.All(i => i.userId != user.Id && i.markerId != markerId))
                return;
            var trans = _db.Database.BeginTransaction();
            try
            {
                var tempNote =
                    _db.favorites_marker.FirstOrDefault(i => i.userId == user.Id && i.markerId == markerId);
                if (tempNote == null)
                    return;
                _db.favorites_marker.Remove(tempNote);
                _db.SaveChanges();
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

    }
}