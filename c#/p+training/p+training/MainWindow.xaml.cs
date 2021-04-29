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
        bool mirrorFlag;

        // Initialize DirectInput
        DirectInput directInput;

        // Find all joysticks connected to the system
        

        GamecubeController rController;
        

        ControllerManager cMan;

        Recording r;


        public MainWindow()
        {
            controllerSetup();
            r = new Recording(cMan);

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



            cMan = new ControllerManager(vController, rController);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!r.recording && !r.playing)
            {
                r.play();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!r.recording && !r.playing && !cMan.mirrorMode)
            {
                cMan.startMirrorInputs();
            } else if (!r.recording && !r.playing && cMan.mirrorMode)
            {
                cMan.mirrorMode = false;
            }
        }
    }
}
