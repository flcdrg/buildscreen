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
        public TeamCityClient(ClientConfiguration clientConfiguration)
            : base(clientConfiguration)
        {
        }

        #region Implementation of IContinousIntegrationClient

        public ReadOnlyCollection<Build> Builds()
        {
            var typeIds = GetAllTypeIds();
            var buildIds = typeIds.Select(GetLastBuildIdByTypeId).Where(buildId => !string.IsNullOrEmpty(buildId));
            var builds = buildIds.Select(buildId => GetBuildByBuildId(buildId));

            return new ReadOnlyCollection<Build>(builds.ToList());
        }

        public Build BuildByUniqueIdentifier(string key)
        {
            return GetBuildByBuildId(GetLastBuildIdByTypeId(key));
        }

        #endregion

        internal IEnumerable<string> GetAllTypeIds()
        {
            var uri = new Uri(string.Concat(BaseUri(), "buildTypes/"));

            var xDocument = LoadXmlDocument(uri);
            var typeIdAttributes =
                xDocument.Elements("buildTypes").Elements("buildType").Attributes("id").Select(xml => xml);

            return typeIdAttributes.Select(typeIdAttribute => typeIdAttribute.Value).ToList();
        }

        internal string GetLastBuildIdByTypeId(string typeId)
        {
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}buildTypes/id:{1}/builds/?count=1", BaseUri(), typeId));

            var xDocument = LoadXmlDocument(uri);
            var buildIdAttribute = (xDocument.Elements("builds").Elements("build").Attributes("id").Select(xml => xml)).FirstOrDefault();

            return buildIdAttribute?.Value;
        }

        internal Build GetBuildByBuildId(string buildId)
        {
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}builds/id:{1}", BaseUri(), buildId));

            var xDocument = LoadXmlDocument(uri);
            var xElementBuild = xDocument.Element("build");
            var xElementBuildType = xElementBuild.Element("buildType");

            return new Build
                {
                    Number = xElementBuild.Attribute("number").Value,
                    BuildStatus = xElementBuild.Attribute("status").Value.Equals("success", StringComparison.OrdinalIgnoreCase) ? BuildStatus.Success : BuildStatus.Fail,
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
