using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BAT.Core.Config;
using BAT.Core.Transformers;
using NUnit.Framework;

namespace BAT.Core.Test
{
    [TestFixture]
    public class ConfigurationTest
    {
        [Test]
        public void CanFindTransformers()
        {
            var config = new Configuration();

            var transType = typeof(ITransformer);
            var types = Assembly.GetAssembly(transType).GetTypes().Where(x => transType.IsAssignableFrom(x) && !x.IsInterface);

            config.Transformers = types.Select(x => x.Name).ToList();

            var transformers = config.GetTransformers();

            Assert.AreEqual(transformers.Count(), types.Count());
        }

        //todo: repeat with analyzers, summarizers, and filters
    }
}
