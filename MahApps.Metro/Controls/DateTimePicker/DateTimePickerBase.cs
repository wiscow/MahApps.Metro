﻿//
// Copyright (c) 2012 Tim Heuer
//
// Licensed under the Microsoft Public License (Ms-PL) (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://opensource.org/licenses/Ms-PL.html
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

// BASE CODE PROVIDED UNDER THIS LICENSE
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls.DateTimePicker
{
    public abstract class DateTimePickerBase : Control
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(DateTime?), typeof(DateTimePickerBase), new PropertyMetadata(null, OnValueChanged));
        public static readonly DependencyProperty ValueStringProperty = DependencyProperty.Register("ValueString", typeof(string), typeof(DateTimePickerBase), null);
        public static readonly DependencyProperty ValueStringFormatProperty = DependencyProperty.Register("ValueStringFormat", typeof(string), typeof(DateTimePickerBase), new PropertyMetadata(null, OnValueStringFormatChanged));

        public event EventHandler<DateTimeValueChangedEventArgs> ValueChanged;

        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((DateTimePickerBase)o).OnValueChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue);
        }

        private void OnValueChanged(DateTime? oldValue, DateTime? newValue)
        {
            UpdateValueString();
            OnValueChanged(new DateTimeValueChangedEventArgs(oldValue, newValue));
        }

        protected virtual void OnValueChanged(DateTimeValueChangedEventArgs e)
        {
            var handler = ValueChanged;
            if (null != handler)
            {
                handler(this, e);
            }
        }

        public string ValueString
        {
            get { return (string)GetValue(ValueStringProperty); }
            set { SetValue(ValueStringProperty, value); }
        }

        
        public string ValueStringFormat
        {
            get { return (string)GetValue(ValueStringFormatProperty); }
            set { SetValue(ValueStringFormatProperty, value); }
        }

        protected virtual string ValueStringFormatFallback { get { return "{0}"; } }

        private static void OnValueStringFormatChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((DateTimePickerBase)o).OnValueStringFormatChanged();
        }

        private void OnValueStringFormatChanged()
        {
            UpdateValueString();
        }

        private void UpdateValueString()
        {
            ValueString = string.Format(PreferredCulture, ValueStringFormat ?? ValueStringFormatFallback, Value);
        }

        protected DateTimePickerBase()
        {
            PreferredCulture = new CultureInfo(DateTimeWrapper.CurrentLanguageTag);
        }

        public CultureInfo PreferredCulture;
    }
}
