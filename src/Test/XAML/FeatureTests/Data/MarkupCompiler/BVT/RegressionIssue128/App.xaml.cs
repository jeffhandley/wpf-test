// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.Test.Logging;
using System.Windows.Threading;

namespace RegressionIssue128
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {      
        void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Window1 window1 = new Window1();
                window1.Show();
                Application.Current.Shutdown(0);
            }
            catch (Exception exception)
            {
                GlobalLog.LogEvidence("Exception from Window1..ctor: " + exception.ToString());
                Microsoft.Test.Logging.TestLog log = TestLog.Current;
                log.Result = TestResult.Fail;
                Application.Current.Shutdown(-1);
            }
        }

    }
}
