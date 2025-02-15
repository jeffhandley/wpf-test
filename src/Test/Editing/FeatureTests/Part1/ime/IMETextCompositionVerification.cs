// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Windows.Controls;
using System.Windows.Documents;

using Microsoft.Test;
using Microsoft.Test.Discovery;

using Test.Uis.Loggers;
using Test.Uis.TestTypes;
using Test.Uis.Utils;

namespace Test.Uis.TextEditing
{
    /// <summary>
    /// Test verifies expected text compostion for a typing sequence.
    /// Reads test case data from a xml file (seperate xml file contents are processed depending on East Asian language being tested) which contatins typing sequence input and expected composition text.
    /// Test Action : Send keyboard input string and compare with the expected text compostion string in both RichTextBox and TextBox
    /// </summary>
    [Test(0, "IME", "IMETextCompositionVerification_Korean", MethodParameters = "/TestCaseType:IMETextCompositionVerification /locale=Korean", SupportFiles = @"FeatureTests\Editing\KoreanImeTestCaseData.xml", Timeout = 120, Keywords = "KoreanIME")]
    [Test(0, "IME", "IMETextCompositionVerification_Japanese", MethodParameters = "/TestCaseType:IMETextCompositionVerification /locale=Japanese", SupportFiles = @"FeatureTests\Editing\JapaneseImeTestCaseData.xml", Timeout = 120, Keywords = "JapaneseIME")]
    [Test(0, "IME", "IMETextCompositionVerification_ChinesePinyin", MethodParameters = "/TestCaseType:IMETextCompositionVerification /locale=ChinesePinyin", SupportFiles = @"FeatureTests\Editing\ChineseSimplifiedImeTestCaseData.xml", Timeout = 120, Keywords = "ChinesePinyinIME")]
    [Test(0, "IME", "IMETextCompositionVerification_ChineseNewPhonetic", MethodParameters = "/TestCaseType:IMETextCompositionVerification /locale=ChineseNewPhonetic", SupportFiles = @"FeatureTests\Editing\ChineseTraditionalImeTestCaseData.xml", Timeout = 120, Keywords = "ChineseNewPhoneticIME")]
    public class IMETextCompositionVerification : ManagedCombinatorialTestCase
    {
        #region Main flow
        /// <summary>Starts the combination</summary>
        protected override void DoRunCombination()
        {
            if (_rtb == null)
            {
                _rtb = new RichTextBox();
                _rtb.Height = 200;
                _rtb.FontSize = 24;
            }
            if (_tb == null)
            {
                _tb = new TextBox();
                _tb.Height = 100;
                _tb.FontSize = 24;
            }
            if (_testTextBox == null)
            {
                _testTextBox = new TextBox();
                _testTextBox.Height = 100;
                _testTextBox.FontSize = 24;
            }
            if (_panel == null)
            {
                _panel = new StackPanel();
                _panel.Children.Add(_rtb);
                _panel.Children.Add(_tb);
                _panel.Children.Add(_testTextBox);
                MainWindow.Content = _panel;
            }

            InitializeTestVariables();
            QueueDelegate(PerformTestSetup);
        }

        private void PerformTestSetup()
        {
            _rtb.Document.Blocks.Clear();
            _testTextBox.Text = string.Empty;
            _tb.Text = string.Empty;

            TextRange tr = new TextRange(_rtb.Document.ContentStart, _rtb.Document.ContentEnd);
            XamlUtils.SetXamlContent(tr, _xamlContent[xamlContentIndex]);

            QueueDelegate(PerformTestActions);
        }

        private void PerformTestActions()
        {
            Log("Load IME keyboard");
            IMEHelper.SetUpIMEKeyboardLayout(_locale, _testTextBox, MainWindow);

            Microsoft.Test.Threading.DispatcherHelper.DoEvents(IMEHelper.CiceroWaitTimeMs);
            // Read test cases from the xml file
            _testcases = IMEHelper.LoadTestCaseDataFromXml(_inputFileName);

            // Perform the following action for each test case
            for (int i = 0; i < _testcases.Count; i++)
            {
                _testData = (string[])_testcases[i];

                // send input to richtextbox
                _rtb.Document.Blocks.Clear();
                _rtb.Focus();
                Microsoft.Test.Threading.DispatcherHelper.DoEvents();

                KeyboardInput.TypeString(_testData[0]);
                Microsoft.Test.Threading.DispatcherHelper.DoEvents(IMEHelper.CiceroWaitTimeMs);

                // send input to textbox
                _tb.Text = string.Empty;
                _tb.Focus();
                Microsoft.Test.Threading.DispatcherHelper.DoEvents();

                KeyboardInput.TypeString(_testData[0]);
                Microsoft.Test.Threading.DispatcherHelper.DoEvents(IMEHelper.CiceroWaitTimeMs);

                // verify contents of textbox and richtextbox
                VerifyContentAfterTyping(_testData[1]);
            }

            NextCombination();
        }

        private void VerifyContentAfterTyping(string expectedString)
        {
            Paragraph p = (Paragraph)_rtb.Document.Blocks.FirstBlock;
            TextRange tr = new TextRange(p.ContentStart, p.ContentEnd);

            Verifier.Verify(tr.Text == expectedString, "Verifying contents of richtextbox after typing: Actual[" +
                tr.Text + "] Expected[" + expectedString + "]", false);

            Verifier.Verify(_tb.Text == expectedString, "Verifying contents of textbox after typing: Actual[" +
                _tb.Text + "] Expected[" + expectedString + "]", false);
        }

        private void InitializeTestVariables()
        {            
            switch (_locale)
            {
                case IMELocales.Korean:
                    _inputFileName = "KoreanImeTestCaseData.xml";
                    break;
                case IMELocales.Japanese:
                    _inputFileName = "JapaneseImeTestCaseData.xml";
                    break;
                case IMELocales.ChinesePinyin:
                    _inputFileName = "ChineseSimplifiedImeTestCaseData.xml";
                    break;
                case IMELocales.ChineseNewPhonetic:
                    _inputFileName = "ChineseTraditionalImeTestCaseData.xml";
                    break;
            }
        }
        #endregion Main flow

        #region Private fields
        // Combinatorial engine variables; set to default values
        private IMELocales _locale = IMELocales.Korean;

        private StackPanel _panel;
        private RichTextBox _rtb;
        private TextBox _tb;
        private TextBox _testTextBox; // Used just to set the appropriate Ime mode

        private const int xamlContentIndex = 0;
        private string[] _xamlContent = new string[] { "<Paragraph><Run></Run></Paragraph>" };

        private string _contentToTypeInIME = string.Empty;
        private string _composedStringByIME = string.Empty;

        private string _inputFileName = "KoreanImeTestCaseData.xml";
        private string[] _testData = new string[2];
        private ArrayList _testcases;

        #endregion Private fields
    }
}