using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Data.Fitness;
using GearChart.Resources;
using GearChart.Data.FilteredStatisticsPlugin;

namespace GearChart.UI.GearFilterCriteria
{
    partial class SelectGearsEquipmentDialog : Form
    {
        public SelectGearsEquipmentDialog(object previousCriteria)
        {
            InitializeComponent();

            String previousEquipmentId = String.Empty;

            if (previousCriteria is TemplateGearFilterCriteria)
            {
                TemplateGearFilterCriteria concreteCriteria = previousCriteria as TemplateGearFilterCriteria;

                previousEquipmentId = concreteCriteria.EquipmentId;
            }

            AddEquipmentToCombobox(previousEquipmentId);

            this.Text = Strings.SelectGearsEquipmentDialogText;
            SelectEquipmentLabel.Text = Strings.SelectEquipmentLabelText;
            OKButton.Text = CommonResources.Text.ActionOk;
            Cancel_Button.Text = CommonResources.Text.ActionCancel;

            OKButton.Enabled = EquipmentComboBox.Items.Count > 0 && EquipmentComboBox.SelectedIndex >= 0;
        }

        private void AddEquipmentToCombobox(string selectedId)
        {
            EquipmentComboBox.Items.Clear();
            int selectedIndex = 0;

            m_SetupEquipmentIds = Common.Data.GetEquipmentIds();

            foreach (string currentId in m_SetupEquipmentIds)
            {
                foreach (IEquipmentItem currentEquipment in PluginMain.GetApplication().Logbook.Equipment)
                {
                    if (currentEquipment.ReferenceId == currentId)
                    {
                        EquipmentComboBox.Items.Add(currentEquipment.Name);

                        if (selectedId == currentId)
                        {
                            selectedIndex = EquipmentComboBox.Items.Count - 1;
                        }

                        break;
                    }
                }
            }

            if (EquipmentComboBox.Items.Count > 0)
            {
                EquipmentComboBox.SelectedIndex = selectedIndex;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void EquipmentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OKButton.Enabled = EquipmentComboBox.Items.Count > 0 && EquipmentComboBox.SelectedIndex >= 0;
        }

        public string SelectedEquipmentId
        {
            get
            {
                if (m_SetupEquipmentIds.Count > EquipmentComboBox.SelectedIndex)
                {
                    return m_SetupEquipmentIds[EquipmentComboBox.SelectedIndex];
                }

                return String.Empty;
            }
        }

        private IList<String> m_SetupEquipmentIds = null;
    }
}