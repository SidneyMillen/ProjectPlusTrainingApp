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
    class GamecubeController : Joystick
    {
        public Dictionary<Buttons, int> buttonNums;
        public GamecubeController(IntPtr nativePtr) : base(nativePtr)
        {
            
            dictSetup();

        }

        public GamecubeController(DirectInput directInput, Guid deviceGuid) : base(directInput, deviceGuid)
        {

            dictSetup();
        }

        private void dictSetup()
        {
            buttonNums = new Dictionary<Buttons, int>();
            buttonNums.Add(Buttons.A, 1);
            buttonNums.Add(Buttons.B, 2);
            buttonNums.Add(Buttons.X, 0);
            buttonNums.Add(Buttons.Y, 3);
            buttonNums.Add(Buttons.LEFTTRIGGER, 4);
            buttonNums.Add(Buttons.RIGHTTRIGGER, 5);
            buttonNums.Add(Buttons.Z, 7);
            buttonNums.Add(Buttons.START, 9);
            buttonNums.Add(Buttons.UP, 12);
            buttonNums.Add(Buttons.DOWN, 14);
            buttonNums.Add(Buttons.LEFT, 15);
            buttonNums.Add(Buttons.RIGHT, 13);

        }

        public bool GetButtonState(Buttons b)
        {
            return this.GetCurrentState().Buttons[buttonNums[b]];
        }
        public int GetAxisState(Axes a)
        {
            switch (a)
            {
                case Axes.X:
                    return this.GetCurrentState().X - 32768;
                    
                case Axes.Y:
                    return -this.GetCurrentState().Y - 32768;
                case Axes.CX:
                    return this.GetCurrentState().RotationZ - 32768;
                case Axes.CY:
                    return -this.GetCurrentState().Z - 32768;
                default:
                    return -1;
            }
        }




        
    }

    enum Buttons
    {
        A,
        B,
        X,
        Y,
        LEFTTRIGGER,
        RIGHTTRIGGER,
        Z,
        START,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    enum Axes
    {
        X,
        Y,
        CX,
        CY
    }
}
