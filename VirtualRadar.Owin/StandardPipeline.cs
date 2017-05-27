﻿// Copyright © 2017 onwards, Andrew Whewell
// All rights reserved.
//
// Redistribution and use of this software in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of the author nor the names of the program's contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHORS OF THE SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceFactory;
using Owin;
using VirtualRadar.Interface.Owin;
using VirtualRadar.Owin.Middleware;

namespace VirtualRadar.Owin
{
    using System.Web.Http;
    using AppFunc = Func<IDictionary<string, object>, Task>;

    /// <summary>
    /// Registers the standard pipeline with an OWIN web app.
    /// </summary>
    static class StandardPipeline
    {
        /// <summary>
        /// Registers all of the standard pipeline middleware.
        /// </summary>
        /// <param name="webAppConfiguration"></param>
        public static void Register(IWebAppConfiguration webAppConfiguration)
        {
            webAppConfiguration.AddCallback(UseAccessFilter,                StandardPipelinePriority.Access);
            webAppConfiguration.AddCallback(UseBasicAuthenticationFilter,   StandardPipelinePriority.Authentication);
            webAppConfiguration.AddCallback(UseRedirectionFilter,           StandardPipelinePriority.Redirection);

            webAppConfiguration.AddCallback(ConfigureHttpConfiguration,     StandardPipelinePriority.WebApiConfiguration);
            webAppConfiguration.AddCallback(UseWebApi,                      StandardPipelinePriority.WebApi);

            webAppConfiguration.AddCallback(UseFileSystemServer,            StandardPipelinePriority.FileSystemServer);
        }

        private static void UseAccessFilter(IAppBuilder app)
        {
            var filter = Factory.Singleton.Resolve<IAccessFilter>();
            var middleware = new Func<AppFunc, AppFunc>(filter.FilterRequest);
            app.Use(middleware);
        }

        private static void UseBasicAuthenticationFilter(IAppBuilder app)
        {
            var filter = Factory.Singleton.Resolve<IBasicAuthenticationFilter>();
            var middleware = new Func<AppFunc, AppFunc>(filter.FilterRequest);
            app.Use(middleware);
        }

        private static void UseRedirectionFilter(IAppBuilder app)
        {
            var filter = Factory.Singleton.Resolve<IRedirectionFilter>();
            var middleware = new Func<AppFunc, AppFunc>(filter.FilterRequest);
            app.Use(middleware);
        }

        private static void ConfigureHttpConfiguration(IAppBuilder app)
        {
            var configuration = Factory.Singleton.Resolve<IWebAppConfiguration>().Singleton.GetHttpConfiguration();
            configuration.MapHttpAttributeRoutes();
            configuration.Routes.MapHttpRoute(
                name:           "DefaultApi",
                routeTemplate:  "api/{controller}/{id}",
                defaults:       new { id = RouteParameter.Optional }
            );
        }

        private static void UseWebApi(IAppBuilder app)
        {
            var configuration = Factory.Singleton.Resolve<IWebAppConfiguration>().Singleton.GetHttpConfiguration();
            app.UseWebApi(configuration);
        }

        private static void UseFileSystemServer(IAppBuilder app)
        {
            var server = Factory.Singleton.Resolve<IFileSystemServer>();
            var middleware = new Func<AppFunc, AppFunc>(server.HandleRequest);
            app.Use(middleware);
        }
    }
}
