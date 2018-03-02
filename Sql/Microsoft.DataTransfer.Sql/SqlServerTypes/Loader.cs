using Microsoft.DataTransfer.Sql;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SqlServerTypes
{
    /// <summary>
    /// Utility methods related to CLR Types for SQL Server 
    /// </summary>
    static class Utilities
    {
        private static object loadLock = new object();
        private static bool loaded;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
            Justification = "Added with NuGet package")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "0",
            Justification = "Added with NuGet package")]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string libname);

        /// <summary>
        /// Loads the required native assemblies for the current architecture (x86 or x64)
        /// </summary>
        /// <param name="rootApplicationPath">
        /// Root path of the current application. Use Server.MapPath(".") for ASP.NET applications
        /// and AppDomain.CurrentDomain.BaseDirectory for desktop applications.
        /// </param>
        public static void LoadNativeAssemblies(string rootApplicationPath)
        {
            if (!loaded) lock (loadLock) if (!loaded)
            {
                var nativeBinaryPath = IntPtr.Size > 4
                    ? Path.Combine(rootApplicationPath, @"SqlServerTypes\x64\")
                    : Path.Combine(rootApplicationPath, @"SqlServerTypes\x86\");

                LoadNativeAssembly(nativeBinaryPath, "msvcr100.dll");
                LoadNativeAssembly(nativeBinaryPath, "SqlServerSpatial110.dll");

                loaded = true;
            }
        }

        private static void LoadNativeAssembly(string nativeBinaryPath, string assemblyName)
        {
            var path = Path.Combine(nativeBinaryPath, assemblyName);
            var ptr = LoadLibrary(path);
            if (ptr == IntPtr.Zero)
            {
                throw Errors.ErrorLoadingNativeBinaries(assemblyName, Marshal.GetLastWin32Error());
            }
        }
    }
}
