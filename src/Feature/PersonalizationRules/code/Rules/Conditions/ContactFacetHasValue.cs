using System.Collections;
using Sitecore.Analytics;
using Sitecore.Analytics.Model.Framework;
using Sitecore.Analytics.Tracking;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Sitecore.Feature.PersonalizationRules.Rules.Conditions
{
    public class ContactFacetHasValue<T> : WhenCondition<T> where T : RuleContext
    {
        public string FacetValue { get; set; }

        public string FacetPath { get; set; }

        protected override bool Execute(T ruleContext)
        {
            var contact = Tracker.Current.Session.Contact;

            if (contact == null)
            {
                Log.Debug(this.GetType() + ": contact is null", this);
                return false;
            }

            if (string.IsNullOrEmpty(this.FacetPath))
            {
                Log.Debug(this.GetType() + ": facet path is empty", this);
                return false;
            }

            var inputPropertyToFind = this.FacetPath;

            var propertyPathArr = inputPropertyToFind.Split('.');
            if (propertyPathArr.Length == 0)
            {
                Log.Debug(this.GetType() + ": facet path is empty", this);
                return false;
            }

            var propertyQueue = new Queue(propertyPathArr);
            var facetName = propertyQueue.Dequeue().ToString();
            var facet = contact.Facets[facetName];
            if (facet == null)
            {
                Log.Debug($"{this.GetType()} : cannot find facet {facetName}", this);
                return false;
            }

            var datalist = facet.Members[propertyQueue.Dequeue().ToString()];
            if (datalist == null)
            {
                Log.Debug($"{this.GetType()} : cannot find facet {facetName}", this);
                return false;
            }

            var member = datalist as IModelAttributeMember;
            if (member != null)
            {
                var propValue = member.Value;
                return propValue?.Equals(this.FacetValue) ?? false;
            }
            if (datalist is IModelDictionaryMember)
            {
                var dictionaryMember = (IModelDictionaryMember)datalist;

                var elementName = propertyQueue.Dequeue().ToString();
                var element = dictionaryMember.Elements[elementName];
                if (element == null)
                {
                    Log.Debug($"{this.GetType()} : cannot find element {elementName}", this);
                    return false;
                }

                var propertyToFind = propertyQueue.Dequeue().ToString();
                var prop = element.Members[propertyToFind];
                if (prop == null)
                {
                    Log.Debug($"{this.GetType()} : cannot find property {propertyToFind}", this);
                    return false;
                }

                var propValue = ((IModelAttributeMember)prop).Value;
                return propValue?.Equals(this.FacetValue) ?? false;
            }
            var modelCollectionMember = datalist as IModelCollectionMember;
            if (modelCollectionMember != null)
            {
                var collectionMember = modelCollectionMember;
                var propertyToFind = propertyQueue.Dequeue().ToString();
                for (var i = 0; i < collectionMember.Elements.Count; i++)
                {
                    var element = collectionMember.Elements[i];
                    var prop = element.Members[propertyToFind];
                    if (prop == null)
                    {
                        Log.Debug($"{this.GetType()} : cannot find property {propertyToFind}", this);
                        return false;
                    }
                    var propValue = ((IModelAttributeMember)prop).Value;
                    if (propValue.Equals(this.FacetValue))
                        return true;
                }
            }

            return false;
        }
    }
}