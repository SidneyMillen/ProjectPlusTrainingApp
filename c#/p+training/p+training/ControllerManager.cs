using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p_training
{
    public class ControllerManager
    {



        DirectInput directInput;

        public GamecubeController rController;


        public ControllerLinker virtualLink;
        public ControllerLinker mirrorLink;


        public ControllerManager()
        {
            directInput = new DirectInput();
        }

        /// <summary>
        /// set the controller that the player is using to a deviceInstance
        /// </summary>
        /// <param name="d"></param>
        public void SetController(DeviceInstance d)
        {
            rController = new GamecubeController(directInput, d.InstanceGuid);
        }

        public void ControllerSetup(IXbox360Controller v, IXbox360Controller m)
        {

            rController.Acquire();

            virtualLink = new ControllerLinker(v, rController);
            mirrorLink = new ControllerLinker(m, rController);

            mirrorLink.startMirrorInputs();
        }

        /// <summary>
        /// get all the controllers plugged into the machine
        /// </summary>
        /// <returns>IList of all the controllers plugged into the machine</returns>
        public IList<DeviceInstance> GetDevices()
        {
            IList<DeviceInstance> connectedJoysticks = new List<DeviceInstance>();
            foreach (var deviceInstance in directInput.GetDevices(DeviceType.FirstPerson, DeviceEnumerationFlags.AllDevices))
            {
                connectedJoysticks.Add(deviceInstance);
            }
            return connectedJoysticks;
        }

    }
}
