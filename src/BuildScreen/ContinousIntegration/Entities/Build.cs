using System;
using System.Collections.Generic;

namespace BuildScreen.ContinousIntegration.Entities
{
    [Serializable]
    public class Build
    {
        public string Number { get; set; } = "X";
        public BuildStatus BuildStatus { get; set; } = BuildStatus.Fail;
        public string StatusText { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime FinishDate { get; set; } = DateTime.UtcNow;
        public string UniqueIdentifier { get; set; }
        public string TypeName { get; set; }
        public string ProjectName { get; set; }
    }

    /// <remarks>
    /// This is a workaround, because generic types List(of T) are not supported in the settings file.
    /// </remarks>
    /// <see href="http://stackoverflow.com/questions/951876/can-you-have-a-generic-listof-t-in-your-settings-file"/>
    [Serializable]
    public class BuildList : List<Build>
    {
    }
}
