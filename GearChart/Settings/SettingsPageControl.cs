// <copyright file="SettingsPageControl.cs" company="N/A">
// Copyright (c) 2008 All Right Reserved
// </copyright>
// <author>mechgt</author>
// <email>mechgt@gmail.com</email>
// <date>2009-09-15</date>
namespace GearChart.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using ZoneFiveSoftware.Common.Data.Fitness;
    using ZoneFiveSoftware.Common.Visuals;
    using ZoneFiveSoftware.Common.Visuals.Chart;

    public partial class SettingsPageControl : UserControl
    {
        # region Fields

        IEquipmentItem equip;

        #endregion

        #region Constructors

        public SettingsPageControl()
        {
            InitializeComponent();

            treeBig.Columns.Add(new TreeList.Column());
            treeBig.Columns[0].Text = Resources.Strings.ChainRing;
            treeSmall.Columns.Add(new TreeList.Column());
            treeSmall.Columns[0].Text = Resources.Strings.Cassette;
            btnEquipOpen.BackgroundImage = CommonResources.Images.MoveDown16;
        }

        #endregion

        public void RefreshPage()
        {
            string equipmentId = string.Empty;

            if (equip != null)
            {
                equipmentId = equip.ReferenceId;
            }
            else if (PluginMain.GetApplication().Logbook != null && PluginMain.GetApplication().Logbook.Equipment.Count > 0)
            {
                // TODO: Migrate to ST3 - Settings
                //popup_ItemSelected(this, new TreeListPopup.ItemSelectedEventArgs(PluginMain.GetApplication().Logbook.Equipment[0]));
                selectItem(this, PluginMain.GetApplication().Logbook.Equipment[0]);
                equipmentId = equip.ReferenceId;
            }
            else
            {
                return;
            }

            List<float> bigGears = Options.Instance.GetBigGears(equipmentId);
            List<float> smallGears = Options.Instance.GetSmallGears(equipmentId);
            float wheelCircum = Options.Instance.GetWheelCircumference(equipmentId);

            treeBig.RowData = bigGears;
            treeSmall.RowData = smallGears;
            txtCircum.Text = (wheelCircum * 1000F).ToString();
        }

        #region Control Event Handlers (Store Settings)


        #endregion

        #region Utilities

        public void ThemeChanged(ITheme visualTheme)
        {
            txtBig.ThemeChanged(visualTheme);
            txtSmall.ThemeChanged(visualTheme);
            txtCircum.ThemeChanged(visualTheme);
            txtEquip.ThemeChanged(visualTheme);
            treeBig.ThemeChanged(visualTheme);
            treeSmall.ThemeChanged(visualTheme);
        }

        public void UICultureChanged(CultureInfo culture)
        {
            // UI text displayed
            //lblATL.Text = Resources.Resources.Label_ATL;

            // Tooltips
            //toolTipHelp.SetToolTip(btnCatReset, Resources.Resources.ToolTip_ResetCategory);
        }

        /// <summary>
        /// Ensures that only digits are allowed to be entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void digitValidator(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true; // input is not passed on to the control(TextBox)`
            }
        }

        #endregion

        #region Event Handlers

        private void BigAddButton_Click(object sender, EventArgs e)
        {
            float gear;
            float.TryParse(txtBig.Text, out gear);
            if (gear != 0 && equip != null && !Options.Instance.GetBigGears(equip.ReferenceId).Contains(gear))
            {
                Options.Instance.AddBigGear(equip.ReferenceId, gear);
                txtBig.Text = string.Empty;
                txtBig.Focus();
            }

            RefreshPage();
            Options.Instance.StoreSettings();
        }

        private void SmallAddButton_Click(object sender, EventArgs e)
        {
            float gear;
            float.TryParse(txtSmall.Text, out gear);
            if (gear != 0 && equip != null && !Options.Instance.GetSmallGears(equip.ReferenceId).Contains(gear))
            {
                Options.Instance.AddSmallGear(equip.ReferenceId, gear);
                txtSmall.Text = string.Empty;
                txtSmall.Focus();
            }

            RefreshPage();
            Options.Instance.StoreSettings();
        }

        private void BigRemoveButton_Click(object sender, EventArgs e)
        {
            if (treeBig.Selected.Count > 0 && equip != null)
            {
                float gear = (float)treeBig.Selected[0];

                Options.Instance.RemoveBigGear(equip.ReferenceId, gear);

            }

            RefreshPage();
            Options.Instance.StoreSettings();
        }

        private void SmallRemoveButton_Click(object sender, EventArgs e)
        {
            if (treeSmall.Selected.Count > 0 && equip != null)
            {
                float gear = (float)treeSmall.Selected[0];
                Options.Instance.RemoveSmallGear(equip.ReferenceId, gear);
            }

            RefreshPage();
            Options.Instance.StoreSettings();
        }

        private void txtCircum_Leave(object sender, EventArgs e)
        {
            float circumference;
            float.TryParse(txtCircum.Text, out circumference);
            if (circumference != 0 && equip != null)
            {
                Options.Instance.SetWheelCircumference(equip.ReferenceId, circumference / 1000F);
                Options.Instance.StoreSettings();
            }
        }

        private void btnEquipOpen_Click(object sender, EventArgs e)
        {
            // Create TreeListPopup
            TreeListPopup popup = new TreeListPopup();
            popup.ThemeChanged(PluginMain.GetApplication().VisualTheme);
            popup.Tree.Columns.Add(new TreeList.Column("Name"));

            // TODO: Migrate to ST3 - Settings
            IEnumerable<IEquipmentItem> equipmentList = PluginMain.GetApplication().Logbook.Equipment;

            popup.Tree.RowData = equipmentList;
            popup.ItemSelected += new TreeListPopup.ItemSelectedEventHandler(popup_ItemSelected);
            Rectangle rect = new Rectangle(txtEquip.Location, txtEquip.Size);
            rect = this.RectangleToScreen(rect);
            popup.Popup(rect);

        }

        private void popup_ItemSelected(object sender, TreeListPopup.ItemSelectedEventArgs e)
        {
            IEquipmentItem item = e.Item as IEquipmentItem;

            if (item != null)
            {
                selectItem(sender, item);
            }
        }

        /// <summary>
        /// Helper to popup_ItemSelected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item"></param>
        private void selectItem(object sender, IEquipmentItem item)
        {
            // Select a piece of equipment
            txtEquip.Text = item.Name.ToString();
            equip = item;
            if (sender != this)
            {
                RefreshPage();
            }
        }

        #endregion
    }
}
