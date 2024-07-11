using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace desktop.data.Models
{
    public class Card
    {
        public ImageSource? CardImage { get; set; }
        public int ManaCost { get; set; }
        public string Name { get; set; } = "";
        public string DrawProbability { get; set; } = "";
        public string Region { get; set; } = "";
        public string CardType { get; set; } = "";
        public int CopiesRemaining { get; set; }
    }
}
