using System.Collections.Generic;
using System.Linq;
using MapBul.DBContext;

namespace MapBul.Web.Models
{
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