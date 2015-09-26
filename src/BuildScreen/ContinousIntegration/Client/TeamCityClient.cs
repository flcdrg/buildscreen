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

        public ReadOnlyCollection<Build> FetchBuilds()
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
                var runningBuild = info.RunningBuildId != null ? GetBuild(info.RunningBuildId) : null;
                completedBuild.PercentageComplete = runningBuild?.PercentageComplete ?? 100;
                return completedBuild;
            });

            var buildList = builds.ToList();
            return new ReadOnlyCollection<Build>(buildList);
        }
        
        public Build BuildByUniqueIdentifier(string key)
        {
            var build = GetBuild(GetLastCompletedBuildIdByTypeId(key));
            var runningBuildId = GetRunningBuildIdByTypeId(key);
            var runningBuild = runningBuildId != null ? GetBuild(runningBuildId) : null;
            build.PercentageComplete = runningBuild?.PercentageComplete ?? 100;
            return build;
        }

        #endregion

        internal IEnumerable<string> GetAllTypeIds()
        {
            var uri = new Uri(string.Concat(BaseUri(), "buildTypes/"));

            var xDocument = LoadXmlDocument(uri);
            var typeIdAttributes = xDocument.Elements("buildTypes").Elements("buildType").Attributes("id").Select(xml => xml);

            return typeIdAttributes.Select(typeIdAttribute => typeIdAttribute.Value).ToList();
        }

        internal string GetLastCompletedBuildIdByTypeId(string typeId)
        {
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}buildTypes/id:{1}/builds/?count=1", BaseUri(), typeId));

            var xDocument = LoadXmlDocument(uri);
            var buildIdAttribute = (xDocument.Elements("builds").Elements("build").Attributes("id").Select(xml => xml)).FirstOrDefault();

            return buildIdAttribute?.Value;
        }

        private string GetRunningBuildIdByTypeId(string typeId)
        {
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}builds?locator=running:true", BaseUri()));
            var xDocument = LoadXmlDocument(uri);
            var xBuilds = xDocument.Element("builds")?.Elements("build");
            var xRunningBuild = xBuilds?.FirstOrDefault(b => b.Attribute("buildTypeId").Value == typeId);
            return xRunningBuild?.Attribute("id").Value;
        }

        internal Build GetBuild(string buildId)
        {
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}builds/id:{1}", BaseUri(), buildId));

            var xDocument = LoadXmlDocument(uri);
            var xBuildId = xDocument.Element("build");
            var xBuildType = xBuildId.Element("buildType");
            var xFinishDate = xBuildId.Element("finishDate");
            var xPercentageComplete = xBuildId.Attribute("percentageComplete");

            return new Build
                {
                    Number = xBuildId.Attribute("number").Value,
                    Status = xBuildId.Attribute("status").Value.Equals("success", StringComparison.OrdinalIgnoreCase) ? Status.Success : Status.Fail,
                    StatusText = xBuildId.Element("statusText").Value,
                    UniqueIdentifier = xBuildType.Attribute("id").Value,
                    TypeName = xBuildType.Attribute("name").Value,
                    ProjectName = xBuildType.Attribute("projectName").Value,

                    StartDate = DateTime.ParseExact(xBuildId.Element("startDate").Value, "yyyyMMddTHHmmsszzzz", CultureInfo.InvariantCulture),
                    FinishDate = xFinishDate != null ? DateTime.ParseExact(xFinishDate.Value, "yyyyMMddTHHmmsszzzz", CultureInfo.InvariantCulture) : DateTime.MaxValue,
                    PercentageComplete = xPercentageComplete != null ? int.Parse(xPercentageComplete.Value) : 100
                };
        }
    }
}
