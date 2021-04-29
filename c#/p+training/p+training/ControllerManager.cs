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
    class ControllerManager
    {
        public IXbox360Controller vController { get; set; }
        public GamecubeController rController { get; set; }
        public static Buttons[] allButtons;
        public static Axes[] allAxes;
        public bool mirrorMode;

        public ControllerManager(IXbox360Controller vController, GamecubeController rController)
        {
            this.vController = vController;
            this.rController = rController;
            allButtons = (Buttons[])Enum.GetValues(typeof(Buttons));
            allAxes = (Axes[])Enum.GetValues(typeof(Axes));
            mirrorMode = false;
        }


        public static Xbox360Button translateButton(Buttons b)
        {
            switch (b)
            {
                case Buttons.A:
                    return Xbox360Button.A;
                case Buttons.B:
                    return Xbox360Button.B;
                case Buttons.X:
                    return Xbox360Button.X;
                case Buttons.Y:
                    return Xbox360Button.Y;
                case Buttons.LEFTTRIGGER:
                    return Xbox360Button.LeftThumb;
                case Buttons.RIGHTTRIGGER:
                    return Xbox360Button.RightThumb;
                case Buttons.Z:
                    return Xbox360Button.RightShoulder;
                case Buttons.START:
                    return Xbox360Button.Start;
                case Buttons.UP:
                    return Xbox360Button.Up;
                case Buttons.DOWN:
                    return Xbox360Button.Down;
                case Buttons.LEFT:
                    return Xbox360Button.Left;
                case Buttons.RIGHT:
                    return Xbox360Button.Right;
                default:
                    return null;
            }
        }

        public static Xbox360Axis translateAxis(Axes a)
        {
            switch (a)
            {
                case Axes.X:
                    return Xbox360Axis.LeftThumbX;
                case Axes.Y:
                    return Xbox360Axis.LeftThumbY;
                case Axes.CX:
                    return Xbox360Axis.RightThumbX;
                case Axes.CY:
                    return Xbox360Axis.RightThumbY;
                default:
                    return null;
            }
        }



        public void setButtons(Buttons[] b)
        {
            foreach (var button in b)
            {
                vController.SetButtonState(translateButton(button), rController.GetButtonState(button));
            }
        }
        public void setButtons(Dictionary<Buttons, bool> b)
        {
            foreach (KeyValuePair<Buttons, bool> button in b)
            {
                vController.SetButtonState(translateButton(button.Key), button.Value);
            }
        }

        public void setAxes(Axes[] a)
        {
            foreach (var axis in a)
            {
                vController.SetAxisValue(translateAxis(axis), (short)rController.GetAxisState(axis));
            }
        }
        public void setAxes(Dictionary<Axes, int> a)
        {
            foreach (KeyValuePair<Axes, int> axis in a)
            {
                vController.SetAxisValue(translateAxis(axis.Key), (short)axis.Value);
            }
        }
        public void setAxes(Dictionary<Axes, short> a)
        {
            foreach (KeyValuePair<Axes, short> axis in a)
            {
                vController.SetAxisValue(translateAxis(axis.Key), axis.Value);
            }
        }

        public void setNoInput()
        {
            Dictionary<Buttons, bool> b = new Dictionary<Buttons, bool>();
            foreach(Buttons but in allButtons)
            {
                b.Add(but, false);
            }
            Dictionary<Axes, int> a = new Dictionary<Axes, int>();
            foreach(Axes ax in allAxes)
            {
                a.Add(ax, 0);
            }

            setButtons(b);
            setAxes(a);

        }

        public void startMirrorInputs()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            mirrorMode = true;
            while (mirrorMode)
            {
                setButtons(allButtons);
                setAxes(allAxes);
            }
            setNoInput();
        }
    }
}
