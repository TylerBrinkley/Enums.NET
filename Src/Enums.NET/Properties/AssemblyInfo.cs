using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if NET20
[assembly: AssemblyTitle("Enums.NET .NET 2.0")]
[assembly: AllowPartiallyTrustedCallers]
#elif NET35
[assembly: AssemblyTitle("Enums.NET .NET 3.5")]
[assembly: AllowPartiallyTrustedCallers]
#elif NET40
[assembly: AssemblyTitle("Enums.NET .NET 4.0")]
[assembly: AllowPartiallyTrustedCallers]
#else
[assembly: AssemblyTitle("Enums.NET")]
[assembly: AllowPartiallyTrustedCallers]
#endif

[assembly: AssemblyDescription("Enums.NET is a high-performance type-safe .NET enum utility library")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Enums.NET")]
[assembly: AssemblyCopyright("Copyright © Tyler Brinkley 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("f2059370-3a4e-4885-8234-9aedade06108")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: CLSCompliant(true)]