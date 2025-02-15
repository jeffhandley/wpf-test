// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Avalon.Test.CoreUI.Trusted;
using Avalon.Test.CoreUI;
using System.Windows;
using System.Windows.Controls;
using Avalon.Test.CoreUI.Common;
using System.Windows.Media;
using System.Windows.Documents;
using System.Collections;
using Avalon.Test.CoreUI.Parser;
using Microsoft.Test.RenderingVerification;

using Microsoft.Test.Serialization.CustomElements;

namespace Avalon.Test.CoreUI.Serialization
{
    /// <summary>
    /// Cell resources verification
    /// Need to add more strict verification applied to every theme. 
    /// </summary>
    public class BamlizedThemeTest
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Verify(UIElement uie)
        {
            CoreLogger.LogStatus("Inside LayoutXamlVerifiers.BorderVerify()...");
            
            DockPanel myPanel = uie as DockPanel;

            if (null == myPanel)
            {
                throw new Microsoft.Test.TestValidationException("Should be DockPanel");
            }
            VerifyElement.VerifyBool(null == myPanel, false);

            //CoreLogger.LogStatus("Changing theme.");
            //string originalTheme = DisplayConfiguration.GetTheme();

            Color lunaBlue = Color.FromArgb(0xff, 0x00, 0x00, 0x00);
            SolidColorBrush myFG = null;


            //Button
            Button button1 = (Button)LogicalTreeHelper.FindLogicalNode(uie, "button1");

            VerifyElement.VerifyBool(null == button1, false);
            myFG = ((Control)button1).Foreground as SolidColorBrush;
            //VerifyElement.VerifyBool(null == myFG, false);
            //myColor = myFG.Color;
            //VerifyElement.VerifyColor(myColor, lunaBlue);
            CoreLogger.LogStatus("Button1 OK");

            Button button2 = (Button)LogicalTreeHelper.FindLogicalNode(uie, "button2");

            VerifyElement.VerifyBool(null == button2, false);
            //myFG = ((Control)button2).Foreground as SolidColorBrush;
            //VerifyElement.VerifyBool(null == myFG, false);
            //myColor = myFG.Color;
            //VerifyElement.VerifyColor(myColor, Colors.Black);
            CoreLogger.LogStatus("Button2 OK");
            
            //CheckBox
            CheckBox checkBox1 = (CheckBox)LogicalTreeHelper.FindLogicalNode(uie, "checkbox1");

            VerifyElement.VerifyBool(null == checkBox1, false);
            //myFG = ((Control)checkBox1).Foreground as SolidColorBrush;
            //VerifyElement.VerifyBool(null == myFG, false);
            //myColor = myFG.Color;
            //VerifyElement.VerifyColor(myColor, lunaBlue);
            CoreLogger.LogStatus("checkBox1 OK");

            CheckBox checkBox2 = (CheckBox)LogicalTreeHelper.FindLogicalNode(uie, "checkbox2");

            VerifyElement.VerifyBool(null == checkBox2, false);
            //myFG = ((Control)checkBox2).Foreground as SolidColorBrush;
            //VerifyElement.VerifyBool(null == myFG, false);
            //myColor = myFG.Color;
            //VerifyElement.VerifyColor(myColor, Colors.Black);
            CoreLogger.LogStatus("checkBox1 OK");
        }
    }
}
