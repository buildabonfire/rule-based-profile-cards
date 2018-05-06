using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Sitecore.Analytics;
using Sitecore.Analytics.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Actions;

namespace Sitecore.Feature.PersonalizationRules.Rules.Actions
{
    /// <summary>Defines the run class.</summary>
    /// <typeparam name="T">The rule context.</typeparam>
    public class ApplyProfileValue<T> : RuleAction<T> where T : RuleContext
    {
        /// <summary>Gets or sets the script id.</summary>
        /// <value>The script id.</value>
        public string Profilecardkey { get; set; }
        

        /// <summary>Executes the specified rule context.</summary>
        /// <param name="ruleContext">The rule context.</param>
        public override void Apply(T ruleContext)
        {
            Assert.ArgumentNotNull((object)ruleContext, "ruleContext");
            var ruleItem = ruleContext.Item;
            var profile = ruleItem?.Database.GetItem(this.Profilecardkey);
            if (profile == null)
                return;

            ProcessProfile(profile);
        }

        private static void ProcessProfile(BaseItem profileItem)
        {
            var isActive = Tracker.Current != null && Tracker.Current.IsActive;
            if (!isActive) return;

            var trackingFields = new List<TrackingField>
            {
                new TrackingField(profileItem.Fields[Templates.ProfileCard.Fields.ProfileCardValue])
            };

            var fields = (IEnumerable<TrackingField>)trackingFields;

            var interaction = Tracker.Current?.Session?.Interaction;
            if (interaction == null) return;
            
            TrackingFieldProcessor.ProcessProfiles(interaction, fields.FirstOrDefault());
        }
    }
}