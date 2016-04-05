using System;
using System.Globalization;
using System.Net;
using System.Xml.Linq;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;

namespace MapBul.Web.ExternalRequest
{
    public static class ExternalRequestProvider
    {
        public static dynamic GetCoordinates(string address)
        {
            var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false",address);

            var request = WebRequest.Create(requestUri);
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());

            var result = xdoc.Element("GeocodeResponse").Element("result");
            var type = result.Element("type").Value;
            if(type!="locality")
                throw new MyException(Errors.NotFound);
            var locationElement = result.Element("geometry").Element("location");
            var lat = Convert.ToSingle(locationElement.Element("lat").Value,
                new NumberFormatInfo {NumberDecimalSeparator = ".", NumberGroupSeparator = ","});
            var lng = Convert.ToSingle(locationElement.Element("lng").Value,
                new NumberFormatInfo {NumberDecimalSeparator = ".", NumberGroupSeparator = ","});
            return new {Lat=lat, Lng=lng};
        }
    }
}