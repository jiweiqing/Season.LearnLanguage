using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaning.EventBus
{
    public class IntegrationEventRabbitMQOptions
    {
        public string HostName { get; set; } = string.Empty;
        public string ExchangeName { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
