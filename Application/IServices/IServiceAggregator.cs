using Application.IEventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IServiceAggregator
    {
        IClassService ClassService { get; }
        ISystemAccountService SystemAccountService { get; }

         IMessageConsumer MessageConsumer { get; }
         IMessagePublisher MessagePublisher { get; }
    }
}
