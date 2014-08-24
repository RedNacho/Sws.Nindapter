using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sws.Nindapter
{
    internal class NindapterServiceContainer<TService, TAdapted>
    {

        private readonly TService _service;

        public NindapterServiceContainer(TService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            _service = service;
        }

        public TService Service
        {
            get { return _service; }
        }

    }
}
