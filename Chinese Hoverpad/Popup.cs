using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Hoverpad
{
    public class Popup
    {
        public string Text;
        public Point Position;
        public bool Visible;
        public int LastChar;
        public int LastCharIndex;

        public Popup() { Visible = false; }
    }
}
