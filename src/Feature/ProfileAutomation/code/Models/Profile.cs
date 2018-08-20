using System;
using System.Collections.Generic;

namespace Sitecore.Feature.ProfileAutomation.Models
{
    public class Profile
    {
        public Guid Id { get; set; }
        public List<ProfileKey> Keys { get; set; } = new List<ProfileKey>();
    }
}