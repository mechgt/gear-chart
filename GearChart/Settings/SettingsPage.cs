// <copyright file="SettingsPage.cs" company="N/A">
// Copyright (c) 2008 All Right Reserved
// </copyright>
// <author>mechgt</author>
// <email>mechgt@gmail.com</email>
// <date>2009-09-15</date>
namespace GearChart.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using ZoneFiveSoftware.Common.Data.Fitness;
    using ZoneFiveSoftware.Common.Visuals;

    /// <summary>
    /// Plugin Settings Page
    /// </summary>
    class SettingsPage : ISettingsPage
    {
        #region Private Members

        private SettingsPageControl control;
        private bool pageLoaded;

        #endregion

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region ISettingsPage Members

        /// <summary>
        /// Gets the Settings Page GUID
        /// </summary>
        public Guid Id
        {
            get { return GUIDs.SettingsPage; }
        }

        /// <summary>
        /// Gets the Settings Page SubPages
        /// </summary>
        public IList<ISettingsPage> SubPages
        {
            get { return null; }
        }

        #endregion

        #region IDialogPage Members

        /// <summary>
        /// Gets the name as shown in list of plugins
        /// </summary>
        public string PageName
        {
            get { return Resources.Strings.GearSelection; }
        }

        /// <summary>
        /// Gets the page status.  Currently unused.
        /// </summary>
        public IPageStatus Status
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the title at top of banner
        /// </summary>
        public string Title
        {
            get { return "Settings"; }
        }

        /// <summary>
        /// Create SettingsPage control
        /// </summary>
        /// <returns>SettingsPage control</returns>
        public System.Windows.Forms.Control CreatePageControl()
        {
            this.control = new SettingsPageControl();
            return this.control;
        }

        /// <summary>
        /// Called when entering view.
        /// </summary>
        /// <param name="bookmark">Currently unused</param>
        public void ShowPage(string bookmark)
        {
            // Populate page with the logbook data
            Options.LoadSettings();

            // Initialize some items only if this is not already the active view
            if (!pageLoaded)
            {
                PluginMain.GetApplication().PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SportTracksApplication_PropertyChanged);
            }

            this.control.RefreshPage();
            pageLoaded = true;
        }

        /// <summary>
        /// Called when exiting view.
        /// </summary>
        /// <returns></returns>
        public bool HidePage()
        {
            // Save any changes on settings exit
            PluginMain.GetApplication().PropertyChanged -= SportTracksApplication_PropertyChanged;
            pageLoaded = false;

            // Update plugin component enabled status
            for (int i = 0; i < ExtendActions.RouteExportActions.Count; i++)
            {
                //ExtendActions.ActivityDetailPages[i].RefreshPage();
            }

            // Update plugin component enabled status
            for (int i = 0; i < ExtendActions.RouteExportActions.Count; i++)
            {
                ExtendActions.ActivityEditActions[i].Refresh();
            }
            
            // Update plugin component enabled status
            for (int i = 0; i < ExtendActions.RouteExportActions.Count; i++)
            {
                ExtendActions.ActivityExportActions[i].Refresh();
            }
            
            // Update plugin component enabled status
            for (int i = 0; i < ExtendActions.RouteExportActions.Count; i++)
            {
                ExtendActions.RouteEditActions[i].Refresh();
            }

            // Update plugin component enabled status
            for (int i = 0; i < ExtendActions.RouteExportActions.Count; i++)
            {
                ExtendActions.RouteExportActions[i].Refresh();
            }

            return true;
        }

        /// <summary>
        /// Sets theme visual colors
        /// </summary>
        /// <param name="visualTheme">Contains visual definitions</param>
        public void ThemeChanged(ITheme visualTheme)
        {
            if (this.control != null)
            {
                this.control.ThemeChanged(visualTheme);
            }
        }

        /// <summary>
        /// Defines language options.  Update display to reflect selected language
        /// </summary>
        /// <param name="culture">Selected culture</param>
        public void UICultureChanged(System.Globalization.CultureInfo culture)
        {
            if (this.control != null)
            {
                this.control.UICultureChanged(culture);
            }
        }

        #endregion
        
        private void SportTracksApplication_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Logbook")
            {
                // Logbook was changed.  Reload data from new logbook.
                Options.Loaded = false;
                this.control.RefreshPage();
            }
        }
    }
}
