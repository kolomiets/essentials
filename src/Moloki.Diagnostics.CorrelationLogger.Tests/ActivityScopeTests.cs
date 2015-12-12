using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Moloki.Diagnostics.CorrelationLogger.Tests
{
    [TestFixture]
    class ActivityScopeTests
    {
        [Test]
        public void CreateActivityScopeWithEmptySourceShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => new ActivityScope(null, "name"));
        }

        [Test]
        public void CreateNamedActivityScope()
        {
            var source = new TraceSource("source name");
            using (var scope = new ActivityScope(source, "name"))
            {
                Assert.That(scope.ActivityName, Is.EqualTo("name"));
            }                
        }
    }
}
