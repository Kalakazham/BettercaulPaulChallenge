namespace BettercallPaulApp.Models;

public class Country
{
    public string Name { get; set; }
    public string Capital { get; set; }
    public string AccessionYear { get; set; } // Some values are strings
    public double Population { get; set; }
    public double Area { get; set; }
    public double GDP { get; set; } // GDP in US$ millions
    public double HDI { get; set; } // Human Development Index
    public int MEPs { get; set; } // Members of European Parliament
}