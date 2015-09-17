using System;
using System.Collections.Generic;
using System.Linq;
using BuildScreen.ContinousIntegration;
using BuildScreen.ContinousIntegration.Entities;
using BuildScreen.ContinousIntegration.Exceptions;
using BuildScreen.ContinousIntegration.Persistance;
using BuildScreen.Plugin;
using BuildScreen.Properties;

namespace BuildScreen
{
    public class Updater
    {
        private readonly PluginHandler _pluginHandler = new PluginHandler();
        private readonly IClientFactory _clientFactory = new ClientFactory();

        private readonly ClientConfiguration _config;

        public Updater(ClientConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<Build> GetBuildsFromCI()
        {
            if (!_config.AllDataExists) return new List<Build>();

            try
            {
                using (var client = _clientFactory.CreateClient(_config))
                {
                    if (!client.IsConnected) return new List<Build>();

                    return Settings.Default.Builds.Select(storedBuild =>
                        {
                            try
                            {
                                var build = client.BuildByUniqueIdentifier(storedBuild.UniqueIdentifier);

                                _pluginHandler.TriggerPlugins(storedBuild, build);

                                var daysLastBuildHappened = DateTime.Now.Subtract(build.StartDate).Days;
                                var happenedInLastWeeks = daysLastBuildHappened < Settings.Default.HideInactiveWeeks * 7;

                                if (!Settings.Default.HideInactive) return build;
                                if (Settings.Default.HideInactive && happenedInLastWeeks) return build;
                                return null; // Build is hidden, based on rules above
                            }
                            catch (ClientLoadDocumentException)
                            {
                                // Return an error-info-containing build when the given build cannot be loaded
                                return new Build { StatusText = "Could not obtain build info. Was this project removed on the server?" };
                            }
                        })
                        .Where(b => b != null)
                        .ToList();
                }
            }
            catch (ClientConnectionException)
            {
                return new List<Build>();
                // TODO: Display error message
            }
        }
    }
}