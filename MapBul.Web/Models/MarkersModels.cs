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
        }
    }
}