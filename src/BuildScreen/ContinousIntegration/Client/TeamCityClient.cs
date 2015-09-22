﻿using System;
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
            IList<Build> builds = (from typeId in GetAllTypeIds()
                                   select GetLastBuildIdByTypeId(typeId)
                                   into buildId where !string.IsNullOrEmpty(buildId) select GetBuildByBuildId(buildId)).ToList();

            return new ReadOnlyCollection<Build>(builds);
        }

        public Build BuildByUniqueIdentifier(string key)
        {
            return GetBuildByBuildId(GetLastBuildIdByTypeId(key));
        }

        #endregion

        internal IEnumerable<string> GetAllTypeIds()
        {
            Uri uri = new Uri(string.Concat(BaseUri(), "buildTypes/"));

            XDocument xDocument = LoadXmlDocument(uri);
            IEnumerable<XAttribute> typeIdAttributes = from xml in xDocument.Elements("buildTypes").Elements("buildType").Attributes("id")
                                                       select xml;

            return typeIdAttributes.Select(typeIdAttribute => typeIdAttribute.Value).ToList();
        }

        internal string GetLastBuildIdByTypeId(string typeId)
        {
            Uri uri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}buildTypes/id:{1}/builds/?count=1", BaseUri(), typeId));

            XDocument xDocument = LoadXmlDocument(uri);
            XAttribute buildIdAttribute = (from xml in xDocument.Elements("builds").Elements("build").Attributes("id")
                                           select xml).FirstOrDefault();

            return buildIdAttribute?.Value;
        }

        internal Build GetBuildByBuildId(string buildId)
        {
            Uri uri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}builds/id:{1}", BaseUri(), buildId));

            XDocument xDocument = LoadXmlDocument(uri);
            XElement xElementBuild = xDocument.Element("build");

            XElement xElementBuildType = xElementBuild.Element("buildType");

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
