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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls.DateTimePicker
{
    public enum DateMode
    {
        Full,
        Abbreviated,
        Numbers
    }

    public class DatePicker : DateTimePickerBase
    {
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(DateMode), typeof(DatePicker), new PropertyMetadata(DateMode.Full));
        public DateMode Mode
        {
            get { return (DateMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        private ComboBox _primarySelector;
        private ComboBox _secondarySelector;
        private ComboBox _tertiarySelector;

        public DatePicker()
        {
            DefaultStyleKey = typeof(DatePicker);
            Loaded += OnDatePickerLoaded;
        }

        void OnDatePickerLoaded(object sender, RoutedEventArgs e)
        {
            var days = new List<string>();
            var months = new List<string>();
            var years = new List<string>();

            var info = new DateTimeFormatInfo();

            switch (Mode)
            {
                case DateMode.Full:
                    months = info.MonthNames.ToList();
                    break;
                case DateMode.Abbreviated:
                    months = info.AbbreviatedMonthNames.ToList();
                    break;
                case DateMode.Numbers:
                    for (int i = 1; i <= 12; i++)
                        months.Add(i.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'));
                    break;
            }

            if (months.Count == 13)
                months.RemoveAt(12);

            _primarySelector.ItemsSource = days;
            _secondarySelector.ItemsSource = months;
            _tertiarySelector.ItemsSource = years;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _primarySelector = GetTemplateChild("PrimarySelector") as ComboBox;
            _secondarySelector = GetTemplateChild("SecondarySelector") as ComboBox;
            _tertiarySelector = GetTemplateChild("TertiarySelector") as ComboBox;

            _primarySelector.SelectedIndex = Value.HasValue ? Value.Value.Day - 1 : DateTime.Now.Day - 1;
            _secondarySelector.SelectedIndex = Value.HasValue ? Value.Value.Month - 1 : DateTime.Now.Month - 1;

            _secondarySelector.SelectionChanged += SecondarySelectorSelectionChanged;
        }

        void SecondarySelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = _secondarySelector.SelectedItem.ToString();
            int month = -1;

            switch (Mode)
            {
                case DateMode.Full:
                    for (var i = 0; i < CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Count(); i++)
                    {
                        if (selected != CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[i])
                            continue;

                        month = i + 1;
                        break;
                    }
                    break;

                case DateMode.Abbreviated:
                    for (var i = 0; i < CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames.Count(); i++)
                    {
                        if (selected != CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[i])
                            continue;

                        month = i + 1;
                        break;
                    }
                    break;
                case DateMode.Numbers:
                    month = Int32.Parse(selected);
                    break;
            }

            var days = new List<string>();
            var dom = DateTime.DaysInMonth(DateTime.Now.Year, month);
            for (var i = 0; i < dom; i++)
            {
                days.Add((i + 1).ToString().PadLeft(2, '0'));
            }

            _primarySelector.ItemsSource = days;
        }
    }
}
