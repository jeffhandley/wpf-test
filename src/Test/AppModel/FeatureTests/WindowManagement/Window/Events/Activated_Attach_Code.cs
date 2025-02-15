// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Windows.Test.Client.AppSec.BVT.ELCommon;
using Microsoft.Test.Win32;

namespace WindowTest
{
    /// <summary>
    /// 
    /// Test for Attaching and firing Activated Event in Code
    ///
    /// </summary>
    public partial class Activated_Attach_Code
    {                                       
        int _expectedHitCount = 1;
        
        void OnActivated(object sender, EventArgs e)
        {
            Logger.RecordHit("OnActivated");
        }
        
        void OnContentRendered(object sender, EventArgs e)
        {            
            Logger.Status("Attaching Window.Activated Event Handler");
            this.Activated += new EventHandler(OnActivated);

            //if we show then close another window, our window is deactivated, then activated.
            Window newWindow = new Window();
            newWindow.Show();
            newWindow.Close();

            Logger.Status("[EXPECTED] HitCount = " + _expectedHitCount.ToString());
            int ActualHitCount = Logger.GetHitCount("OnActivated");
            if (ActualHitCount != _expectedHitCount)
            {
                Logger.LogFail("[ACTUAL] HitCount = " + ActualHitCount.ToString());
            }
            else
            {
                Logger.Status("[ACTUAL] HitCount = " + ActualHitCount.ToString());
            }   

            TestHelper.Current.TestCleanup();
        }
    }
}
