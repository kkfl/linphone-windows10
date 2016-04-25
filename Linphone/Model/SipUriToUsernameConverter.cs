﻿/*
SipUriToUsernameConverter.cs
Copyright (C) 2015  Belledonne Communications, Grenoble, France
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.
This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*/

using System;
using System.Globalization;
using System.Windows;
using BelledonneCommunications.Linphone.Native;
using Windows.UI.Xaml.Data;
using System.Diagnostics;

namespace Linphone.Model
{
    /// <summary>
    /// Converter returning the AccentColorBrush if the boolean is true, else returning a title color.
    /// </summary>
    public class SipUriToUsernameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string sipAddress = (string)value;
            Debug.WriteLine(sipAddress);
            //Address addr = LinphoneManager.Instance.Core.InterpretURL(sipAddress);
            //return addr.UserName;
            return sipAddress;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

    }
}