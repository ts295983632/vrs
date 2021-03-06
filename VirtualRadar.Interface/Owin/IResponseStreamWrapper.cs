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

namespace VirtualRadar.Interface.Owin
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    /// <summary>
    /// Wraps the response stream that other middleware write to. Gives us a bottleneck where we can
    /// modify the stream before sending it back to the browser.
    /// </summary>
    /// <remarks>
    /// This middleware needs to be used fairly early on, before any middleware that generates content
    /// appears. The wrapper doesn't do anything much, it just wraps the stream with a memory stream.
    /// However, it will make callbacks to <see cref="IStreamManipulator"/> objects that have been
    /// registered with the <see cref="IWebAppConfiguration"/>. It is those objects that will be
    /// compressing streams, minifying JavaScript, logging outcomes etc. etc.
    /// </remarks>
    public interface IResponseStreamWrapper
    {
        /// <summary>
        /// Initialises the wrapper with the manipulators that will be called for every request, in the
        /// order in which they should be called.
        /// </summary>
        /// <param name="streamManipulators"></param>
        void Initialise(IEnumerable<IStreamManipulator> streamManipulators);

        /// <summary>
        /// Wraps the request stream with a memory stream.
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        AppFunc WrapResponseStream(AppFunc next);
    }
}
