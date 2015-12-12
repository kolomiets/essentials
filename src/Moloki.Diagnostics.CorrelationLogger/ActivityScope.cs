using System;
using System.Diagnostics;

namespace Moloki.Diagnostics.CorrelationLogger
{
    /// <summary>
    /// Helper class that simplifies work with activities.
    /// </summary>
    /// <example>
    /// <code>
    /// TraceSource trace = ...
    /// using (var scope = new ActivityScope(trace, "Some Interesting Activity"))
    /// {
    ///    // ...
    ///    using (var scope = new ActivityScope(trace, "Nested Activity"))   
    ///    {
    ///       // ...
    ///    }
    ///    // ...
    /// }
    /// </code>
    /// </example>
    public class ActivityScope : IDisposable
    {
        #region Private Fields

        private static readonly object CorrelationLock = new object();

        private readonly TraceSource _ts;
        private readonly Guid _oldActivityId;

        #endregion

        #region Ctors

        /// <summary>
        /// Initializes a new instance of the ActivityScope class.
        /// <para>
        /// Sets new activity id, trace transfer to the new activity and start
        /// new logical operation.
        /// </para>
        /// </summary>
        /// <param name="ts">Trace source</param>
        /// <param name="activityName">Activity name</param>
        public ActivityScope(TraceSource ts, string activityName)
        {
            if (ts == null)
                throw new ArgumentNullException(nameof(ts));

            _ts = ts;
            ActivityName = activityName;

            var newActivityId = Guid.NewGuid();
            ts.TraceTransfer(0, "Transferring to new activity...", newActivityId);

            lock (CorrelationLock)
            {
                _oldActivityId = Trace.CorrelationManager.ActivityId;
                Trace.CorrelationManager.ActivityId = newActivityId;
                Trace.CorrelationManager.StartLogicalOperation(activityName);
            }

            ts.TraceEvent(TraceEventType.Start, 0, activityName);
        }

        /// <summary>
        /// Initializes a new instance of the ActivityScope class.
        /// </summary>
        protected ActivityScope()
        { 
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets null activity scope that does nothing (it does not use Correlation Manager at all).
        /// </summary>
        /// <remarks>We need this to optimize situations when activity logging is turned off.</remarks>
        public static ActivityScope NullScope { get; } = new NullActivityScope();

        /// <summary>
        /// Gets name of the current activity.
        /// </summary>
        public string ActivityName { get; }

        #endregion

        #region Dispose Pattern

        /// <summary>
        /// Dispose method implementation.
        /// Stops current logical operation, transfer to the previous activity and
        /// sets previous activity id.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Correct implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Whether object is currently being disposed or not.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ts.TraceEvent(TraceEventType.Stop, 0, ActivityName);
                _ts.TraceTransfer(0, "Transferring back to the previous activity...", _oldActivityId);

                lock (CorrelationLock)
                {
                    Trace.CorrelationManager.StopLogicalOperation();
                    Trace.CorrelationManager.ActivityId = _oldActivityId;
                }
            }
        }

        #endregion

        #region Helper Classes
        
        private class NullActivityScope : ActivityScope
        {
            protected override void Dispose(bool disposing)
            {
                // Nothing to do here
            }
        }

        #endregion
    }
}