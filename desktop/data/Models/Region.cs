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
        BandleCity,
        Bilgewater,
        Demacia,
        Freljord,
        Ionia,
        Noxus,
        PiltoverZaun,
        Runeterra,
        ShadowIsles,
        Shurima,
        Targon
    }
}
