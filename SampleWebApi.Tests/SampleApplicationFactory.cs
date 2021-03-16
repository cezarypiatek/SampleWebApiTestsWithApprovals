using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace SampleWebApi.Tests
{
    class SampleApplicationFactory : WebApplicationFactory<Program>, IAsyncDisposable
    {
        private IHost? _host;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(collection =>
            {
                //TODO: Register mocked connectors for external services
            });
        }

        /// <summary>
        ///     This method ensures that tested application is fully initialized
        /// </summary>
        public async Task Install()
        {
            var client = this.CreateClient();
            var startupTimeout = TimeSpan.FromMilliseconds(2000);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            do
            {
                var response = await client.GetAsync("health/ready");
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            } while (stopWatch.Elapsed < startupTimeout);

            throw new InvalidOperationException("Cannot initialize service within the expected timeout");
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            // INFO: Intercept host object to stop Hosted Service during the Dispose
            _host = base.CreateHost(builder);
            return _host;
        }

        public async ValueTask DisposeAsync()
        {
            if (_host != null)
            {
                await _host.StopAsync();
            }
            base.Dispose();
        }
    }
}
