using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fcc3_configurator
{
    public partial class FormAdvancedHardware : Form
    {
        public FormAdvancedHardware()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            if (Properties.Settings.Default.UseNewFccGain)
            {
                comboBoxFccRevision.SelectedIndex = 1;
            } else
            {
                comboBoxFccRevision.SelectedIndex = 0;
            }
        }

        private void comboBoxFccRevision_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool newGain = (comboBoxFccRevision.SelectedIndex == 0) ? false : true;
            Properties.Settings.Default.UseNewFccGain = newGain;
            Properties.Settings.Default.Save();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormAdvancedHardware_Load(object sender, EventArgs e)
        {

        }
    }
}
