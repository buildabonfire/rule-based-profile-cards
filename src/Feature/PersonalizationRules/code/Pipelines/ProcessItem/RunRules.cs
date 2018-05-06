using Sitecore.Analytics.Pipelines.ProcessItem;
using Sitecore.Diagnostics;
using Sitecore.Rules;

namespace Sitecore.Feature.PersonalizationRules.Pipelines.ProcessItem
{
    public class RunRules : ProcessItemProcessor
    {
        public override void Process(ProcessItemArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");

            var obj = Context.Item;
            if (obj == null)
                return;
            var parentItem = obj.Database.GetItem(Constants.ProcessItemRules);
            if (parentItem == null)
                return;
            var rules = RuleFactory.GetRules<RuleContext>(parentItem, "Rule");
            if (rules == null)
                return;
            var ruleContext = new RuleContext()
            {
                Item = obj
            };
            rules.Run(ruleContext);
        }
    }
}