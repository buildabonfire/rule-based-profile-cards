using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Sitecore.Feature.ProfileAutomation.Repositories
{
    public interface IItemProfileRepository
    {
        void ApplyProfileCard(Item item, BaseItem profileItem);
        void ApplyProfileValues(Item item, List<Models.Profile> profiles);
    }
}