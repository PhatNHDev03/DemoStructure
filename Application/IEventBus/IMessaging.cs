using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IEventBus
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(string topic, T message);

    }
    public interface IMessageConsumer
    {
        public Task ConsumeAsync(string topic, string groupId, CancellationToken cancellationToken = default);
    }
}