using System.Xml;
using Sitecore.Analytics.Data;
using Sitecore.SecurityModel;

namespace Sitecore.Feature.ProfileAutomation.Repositories
{
    using System.Collections.Generic;
    using Data.Items;

    public class ItemProfileRepository : IItemProfileRepository
    {
        protected XmlDocument Document;

        public void ApplyProfileCard(Item item, BaseItem profileItem)
        {
            var trackingFields = new List<TrackingField>
            {
                new TrackingField(profileItem.Fields[Templates.ProfileCard.Fields.ProfileCardValue])
            };

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item["__tracking"] = trackingFields[0].Value;
                item.Editing.EndEdit();
            }
        }

        public void ApplyProfileValues(Item item, List<Models.Profile> profiles)
        {
            Document = new XmlDocument();
            var tracking = Document.CreateElement("tracking");

            AddProfiles(profiles, tracking);

            Document.AppendChild(tracking);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item["__tracking"] = Document.OuterXml;
                item.Editing.EndEdit();
            }
        }

        protected void AddProfiles(List<Models.Profile> profiles, XmlElement element)
        {
            foreach (var profile in profiles)
            {
                AddProfile(profile, element);
            }
        }

        protected void AddProfile(Models.Profile profile, XmlElement element)
        {
            var profileXml = Document.CreateElement("profile");
            profileXml.SetAttribute("id", profile.Id.ToString());
            profileXml.SetAttribute("name", "Score");
            
            foreach (var key in profile.Keys)
            {
                AddKey(key, profileXml);
            }

            element.AppendChild(profileXml);
        }

        protected void AddKey(Models.ProfileKey profileKey, XmlElement element)
        {
            var keyXml = Document.CreateElement("key");
            keyXml.SetAttribute("name", profileKey.Name);
            keyXml.SetAttribute("value", profileKey.Value.ToString());
            element.AppendChild(keyXml);
        }
    }
}