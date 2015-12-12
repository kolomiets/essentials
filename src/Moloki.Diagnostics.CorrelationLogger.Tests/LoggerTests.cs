using System.Reflection;
using NUnit.Framework;

namespace Moloki.Diagnostics.CorrelationLogger.Tests
{
    [TestFixture]
    class LoggerTests
    {
        [Test]
        public void GetAssemblyLoggerWithDefaultParameters()
        {
            var logger = Logger.GetAssemblyLogger();
            Assert.That(logger.Source.Name, Is.EqualTo(GetCurrentAssemblyName()));
        }

        [Test]
        public void GetAssemblyLoggerWithTypeParameter()
        {
            var logger = Logger.GetAssemblyLogger<ActivityScope>();
            Assert.That(logger.Source.Name, Is.EqualTo(typeof(ActivityScope).Assembly.GetName().Name));
        }

        protected static string GetCurrentAssemblyName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;
        }
    }
}