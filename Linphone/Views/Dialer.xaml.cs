﻿using Linphone.Controls;
using Linphone.Model;
using Linphone.Resources;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Linphone
{
    /// <summary>
    /// Home page for the application, displays a numpad and links to Settings/History/Contacts pages.
    /// </summary>
    public partial class Dialer : BasePage, AddressBoxFocused
    {
        private LocalizedStrings _appStrings = new LocalizedStrings();

        /// <summary>
        /// Public constructor.
        /// </summary>
        public Dialer()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
            addressBox.FocusListener = this;

            ContactManager contactManager = ContactManager.Instance; //Force creation and init of ContactManager
        }

        /// <summary>
        /// Method called when the page is displayed.
        /// Check if the uri contains a sip address, if yes, it starts a call to this address.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            StatusBar = status;

            // Create LinphoneCore if not created yet, otherwise do nothing
            LinphoneManager.Instance.InitLinphoneCore();

            // Check for the navigation direction to avoid going to incall view when coming back from incall view
            if (NavigationContext.QueryString.ContainsKey("sip") && e.NavigationMode != NavigationMode.Back)
            {
                String sipAddressToCall = NavigationContext.QueryString["sip"];
                addressBox.Text = sipAddressToCall;
            }
        }

        private void call_Click_1(object sender, EventArgs e)
        {
            if (addressBox.Text.Length > 0)
            {
                LinphoneManager.Instance.NewOutgoingCall(addressBox.Text);
            }
            else
            {
                string lastDialedNumber = LinphoneManager.Instance.GetLastCalledNumber();
                addressBox.Text = lastDialedNumber == null ? "" : lastDialedNumber;
            }
        }

        private void Numpad_Click_1(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            String tag = button.Tag as String;
            LinphoneManager.Instance.LinphoneCore.PlayDTMF(Convert.ToChar(tag), 1000);

            addressBox.Text += tag;
        }

        private void zero_Hold_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (addressBox.Text.Length > 0)
                addressBox.Text = addressBox.Text.Substring(0, addressBox.Text.Length - 1); ;
            addressBox.Text += "+";
        }

        private void history_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/History.xaml", UriKind.RelativeOrAbsolute));
        }

        private void contacts_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Contacts.xaml", UriKind.RelativeOrAbsolute));
        }

        private void settings_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Settings.xaml", UriKind.RelativeOrAbsolute));
        }

        private void about_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/About.xaml", UriKind.RelativeOrAbsolute));
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarHistory = new ApplicationBarIconButton(new Uri("/Assets/AppBar/time.png", UriKind.Relative));
            appBarHistory.Text = AppResources.HistoryMenu;
            ApplicationBar.Buttons.Add(appBarHistory);
            appBarHistory.Click += history_Click_1;

            ApplicationBarIconButton appBarContacts = new ApplicationBarIconButton(new Uri("/Assets/AppBar/people.contacts.png", UriKind.Relative));
            appBarContacts.Text = AppResources.ContactsMenu;
            ApplicationBar.Buttons.Add(appBarContacts);
            appBarContacts.Click += contacts_Click_1;

            ApplicationBarIconButton appBarSettings = new ApplicationBarIconButton(new Uri("/Assets/AppBar/feature.settings.png", UriKind.Relative));
            appBarSettings.Text = AppResources.SettingsMenu;
            ApplicationBar.Buttons.Add(appBarSettings);
            appBarSettings.Click += settings_Click_1;

            ApplicationBarMenuItem appBarAbout = new ApplicationBarMenuItem(AppResources.AboutMenu);
            appBarAbout.Click += about_Click_1;
            ApplicationBar.MenuItems.Add(appBarAbout);
        }

        private void Title_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LinphoneManager.Instance.LinphoneCore.RefreshRegisters();
        }

        /// <summary>
        /// Called wheh the addressbox get focused.
        /// </summary>
        public void Focused()
        {
            numpad.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Called when the addressbox lost its focus.
        /// </summary>
        public void UnFocused()
        {
            numpad.Visibility = Visibility.Visible;
        }
    }
}