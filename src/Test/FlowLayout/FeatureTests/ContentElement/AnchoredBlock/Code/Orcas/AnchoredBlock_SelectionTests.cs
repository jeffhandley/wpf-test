// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Description : Test Selection in Floaters and Figures..  
//               
//               This loads a xaml with Floaters and Figures that have selectable content (Paragraph)
//               Tests are then run to verify that selection works as expected within and through the AnchoredBlock.
//                                                
// Verification: Basic API validation.  
// Created by:  Microsoft
////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Markup;

using Microsoft.Test.TestTypes;
using Microsoft.Test.Discovery;
using Microsoft.Test.Layout;
using Microsoft.Test.Input;
using Microsoft.Test.Logging;

namespace Microsoft.Test.FlowLayout
{
    /// <summary>  
    /// Test selection in anchored block - Indic.   
    /// </summary>
    [Test(2, "AnchoredBlock", "AnchoredBlockSelectionIndic", MethodName = "Run")]
    public class AnchoredBlocksSelectionTest_Indic : AvalonTest
    {
        private FlowDocumentPageViewer _singlePageViewer;
        private FlowDocument _document;
        private Window _w;
        private string _inputXaml;
        private int _inputID;
        private string _expectedSelectionText;
        private TextSelection _ts;
        private TextRange _tr;

        [Variation("AnchoredBlock_IndicFontSelectionTestContent.xaml", 1, Priority = 2)]
        [Variation("AnchoredBlock_IndicFontSelectionTestContent.xaml", 18, Priority = 2)]
        [Variation("AnchoredBlock_IndicFontSelectionTestContent.xaml", 20, Priority = 2)]
        public AnchoredBlocksSelectionTest_Indic(string xamlFile, int testID)
            : base()
        {
            _inputXaml = xamlFile;
            _inputID = testID;
            InitializeSteps += new TestStep(Initialize);
            CleanUpSteps += new TestStep(CleanUp);
            RunSteps += new TestStep(RunTests);
            RunSteps += new TestStep(VerifyTest);
        }

        /// <summary>
        /// Initialize: Initialize the content
        /// </summary>
        /// <returns>TestResult</returns>
        private TestResult Initialize()
        {
            Status("Initialize");
            _w = new Window();
            _singlePageViewer = new FlowDocumentPageViewer();
            _document = (FlowDocument)XamlReader.Load(File.OpenRead(_inputXaml));
            _singlePageViewer.Document = _document;

            ((DynamicDocumentPaginator)(_singlePageViewer.Document.DocumentPaginator)).PaginationCompleted += new EventHandler(paginator_PaginationCompleted);
            _w.Content = _singlePageViewer;
            if (_w.Content is FrameworkElement)
            {
                // Setting Window.Content size to ensure same size of root element over all themes.  
                // Different themes have different sized window chrome which will cause property dump 
                // and vscan failures even though the rest of the content is the same.
                // 784x564 is the content size of a 800x600 window in Aero theme.
                ((FrameworkElement)_w.Content).Height = 564;
                ((FrameworkElement)_w.Content).Width = 784;
            }
            _w.Left = 0;
            _w.Top = 0;
            _w.Topmost = true;
            _w.Resources.MergedDictionaries.Add(GenericStyles.LoadAllStyles("FlowLayoutOrcasTest.g"));
            _w.SizeToContent = SizeToContent.WidthAndHeight;
            _w.Show();

            WaitForPriority(DispatcherPriority.ApplicationIdle);

            return TestResult.Pass;
        }

        private TestResult CleanUp()
        {
            _w.Close();
            return TestResult.Pass;
        }

        private void paginator_PaginationCompleted(object sender, EventArgs e)
        {
            if (_singlePageViewer.Document.DocumentPaginator.IsPageCountValid)
            {
                _singlePageViewer.NextPage(); //Get to the second page                
            }
            else
            {
                LogComment("**PageCount was invalid after Pagination completed.");
            }
        }

        /// <summary>
        /// RunTests: Runs a set of tests based on the input variation.
        /// </summary>
        /// <returns>TestResult</returns>
        private TestResult RunTests()
        {
            _singlePageViewer.Focus();

            _ts = _singlePageViewer.Selection;
            _tr = (TextRange)_ts;

            Figure fig = LogicalTreeHelper.FindLogicalNode(_document, "testFigure") as Figure;
            Floater flt = LogicalTreeHelper.FindLogicalNode(_document, "testFloater") as Floater;

            switch (_inputID)
            {
                case 1:
                    Status("Running: Programatically select text content of a Floater test...");
                    _expectedSelectionText = "ঁ ং ঃ অ আ ই ঈ উ ঊ ঋ ঌ এ ঐ ও ঔ ক খ গ ঘ ঙ চ ছ জ ঝ ঞ ট";
                    ProgSelectAnchoredBlock(flt);
                    break;

                case 18:
                    Status("Running: Drag Mouse to select indic text content of a Figure test...");
                    _expectedSelectionText = "ಆ ಇ ಈ ಉ ಊ ";
                    _singlePageViewer.NextPage();
                    DragMouseToSelectAnchoredBlock(600, 220, 620, 260);
                    break;

                case 20:
                    Status("Running: Use Keyboard to select indic text content of a Figure test...");
                    _expectedSelectionText = "ಔ ಕ";
                    _singlePageViewer.NextPage();
                    UseKeyBoardToSelectAnchoredBlock(660, 300, 4, "LEFT");
                    break;

                default:
                    Status("Error !!! SettingTestCases: Unexpected failure to match the argument. ");
                    return TestResult.Fail;
            }
            return TestResult.Pass;
        }

