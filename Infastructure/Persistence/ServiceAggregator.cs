using Application.IEventBus;
using Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Persistence
{
    public class ServiceAggregator : IServiceAggregator
    {
        public IClassService ClassService { get; }

        public ISystemAccountService SystemAccountService { get; }

        public IMessageConsumer MessageConsumer { get; }
        public IMessagePublisher MessagePublisher { get; }
        public ServiceAggregator(IClassService classService, ISystemAccountService systemAccountService, IMessagePublisher messagePublisher, IMessageConsumer messageConsumer)
        {
            ClassService = classService;
            SystemAccountService = systemAccountService;
            MessagePublisher = messagePublisher;
            MessageConsumer = messageConsumer;
        }

    }

}
