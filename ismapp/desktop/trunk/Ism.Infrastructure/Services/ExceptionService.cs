using Ism.Infrastructure.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Services
{
    public class ExceptionService : IExceptionService
    {
        private readonly IEventAggregator _eventAggregator;
        public ExceptionService(IEventAggregator evetnAggregator)
        {
            _eventAggregator = evetnAggregator;
        }
        public void RaiseException(Exception exception)
        {
            _eventAggregator.GetEvent<RaiseException>().Publish(exception);
        }
    }
}
