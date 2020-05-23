using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Visuals.Util;

namespace GearChart.UI.DetailPage
{
    class GearChartPage : IDetailPage
    {
        #region Fields

        private static IActivity activity;
        private static GearChartDetail control;
        private static Boolean loaded;
        private static GearChartPage instance;
        private static IDailyActivityView view;
        private static bool maximized;

        #endregion

        #region Properties

        public static GearChartPage Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region Constructor

        internal GearChartPage(IDailyActivityView view)
        {
            GearChartPage.view = view;
            view.SelectionProvider.SelectedItemsChanged += new EventHandler(SelectionProvider_SelectedItemsChanged);
        }

        #endregion

        #region ST3 IDetailPage Members

        /// <summary>
        /// Gets the detail page GUID        
        /// </summary>
        public Guid Id
        {
            get { return GUIDs.GearDetailPage; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the detail page menu is enabled (or greyed out)
        /// </summary>
        public bool MenuEnabled
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the menu structure to access detail page entry
        /// </summary>
        public IList<string> MenuPath
        {
            get { return new List<string> { CommonResources.Text.LabelCadence }; }
        }

        /// <summary>
        /// Gets a value indicating if the menu item is visible or listed
        /// </summary>
        public bool MenuVisible
        {
            get { return true; }
        }

        public bool PageMaximized
        {
            get
            {
                return maximized;
            }
            set
            {
                maximized = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("PageMaximized"));
                }
            }
        }

        #endregion

        #region IDialogPage Members

        public Control CreatePageControl()
        {
            if (control == null)
            {
                instance = this;
                control = new GearChartDetail();
                control.Activity = activity;
                PropertyChanged += new PropertyChangedEventHandler(GearChart_PropertyChanged);
            }
            return control;
        }

        public bool HidePage()
        {
            // View is no longer active
            loaded = false;
            return true;
        }

        public string PageName
        {
            get
            {
                return Resources.Strings.GearSelection;
            }
        }

        public void ShowPage(string bookmark)
        {
            loaded = true;
            control.RefreshPage();
        }

        public IPageStatus Status
        {
            get { return null; }
            set { }
        }

        public void ThemeChanged(ITheme visualTheme)
        {
            CreatePageControl();
            control.ThemeChanged(visualTheme);
        }

        public string Title
        {
            get { return Resources.Strings.GearSelection; }
        }

        public void UICultureChanged(System.Globalization.CultureInfo culture)
        {

        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        void GearChart_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Event Handlers

        void SelectionProvider_SelectedItemsChanged(object sender, EventArgs e)
        {
            if (control != null)
            {
                control.Activity = CollectionUtils.GetSingleItemOfType<IActivity>(view.SelectionProvider.SelectedItems);
            }
        }

            #endregion
        }
}
