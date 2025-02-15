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
    /// Tests Attaching (0, 0) (0, 1) (1,0) (1,1) and (2, 2), eventhanders to 5 UIElements, where the first
    /// number in the () represent number of instance handlers and second number represent number of class 
    /// handler
    /// <para />
    /// This is a BVT scenario for attaching Bubble event to the follow tree
    /// star=visual
    /// line=model
    ///   
    ///  
    ///     cc1 (CustomControl2 : CustomControl) 2 ClassHandlers attached
    ///      *  
    ///      *    
    ///     cc2 (CustomControl1 : CustomControl) one ClassHandler attached
    ///      *    
    ///      *  
    ///     cc3 (CustomControl1 : CustomControl) one ClassHandler attached
    ///      * 
    ///      * 
    ///     cc4 (CustomControl)
    ///      *
    ///      *
    ///     cc5 (CustomControl)
    ///
    /// </summary>
    /// <remarks>
    /// <para />
    /// Area: Events\UIElement
    /// <para />
    /// <para />
    /// <para />
    /// FileName:  BubbleMultipleHandlerOnUIElement.cs
    /// <para />
    /// <ol>Scenarios covered:
    /// <li>Fetch RoutedEvent for Bubble event</li>
    /// <li>Register 1 class handler for CustomControl1 and 2 class handlers for CustomControl2</li>
    /// <li>Create objects, as shown on the graph above, for the configuration</li>
    /// <li>Build the route</li>
    ///
    /// <li>Attach 1 instance handler for cc4</li>
    /// <li>Attach 1 instance handler for cc2</li>
    /// <li>Attach 2 instance handlers for cc1</li>
    /// <li>Raise the Bubble event</li>
    /// <li>Ensure Handlers are called in the correct order: Bubble up, last added last evoked, class handlers are invoked before instance handler</li>
    /// </ol>
    /// </remarks>
    /// <seealso cref="TestCaseType"/>
    [Test(0, "Events.UIElement", "BubbleMultipleHandlerOnUIElement")]
    public class BubbleMultipleHandlerOnUIElement : EventHelper
    {
        #region Constructor
        public BubbleMultipleHandlerOnUIElement()
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
            CoreLogger.LogStatus ("This is a BVT scenario for UIElement with multiple class handler and instance handler");
            CoreLogger.LogStatus ("Tests attach Bubbleing event");

            // Local Varables
            // Create the objects to build a tree later and test Events
            CustomControl cc5 = null;
            CustomControl cc4 = null;
            CustomControl1 cc3 = null;
            CustomControl1 cc2 = null;
            CustomControl2 cc1 = null;

            // Create a routedEvent to get EventManager.GetRoutedEventFromName
            RoutedEvent routedEvent;

            // Create size of three object array to contain three CustomAvalonControl
            RouteTarget[] targets = new RouteTarget[8];

            // Create a CustomRoutedEventArgs for later RaiseEvent use
            CustomRoutedEventArgs args;

            // Create Dispatcher        
            Dispatcher context = MainDispatcher;

            // Enter Dispatcher
            
            using (CoreLogger.AutoStatus ("Creating custom controls"))
            {
                cc5 = new CustomControl();
                cc4 = new CustomControl();
                cc3 = new CustomControl1 ();
                cc2 = new CustomControl1 ();
                cc1 = new CustomControl2 ();
            }

            using (CoreLogger.AutoStatus ("Construct tree"))
            {
                cc1.AppendChild (cc2);
                cc2.AppendChild (cc3);
                cc3.AppendChild (cc4);
                cc4.AppendChild (cc5);
            }

            using (CoreLogger.AutoStatus ("Fetch RoutedEvent for Bubble event"))
            {
                routedEvent = EventHelper.RoutedEvent1;
            }
            using (CoreLogger.AutoStatus ("Register ClassHandlers"))
            {
                EventManager.RegisterClassHandler (typeof(CustomControl1), routedEvent, new CustomRoutedEventHandler (OnRoutedEvent));
                EventManager.RegisterClassHandler (typeof(CustomControl2), routedEvent, new CustomRoutedEventHandler (OnRoutedEvent));
                EventManager.RegisterClassHandler (typeof(CustomControl2), routedEvent, new CustomRoutedEventHandler (OnRoutedEvent));
            }

            using (CoreLogger.AutoStatus ("Add event instance handlers"))
            {
                cc4.AddHandler (routedEvent, new CustomRoutedEventHandler (OnRoutedEvent1));
                cc2.AddHandler (routedEvent, new CustomRoutedEventHandler (OnRoutedEvent4));
                cc1.AddHandler (routedEvent, new CustomRoutedEventHandler (OnRoutedEvent7));
                cc1.AddHandler (routedEvent, new CustomRoutedEventHandler (OnRoutedEvent8));
            }

            using (CoreLogger.AutoStatus ("Raise the Bubble event"))
            {
                targets[0].Source = cc5;
                targets[0].Sender = cc4;
                targets[1].Source = cc5;
                targets[1].Sender = cc3;
                targets[2].Source = cc5;
                targets[2].Sender = cc2;
                targets[3].Source = cc5;
                targets[3].Sender = cc2;
                targets[4].Source = cc5;
                targets[4].Sender = cc1;
                targets[5].Source = cc5;
                targets[5].Sender = cc1;
                targets[6].Source = cc5;
                targets[6].Sender = cc1;
                targets[7].Source = cc5;
                targets[7].Sender = cc1;
                args = new CustomRoutedEventArgs (routedEvent, targets);
                cc5.RaiseEvent (args);
            }

            using (CoreLogger.AutoStatus ("Validation for event"))
            {
                if (8 != args.HandlersCalledCount)
                {
                    throw new Microsoft.Test.TestValidationException ("Incorrect HandlersCalledCount");
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
        public void OnRoutedEvent1 (object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus ("OnRoutedEvent1");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent (sender, args, 0);
        }

        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent2 (object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus ("OnRoutedEvent2");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent (sender, args, 1);
        }

        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent3 (object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus ("OnRoutedEvent3");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent (sender, args, 2);
        }

        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent4 (object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus ("OnRoutedEvent4");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent (sender, args, 3);
        }

        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent5 (object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus ("OnRoutedEvent5");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent (sender, args, 4);
        }

        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent6 (object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus ("OnRoutedEvent6");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent (sender, args, 5);
        }

        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent7 (object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus ("OnRoutedEvent7");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent (sender, args, 6);
        }

        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent8 (object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus ("OnRoutedEvent8");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent (sender, args, 7);
        }

        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent (object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus ("OnRoutedEvent");
            args.HandlersCalledCount++;
        }
        #endregion


        #region Private Members
        private class CustomControl1 : CustomControl
        {
            public CustomControl1 () : base() { }
        }
        private class CustomControl2 : CustomControl
        {
            public CustomControl2 () : base() { }
        }
        #endregion
    }
}
