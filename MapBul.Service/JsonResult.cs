using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MapBul.Service
{
   
    internal class JsonResult
    {
        private static readonly string[] NotSerializableFields = {"Password"};
        public bool Success { get; set; }
        public string ErrorReason { get; set; }
        public List<Dictionary<string, object>> Data { get; set; }

        public void AddObjectToResult(object o, int index)
        {
            if (Data.Count <= index)
            {
                Data.Add(new Dictionary<string, object>());
            }
            foreach (
                var prop in
                    o.GetType()
                        .GetProperties()
                        .Where(p => p.PropertyType.Attributes.HasFlag(TypeAttributes.Serializable))
                        .Where(prop => !NotSerializableFields.Contains(prop.Name)))
            {
                if (!Data[index].ContainsKey(prop.Name))
                    Data[index].Add(prop.Name, prop.GetValue(o));
            }
        }

        public JsonResult(string errorReason)
        {
            ErrorReason = errorReason;
            Success = false;
        }

        public JsonResult(List<Dictionary<string, object>> data)
        {
            Success = true;
            Data = data;
        }

        public JsonResult()
        {
            Success = false;
        }
    }
}
