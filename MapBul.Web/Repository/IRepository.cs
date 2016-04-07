using System.Collections.Generic;
using MapBul.DBContext;
using MapBul.Web.Controllers;
using MapBul.Web.Models;

namespace MapBul.Web.Repository
{
    public interface IRepository
    {
        user GetUserByLoginAndPassword(string login, string password);
        user GetUserByGuid(string guid);
        List<journalist> GetJournalists();
        List<editor> GetEditors();
        List<admin> GetAdmins();
        void AddNewEditor (NewEditorModel model);
        editor GetEditor(int editorId);
        void SaveEditorChanges(NewEditorModel model);
        List<country> GetCountries();
        List<region> GetRegions();
        List<city> GetCities();
        void AddNewJournalist(NewJournalistModel model);
        journalist GetJournalist(int journalistId);
        void SaveJournalistChanges(NewJournalistModel model);
        void AddCountry(string name);
        void AddRegion(string name, int countryId);
        void AddCity(string name, int regionId);
        List<category> GetCategories();
        void SaveCategoriesStructure(List<NestableElement> structure);
        void AddNewCategory(category model);
        category GetCategory(int categoryId);
        void EditCategory(category model);
        void AddNewAdmin(NewAdminModel model);
        List<marker> GetMarkers();
        List<discount> GetDiscounts();
        List<status> GetStatuses();
        List<weekday> GetWeekDays();
        void AddMarker(NewMarkerModel model, List<WorkTimeDay> openTimes, List<WorkTimeDay> closeTimes, string userGuid);
        void ChangeMarkerStatus(int markerId, int statusId);
        marker GetMarker(int markerId);
        void EditMarker(NewMarkerModel model, List<WorkTimeDay> openTimes, List<WorkTimeDay> closeTimes, string userGuid);
        List<article> GetArticles();
    }
}