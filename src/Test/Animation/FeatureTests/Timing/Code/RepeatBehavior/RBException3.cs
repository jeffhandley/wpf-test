// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Avalon Timing Verification Test Automation 
//  

/* 
 * Description: Verify reading RepeatBehavior when RepeatBehavior is not set
 */

//Instructions:
//  1. Create a Timeline, setting RepeatBehavior with a TimeSpan.
//  2. Read RepeatBehavior

//Pass Condition:
//  An appropriate exception should be thrown.

//Pass Verification:
//  The output of this test should match the expected output in RBException3Expect.txt.

//Warnings:
//  Any changes made in the output should be reflected RBException3Expect.txt file

//Dependencies:
//  TestRuntime.dll, Timing\Common\GlobalClasses.cs

using System;
using System.Windows;
using System.Windows.Media.Animation;
using Microsoft.Test.Logging;

namespace Microsoft.Test.Animation
{
    class RBException3 :ITimeBVT
    {
        /*
         *  Function:    Test
         *  Arguments:   Framework
         *  Description: Constructs a Timeline and Checks whether events are properly caught.
         *              Logs the results.
         */
        public override string Test()
        {
            //Intialize output String
            outString = "";

            // Create a TimeManager
            TimeManagerInternal tMan = EstablishTimeManager(this);
            DEBUG.ASSERT(tMan != null, "Cannot create TimeManager" , " Created TimeManager ");

            // Create a TimeNode
            ParallelTimeline timeline = new ParallelTimeline();
            DEBUG.ASSERT(timeline != null, "Cannot create Timeline" , " Created Timeline " );
            //Intialize output String
            outString = "";

            try
            {
                TimeSpan val = timeline.RepeatBehavior.Duration;
                outString += val;
            }
            catch (Exception e)
            {
                TimeGenericWrappers.CHECKEXCEPTION( typeof(System.InvalidOperationException), e.GetType(), ref outString );
            }

            return outString;
        }
    }
}
