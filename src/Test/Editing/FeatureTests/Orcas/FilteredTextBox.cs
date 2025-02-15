// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//  Implementaion of FilteredTextBox which accepts only numbers

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Test.Uis.TextEditing
{
    /// <summary>Provides an implementation of a FiltedredTextBox which accepts only numbers.</summary>
    public class FilteredTextBox : TextBox
    {
        static FilteredTextBox()
        {
            EventManager.RegisterClassHandler(typeof(FilteredTextBox), TextCompositionManager.TextInputStartEvent, new TextCompositionEventHandler(OnTextInputStart));
            EventManager.RegisterClassHandler(typeof(FilteredTextBox), TextCompositionManager.TextInputUpdateEvent, new TextCompositionEventHandler(OnTextInputUpdate));
        }

        /// <summary>Default Constructor</summary>
        public FilteredTextBox()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the FilteredTextBox class, with an arugment 
        /// for character filter which has to be applied.
        /// </summary>
        /// <param name="charFilter">Filter to be applied on the TextBox</param>
        public FilteredTextBox(ICollection<char> charFilter)
        {
            _filter = charFilter;
        }

        private static void OnTextInputStart(object sender, TextCompositionEventArgs args)
        {
            FilteredTextBox textbox = (FilteredTextBox)sender;

            FrameworkTextComposition composition = args.TextComposition as FrameworkTextComposition;
            if (composition != null)
            {
                textbox._newTextMin = composition.CompositionOffset;
                textbox._newTextMax = composition.CompositionOffset + composition.CompositionLength;
            }

            // Leave the event unhandled.
        }

        private static void OnTextInputUpdate(object sender, TextCompositionEventArgs args)
        {
            FilteredTextBox textbox = (FilteredTextBox)sender;

            FrameworkTextComposition composition = args.TextComposition as FrameworkTextComposition;
            if (composition != null)
            {
                textbox._newTextMin = Math.Min(textbox._newTextMin, composition.CompositionOffset);

                int adjustedMax = textbox._newTextMax + composition.CompositionText.Length - composition.CompositionLength;
                int compositionEnd = composition.CompositionOffset + composition.CompositionText.Length;
                textbox._newTextMax = Math.Max(adjustedMax, compositionEnd);
            }

            // Leave the event unhandled.
        }

        /// <summary>
        /// Monitors the input arguments and filters the characters not in the Filter
        /// </summary>
        /// <param name="args">TextCompositionEventArgs</param>
        protected override void OnTextInput(TextCompositionEventArgs args)
        {
            base.OnTextInput(args);

            if (_filter != null)
            {
                FrameworkTextComposition composition = args.TextComposition as FrameworkTextComposition;
                if (composition == null)
                {
                    _newTextMin = this.CaretIndex - args.Text.Length;
                    _newTextMax = this.CaretIndex;
                }
                else
                {
                    _newTextMin = Math.Min(_newTextMin, composition.ResultOffset);

                    int adjustedMax = _newTextMax + args.Text.Length - composition.ResultLength;
                    int compositionEnd = composition.ResultOffset + args.Text.Length;
                    _newTextMax = Math.Max(adjustedMax, compositionEnd);
                }

                for (int i = _newTextMin; i < _newTextMax; i++)
                {
                    if (!_filter.Contains(this.Text[i]))
                    {
                        this.Text = this.Text.Substring(0, _newTextMin) + this.Text.Substring(_newTextMax);
                        this.CaretIndex = _newTextMin;
                        if (_isDebugging)
                        {
                            Console.Beep();
                        }
                        break;
                    }
                }
            }

            _newTextMin = -1;
            _newTextMax = -1;
        }

        /// <summary>Collection of characters which TextBox should accept</summary>
        public ICollection<char> Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                char[] copy = new char[value.Count];
                value.CopyTo(copy, 0);

                _filter = new ReadOnlyCollection<char>(copy);
            }
        }

        private ICollection<char> _filter;

        private int _newTextMin = -1;

        private int _newTextMax = -1;

        private bool _isDebugging = false;
    }
}