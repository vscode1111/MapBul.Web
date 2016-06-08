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
            return _db.user.First(u => u.Guid == userGuid);
        }

        public int GetDiscountId(int discount)
        {
            return _db.discount.First(d => d.Value == discount).Id;
        }

        public List<status> GetStatuses()
        {
            return _db.status.ToList();
        }
    }
}