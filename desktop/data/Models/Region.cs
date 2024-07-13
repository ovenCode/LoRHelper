using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desktop.data.Models
{
    public class Region
    {
        public Regions RegionType { get; set; }
    }

    public enum Regions
    {
        All,
        BandleCity,
        Bilgewater,
        Demacia,
        Freljord,
        Ionia,
        Noxus,
        PiltoverZaun,
        ShadowIsles,
        Shurima,
        Targon
    }
}
