using System.Collections.ObjectModel;
using BuildScreen.ContinousIntegration.Entities;

namespace BuildScreen.ContinousIntegration.Persistance
{
    public interface IClient : IBaseClient
    {
        ReadOnlyCollection<Build> FetchBuilds();
        Build BuildByUniqueIdentifier(string key);
    }
}