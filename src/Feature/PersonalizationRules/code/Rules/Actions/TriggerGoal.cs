using System.Linq;
using Sitecore.Analytics;
using Sitecore.Analytics.Data.Items;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Actions;

namespace Sitecore.Feature.PersonalizationRules.Rules.Actions
{
    /// <summary>Defines the run class.</summary>
    /// <typeparam name="T">The rule context.</typeparam>
    public class TriggerGoal<T> : RuleAction<T> where T : RuleContext
    {
        /// <summary>Gets or sets the script id.</summary>
        /// <value>The script id.</value>
        public string GoalId { get; set; }

        /// <summary>Executes the specified rule context.</summary>
        /// <param name="ruleContext">The rule context.</param>
        public override void Apply(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            var item = ruleContext.Item;
            var goalItem = item?.Database.GetItem(this.GoalId);
            if (goalItem == null)
                return;

            ProcessGoal(goalItem);
        }

        private static void ProcessGoal(Item goalItem)
        {
            if (!IfGoalAlreadyExists(goalItem))
            {
                FireGoal(goalItem.ID);
            }
        }

        private static bool IfGoalAlreadyExists(Item goalItem)
        {
            var goalsTriggered = Tracker.Current.Session.Interaction.Pages.SelectMany(x => x.PageEvents).Where(x => x.IsGoal).ToList();
            return goalsTriggered.Any(x => x.PageEventDefinitionId == goalItem.ID.ToGuid());
        }

        public static void FireGoal(ID goalPath, bool fromApi = false)
        {
            if (Tracker.IsActive && Tracker.Current?.CurrentPage != null)
            {
                // Trigger a goal
                var goalItem = Context.Database.GetItem(goalPath); // Goal item

                if (goalItem == null) return;
                
                var goal = new PageEventItem(goalItem); // Wrapper for goal
                var page = fromApi ? Tracker.Current.Session.Interaction.PreviousPage : Tracker.Current.CurrentPage;

                if (page == null) return;

                var pageEventsRow = page.Register(goal); // Goal rtecord to be stored
                pageEventsRow.Data = goalItem["Description"];
                Tracker.Current.Interaction.AcceptModifications();
                Log.Debug($"Goal Triggered: Contact: {Tracker.Current.Contact.ContactId} : {goalPath} / {goalItem.Name}", "");
            }
        }
    }
}