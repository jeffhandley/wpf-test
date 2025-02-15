// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

namespace RegressionIssue44
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// When auto freeze is enabled, verify that there is no exception when loading XAML
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}
