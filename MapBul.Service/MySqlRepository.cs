using System;
using System.Collections.Generic;
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
            if (userType.Tag == UserTypes.Admin || userType.Tag == UserTypes.Editor || userType.Tag == UserTypes.Guide)
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


        public List<marker> GetMarkersInSquare(double p1Lat, double p1Lng, double p2Lat, double p2Lng)
        {
            return _db.marker.ToList().Where(m => (m.Lat < Math.Max(p1Lat,p2Lat) && m.Lat > Math.Min(p1Lat,p2Lat)) && (m.Lng <  Math.Max(p1Lng,p2Lng) && m.Lng > Math.Min(p1Lng,p2Lng))&&m.status.Tag==MarkerStatuses.Published).ToList();
        }

        public marker GetMarker(int markerId)
        {
            return _db.marker.First(m => m.Id == markerId);
        }

        public List<category> GetRootCategories()
        {
            return _db.category.Where(c => c.ParentId == null).ToList();
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
            List<category> childCategories=new List<category>();
            FindChildCategoriesIteration(ref childCategories, id);
            return childCategories;
        }

        public List<article> GetArticles()
        {
            return _db.article.ToList();
        }

        public List<article> GetEvents()
        {
            return _db.article.Where(a=>a.EventDate!=null).ToList();
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

        public List<city> GetPermittedCities(string userGuid)
        {
            var user = _db.user.First(u => u.Guid == userGuid);
            var permittedCities = user.city_permission.Select(cp => cp).Select(city => city.city).ToList();
            foreach (var city in from country in user.country_permission.Select(countryPermission => countryPermission.country) from city in country.city where !permittedCities.Contains(city) select city)
            {
                permittedCities.Add(city);
            }
            return permittedCities;
        }

        public bool HavePermissions(string guid, int cityId)
        {
            var permittedCities = GetPermittedCities(guid);
            return permittedCities.Any(p => p.Id == cityId);
        }

        public List<category> GetCategories()
        {
            return _db.category.ToList();
        }

        public void AddNewTenant(string email, string firstName, string middleName, string lastName, DateTime birthDate, string gender, string phone, string address)
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
                MailProvider.SendMailWithCredintails(password,firstName,middleName,email);
            }
            catch
            {
                trans.Rollback();
                throw;
            }

        }

        public void RecoverPassword(string email)
        {
            var tenant = _db.tenant.FirstOrDefault(t => t.user.Email == email);
            if(tenant==null)
                throw new MyException(Errors.UserNotFound);
            var password = StringTransformationProvider.GeneratePassword();
            tenant.user.Password = StringTransformationProvider.Md5(password);
            _db.SaveChanges();
            MailProvider.SendMailWithCredintails(password,tenant.FirstName,tenant.MiddleName,tenant.LastName);
        }
    }
}