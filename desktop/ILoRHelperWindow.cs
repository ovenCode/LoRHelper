using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace desktop
{
    interface ILoRHelperWindow
    {
        public static Brush GetBackground() { return new SolidColorBrush(); }
    }
}
