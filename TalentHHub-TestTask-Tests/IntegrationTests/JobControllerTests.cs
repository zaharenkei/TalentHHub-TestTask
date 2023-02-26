using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using TalentHHub_TestTask;
using TalentHHub_TestTask.Handlers.Models;

namespace TalentHHub_TestTask_Tests.IntegrationTests
{
    public class JobControllerTests
    {
        private TestServer server;

        [SetUp]
        public void Setup()
        {
            var builder = new WebHostBuilder();
            builder.ConfigureAppConfiguration(cb => cb.AddJsonFile("appsettings.json").Build());
            builder.UseStartup<Startup>();
            server = new TestServer(builder);
        }

        internal static IEnumerable<(CalculationRequest, CalculationResponse)> TestCaseSource()
        {
            var request1 = new CalculationRequest(true, new[]
            {
                new RequestPrintItem("envelopes", 520m),
                new RequestPrintItem("letterhead", 1983.37m, true)
            });
            var response1 = new CalculationResponse("$2,940.30", new[]
            {
                new ResponsePrintItem("envelopes", "$556.40"),
                new ResponsePrintItem("letterhead", "$1,983.37")
            });

            var request2 = new CalculationRequest(false, new[]
            {
                new RequestPrintItem("t-shirts", 294.04m),
            });
            var response2 = new CalculationResponse("$346.96", new[]
            {
                new ResponsePrintItem("t-shirts", "$314.62")
            });
            
            var request3 = new CalculationRequest(true, new[]
            {
                new RequestPrintItem("frisbees", 19385.38m, true),
                new RequestPrintItem("yo-yos", 1829m, true),
            });
            var response3 = new CalculationResponse("$24,608.68", new[]
            {
                new ResponsePrintItem("frisbees", "$19,385.38"),
                new ResponsePrintItem("yo-yos", "$1,829.00")
            });

            return new[]
            {
                (request1, response1),
                (request2, response2),
                (request3, response3)
            };
        }

        [Test]
        [TestCaseSource(nameof(TestCaseSource))]
        public async Task Test1((CalculationRequest Request, CalculationResponse Response) testCase)
        {
            // Arrange
            var client = server.CreateClient();
            var content = JsonContent.Create(testCase.Request, typeof(CalculationRequest));

            // Act
            var rawResponse = await client.PostAsync("job/totalCharge", content);
            var response = await rawResponse.Content.ReadFromJsonAsync<CalculationResponse>();

            // Assert
            Assert.NotNull(response);
            Assert.That(response.Total, Is.EqualTo(testCase.Response.Total));
            Assert.That(response.Items.Count(), Is.EqualTo(testCase.Response.Items.Count()));
            var expectedArray = testCase.Response.Items.ToArray();
            var responseArray = response.Items.ToArray();

            for (var i = 0; i < testCase.Response.Items.Count(); i++)
            {
                Assert.That(responseArray[i].Name, Is.EqualTo(expectedArray[i].Name));
                Assert.That(responseArray[i].Cost, Is.EqualTo(expectedArray[i].Cost));
            }
        }
    }
}
