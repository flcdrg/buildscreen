using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using BuildScreen.ContinousIntegration.Entities;
using BuildScreen.ContinousIntegration.Persistance;

namespace BuildScreen.ContinousIntegration.Client
{
    public class TeamCityClient : BaseClient, IClient
    {
        private struct BuildInfo
        {
            public string CompletedBuildId { get; set; }
            public string RunningBuildId { get; set; }
        }

        public TeamCityClient(ClientConfiguration clientConfiguration)
            : base(clientConfiguration)
        {
        }

        #region Implementation of IContinousIntegrationClient

        public ReadOnlyCollection<Build> Builds()
        {
            var typeIds = GetAllTypeIds();

            var lastBuildIdsByTypeId = typeIds.Select(typeId =>
            {
                var completedBuildId = GetLastCompletedBuildIdByTypeId(typeId);
                var runningBuildId = GetRunningBuildIdByTypeId(typeId);
                return new BuildInfo { CompletedBuildId = completedBuildId, RunningBuildId = runningBuildId };
            }).Where(info => !string.IsNullOrEmpty(info.CompletedBuildId));

            var builds = lastBuildIdsByTypeId.Select(info =>
            {
                var completedBuild = GetBuild(info.CompletedBuildId);
                var runningBuild = GetBuild(info.RunningBuildId);
                return completedBuild;
            });

            return new ReadOnlyCollection<Build>(builds.ToList());
        }

        private string GetRunningBuildIdByTypeId(string typeId)
        {
            return null;
        }

        public Build BuildByUniqueIdentifier(string key)
        {
            return GetBuild(GetLastCompletedBuildIdByTypeId(key));
        }

        #endregion

        internal IEnumerable<string> GetAllTypeIds()
        {
            var uri = new Uri(string.Concat(BaseUri(), "buildTypes/"));

            var xDocument = LoadXmlDocument(uri);
            var typeIdAttributes = from xml in xDocument.Elements("buildTypes").Elements("buildType").Attributes("id")
                                                       select xml;

            return typeIdAttributes.Select(typeIdAttribute => typeIdAttribute.Value).ToList();
        }

        internal string GetLastCompletedBuildIdByTypeId(string typeId)
        {
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}buildTypes/id:{1}/builds/?count=1", BaseUri(), typeId));

            var xDocument = LoadXmlDocument(uri);
            var buildIdAttribute = (from xml in xDocument.Elements("builds").Elements("build").Attributes("id")
                                           select xml).FirstOrDefault();

            return buildIdAttribute?.Value;
        }

        internal Build GetBuild(string buildId)
        {
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}builds/id:{1}", BaseUri(), buildId));

            var xDocument = LoadXmlDocument(uri);
            var xElementBuild = xDocument.Element("build");

            var xElementBuildType = xElementBuild.Element("buildType");

            return new Build
                {
                    Number = xElementBuild.Attribute("number").Value,
                    Status = xElementBuild.Attribute("status").Value.Equals("success", StringComparison.OrdinalIgnoreCase) ? Status.Success : Status.Fail,
                    StatusText = xElementBuild.Element("statusText").Value,
                    UniqueIdentifier = xElementBuildType.Attribute("id").Value,
                    TypeName = xElementBuildType.Attribute("name").Value,
                    ProjectName = xElementBuildType.Attribute("projectName").Value,

                    StartDate = DateTime.ParseExact(xElementBuild.Element("startDate").Value, "yyyyMMddTHHmmsszzzz", CultureInfo.InvariantCulture),
                    FinishDate = DateTime.ParseExact(xElementBuild.Element("finishDate").Value, "yyyyMMddTHHmmsszzzz", CultureInfo.InvariantCulture),
                };
        }
    }
}
