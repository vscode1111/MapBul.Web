using System;
using System.Collections.Generic;
using System.Linq;
using MapBul.DBContext;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;
using MapBul.Web.Controllers;
using MapBul.Web.Models;

namespace MapBul.Web.Repository
{
    public class MySqlRepository : IRepository
    {
        private readonly Context _db;

        public MySqlRepository()
        {
            _db = new Context();
        }

        public user GetUserByLoginAndPassword(string email, string password)
        {
            var md5Pass = TransformationProvider.Md5(password);
            email = TransformationProvider.TransformEmail(email);
            user user = _db.user.FirstOrDefault(u => u.Email == email && md5Pass == u.Password);
            if (user == null)
                throw new MyException(Errors.UserNotFound);
            return user;
        }

        public user GetUserByGuid(string guid)
        {
            user user = _db.user.FirstOrDefault(u => u.Guid == guid);
            if (user == null)
                throw new MyException(Errors.UserNotFound);
            return user;
        }

        public List<journalist> GetJournalists()
        {
            return _db.journalist.ToList();
        }

        public List<editor> GetEditors()
        {
            return _db.editor.ToList();
        }

        public int GetUserTypeByTag(string tag)
        {
            return _db.usertype.First(u => u.Tag == tag).Id;
        }

        public List<admin> GetAdmins()
        {
            return _db.admin.ToList();
        }

