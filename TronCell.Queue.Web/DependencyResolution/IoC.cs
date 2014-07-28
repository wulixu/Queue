// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace TronCell.Queue.Web.DependencyResolution {
    using StructureMap;
	
    public static class IoC {
        public static IContainer Initialize() {
            var container = new Container(c => c.AddRegistry<DefaultRegistry>());
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(container));
            return container;
        }
    }

    /// <summary>

    /// Implementation of StructureMapServiceLocator from

    /// http://www.codeplex.com/CommonServiceLocator/Wiki/View.aspx?title=StructureMap%20Adapter&referringTitle=Home.

    /// </summary>

    public class StructureMapServiceLocator : ServiceLocatorImplBase
    {

        private readonly IContainer container;



        [CLSCompliant(false)]

        public StructureMapServiceLocator(IContainer container)
        {

            this.container = container;

        }



        /// <summary>

        /// When implemented by inheriting classes, this method will do the actual work of resolving

        /// the requested service instance.

        /// </summary>

        /// <param name="serviceType">Type of instance requested.</param>

        /// <param name="key">Name of registered service you want. May be null.</param>

        /// <returns>

        /// The requested service instance.

        /// </returns>

        protected override object DoGetInstance(Type serviceType, string key)
        {

            if (string.IsNullOrEmpty(key))
            {

                return container.GetInstance(serviceType);

            }



            return container.GetInstance(serviceType, key);

        }



        /// <summary>

        /// When implemented by inheriting classes, this method will do the actual work of

        /// resolving all the requested service instances.

        /// </summary>

        /// <param name="serviceType">Type of service requested.</param>

        /// <returns>

        /// Sequence of service instance objects.

        /// </returns>

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {

            foreach (var obj in container.GetAllInstances(serviceType))
            {

                yield return obj;

            }

        }

    }
 

}