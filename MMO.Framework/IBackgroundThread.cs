using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Framework
{
    /// <summary>
    /// Interface for BackgroundThreads
    /// Usage: Any time consuming loop that the server should initialize and run on a separate thread.
    /// </summary>
    public interface IBackgroundThread
    {
        /// <summary>
        /// Ran once before the thread starts up
        /// </summary>
        void Setup();
        /// <summary>
        /// Called ONCE when thread begins. MUST contain it's own loop.
        /// Why: Background threads can be used not only for loops but just time consuming tasks so we don't need to continuously loop for things we finished calculating.
        /// </summary>
        /// <param name="threadContext">Context in which the loop runs, can be any packaged data the loop needs.</param>
        void Run(Object threadContext);
        /// <summary>
        /// Called when the thread needs to stop. SHOULD CLOSE YOUR RUN LOOP! Because the loops for this interface are internal, you must close them internally as well.
        /// </summary>
        void Stop();
    }
}