        public void AddNewEditor(NewEditorModel model)
        {
            model.Email = TransformationProvider.TransformEmail(model.Email);
            if (_db.user.Any(u => u.Email == model.Email))
                throw new MyException(Errors.UserExists);
            var trans = _db.Database.BeginTransaction();
            try
            {
                var newUser = new user
                {
                    Guid = Guid.NewGuid().ToString(),
                    Password = TransformationProvider.Md5(model.Password),
                    Email = model.Email,
                    UserTypeId = GetUserTypeByTag(UserTypes.Editor),
                    Deleted = model.Deleted
                };
                _db.user.Add(newUser);
                _db.SaveChanges();
                editor newEditor = new editor();
                model.CopyTo(ref newEditor);
                newEditor.UserId = newUser.Id;
                _db.editor.Add(newEditor);

                _db.country_permission.AddRange(
                    model.PermittedCountries.Select(c => new country_permission {CountryId = c, UserId = newUser.Id}));
                _db.city_permission.AddRange(
                    model.PermittedCities.Select(c => new city_permission {CityId = c, UserId = newUser.Id}));
                _db.region_permission.AddRange(
                    model.PermittedRegions.Select(c => new region_permission {RegionId = c, UserId = newUser.Id}));

                _db.SaveChanges();
                trans.Commit();


            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public editor GetEditor(int editorId)
        {
            return _db.editor.First(e => e.Id == editorId);
        }

        public void SaveEditorChanges(NewEditorModel model)
        {
            editor editor = _db.editor.FirstOrDefault(e => e.Id == model.Id);
            if (editor == null)
                throw new MyException(Errors.UserNotFound);

            model.CopyTo(ref editor);
            //editor.user.Password = TransformationProvider.Md5(model.Password);
            editor.user.Email = TransformationProvider.TransformEmail(model.Email);
            editor.user.Deleted = model.Deleted;

            //сохранение новых прав
            _db.country_permission.RemoveRange(editor.user.country_permission);
            _db.city_permission.RemoveRange(editor.user.city_permission);
            _db.region_permission.RemoveRange(editor.user.region_permission);

            _db.country_permission.AddRange(
                model.PermittedCountries.Select(c => new country_permission {CountryId = c, UserId = editor.UserId}));
            _db.city_permission.AddRange(
                model.PermittedCities.Select(c => new city_permission {CityId = c, UserId = editor.UserId}));
            _db.region_permission.AddRange(
                model.PermittedRegions.Select(c => new region_permission {RegionId = c, UserId = editor.UserId}));


            _db.SaveChanges();
        }

        public List<country> GetCountries()
        {
            return _db.country.ToList();
        }

        public List<region> GetRegions()
        {
            return _db.region.ToList();
        }

        public List<city> GetCities()
        {
            return _db.city.ToList();
        }

        public void AddNewJournalist(NewJournalistModel model)
        {
            model.Email = TransformationProvider.TransformEmail(model.Email);
            if (_db.user.Any(u => u.Email == model.Email))
                throw new MyException(Errors.UserExists);
            var trans = _db.Database.BeginTransaction();
            try
            {
                var newUser = new user
                {
                    Guid = Guid.NewGuid().ToString(),
                    Password = TransformationProvider.Md5(model.Password),
                    Email = model.Email,
                    UserTypeId = GetUserTypeByTag(UserTypes.Editor),
                    Deleted = model.Deleted
                };
                _db.user.Add(newUser);
                _db.SaveChanges();
                journalist newEditor = new journalist();
                model.CopyTo(ref newEditor);
                newEditor.UserId = newUser.Id;
                _db.journalist.Add(newEditor);
                _db.country_permission.AddRange(
                    model.PermittedCountries.Select(c => new country_permission {CountryId = c, UserId = newUser.Id}));
                _db.city_permission.AddRange(
                    model.PermittedCities.Select(c => new city_permission {CityId = c, UserId = newUser.Id}));
                _db.region_permission.AddRange(
                    model.PermittedRegions.Select(c => new region_permission {RegionId = c, UserId = newUser.Id}));

                _db.SaveChanges();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public journalist GetJournalist(int journalistId)
        {
            return _db.journalist.First(j => j.Id == journalistId);
        }

        public void SaveJournalistChanges(NewJournalistModel model)
        {
            journalist journalist = _db.journalist.FirstOrDefault(e => e.Id == model.Id);
            if (journalist == null)
                throw new MyException(Errors.UserNotFound);

            model.CopyTo(ref journalist);
            //editor.user.Password = TransformationProvider.Md5(model.Password);
            journalist.user.Email = TransformationProvider.TransformEmail(model.Email);
            journalist.user.Deleted = model.Deleted;

            //сохранение новых прав
            _db.country_permission.RemoveRange(journalist.user.country_permission);
            _db.city_permission.RemoveRange(journalist.user.city_permission);
            _db.region_permission.RemoveRange(journalist.user.region_permission);

            _db.country_permission.AddRange(
                model.PermittedCountries.Select(c => new country_permission { CountryId = c, UserId = journalist.UserId }));
            _db.city_permission.AddRange(
                model.PermittedCities.Select(c => new city_permission { CityId = c, UserId = journalist.UserId }));
            _db.region_permission.AddRange(
                model.PermittedRegions.Select(c => new region_permission { RegionId = c, UserId = journalist.UserId }));


            _db.SaveChanges();
        }

        public void AddCountry(string name)
        {
            _db.country.Add(new country {Name = name});
            _db.SaveChanges();
        }

        public void AddRegion(string name, int countryId)
        {
            _db.region.Add(new region {Name = name, CountryId = countryId});
            _db.SaveChanges();
        }

        public void AddCity(string name, int regionId)
        {
            region region = GetRegion(regionId);
            var coordinates =
                ExternalRequest.ExternalRequestProvider.GetCoordinates(region.country.Name + ", " + region.Name + ", " +
                                                                       name);
            _db.city.Add(new city {Name = name, RegionId = regionId, Lat = coordinates.Lat, Lng = coordinates.Lng});
            _db.SaveChanges();
        }

        public List<category> GetCategories()
        {
            return _db.category.ToList();
        }

        private void UpdateCategoryParent(NestableElement structure, int? parrentId)
        {
            _db.category.First(c => c.Id == structure.id).ParentId = parrentId;
            if(structure.children==null)
                return;
            foreach (var element in structure.children)
            {
                UpdateCategoryParent(element, structure.id);
            }
        }

        public void SaveCategoriesStructure(List<NestableElement> structure)
        {
            foreach (var element in structure)
            {
                UpdateCategoryParent(element,null);
            }
            _db.SaveChanges();
        }

        public void AddNewCategory(category model)
        {
            _db.category.Add(model);
            _db.SaveChanges();
        }

        public category GetCategory(int categoryId)
        {
            return _db.category.First(c => c.Id == categoryId);
        }

        public void EditCategory(category model)
        {
            var existingCategory=_db.category.First(c => c.Id == model.Id);
            existingCategory.Icon = model.Icon;
            existingCategory.Name = model.Name;
            existingCategory.ParentId = model.ParentId;
            _db.SaveChanges();
        }

        public void AddNewAdmin(NewAdminModel model)
        {
            model.Email = TransformationProvider.TransformEmail(model.Email);
            if (_db.user.Any(u => u.Email == model.Email))
                throw new MyException(Errors.UserExists);
            var trans = _db.Database.BeginTransaction();
            try
            {
                var newUser = new user
                {
                    Guid = Guid.NewGuid().ToString(),
                    Password = TransformationProvider.Md5(model.Password),
                    Email = model.Email,
                    UserTypeId = GetUserTypeByTag(UserTypes.Admin),
                    Deleted = false
                };
                _db.user.Add(newUser);
                _db.SaveChanges();
                var admin = new admin();
                model.CopyTo(ref admin);
                admin.UserId = newUser.Id;
                _db.admin.Add(admin);
                _db.SaveChanges();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public List<marker> GetMarkers()
        {
            return _db.marker.OrderByDescending(m=>m.AddedDate).ToList();
        }

        public region GetRegion(int regionId)
        {
            return _db.region.First(r => r.Id == regionId);
        }
    }
}