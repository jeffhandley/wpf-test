// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

using Avalon.Test.CoreUI;
using Avalon.Test.CoreUI.Events;
using Avalon.Test.CoreUI.Trusted;

using Microsoft.Test;
using Microsoft.Test.Discovery;
using Microsoft.Test.Logging;
using Microsoft.Test.TestTypes;


namespace Avalon.Test.CoreUI.Events
{
    /// <summary>
    /// Tests Attaching Tunnel EventHandlers to CustomControl, CustomControl, CustomControl
    /// <para />
    /// This is a BVT scenario for attaching tunnel event to a simple tree with control1 visual parent of control2 model parent of control3. 
    /// star=visual
    /// line=model
    /// CC1 ** CC2 -- CC3
    /// </summary>
    /// <remarks>
    /// <para />
    /// Area: Events\Tunnel
    /// <para />
    /// <para />
    /// <para />
    /// FileName:  TunnelingEventRoutingFrameworkFig41.cs
    /// <para />
    /// <ol>Scenarios covered:
    /// <li>Fetch RoutedEvent for tunnel event</li>
    /// <li>Add event handlers for tunnel event</li>
    /// <li>Raise the tunnel event</li>
    /// <li>Handlers are called in the correct order</li>
    /// </ol>
    /// </remarks>
    /// <seealso cref="TestCaseType"/>
    [Test(0, "Events.Tunnel", "TunnelingEventRoutingFrameworkFig41")]
    public class TunnelingEventRoutingFrameworkFig41 : EventHelper
    {
       #region Constructor
        public TunnelingEventRoutingFrameworkFig41()
        {
            RunSteps += new TestStep(StartTest);
        }
        #endregion


        #region Test Steps
        /// <summary>
        /// Entry Method for the test case
        /// </summary>
        TestResult StartTest()
        {
            CoreLogger.LogStatus("This is a BVT scenario for attaching tunnel event to a simple tree with control1 visual parent of control3, and control1 model parent of control2 model parent of control3");
            CoreLogger.LogStatus("Tests attach tunneling event to CustomControl1, CustomControl2, CustomControl3");

            // Local Varaibles
            // Create three CustomControl to build a tree later and test Events
            CustomControl control1 = null;
            CustomControl control2 = null;
            CustomControl control3 = null;
            
            // Create a routedEvent to get EventManager.GetRoutedEventFromName
            RoutedEvent routedEvent;
            
            // Create size of three object array to contain three CustomControls
            RouteTarget[] targets = new RouteTarget[3];

            // Create a CustomRoutedEventArgs for later RaiseEvent use
            CustomRoutedEventArgs args;

            // Create Dispatcher        
            Dispatcher context = MainDispatcher;

            // Enter Dispatcher
            
            
            using(CoreLogger.AutoStatus("Creating custom controls"))
            {
                control1 = new CustomControl();
                control2 = new CustomControl();
                control3 = new CustomControl();
            }
                
            using(CoreLogger.AutoStatus("Construct tree"))
            {
                control1.AppendModelChild(control2);
                control2.AppendChild(control3);
            }
            
            using(CoreLogger.AutoStatus("Fetch RoutedEvent for tunnel event"))
            {
                routedEvent = TunnelingEventRoutingFrameworkFig41.PreviewRoutedEvent1;
            }
            
            using(CoreLogger.AutoStatus("Add event handlers for tunnel event"))
            {
                control1.AddHandler(routedEvent, new CustomRoutedEventHandler(OnRoutedEvent1));
                control2.AddHandler(routedEvent, new CustomRoutedEventHandler(OnRoutedEvent2));
                control3.AddHandler(routedEvent, new CustomRoutedEventHandler(OnRoutedEvent3));
            }
        
            using(CoreLogger.AutoStatus("Raise the tunnel event on control3"))
            {
                targets[0].Source = control2;
                targets[0].Sender = control1;
                targets[1].Source = control2;
                targets[1].Sender = control2;
                targets[2].Source = control3;
                targets[2].Sender = control3;
                args = new CustomRoutedEventArgs(routedEvent, targets);
                control3.RaiseEvent(args);
            }

            using(CoreLogger.AutoStatus("Validation for event"))
            {
                if (args.HandlersCalledCount != 3)
                {
                    throw new Microsoft.Test.TestValidationException("Incorrect HandlersCalledCount");
                }
            }

            //Any test failures will be caught by throwing an Exception during verification.
            return TestResult.Pass;
            
            // Exit Dispatcher
        }    
        #endregion


        #region Public Members
        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent1(object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus("OnRoutedEvent1");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent(sender, args, 0);
        }

        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent2(object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus("OnRoutedEvent2");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent(sender, args, 1);
        }

        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent3(object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus("OnRoutedEvent3");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent(sender, args, 2);
        }
        #endregion
    }
}


