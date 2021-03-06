﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared.Models;
using Lidgren.Network;

namespace BlastersShared.Services
{
    /// <summary>
    /// A generic service container that provides collections and a base service 
    /// </summary>
    public class ServiceContainer
    {

        // Used to implement the GetService function
        private Dictionary<Type, Service> _serviceLookupTable;


        /// <summary>
        /// A lookup table of users which can be shared by the services. 
        /// </summary>
        public Dictionary<NetConnection, User> Users { get; set; }


        public ServiceContainer()
        {
            _serviceLookupTable = new Dictionary<Type, Service>();
            Users = new Dictionary<NetConnection, User>();
        }

        /// <summary>
        /// Gets a specific type of service from the service container.
        /// This is useful for letting services talk to each other if required.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public Service GetService(Type serviceType)
        {
            
            if (_serviceLookupTable.ContainsKey(serviceType))
            {
                return _serviceLookupTable[serviceType];
            }

            else
            {
                throw new Exception("This type of service is not present. Is it a subclass of Service?");
            }

        }

        /// <summary>
        /// Registers a service into the container.
        /// </summary>
        /// <param name="service"></param>
        public void RegisterService(Service service)
        {
            service.ServiceContainer = this;
            _serviceLookupTable.Add(service.GetType(), service);
        }

        /// <summary>
        /// Performs updates on all the services
        /// </summary>
        public void PerformUpdates()
        {
            foreach (var service in _serviceLookupTable.Values)
            {
                service.PeformUpdate();
            }
        }




    }
}
