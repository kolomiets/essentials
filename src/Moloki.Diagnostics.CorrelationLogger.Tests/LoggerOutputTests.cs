using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace Moloki.Diagnostics.CorrelationLogger.Tests
{
    [TestFixture]
    class LoggerOutputTests: LoggerTests
    {
        private static readonly SourceLevels[] LogLevels = {
            SourceLevels.Off,
            SourceLevels.Information,
            SourceLevels.Warning,
            SourceLevels.Error,
            SourceLevels.Critical,
            SourceLevels.ActivityTracing, 
            SourceLevels.All
        };

        [Test]
        public void MockLoggerOutput()
        {
            var writer = new StringWriter();
            var listener = new TextWriterTraceListener(writer);

            var logger = GetConfiguredLogger(listener, SourceLevels.Off);
            logger.LogAssemblyInformation();
            Assert.That(writer.ToString(), Is.Empty);

            logger = GetConfiguredLogger(listener, SourceLevels.Information);
            logger.LogAssemblyInformation();
            Assert.That(writer.ToString(), Is.Not.Empty);

            var assemblyInfo = writer.ToString();
        }

        private static Logger GetConfiguredLogger(TextWriterTraceListener listener, SourceLevels level)
        {
            var logger = Logger.GetAssemblyLogger();
            logger.Source.Listeners.Clear();
            logger.Source.Listeners.Add(listener);
            logger.Source.Switch.Level = level;
            logger.LogAssemblyInformation();
            return logger;
        }
    }
}
