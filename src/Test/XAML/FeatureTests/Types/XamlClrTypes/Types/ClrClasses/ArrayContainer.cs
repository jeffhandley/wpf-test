﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Test.Xaml.Types.ClrClasses
{
    /// <summary>
    /// ArrayContainer class
    /// </summary>
    public class ArrayContainer
    {
        /// <summary>
        /// Gets or sets the ListArray.
        /// </summary>
        /// <value>The text value.</value>
        public List<int>[] ListArray { get; set; }

        /// <summary>
        /// Gets or sets the IntArray.
        /// </summary>
        /// <value>The text value.</value>
        public int[] IntArray { get; set; }
    }
}