        /// <summary>
        /// VerifyTest: Verifies the test.
        /// </summary>
        /// <returns>TestResult</returns>
        private TestResult VerifyTest()
        {
            WaitFor(1000);
            if (String.Compare(_tr.Text.Trim(), _expectedSelectionText.Trim(), StringComparison.InvariantCulture) != 0)
            {
                LogComment("Selected text is wrong!");
                LogComment("Expected: " + _expectedSelectionText);
                LogComment("Instead got: " + _tr.Text.Trim());
                TestLog.Current.Result = TestResult.Fail;
            }
            else
            {
                LogComment("Selected text was as expected.");
                TestLog.Current.Result = TestResult.Pass;
            }
            return TestLog.Current.Result;
        }

        private void ProgSelectAnchoredBlock(AnchoredBlock block)
        {
            TextPointer tp1 = block.Blocks.FirstBlock.ContentStart;
            TextPointer tp2 = block.Blocks.FirstBlock.ContentEnd;
            _ts.Select(tp1, tp2);
            Status("Selected text: " + _tr.Text);
        }

        private void DragMouseToSelectAnchoredBlock(int startX, int startY, int endX, int endY)
        {
            WaitForPriority(DispatcherPriority.ApplicationIdle);
            Status("Moving mouse to the start of the Selection...");
            UserInput.MouseButton(_singlePageViewer, startX, startY, "Move");

            WaitForPriority(DispatcherPriority.ApplicationIdle);
            Status("MouseLeftButtonDown...");
            Microsoft.Test.Input.Input.SendMouseInput(0, 0, 0, SendMouseInputFlags.LeftDown);

            WaitForPriority(DispatcherPriority.ApplicationIdle);
            Status("Move Mouse within the Anchored Block...");
            UserInput.MouseButton(_singlePageViewer, endX, endY, "Move");

            WaitForPriority(DispatcherPriority.ApplicationIdle);
            Status("MouseLeftButtonUp...");
            Microsoft.Test.Input.Input.SendMouseInput(0, 0, 0, SendMouseInputFlags.LeftUp);

            WaitForPriority(DispatcherPriority.ApplicationIdle);
            Status("Selected text: " + _tr.Text);
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(ushort bVk, ushort bScan, uint dwFlags, IntPtr dwExtraInfo);

        private void UseKeyBoardToSelectAnchoredBlock(int startX, int startY, int count, string direction)
        {
            if (Keyboard.IsKeyToggled(Key.NumLock))
            {
                Status("Disabling NumLock...");
                const ushort VK_NUMLOCK = 0x90;
                const ushort BSCAN = 0x45;
                const uint KEYEVENTF_EXTENDEDKEY = 0x1;
                const uint KEYEVENTF_KEYUP = 0x2;

                keybd_event(VK_NUMLOCK, BSCAN, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
                keybd_event(VK_NUMLOCK, BSCAN, KEYEVENTF_KEYUP, IntPtr.Zero);
            }

            WaitForPriority(DispatcherPriority.ApplicationIdle);
            Status("Moving mouse to the start of the Selection...");
            UserInput.MouseButton(_singlePageViewer, startX, startY, "Move");

            WaitForPriority(DispatcherPriority.ApplicationIdle);
            Status("MouseLeftButtonDown...");
            Microsoft.Test.Input.Input.SendMouseInput(0, 0, 0, SendMouseInputFlags.LeftDown);

            WaitForPriority(DispatcherPriority.ApplicationIdle);
            Status("MouseLeftButtonUp...");
            Microsoft.Test.Input.Input.SendMouseInput(0, 0, 0, SendMouseInputFlags.LeftUp);

            WaitForPriority(DispatcherPriority.ApplicationIdle);
            Status("Pressing shift key...");
            InputW.PressShift();

            WaitForPriority(DispatcherPriority.ApplicationIdle);
            Status("Pressing the " + direction + " arrow key " + count.ToString() + " times...");
            for (int i = 0; i < count; i++)
            {
                Status("Pressing " + direction + "...");
                InputW.KeyboardType("+{" + direction + "}");
            }

            WaitForPriority(DispatcherPriority.ApplicationIdle);
            Status("Releasing shift key...");
            InputW.ReleaseShift();
        }
    }
} 
