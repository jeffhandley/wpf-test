// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Avalon Timing Verification Test Automation 
//  

/* 
 * Description: Verify Timeline event firing when the Clock's Seek method is invoked
 *                                        
 */

//Instructions:
//  1. Create a TimeManager
//  2. Create a Timeline with begin=0 and duration=20 and attach to Manager
//  3. Start the TimeManager at timeline = 0
//  4. Seek the Timeline with offset = 3 and origin = TimeSeekOrigin.BeginTime at Time = 7
//  5. Check the CurrentProgress, CurrentState properties after each Tick
//  6. Check whether events are fired (NOTE: all events should fire)

//Pass Verification:
//  The output of this test should match the expected output in SeekExpect.txt

//Warnings:
//  Any changes made in the output should be reflected SeekExpect.txt file

//Dependencies:
//  TestRuntime.dll, Timing\Common\GlobalClasses.cs

using System;
using System.Windows.Media.Animation;
using Microsoft.Test.Logging;


namespace Microsoft.Test.Animation
{
    class Seek :ITimeBVT
    {
        ParallelTimeline    _tNode;
        Clock               _tClock;
        
        /*
         *  Function:   Test
         *  Arguments:  Framework
         */
        public override string Test()
        {
            //Intialize output String
            outString = "";

            TimeManagerInternal tManager = EstablishTimeManager(this);
            DEBUG.ASSERT(tManager != null, "Cannot create TimeManager" , " Created TimeManager ");
            
            // Create a TimeNode
            _tNode = new ParallelTimeline();
            DEBUG.ASSERT(_tNode != null, "Cannot create Timeline" , " Created Timeline " );
            _tNode.BeginTime = TimeSpan.FromMilliseconds(0);
            _tNode.Duration  = new System.Windows.Duration(TimeSpan.FromMilliseconds(20));
            _tNode.Name        = "TimeNode";           
            DEBUG.LOGSTATUS(" Set Timeline Properties ");
 
            //Attach Handlers to the Timeline
            AttachAllHandlers(_tNode);
            DEBUG.LOGSTATUS(" Attached EventHandlers to the Timeline ");
           
            // Create a Clock, connected to the Timeline.
            _tClock = _tNode.CreateClock();     
            DEBUG.ASSERT(_tClock != null, "Cannot create Clock" , " Created Clock " );
            
            //Run the Timer         
            TimeGenericWrappers.TESTSTATE( this, _tClock, tManager, 0, 25, 1, ref outString);
            
            return outString;
        }

        public override void PostTick( int i )
        {
            if ( i == 7 )
            {
                _tClock.Controller.Seek( TimeSpan.FromMilliseconds(3), TimeSeekOrigin.BeginTime );
            }
        }

        public override void OnCurrentStateInvalidated(object subject, EventArgs args)
        {
            outString += "----------CurrentState: " + ((Clock)subject).CurrentState + "\n";
        }

        public override void OnCurrentGlobalSpeedInvalidated(object subject, EventArgs args)
        {    
            outString += "----------CurrentState: " + ((Clock)subject).CurrentState + "\n";
        }

        public override void OnCurrentTimeInvalidated(object subject, EventArgs args)
        {    
            outString += "----------CurrentState: " + ((Clock)subject).CurrentState + "\n";
        }

        public override void OnCompleted(object subject, EventArgs args)
        {    
            outString += "----------CurrentState: " + ((Clock)subject).CurrentState + "\n";
        }
    }
}
