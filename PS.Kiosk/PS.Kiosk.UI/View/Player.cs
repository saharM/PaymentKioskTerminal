using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PS.Kiosk.UI.View
{
    public partial class Player : UserControl
    {
        public Player()
        {
            InitializeComponent();
        }

        
        public new int Width 
        {
            get{ return axShockwaveFlash.Width; }
            set{ axShockwaveFlash.Width = value; }
        }

        public new int Height 
        {
            get{ return axShockwaveFlash.Height; }
            set{ axShockwaveFlash.Height = value; }
        }

        public void LoadMovie(string strPath)
        {
            axShockwaveFlash.LoadMovie(0, strPath);            
        }
       
        public void Play()
        {            
            axShockwaveFlash.Play();
        }

        public void Stop()
        {
            axShockwaveFlash.Stop();
        }
        
    }
}
