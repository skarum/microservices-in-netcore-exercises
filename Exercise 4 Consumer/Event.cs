using System.Collections.Generic;

namespace Consumer
{
    public class Event
    {
        public string EventName { get; set; }
        public int UserId { get; set; }
        public Product Products { get; set; }
    }
}