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

namespace p_training
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViGEmClient client;

        IXbox360Controller vController;
        IXbox360Controller mirrorController;
        IXbox360Controller macController;

        // Initialize DirectInput
        DirectInput directInput;

        // Find all joysticks connected to the system
        

        GamecubeController rController;
        MacroController macroCon;
        

        ControllerManager virtualMan;
        ControllerManager mirrorMan;

        Recording r;


        public MainWindow()
        {
            controllerSetup();
            r = new Recording(virtualMan);

            InitializeComponent();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (r.recording)
            {
                r.recording = false;
                
            }
            else
            {
                macroCon.SaveState(1);
                r.recording = true;
                r.Record();
            }

        }

       

        private void controllerSetup()
        {
            directInput = new DirectInput();
            IList<DeviceInstance> connectedJoysticks = new List<DeviceInstance>();
            foreach (var deviceInstance in directInput.GetDevices(DeviceType.FirstPerson, DeviceEnumerationFlags.AllDevices))
            {
                connectedJoysticks.Add(deviceInstance);
            }
            rController = new GamecubeController(directInput, connectedJoysticks[0].InstanceGuid);
            foreach (var a in connectedJoysticks)
            {
                Console.WriteLine(a.InstanceName + " " + a.Type);

            }
            rController.Acquire();


            client = new ViGEmClient();
            vController = client.CreateXbox360Controller();
            vController.Connect();
            mirrorController = client.CreateXbox360Controller();
            mirrorController.Connect();
            macController = client.CreateXbox360Controller();
            macController.Connect();
            macroCon = new MacroController(macController);



            virtualMan = new ControllerManager(vController, rController);
            mirrorMan = new ControllerManager(mirrorController, rController);

            mirrorMan.startMirrorInputs();


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!r.recording && !r.playing)
            {
                virtualMan.mirrorMode = false;
                macroCon.LoadState(1);
                Thread.Sleep(1000);
                r.play();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbPort.SelectedIndex)
            {
                case 0:
                    virtualMan.mirrorMode = false;
                    if (!mirrorMan.mirrorMode)
                    { 
                        mirrorMan.startMirrorInputs();
                    }
                    break;

                case 1:
                    mirrorMan.mirrorMode = false;
                    if (!r.recording && !r.playing && !virtualMan.mirrorMode)
                    {
                        virtualMan.startMirrorInputs();
                    }
                    break;

                case 2:
                    if (!r.recording && !r.playing)
                    {
                        if (!virtualMan.mirrorMode)
                        {
                            virtualMan.startMirrorInputs();
                        }
                        if (!mirrorMan.mirrorMode)
                        {
                            mirrorMan.startMirrorInputs();
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
