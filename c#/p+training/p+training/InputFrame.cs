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

namespace p_training
{
    /// <summary>
    /// the state of all the buttons and stick on a frame
    /// </summary>
    public class InputFrame
    {
        public Dictionary<Buttons, bool> buttonStates;
        public Dictionary<Axes, int> axisStates;
        public int frameNumber;

        GamecubeController gamecubeController;


        public InputFrame(GamecubeController gamecubeController, int frameNumber)
        {
            this.gamecubeController = gamecubeController;

            buttonStates = new Dictionary<Buttons, bool>();
            axisStates = new Dictionary<Axes, int>();

            foreach (Buttons b in ControllerLinker.allButtons)
            {
                bool state = gamecubeController.GetButtonState(b);
                buttonStates.Add(b, state);
            }

            foreach (Axes a in ControllerLinker.allAxes)
            {
                int state = gamecubeController.GetAxisState(a);
                axisStates.Add(a, state);
            }

            this.frameNumber = frameNumber;
        }



    }
}
