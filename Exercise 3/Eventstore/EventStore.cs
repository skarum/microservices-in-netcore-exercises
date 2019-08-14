using System.Collections.Generic;
using System.Linq;

namespace Exercise_4.Eventstore
{
    public class EventStore
    {
        private List<EventSequence> events = new List<EventSequence>();
        public void Add(Event @event)
        {
            long newSequenceNumber = !events.Any() ? 0 : events.Select(x => x.SuquenceNumber).Max() + 1;

            events.Add(new EventSequence
            {
                SuquenceNumber = newSequenceNumber,
                Event = @event
            });
        }

        public List<EventSequence> GetEvents(long from = 0, long to = long.MaxValue)
        {
            return events.Where(x => x.SuquenceNumber >= from
            && x.SuquenceNumber < to)
            .ToList();
        }
    }
}