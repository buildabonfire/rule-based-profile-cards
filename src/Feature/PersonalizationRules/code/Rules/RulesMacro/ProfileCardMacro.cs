using System.Linq;
using System.Xml.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Marketing.Definitions.Profiles;
using Sitecore.Rules.RuleMacros;
using Sitecore.Shell.Applications.Dialogs.ItemLister;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Feature.PersonalizationRules.Rules.RulesMacro
{
    public class ProfileCardMacro : IRuleMacro
    {
        public void Execute(XElement element, string name, UrlString parameters, string value)
        {
            Assert.ArgumentNotNull((object)element, "element");
            Assert.ArgumentNotNull((object)name, "name");
            Assert.ArgumentNotNull((object)parameters, "parameters");
            Assert.ArgumentNotNull((object)value, "value");

            var selectItemOptions = new SelectItemOptions();
            var obj1 = (Item)null;
            if (!string.IsNullOrEmpty(value))
                obj1 = Client.ContentDatabase.GetItem(value);
            var path = XElement.Parse(element.ToString()).FirstAttribute.Value;
            if (!string.IsNullOrEmpty(path))
            {
                var obj2 = Client.ContentDatabase.GetItem(path);
                if (obj2 != null)
                    selectItemOptions.FilterItem = obj2;
            }
            selectItemOptions.Root = Client.ContentDatabase.GetItem(new ID(WellKnownIdentifiers.ProfilesContainerId));
            selectItemOptions.SelectedItem = obj1 ?? (selectItemOptions.Root != null ? selectItemOptions.Root.Children.FirstOrDefault<Item>() : (Item)null);
            selectItemOptions.IncludeTemplatesForSelection = SelectItemOptions.GetTemplateList("{0FC09EA4-8D87-4B0E-A5C9-8076AE863D9C}");
            selectItemOptions.Title = "Select Profile Card";
            selectItemOptions.Text = "Select the profile card to use in this rule.";
            selectItemOptions.Icon = "Business/16x16/chart.png";
            selectItemOptions.ShowRoot = false;
            SheerResponse.ShowModalDialog(selectItemOptions.ToUrlString().ToString(), "1200px", "700px", string.Empty, true);
        }
    }
}