using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Data.Fitness;
using GearChart.UI.GearFilterCriteria;

namespace GearChart.Data.FilteredStatisticsPlugin
{
    class GearFilterCriteriasProvider
    {
        public GearFilterCriteriasProvider()
        {
            Common.Data.BikeSetupChanged += new GearChart.Common.Data.BikeSetupChangedEventHandler(OnBikeSetupChanged);

            RebuildFilterCriterias();
        }

        void OnBikeSetupChanged(object sender, string setupId)
        {
            TriggerFilterCriteriasChanged();
        }

#region IFilterCriteriaProvider Members

        public IList<object> GetFilterCriterias(ILogbook logbook)
        {
            return m_Criterias;
        }

        public event PropertyChangedEventHandler FilterCriteriasChanged;

#endregion

        private void RebuildFilterCriterias()
        {
            Int32 index = 0;

            index = AddGenericCriteria(index, typeof(GearFilterCriteria));
            index = AddGenericCriteria(index, typeof(TemplateGearPlaceholderFilterCriteria));

            if (m_Criterias.Count > index)
            {
                m_Criterias.RemoveRange(index, m_Criterias.Count - index);
            }

            // Register on the template UI callback
            if (!m_TemplateSelectionPlaceholderRegistered)
            {
                (m_Criterias[1] as TemplateGearPlaceholderFilterCriteria).GearCriteriaSelected += new TemplateGearPlaceholderFilterCriteria.GearCriteriaSelectedEventHandler(OnTemplateGearCriteriaSelected);

                m_TemplateSelectionPlaceholderRegistered = true;
            }

            TriggerFilterCriteriasChanged();
        }

        void OnTemplateGearCriteriaSelected(TemplateGearPlaceholderFilterCriteria criteria, object previousCriteria, out object resultCriteria)
        {
            SelectGearsEquipmentDialog dlg = new SelectGearsEquipmentDialog(previousCriteria);

            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                resultCriteria = new TemplateGearFilterCriteria(null, dlg.SelectedEquipmentId);
            }
            else
            {
                resultCriteria = previousCriteria;
            }
        }

        protected void TriggerFilterCriteriasChanged()
        {
            if (FilterCriteriasChanged != null)
            {
                FilterCriteriasChanged(this, new PropertyChangedEventArgs("FilterCriterias"));
            }
        }

        private Int32 AddGenericCriteria(Int32 index, Type criteriaType)
        {
            ConstructorInfo constructor = criteriaType.GetConstructor(System.Type.EmptyTypes);

            Debug.Assert(constructor != null);

            if (m_Criterias.Count == index)
            {
                m_Criterias.Add(constructor.Invoke(null));
            }
            else if (!(m_Criterias[index].GetType() == criteriaType))
            {
                m_Criterias[index] = constructor.Invoke(null);
            }

            return ++index;
        }

        private List<object> m_Criterias = new List<object>();
        private bool m_TemplateSelectionPlaceholderRegistered = false;
    }
}
