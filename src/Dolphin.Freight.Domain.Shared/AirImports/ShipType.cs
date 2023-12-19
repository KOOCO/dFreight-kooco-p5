using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dolphin.Freight.AirImports
{
    public enum ShipType
    {
        Normal = 1,
        [Display(Name = "S/W")]
        Sw,
        [Display(Name = "T/S")]
        Ts
    }
}
