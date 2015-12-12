using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Moloki.Diagnostics.CorrelationLogger
{
    /// <summary>
    /// The class provides all necessary log operations.
    /// </summary>
    public class Logger : LoggerBase
    {
        #region Private Fields

        private static readonly IDictionary<string, TraceSource> TraceSources = new Dictionary<string, TraceSource>();

        #endregion

        #region Ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="trace">Trace source to use.</param>
        private Logger(TraceSource trace) : base(trace)
        {
        }

        #endregion
        
        #region Factory Methods

        /// <summary>
        /// Fabric method that returns new instance of the logger initialized 
        /// with assembly name of the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// Trace source name will be equal to name of the assembly that contains specified type.
        /// </typeparam>
        /// <returns>Configured instance of the logger.</returns>
        public static Logger GetAssemblyLogger<T>()
        {
            return new Logger(GetSourceByName(GetAssemblyName<T>()));
        }

        /// <summary>
        /// Fabric method that returns new instance of the logger initialized 
        /// with calling assembly name.
        /// </summary>
        /// <returns>Configured instance of the logger.</returns>
        public static Logger GetAssemblyLogger()
        {
            var callingAssemblyName = Assembly.GetCallingAssembly().GetName().Name;
            return new Logger(GetSourceByName(callingAssemblyName));
        }
       
        #endregion

        #region Public Methods

        /// <summary>
        /// Logs basic information about calling assembly, such as full name and file version.
        /// </summary>       
        public void LogAssemblyInformation()
        {
            var assembly = Assembly.GetCallingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            LogInfo("{0}, File Version: {1}", assembly.GetName(), versionInfo.FileVersion);
        }

        #endregion

        #region Private Methods

        private static TraceSource GetSourceByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            TraceSource result;
            if (!TraceSources.TryGetValue(name, out result))
            {
                result = new TraceSource(name);
                TraceSources[name] = result;
            }

            return result;
        }

        private static string GetAssemblyName<T>()
        {
            return typeof(T).Assembly.GetName().Name;
        }

        #endregion
    }
}