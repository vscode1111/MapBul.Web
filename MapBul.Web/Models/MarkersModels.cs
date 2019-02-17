using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.Web.Repository;

namespace MapBul.Web.Models
{

    public class MarkersListModel
    {
        public MarkersListModel(string guid)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var user = repo.GetUserByGuid(guid);
            var avaliableMarkers = repo.GetMarkers(guid);
            MyMarkers = avaliableMarkers.Where(m => m.UserId == user.Id).ToList();
            OtherMarkers = avaliableMarkers.Where(m => m.UserId != user.Id).ToList();
        }
        public List<marker> MyMarkers { get; set; }
        public List<marker> OtherMarkers { get; set; }
    }

    public class NewMarkerModel : marker
    {
        public NewMarkerModel()
        {
            
        }
        public NewMarkerModel(marker marker)
        {
            foreach (var propertyInfo in marker.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name == "String")
                {
                    propertyInfo.SetValue(this, propertyInfo.GetValue(marker));
                }
            }
            WorkTimes = marker.worktime.ToList();
            SubCategories = marker.subcategory.Select(sc => sc.CategoryId).ToList();
            Phones = marker.phone.Select(p => p.Number).ToList();
            marker_photos = marker.marker_photos.ToList();
        }

        public List<int> SubCategories { get; set; }

        public List<string> Phones { get; set; }

        public List<worktime> WorkTimes { get; set; } 

        public void CopyTo(marker marker)
        {
            foreach (var propertyInfo in marker.GetType().GetProperties())
            {
                if (!propertyInfo.Name.Contains("Id") && (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name == "String"))
                {
                    propertyInfo.SetValue(marker, propertyInfo.GetValue(this));
                }
            }
        }
    }
}