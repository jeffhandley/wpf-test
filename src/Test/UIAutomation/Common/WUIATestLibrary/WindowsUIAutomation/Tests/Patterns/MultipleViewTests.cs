// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/******************************************************************* 
* Purpose: InternalHelper
* Owner: Microsoft
* Contributors:
*******************************************************************/
using System;
using System.CodeDom;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Automation;
using System.Windows;

namespace Microsoft.Test.WindowsUIAutomation.Tests.Patterns
{
    using InternalHelper;
    using InternalHelper.Tests;
    using InternalHelper.Enumerations;
    using Microsoft.Test.WindowsUIAutomation;
    using Microsoft.Test.WindowsUIAutomation.Core;
    using Microsoft.Test.WindowsUIAutomation.TestManager;
    using Microsoft.Test.WindowsUIAutomation.Interfaces;

    /// -----------------------------------------------------------------------
    /// <summary></summary>
    /// -----------------------------------------------------------------------
	public sealed class MultipleViewTests : PatternObject
    {
        #region Member variables

        /// <summary>
        /// Specific interface associated with this pattern
        /// </summary>
        MultipleViewPattern _pattern = null;


        #endregion Member variables
        const string THIS = "MultipleViewTests";

		/// -------------------------------------------------------------------
		/// <summary></summary>
		/// -------------------------------------------------------------------
		public const string TestSuite = NAMESPACE + "." + THIS;

		/// -------------------------------------------------------------------
		/// <summary></summary>
		/// -------------------------------------------------------------------
		public static readonly string TestWhichPattern = Automation.PatternName(MultipleViewPattern.Pattern);

		/// -------------------------------------------------------------------
		/// <summary></summary>
		/// -------------------------------------------------------------------
		public MultipleViewTests(AutomationElement element, TestPriorities priority, string dirResults, bool testEvents, TypeOfControl typeOfControl, IApplicationCommands commands)
            :
            base(element, TestSuite, priority, typeOfControl, TypeOfPattern.MultipleView, dirResults, testEvents, commands)
        {
            _pattern = (MultipleViewPattern)element.GetCurrentPattern(MultipleViewPattern.Pattern);
            if (_pattern == null)
                throw new Exception(Helpers.PatternNotSupported);
        }


        #region Tests


        #endregion Tests

        #region Step/Verification
        #endregion Step/Verification
    }
}
