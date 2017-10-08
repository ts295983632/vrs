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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualRadar.Interface;
using VirtualRadar.Interface.Owin;

namespace Test.VirtualRadar.Owin.Configuration
{
    [TestClass]
    public class JavascriptManipulatorConfigurationTests
    {
        class TextResponseManipulator : ITextResponseManipulator
        {
            public IDictionary<string, object> Environment { get; private set; }
            public TextContent TextContent { get; private set; }

            public void ManipulateTextResponse(IDictionary<string, object> environment, TextContent textContent)
            {
                Environment = environment;
                TextContent = textContent;
            }
        }

        public TestContext TestContext { get; set; }
        private TextResponseManipulator _Manipulator;
        private IJavascriptManipulatorConfiguration _Config;

        [TestInitialize]
        public void TestInitialise()
        {
            _Manipulator = new TextResponseManipulator();
            _Config = Factory.Singleton.ResolveNewInstance<IJavascriptManipulatorConfiguration>();
        }

        [TestMethod]
        public void JavascriptManipulatorConfiguration_AddTextResponseManipulator_Adds_Manipulator()
        {
            _Config.AddTextResponseManipulator(_Manipulator);

            var manipulators = _Config.GetTextResponseManipulators().ToArray();
            Assert.AreEqual(1, manipulators.Length);
            Assert.AreSame(_Manipulator, manipulators[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void JavascriptManipulatorConfiguration_AddTextResponseManipulator_Throws_If_Passed_Null()
        {
            _Config.AddTextResponseManipulator(null);
        }

        [TestMethod]
        public void JavascriptManipulatorConfiguration_AddTextResponseManipulator_Ignores_Double_Add()
        {
            _Config.AddTextResponseManipulator(_Manipulator);
            _Config.AddTextResponseManipulator(_Manipulator);

            var manipulators = _Config.GetTextResponseManipulators().ToArray();
            Assert.AreEqual(1, manipulators.Length);
            Assert.AreSame(_Manipulator, manipulators[0]);
        }

        [TestMethod]
        public void JavascriptManipulatorConfiguration_RemoveTextResponseManipulator_Removes_Manipulator()
        {
            _Config.AddTextResponseManipulator(_Manipulator);
            _Config.RemoveTextResponseManipulator(_Manipulator);

            var manipulators = _Config.GetTextResponseManipulators().ToArray();
            Assert.AreEqual(0, manipulators.Length);
        }

        [TestMethod]
        public void JavascriptManipulatorConfiguration_RemoveTextResponseManipulator_Ignores_Null_Removal()
        {
            _Config.RemoveTextResponseManipulator(null);
        }

        [TestMethod]
        public void JavascriptManipulatorConfiguration_RemoveTextResponseManipulator_Ignores_Double_Removal()
        {
            _Config.AddTextResponseManipulator(_Manipulator);
            _Config.RemoveTextResponseManipulator(_Manipulator);
            _Config.RemoveTextResponseManipulator(_Manipulator);

            var manipulators = _Config.GetTextResponseManipulators().ToArray();
            Assert.AreEqual(0, manipulators.Length);
        }
    }
}
