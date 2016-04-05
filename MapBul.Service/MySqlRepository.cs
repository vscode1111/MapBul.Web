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
            email = TransformationProvider.TransformEmail(email);
            var md5Pass = TransformationProvider.Md5(password);
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
            if (userType.Tag == UserTypes.Admin || userType.Tag == UserTypes.Editor || userType.Tag == UserTypesMobile.Guide)
                userTypeMobile = _db.usertype.First(u=>u.Tag==UserTypesMobile.Guide);
            else
            {
                userTypeMobile = _db.usertype.First(u => u.Tag == UserTypesMobile.Tenant);
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
            return _db.marker.Where(m => (m.Lat < p1Lat && m.Lat > p2Lat) && (m.Lng > p1Lng && m.Lng < p2Lng)).ToList();
        }

        public marker GetMarker(int markerId)
        {
            return _db.marker.First(m => m.Id == markerId);
        }
    }
}