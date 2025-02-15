// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Avalon.Test.CoreUI.Trusted;
using Avalon.Test.CoreUI;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;

using Avalon.Test.CoreUI.Common;

using Avalon.Test.CoreUI.CoreInput.Common;
using Avalon.Test.CoreUI.CoreInput.Common.Controls;
using Microsoft.Test.Win32;
using Microsoft.Test.Discovery;
using Microsoft.Test;
using Microsoft.Test.Logging;

namespace Avalon.Test.CoreUI.CoreInput
{
    /// <summary>
    /// Verify UIElement InputHitTest works for UIElement in window.
    /// </summary>
    /// <description>
    /// This is part of a collection of unit tests for core input.
    /// </description>
    /// <author>Microsoft</author>
 
    [CoreTestsLoader(CoreTestsTestType.MethodBase)]
    public class UIElementInputHitTestApp: TestApp
    {
        /// <summary>
        /// Launch our test.
        /// </summary>
        [TestCaseContainerAttribute("All", "All", "0", @"CoreInput\InputManager", TestCaseSecurityLevel.FullTrust, "Verify UIElement InputHitTest works for UIElement in window.")]
        public void LaunchTest()
        {
            Run();
        }


        /// <summary>
        /// Setup the test.
        /// </summary>
        /// <param name="sender">App sending the callback.</param>
        /// <returns>Null object.</returns>
        public override object DoSetup(object sender)
        {
            CoreLogger.LogStatus("Constructing window....");

            // Construct test element, add event handling
            InstrPanel p = new InstrPanel();

            DisplayMe(p, 10, 10, 100, 100);

            return null;
        }

        /// <summary>
        /// Execute stuff.
        /// </summary>
        /// <param name="arg">Not used.</param>
        /// <returns>Null object.</returns>
        protected override object DoExecute(object arg)
        {
            CoreLogger.LogStatus("Getting point on element....");
            MouseHelper.Move(_rootElement);
            Point p = Mouse.GetPosition(_rootElement);
            CoreLogger.LogStatus("Found point: "+p);

            CoreLogger.LogStatus("Getting input hit element....");
            // This should return our element.
            _hitEl = _rootElement.InputHitTest(p);
            // This should not return any element.
            _invalidHitEl = _rootElement.InputHitTest(new Point(p.X+300, p.Y+300));

            base.DoExecute(arg);
            return null;
        }

        /// <summary>
        /// Validate the test.
        /// </summary>
        /// <param name="sender">App sending the callback.</param>
        /// <returns>Null object.</returns>
        public override object DoValidate(object sender)
        {
            CoreLogger.LogStatus("Validating...");

            // For this test we need the API to return the element we expected.
            CoreLogger.LogStatus("Invalid,valid hit elements? " + (_invalidHitEl != null) + "," + (_hitEl != null));
            if (_hitEl != null)
            {
                CoreLogger.LogStatus(" Hit element: " + _hitEl.GetType().ToString());
            }

            bool actual = (_invalidHitEl == null) && (_hitEl != null) && (_hitEl.GetType() == typeof(InstrPanel));
            bool expected = true;
            bool eventFound = (actual == expected);

            CoreLogger.LogStatus("Setting log result to " + eventFound);
            this.TestPassed = eventFound;

            CoreLogger.LogStatus("Validation complete!");

            return null;
        }

        /// <summary>
        /// Stores results of InputHitTest.
        /// </summary>
        private IInputElement _hitEl = null;
        private IInputElement _invalidHitEl = null;
    }
}
