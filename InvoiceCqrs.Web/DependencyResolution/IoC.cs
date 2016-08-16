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


using InvoiceCqrs.Registries;
using StructureMap;

namespace InvoiceCqrs.Web.DependencyResolution {
    using StructureMap;
	
    public static class IoC
    {
        private static IContainer _Container;

        public static IContainer Current => _Container ?? (_Container = Initialize());

        private static IContainer Initialize() {
            return new Container(c =>
            {
                c.AddRegistry<DefaultRegistry>();
                c.AddRegistry<StructureMap.Registries.DatabaseRegistry>();
                c.AddRegistry<EventStoreRegistry>();
                c.AddRegistry<GeneratorRegistry>();
                c.AddRegistry<MediatorRegistry>();
                c.AddRegistry<ValidatorRegistry>();
                c.AddRegistry<VisitorRegistry>();
            });
        }
    }
}