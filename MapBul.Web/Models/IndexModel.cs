using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.Web.Repository;

namespace MapBul.Web.Models
{
    [Serializable]
    public class MapInfoModel
    {
        public MapInfoModel()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            Markers = repo.GetMarkers();
            Cities = repo.GetCities();
        }

        public List<marker> Markers { get; set; }
        public List<city> Cities { get; set; } 
    }
}