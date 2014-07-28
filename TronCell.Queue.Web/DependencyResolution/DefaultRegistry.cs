// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
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

using Microsoft.Owin.Security.DataHandler.Serializer;
using Queue.Entities.Models;
using Queue.Repository.Services;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Ef6.Factories;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using TronCell.Queue.Web;
using TronCell.Queue.Web.Models;

namespace TronCell.Queue.Web.DependencyResolution {
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
	
    public class DefaultRegistry : Registry {
        #region Constructors and Destructors

        public DefaultRegistry() {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
            var db = new RetailDataContext("DefaultConnection");
            //For<IDataContextAsync>().Singleton().Use(db);
            For<RetailDataContext>().Singleton().Use(db);
            For<IRepositoryProvider>().Use<RepositoryProvider>().Ctor<RepositoryFactories>("repositoryFactories").Is(new RepositoryFactories());
            //For<IExample>().Use<Example>();
            For<Microsoft.AspNet.Identity.IUserStore<ApplicationUser>>().Use<Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>>();
            For<System.Data.Entity.DbContext>().Use(() => new ApplicationDbContext());
            For<IUnitOfWorkAsync>().Use<UnitOfWork>();

            For<IRepositoryAsync<Fitting>>().Use<Repository<Fitting>>();
            For<IFittingService>().Use<FittingService>();
        }

        #endregion
    }
}