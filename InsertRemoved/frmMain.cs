using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InsertRemoved
{
    public partial class frmMain : Form
    {
        private const int DBT_DEVICE_ARRIVAL = 0x8000;
        private const int DBT_DEVICE_REMOVE_COMPLETE = 0x8004;
        private const int DBT_DEVTYP_VOLUME = 0x0000002;
        private const int WM_DEVICE_CHANGE = 0x219;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_DEVICE_CHANGE:
                    switch ((int)m.WParam)
                    {
                        case DBT_DEVICE_ARRIVAL:
                            int devType = Marshal.ReadInt32(m.LParam, 4);
                            if (devType == DBT_DEVTYP_VOLUME)
                            {
                                lstDevices.Items.Add("USB Volume ajouter");
                                DevBroadcastVolume vol;
                                vol = (DevBroadcastVolume)
                                    Marshal.PtrToStructure(m.LParam, typeof(DevBroadcastVolume));
                                lstDevices.Items.Add("Maqrue est " + vol.Mask);
                                lstDevices.Items.Add("Lettre est " + GetLetter(vol.Mask));
                            }
                            break;
                        case DBT_DEVICE_REMOVE_COMPLETE:
                            lstDevices.Items.Add("Device removed");
                            break;
                        default:
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        public char GetLetter(int mask)
        {
            int ch = 0;
            for (;ch < 26; ch++)
            {
                if ((mask & 0x1) == 0x1)
                    break;
                mask >>= 1;
            }
            ch += 0x41;
            return (char)ch;
        }

        public frmMain()
        {

            InitializeComponent();
        }
    }
}
