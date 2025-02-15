// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Markup;

[assembly: XmlnsDefinition("http://a", "Microsoft.Test.Xaml.Schema")]

namespace Microsoft.Test.Xaml.Schema
{
    /// <summary>
    /// No XmlnsCompatibleWith attribute in the assembly
    /// </summary>
    [SchemaTest]
    class NoCompat : TryGetCompatibleXamlNamespaceTest
    {
        public override void Run()
        {
            CallAndVerify("http://a", true, "http://a");
            CallAndVerify("http://unknown", false, null);
        }
    }
}
