using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Feature.ProfileAutomation.Models;
using Sitecore.Feature.ProfileAutomation.Repositories;

namespace Sitecore.Feature.ProfileAutomation.Tasks
{
    public class ApplyProfiles
    {
        private readonly IItemProfileRepository itemProfileRepository;

        public ApplyProfiles()
        {
            itemProfileRepository =
                (IItemProfileRepository) ServiceLocator.ServiceProvider.GetService(typeof(IItemProfileRepository));

        }

        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            Diagnostics.Log.Info("My Sitecore scheduled task is being run!", this);

            var profiles = new List<Profile>();
            var profile = new Profile();
            profile.Id = new ID("{9311296F-1A3D-4E58-AED1-0A6B4B1E654B}").ToGuid();
            profile.Keys.Add(new ProfileKey { Name = "Lead", Value = 5});
            profile.Keys.Add(new ProfileKey { Name = "Test2", Value = 7});
            profiles.Add(profile);

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var item = db.GetItem(new ID("{25DC062A-23FB-48E4-A93F-C20080C823BD}"));
            itemProfileRepository.ApplyProfileValues(item, profiles);
        }
    }
}