using System;
using System.Diagnostics;
using System.Globalization;

namespace Moloki.Diagnostics.CorrelationLogger
{
    /// <summary>
    /// The class provides all necessary log operations.
    /// </summary>
    public abstract class LoggerBase
    {
        // Define message size as 30Kb. Event Log does not support bigger messages.
        // See documentation for details: http://msdn.microsoft.com/en-us/library/6w20x90k.aspx
        private const int MaxMessageSize = 30 * 1024;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerBase"/> class.
        /// </summary>
        /// <param name="trace">Trace source to use.</param>
        protected LoggerBase(TraceSource trace)
        {
            if (trace == null)
                throw new ArgumentNullException(nameof(trace));

            Source = trace;
        }

        /// <summary>
        /// Gets trace source that is used to write messages to.
        /// </summary>
        public TraceSource Source { get; }

        /// <summary>
        /// Convenient method that returns new activity scope with specified name.
        /// </summary>
        /// <example>
        /// <code>
        /// Logger log = ...
        /// using(log.StartNewActivity("Some Interesting Activity"))
        /// { 
        ///    // ...
        /// }
        /// </code>
        /// </example>
        /// <param name="activityName">New activity name.</param>
        /// <returns>Activity scope</returns>
        public virtual ActivityScope StartNewActivity(string activityName)
        {
            return Source.Switch.ShouldTrace(TraceEventType.Start)
                ? new ActivityScope(Source, activityName)
                : ActivityScope.NullScope;
        }

        /// <summary>
        /// Convenient method that returns new activity scope with specified name.
        /// </summary>
        /// <example>
        /// <code>
        /// Logger log = ...
        /// using(log.StartNewActivity("Some Interesting Activity"))
        /// { 
        ///    // ...
        /// }
        /// </code>
        /// </example>
        /// <param name="activityNameFormat">A format string that contains zero or more format items, 
        /// which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <returns>Activity scope</returns>
        public virtual ActivityScope StartNewActivity(string activityNameFormat, params object[] args)
        {
            return StartNewActivity(string.Format(CultureInfo.InvariantCulture, activityNameFormat, args));
        }

        /// <summary>
        /// Writes an error message to the log using specified message.
        /// </summary>
        /// <param name="message">The informative message to write.</param>
        public void LogError(string message)
        {
            LogMessage(TraceEventType.Error, message);
        }

        /// <summary>
        /// Writes an error message to the log using the specified 
        /// array of objects and formatting information.
        /// </summary>
        /// <param name="format">A format string that contains zero or more format items, 
        /// which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        public void LogError(string format, params object[] args)
        {
            LogMessage(TraceEventType.Error, format, args);
        }

        /// <summary>
        /// Writes a warning message to the log using specified message.
        /// </summary>
        /// <param name="message">The informative message to write.</param>
        public void LogWarning(string message)
        {
            LogMessage(TraceEventType.Warning, message);
        }

        /// <summary>
        /// Writes a warning message to the log using the specified 
        /// array of objects and formatting information.
        /// </summary>
        /// <param name="format">A format string that contains zero or more format items, 
        /// which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        public void LogWarning(string format, params object[] args)
        {
            LogMessage(TraceEventType.Warning, format, args);
        }

        /// <summary>
        /// Writes an informational message to the log using specified message.
        /// </summary>
        /// <param name="message">The informative message to write.</param>
        public void LogInfo(string message)
        {
            LogMessage(TraceEventType.Information, message);
        }

        /// <summary>
        /// Writes an informational message to the log using the specified 
        /// array of objects and formatting information.
        /// </summary>
        /// <param name="format">A format string that contains zero or more format items, 
        /// which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        public void LogInfo(string format, params object[] args)
        {
            LogMessage(TraceEventType.Information, format, args);
        }

        /// <summary>
        /// Writes a verbose message to the log using specified message.
        /// </summary>
        /// <param name="message">The informative message to write.</param>
        public void LogVerbose(string message)
        {
            LogMessage(TraceEventType.Verbose, message);
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified 
        /// array of objects and formatting information.
        /// </summary>
        /// <param name="format">A format string that contains zero or more format items, 
        /// which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        public void LogVerbose(string format, params object[] args)
        {
            LogMessage(TraceEventType.Verbose, format, args);
        }

        /// <summary>
        /// Writes message of specified type to the log using specified message.
        /// </summary>
        /// <param name="eventType">Type of the message.</param>
        /// <param name="message">The informative message to write.</param>
        protected virtual void LogMessage(TraceEventType eventType, string message)
        {
            if (Source.Switch.ShouldTrace(eventType))
                Source.TraceEvent(eventType, 0, message);
        }

        /// <summary>
        /// Writes message of specified type to the log using the specified 
        /// array of objects and formatting information.
        /// </summary>
        /// <param name="eventType">Type of the message.</param>
        /// <param name="format">A format string that contains zero or more format items, 
        /// which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        protected virtual void LogMessage(TraceEventType eventType, string format, params object[] args)
        {
            Console.WriteLine("test");
            if (Source.Switch.ShouldTrace(eventType))
                Source.TraceEvent(eventType, 0, format, args);
        }
    }
}