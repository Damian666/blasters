using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlastersShared.Services
{
    /// <summary>
    /// A service that is performed by a process
    /// </summary>
    public abstract class Service
    {

        /// <summary>
        /// Performs updating on the service object
        /// </summary>
        public abstract void PeformUpdate();

        /// <summary>
        /// The parent service container
        /// </summary>
        public ServiceContainer ServiceContainer { get; set; }


    }
}
