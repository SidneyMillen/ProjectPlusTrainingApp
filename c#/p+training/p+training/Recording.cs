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
    class Recording
    {
        List<InputFrame> inputs;
        ControllerManager cMan;
        public bool recording;
        public bool playing;

        public Recording(ControllerManager cMan)
        {
            this.cMan = cMan;
            inputs = new List<InputFrame>();
        }


        public void Record()
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
            
            inputs = new List<InputFrame>();
            while (recording)
            {
                InputFrame input = new InputFrame(cMan.rController);
                inputs.Add(input);
                Thread.Sleep(50 / 3);
            }
        }

        public void play()
        {

            BackgroundWorker worker2 = new BackgroundWorker();

            worker2.DoWork += worker_DoWork2;
            worker2.RunWorkerCompleted += worker_RunWorkerCompleted2;
            worker2.RunWorkerAsync();
            
        }

        private void worker_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void worker_DoWork2(object sender, DoWorkEventArgs e)
        {
            playing = true;
            foreach (InputFrame i in inputs)
            {
                cMan.setButtons(i.buttonStates);
                cMan.setAxes(i.axisStates);
                Thread.Sleep(50 / 3);
            }
            cMan.setNoInput();
            playing = false;
        }
    }
}
