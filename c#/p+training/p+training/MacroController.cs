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
    /// an xbox controller that knows how to translate a specific save slot to a button thats set up in dolphin to be a hotkey for that savetate
    /// </summary>
    public class MacroController
    {
        private IXbox360Controller controller;
        private static Dictionary<int, Xbox360Button> SaveButtons;
        private static Dictionary<int, Xbox360Button> LoadButtons;

        public MacroController(IXbox360Controller controller)
        {
            this.controller = controller;

            SaveButtons = new Dictionary<int, Xbox360Button>
            {

                { 1, Xbox360Button.Up },
                { 2, Xbox360Button.Down },
                { 3, Xbox360Button.Left },
                { 4, Xbox360Button.Right },
                { 5, Xbox360Button.LeftShoulder },
                { 6, Xbox360Button.LeftThumb }
            };

            LoadButtons = new Dictionary<int, Xbox360Button>
            {
                { 1, Xbox360Button.A },
                { 2, Xbox360Button.B },
                { 3, Xbox360Button.X },
                { 4, Xbox360Button.Y },
                { 5, Xbox360Button.RightShoulder },
                { 6, Xbox360Button.RightThumb }
            };
        }

        //TODO make these run on a separate thread in case save states become fast enough
        public void SaveState(int num)
        {
            if (num <= SaveButtons.Count)
            {
                controller.SetButtonState(SaveButtons[num], true);
                Thread.Sleep(80);
                controller.SetButtonState(SaveButtons[num], false);
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public void LoadState(int num)
        {
            if (num <= LoadButtons.Count)
            {
                controller.SetButtonState(LoadButtons[num], true);
                Thread.Sleep(80);
                controller.SetButtonState(LoadButtons[num], false);
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }
    }
}
