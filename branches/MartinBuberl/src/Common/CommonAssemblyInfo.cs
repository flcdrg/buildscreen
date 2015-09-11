using System.Reflection;

[assembly: AssemblyProduct("BuildScreen")]
// this name is used in the folder "C:\Users\YourUserName\AppData\Local\[->BuildScreen<-]" to store the settings
[assembly: AssemblyCompany("BuildScreen")]

[assembly: AssemblyCopyright("Copyright (c) 2010, Martin Buberl, Carl-Otto Kjellkvist")]
[assembly: AssemblyTrademark("")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else // RELEASE
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyVersion("0.2.5.0")]
[assembly: AssemblyFileVersion("0.2.5.0")]
