using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GearChart.Data.ActivityDocumentationComponent
{
    public partial class EditForm : Form
    {
        public EditForm()
        {
            InitializeComponent();

            // Set Icon
            this.Icon = Icon.FromHandle(Resources.Images.Settings.GetHicon());
        }

        /// <summary>
        /// Gets a value indicating whether the chart basis is Time or Distance
        /// </summary>
        public bool Time
        {
            get { return radTime.Checked; }
            set
            {
                if (value)
                {
                    radTime.Checked = true;
                }
                else
                {
                    radDistance.Checked = true;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not laps are shown
        /// </summary>
        public bool ShowLaps
        {
            get { return chkShowLaps.Checked; }
            set { chkShowLaps.Checked = value; }
        }

        /// <summary>
        /// Gets a value indicating whether raw gear data is to be shown on the chart or not
        /// </summary>
        public bool ShowRawData
        {
            get { return chkRawData.Checked; }
            set { chkRawData.Checked = value; }
        }

        /// <summary>
        /// Gets the user selected chart width
        /// </summary>
        public int ChartWidth
        {
            get
            {
                bool success;
                int value;
                success = int.TryParse(txtWidth.Text, out value);

                if (success)
                {
                    return value;
                }
                else
                {
                    // Default value
                    return 400;
                }
            }
            set { txtWidth.Text = value.ToString(); }
        }

        /// <summary>
        /// Gets the user selected chart height
        /// </summary>
        public int ChartHeight
        {
            get
            {
                bool success;
                int value;
                success = int.TryParse(txtHeight.Text, out value);

                if (success)
                {
                    return value;
                }
                else
                {
                    // Default value
                    return 400;
                }
            }
            set { txtHeight.Text = value.ToString(); }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int value;

            if (!int.TryParse(txtHeight.Text, out value))
            {
                MessageBox.Show("Please enter a valid height.");
            }
            else if (!int.TryParse(txtWidth.Text, out value))
            {
                MessageBox.Show("Please enter a valid width.");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
