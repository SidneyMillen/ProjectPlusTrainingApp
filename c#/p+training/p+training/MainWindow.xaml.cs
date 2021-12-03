using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpDX.DirectInput;
using System.ComponentModel;
using Xceed.Wpf.Toolkit;
using System.IO;

namespace p_training
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ControllerManager cMan;

        /// <summary>
        /// the gamecube controller plugged in to your computer that you play with
        /// </summary>
        GamecubeController rController;


        MacroController macroCon;

        //vigem is what allows us to make vitual controllers, this client allows us to do that
        ViGEmClient client;

        /// <summary>
        /// the virtal controller for player two
        /// </summary>
        IXbox360Controller vController;

        /// <summary>
        /// the virtual controller for player one, has to go through this layer of mirroring so it can be turned on and off as need be
        /// </summary>
        IXbox360Controller mirrorController;

        /// <summary>
        /// virtual controller used for dolphin savestates
        /// </summary>
        IXbox360Controller macController;


        Recording[] recordings;

        int savestateSlot = 1;

        /// <summary>
        ///  a list of all the controllers plugged into the machine
        /// </summary>
        IList<DeviceInstance> devices;

        const int NUM_SAVESLOTS = 6;



        public MainWindow()
        {
            recordings = new Recording[NUM_SAVESLOTS];

            cMan = new ControllerManager();


            //this section initializes the vigem client and creates 3 digital controllers: virtual, mirror, and macro
            client = new ViGEmClient();
            vController = client.CreateXbox360Controller();
            vController.Connect();
            mirrorController = client.CreateXbox360Controller();
            mirrorController.Connect();
            macController = client.CreateXbox360Controller();
            macController.Connect();

            macroCon = new MacroController(macController);



            InitializeComponent();
        }

        private void findDevices()
        {
            cmbDevices.Items.Clear();
            //sets devices to a list of all the controllers plugged into the machine
            devices = cMan.GetDevices();

            //set the devices combo box to show devices
            devices.ToList<DeviceInstance>().ForEach(Device => cmbDevices.Items.Add(Device));
        }


        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (recordings[(int)numSlot.Value-1].recording)
            {
                recordings[(int)numSlot.Value-1].recording = false;

            }
            else
            {
                macroCon.SaveState(savestateSlot);
                recordings[(int)numSlot.Value-1].recording = true;
                recordings[(int)numSlot.Value-1].Record();
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (!recordings[(int)numSlot.Value-1].recording && !recordings[(int)numSlot.Value-1].playing)
            {
                cMan.virtualLink.mirrorMode = false;
                macroCon.LoadState(savestateSlot);
                //this is to wait until it finishes loading the save state, if save states get faster this can be reduced, might also vary by machine
                //TODO add to settings so you can change the sleep time
                Thread.Sleep(1000);
                recordings[(int)numSlot.Value-1].play();
            }
        }



      



        private void cmbPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbPort.SelectedIndex)
            {
                case 0:
                    cMan.virtualLink.mirrorMode = false;
                    if (!cMan.mirrorLink.mirrorMode)
                    {
                        cMan.mirrorLink.startMirrorInputs();
                    }
                    break;

                case 1:
                    cMan.mirrorLink.mirrorMode = false;
                    if (!recordings[(int)numSlot.Value-1].recording && !recordings[(int)numSlot.Value-1].playing && !cMan.virtualLink.mirrorMode)
                    {
                        cMan.virtualLink.startMirrorInputs();
                    }
                    break;

                case 2:
                    if (!recordings[(int)numSlot.Value-1].recording && !recordings[(int)numSlot.Value-1].playing)
                    {
                        if (!cMan.virtualLink.mirrorMode)
                        {
                            cMan.virtualLink.startMirrorInputs();
                        }
                        if (!cMan.mirrorLink.mirrorMode)
                        {
                            cMan.mirrorLink.startMirrorInputs();
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private void cmbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (cmbDevices.SelectedItem != null)
            {
                EnableControls();

                cMan.SetController(devices[cmbDevices.SelectedIndex]);
                cMan.ControllerSetup(vController, mirrorController);


                for (int i = 0; i < NUM_SAVESLOTS; i++)
                {
                    recordings[i] = new Recording(cMan.virtualLink);
                }

                cmbPort.SelectedIndex = 0;
            }
        }
        private void EnableControls()
        {
            btnPlay.IsEnabled = true;
            btnRecord.IsEnabled = true;
            cmbPort.IsEnabled = true;
            numSlot.IsEnabled = true;
            btnSaveAs.IsEnabled= true;
        }


        private void Main_load(object sender, RoutedEventArgs e)
        {
            findDevices();

            if (cmbDevices.SelectedItem != null)
            {
                EnableControls();
                cmbPort.SelectedIndex = 0;
            }
        }

        private void btnRefreshDevices_Click(object sender, RoutedEventArgs e)
        {
            findDevices();
        }

        private void btnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Recording"; // Default file name
            dlg.DefaultExt = ".json"; // Default file extension
            dlg.Filter = "JSON files (.json)|*.json"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                File.WriteAllText(dlg.FileName, recordings[(int)numSlot.Value-1].getJson());


            }


        }

        private void numSlot_ValueChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            if (numSlot.Value > NUM_SAVESLOTS)
            {
                numSlot.Value = NUM_SAVESLOTS ;
            }
            else if (numSlot.Value <= 0)
            {
                numSlot.Value = 1;
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON Files (.json)|*.json";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                string json = File.ReadAllText(filename);
                recordings[(int)numSlot.Value - 1].loadJson(json);
            }
        }
    }
}
