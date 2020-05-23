using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using GearChart.UI.DetailPage;

namespace GearChart
{
    class ExtendActions : IExtendActivityDetailPages
    {
        #region Controls

        public static IList<IAction> RouteExportActions = new List<IAction>();
        public static IList<IAction> RouteEditActions = new List<IAction>();
        public static IList<IAction> ActivityExportActions = new List<IAction>();
        public static IList<IAction> ActivityEditActions = new List<IAction>();

        #endregion

        #region IExtendActivityDetailPages Members

        public IList<IDetailPage> GetDetailPages(IDailyActivityView view, ExtendViewDetailPages.Location location)
        {
            return new List<IDetailPage> { new GearChartPage(view) };
        }

        #endregion
    }
}
