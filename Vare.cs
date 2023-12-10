public class Vare
{
    public string VareNavn { get; set; }
    public string VareKode { get; set; }
    public double Pris { get; set; }
    public int VareGruppe { get; set; }

    public Vare(string vareNavn, string vareKode, double pris, int vareGruppe)
    {
        VareNavn = vareNavn;
        VareKode = vareKode;
        Pris = pris;
        VareGruppe = vareGruppe;
    }
}
