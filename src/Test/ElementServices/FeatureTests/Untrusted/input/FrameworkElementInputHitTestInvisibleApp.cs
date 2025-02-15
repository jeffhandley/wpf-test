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
    /// Verify FrameworkElement InputHitTest works for invisible FrameworkElement in window.
    /// </summary>
    /// <description>
    /// This is part of a collection of unit tests for core input.
    /// </description>
    /// <author>Microsoft</author>
 
    /// <



    [CoreTestsLoader(CoreTestsTestType.MethodBase)]
    public class FrameworkElementInputHitTestInvisibleApp: TestApp
    {
        /// <summary>
        /// Launch our test.
        /// </summary>
        [TestCase("2",@"CoreInput\InputManager","HwndSource", @"Compile and Verify FrameworkElement InputHitTest works for invisible FrameworkElement in HwndSource.")]
        [TestCase("2",@"CoreInput\InputManager","Browser", @"Compile and Verify FrameworkElement InputHitTest works for invisible FrameworkElement in Browser.")]
        [TestCase("2",@"CoreInput\InputManager","Window", @"Compile and Verify FrameworkElement InputHitTest works for invisible FrameworkElement in window.")]        
        public static void LaunchTestCompile() 
        {
            HostType hostType = (HostType)Enum.Parse(typeof(HostType), DriverState.DriverParameters["TestParameters"]);

            GenericCompileHostedCase.RunCase(
                "Avalon.Test.CoreUI.CoreInput", 
                "FrameworkElementInputHitTestInvisibleApp",
                "Run", 
                hostType);
            
        }

        /// <summary>
        /// Launch our test.
        /// </summary>
        [TestCase("1",@"CoreInput\InputManager","HwndSource",  @"Verify FrameworkElement InputHitTest works for invisible FrameworkElement in HwndSource.")]
        [TestCase("1",@"CoreInput\InputManager","Window",  @"Verify FrameworkElement InputHitTest works for invisible FrameworkElement in window.")]        
        public static void LaunchTest() 
        {
            HostType hostType = (HostType)Enum.Parse(typeof(HostType),DriverState.DriverParameters["TestParameters"]);

            ExeStubContainerFramework exe = new ExeStubContainerFramework(hostType);
            exe.Run(new FrameworkElementInputHitTestInvisibleApp(),"Run");
            
        }

        /// <summary>
        /// Setup the test.
        /// </summary>
        /// <param name="sender">App sending the callback.</param>
        /// <returns>Null object.</returns>
        public override object DoSetup(object sender) 
        {
            // Construct test element, add event handling
            _rootElement = new InstrFrameworkPanel();

            // Construct related Win32 window
            DisplayMe(_rootElement, 10, 10, 100, 100);

            return null;
        }

        /// <summary>
        /// Execute stuff.
        /// </summary>
        /// <returns>Null object.</returns>
        private void DoBeforeExecute()
        {
            CoreLogger.LogStatus("Element visibility status? " + (_rootElement.Visibility.ToString()));

            CoreLogger.LogStatus("Setting visibility....");
            _rootElement.Visibility = Visibility.Hidden;
            
            CoreLogger.LogStatus("Getting input hit element....");
            MouseHelper.Move(_rootElement);
            Point p = Mouse.GetPosition(_rootElement);
            CoreLogger.LogStatus("Found point: "+p);

            // This should not return any element.
            _hitEl = _rootElement.InputHitTest(p);

            // This should not return any element.
            _invalidHitEl = _rootElement.InputHitTest(new Point(p.X+100, p.Y+100));
        }

        /// <summary>
        /// Identify test operations to run.
        /// </summary>
        /// <param name="hwnd">Window handle.</param>
        /// <returns>Array of test operations.</returns>
        protected override InputCallback[] GetTestOps(HandleRef hwnd)
        {
            InputCallback[] ops = new InputCallback[] {
                delegate
                {
                    DoBeforeExecute();
                },
                delegate
                {
                    MouseHelper.Move((IntPtr)hwnd);
                }                                    
            };

            return ops;
        }

        /// <summary>
        /// Validate the test.
        /// </summary>
        /// <param name="sender">App sending the callback.</param>
        /// <returns>Null object.</returns>
        public override object DoValidate(object sender) 
        {
            CoreLogger.LogStatus("Validating...");
            CoreLogger.LogStatus("Element visibility status? " + (_rootElement.Visibility.ToString()));

            // For this test we need the API to return the element we expected.
            CoreLogger.LogStatus("Invalid,valid hit elements? " + (_invalidHitEl!=null)+","+(_hitEl!=null));
            if (_hitEl != null)
            {
                CoreLogger.LogStatus(" Hit element: " + _hitEl.GetType().ToString());
            }

            bool actual = (_invalidHitEl == null) && 
                          (_hitEl == null) &&
                          (_rootElement.Visibility == Visibility.Hidden);
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
