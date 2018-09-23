using System.Collections.Generic;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.Web.Repository;

namespace MapBul.Web.Models
{
    public class TenantsListModel
    {
        public List<tenant> Tenants { get; set; }

        public TenantsListModel()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            Tenants = repo.GetTenants();
        }
    }
    public class JournalistsListModel
    {
        public JournalistsListModel()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            Journalists = repo.GetJournalists();
        }
        public JournalistsListModel(string userGuid)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            Journalists = repo.GetJournalists(userGuid);
        }
        public List<journalist> Journalists { get; set; }
    }

    public class GuidesListModel
    {
        public GuidesListModel()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            Guides = repo.GetGuides();
        }

        public GuidesListModel(string userGuid)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            Guides = repo.GetGuides(userGuid);
        }

        public List<guide> Guides { get; set; } 
    }

    public class EditorsListModel
    {
        public EditorsListModel()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            Editors = repo.GetEditors();
        }
        public List<editor> Editors { get; set; }
    }

    public class AdminsListModel
    {
        public AdminsListModel()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            Admins = repo.GetAdmins();
        }
        public List<admin> Admins { get; set; }
    }

    public class NewEditorModel:editor
    {
        public NewEditorModel()
        {
            PermittedCountries=new List<int>();
            PermittedCities=new List<int>();
            //PermittedRegions=new List<int>();
        }
        public NewEditorModel(editor editor)
        {
            foreach (var propertyInfo in editor.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name == "String")
                {
                    propertyInfo.SetValue(this, propertyInfo.GetValue(editor));
                }
            }
            Email = editor.user.Email;
            Password = editor.user.Password;
            Deleted = editor.user.Deleted;
            PermittedCountries=new List<int>();
            PermittedCities = new List<int>();
            //PermittedRegions = new List<int>();
            foreach (var countryPermission in editor.user.country_permission)
            {
                PermittedCountries.Add(countryPermission.country.Id);
            }
            /*foreach (var regionPermission in editor.user.region_permission)
            {
                PermittedRegions.Add(regionPermission.region.Id);
            }*/
            foreach (var cityPermission in editor.user.city_permission)
            {
                PermittedCities.Add(cityPermission.city.Id);
            }

        }

        public void CopyTo(ref editor editor)
        {
            foreach (var propertyInfo in editor.GetType().GetProperties())
            {
                if (!propertyInfo.Name.Contains("Id")&&(propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name=="String"))
                {
                    propertyInfo.SetValue(editor,propertyInfo.GetValue(this));
                }
            }
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public List<int> PermittedCountries { get; set; }
        //public List<int> PermittedRegions { get; set; }
        public List<int> PermittedCities { get; set; }
        public bool Deleted { get; set; }
    }

    public class NewJournalistModel:journalist
    {
        public NewJournalistModel()
        {
            PermittedCountries=new List<int>();
            PermittedCities=new List<int>();
            //PermittedRegions=new List<int>();
        }
        public NewJournalistModel(journalist journalist)
        {
            foreach (var propertyInfo in journalist.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name == "String")
                {
                    propertyInfo.SetValue(this, propertyInfo.GetValue(journalist));
                }
            }
            Email = journalist.user.Email;
            Password = journalist.user.Password;
            Deleted = journalist.user.Deleted;
            PermittedCountries=new List<int>();
            PermittedCities = new List<int>();
            //PermittedRegions = new List<int>();
            foreach (var countryPermission in journalist.user.country_permission)
            {
                PermittedCountries.Add(countryPermission.country.Id);
            }
            /*foreach (var regionPermission in journalist.user.region_permission)
            {
                PermittedRegions.Add(regionPermission.region.Id);
            }*/
            foreach (var cityPermission in journalist.user.city_permission)
            {
                PermittedCities.Add(cityPermission.city.Id);
            }

        }

        public void CopyTo(ref journalist journalist)
        {
            foreach (var propertyInfo in journalist.GetType().GetProperties())
            {
                if (!propertyInfo.Name.Contains("Id")&&(propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name=="String"))
                {
                    propertyInfo.SetValue(journalist,propertyInfo.GetValue(this));
                }
            }
            journalist.EditorId = EditorId;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public List<int> PermittedCountries { get; set; }
        //public List<int> PermittedRegions { get; set; }
        public List<int> PermittedCities { get; set; }
        public bool Deleted { get; set; }
    }

    public class NewAdminModel : admin
    {
        public NewAdminModel() { }
        public NewAdminModel(admin admin)
        {
            foreach (var propertyInfo in admin.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name == "String")
                {
                    propertyInfo.SetValue(this, propertyInfo.GetValue(admin));
                }
            }
            Email = admin.user.Email;
            Password = admin.user.Password;
            Deleted = admin.user.Deleted;
        }
        public void CopyTo(ref admin admin)
        {
            foreach (var propertyInfo in admin.GetType().GetProperties())
            {
                if (!propertyInfo.Name.Contains("Id") && (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name == "String"))
                {
                    propertyInfo.SetValue(admin, propertyInfo.GetValue(this));
                }
            }
        }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Deleted { get; set; }
    }




    public class NewGuideModel : guide
    {
        public NewGuideModel()
        {
            PermittedCountries = new List<int>();
            PermittedCities = new List<int>();
            //PermittedRegions = new List<int>();
        }
        public NewGuideModel(guide guide)
        {
            foreach (var propertyInfo in guide.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name == "String")
                {
                    propertyInfo.SetValue(this, propertyInfo.GetValue(guide));
                }
            }
            Email = guide.user.Email;
            Password = guide.user.Password;
            Deleted = guide.user.Deleted;
            PermittedCountries = new List<int>();
            PermittedCities = new List<int>();
            //PermittedRegions = new List<int>();
            foreach (var countryPermission in guide.user.country_permission)
            {
                PermittedCountries.Add(countryPermission.country.Id);
            }
            /*foreach (var regionPermission in guide.user.region_permission)
            {
                PermittedRegions.Add(regionPermission.region.Id);
            }*/
            foreach (var cityPermission in guide.user.city_permission)
            {
                PermittedCities.Add(cityPermission.city.Id);
            }
        }
        public void CopyTo(ref guide guide)
        {
            foreach (var propertyInfo in guide.GetType().GetProperties())
            {
                if (!propertyInfo.Name.Contains("Id") && (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name == "String"))
                {
                    propertyInfo.SetValue(guide, propertyInfo.GetValue(this));
                }
            }
            guide.EditorId = EditorId;
        }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Deleted { get; set; }
        public List<int> PermittedCountries { get; set; }
        //public List<int> PermittedRegions { get; set; }
        public List<int> PermittedCities { get; set; }
    }

    public class NewTenantModel : tenant
    {
        public NewTenantModel(tenant tenant)
        {
            foreach (var propertyInfo in tenant.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name == "String")
                {
                    propertyInfo.SetValue(this, propertyInfo.GetValue(tenant));
                }
            }
            Email = tenant.user.Email;
            Password = tenant.user.Password;
            Deleted = tenant.user.Deleted;
        }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Deleted { get; set; }

        public void CopyTo(ref tenant tenant)
        {
            foreach (var propertyInfo in tenant.GetType().GetProperties())
            {
                if (!propertyInfo.Name.Contains("Id") && (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name == "String"))
                {
                    propertyInfo.SetValue(tenant, propertyInfo.GetValue(this));
                }
            }
        }
    }

}