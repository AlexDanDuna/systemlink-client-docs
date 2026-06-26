using System;
using NationalInstruments.SystemLink.Clients.Core;
using NationalInstruments.SystemLink.Clients.Message;
using NationalInstruments.SystemLink.Clients.Tag;

namespace NationalInstruments.SystemLink.Clients.Examples.Configuration
{
    /// <summary>
    /// Basic example console application demonstrating the different ways to
    /// supply a configuration for SystemLink APIs.
    /// </summary>
    class Configuration
    {
        static void Main(string[] args)
        {
            try
            {
                /*
                 * When SystemLink Server is installed locally, or when running
                 * on a system managed by a SystemLink Server, the
                 * HttpConfigurationManager provides access to an automatic
                 * configuration to communicate with the SystemLink Server.
                 */
                var manager = new HttpConfigurationManager();
                var autoConfiguration = manager.GetConfiguration();
                Console.WriteLine("Found automatic configuration for {0}",
                    autoConfiguration.ServerUri);

                /*
                 * Each class that takes in a configuration also has a default
                 * constructor that uses the automatic configuration instead.
                 */
                using (var autoManager = new TagManager())
                {
                }
            }
            catch (ApiException ex)
            {
                Console.WriteLine("Automatic configuration: {0}",
                    ex.Message);
            }

            /*
             * When an automatic configuration isn't available (often during
             * application development) the HttpConfiguration class can
             * reference any SystemLink Server available over HTTP/HTTPS.
             *
             * Ideally, the username and password would be read from the user at
             * run time or from a file rather than checked into source.
             */
            var serverConfiguration1 = new HttpConfiguration(
                new Uri("https://myserver"), "my_user1", "my_password1");
            var serverConfiguration2 = new HttpConfiguration(
                new Uri("https://myserver2"), "my_user2", "my_password2");

            /*
             * Configurations are shared across all SystemLink client APIs.
             */
            var exampleConfiguration = ExampleConfiguration.Obtain(args);

            using (var manager = new TagManager(exampleConfiguration))
            using (var session = MessageSession.Open(exampleConfiguration))
            {
            }

            /*
             * Mixing configurations enables applications to synchronize data
             * across multiple servers.
             */
            using (var serverManager1 = new TagManager(serverConfiguration1))
            using (var serverManager2 = new TagManager(serverConfiguration2))
            {
            }

            // See the tag and message API examples for specific usage.
        }
    }
}
