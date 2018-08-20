namespace Sitecore.Feature.ProfileAutomation.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using DependencyInjection;
    using Repositories;

    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IItemProfileRepository, ItemProfileRepository>();
        }
    }
}