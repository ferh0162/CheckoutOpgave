public class Vare
{
    //Properties
    public string VareNavn { get; set; }
    public string VareKode { get; set; }
    public double Pris { get; set; }
    public int VareGruppe { get; set; }

     // Nye egenskaber
    public bool ErMultipack { get; set; }
    public int MultipackAntal { get; set; }
    public double MultipackPris { get; set; }
    public bool HarKampagnepris { get; set; }
    public int KampagneAntal { get; set; }
    public double KampagnePris { get; set; }
    public bool HarPant { get; set; }
    public double PantBeløb { get; set; }

     public Vare(string vareNavn, string vareKode, double pris, int vareGruppe, 
                bool erMultipack = false, int multipackAntal = 0, double multipackPris = 0,
                bool harKampagnepris = false, int kampagneAntal = 0, double kampagnePris = 0,
                bool harPant = false, double pantBeløb = 0)
    {
        VareNavn = vareNavn;
        VareKode = vareKode;
        Pris = pris;
        VareGruppe = vareGruppe;
        ErMultipack = erMultipack;
        MultipackAntal = multipackAntal;
        MultipackPris = multipackPris;
        HarKampagnepris = harKampagnepris;
        KampagneAntal = kampagneAntal;
        KampagnePris = kampagnePris;
        HarPant = harPant;
        PantBeløb = pantBeløb;
    }
}
