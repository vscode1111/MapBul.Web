using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.XIsland.Repository;

namespace MapBul.XIsland.Models
{
    public class EventsListModel
    {
        private DateTime? _startDateTime;
        private DateTime? _endDateTime;

        public EventsListModel()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            Events = repo.GetEvents();
        }
        public DateTime? StartDateTime
        {
            get
            {
                return _startDateTime??DateTime.Now;
            }
            set
            {
                var repo = DependencyResolver.Current.GetService<IRepository>();
                _startDateTime = value;
                Events = repo.GetEvents(this);
            }
        }

        public DateTime? EndDateTime
        {
            get
            {
                return _endDateTime??DateTime.Now;
            }
            set
            {
                var repo = DependencyResolver.Current.GetService<IRepository>();
                _endDateTime = value;
                Events = repo.GetEvents(this);
            }
        }
        public List<article> Events { get; set; } 
    }
}