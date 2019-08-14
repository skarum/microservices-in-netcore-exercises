using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Polly;

namespace Consumer
{
    public class EventSubscriber
    {
        private readonly string loyaltyProgramHost;
        private long start = 0,
         chunkSize = 2;
        private readonly Timer timer;
        private static Policy exponentionalRetryPolicy =
        Policy.Handle<Exception>()
            .WaitAndRetryAsync(10, attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)),
            (ex, ts) =>
            {
                Console.WriteLine($"Got: {ex.Message} Retrying in {ts.TotalMilliseconds} ms");
            });

        public EventSubscriber(string loyaltyProgramHost)
        {
            Console.WriteLine("Setting up subscriber");
            this.loyaltyProgramHost = loyaltyProgramHost;
            this.timer = new Timer(10 * 1000);
            this.timer.AutoReset = false;
            this.timer.Elapsed += (_, __) => SubscriptionCycleCallback().Wait();
            this.timer.Start();
        }

        private async Task SubscriptionCycleCallback()
        {
            Console.WriteLine($"Looking for messages from {start}");

            HttpResponseMessage response = null;

            await exponentionalRetryPolicy.ExecuteAsync(async () =>
            {
                response = await ReadEvents();
            });
            Console.WriteLine($"Statuscode {response.StatusCode}");
            if (response.StatusCode == HttpStatusCode.OK)
                HandleEvents(await response.Content.ReadAsStringAsync());
            this.timer.Start();
        }
        private async Task<HttpResponseMessage> ReadEvents()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"http://{loyaltyProgramHost}");
                httpClient.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.GetAsync($"api/cart/events/?firstEventSequenceNumber={this.start}&lastEventSequenceNumber={this.start + this.chunkSize}");
                return response;
            }
        }

        private void HandleEvents(string content)
        {
            Console.WriteLine($"Raw message content: {content}");
            var events = JsonConvert.DeserializeObject<IEnumerable<EventSequence>>(content);

            if (!events.Any())
            {
                Console.WriteLine("No events found");
                return;
            }

            foreach (var ev in events)
            {
                Console.WriteLine($"Processsing event {ev.Event.EventName}");
                
            }

            start = events.ToList().Select(x => x.SuquenceNumber).Max() + 1;
        }
    }
}