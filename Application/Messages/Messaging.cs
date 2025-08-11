using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages
{
    public interface IMessagePublisher
    {
        //Task PublishAsync<T>(string topic, T message);

    }
    public interface IMessageConsummer
    {
        //Task ConsumeAsync(string topic, CancellationToken cancellationToken = default);
    }
}
