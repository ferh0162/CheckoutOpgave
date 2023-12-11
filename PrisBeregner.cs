public abstract class Prisberegner
{
    public abstract void BeregnPris(Vare vare);
}

public class BilligPrisberegner : Prisberegner
{
    private double total = 0;

    public override void BeregnPris(Vare vare)
    {
    
        total += vare.Pris;
        Console.WriteLine($"Nuværende total: {total}");
    }
}

public class DyrPrisberegner : Prisberegner
{
    private List<Vare> scannedeVarer = new List<Vare>();

    public override void BeregnPris(Vare vare)
    {
        scannedeVarer.Add(vare);

        // Opdater totalen hver gang en ny vare scannes
        OpdaterTotal();
    }

    private void OpdaterTotal()
    {
        double totalPris = 0;

        // Beregning af total pris under hensyntagen til multipack, kampagnepriser og pant
        foreach (var vare in scannedeVarer)
        {
            totalPris += BeregnAktuelPris(vare);
        }

        VisDetaljeretKvittering(totalPris);
    }
private double BeregnAktuelPris(Vare vare)
{
    double pris = vare.Pris;
    int antalAfSammeVare = scannedeVarer.Count(v => v.VareKode == vare.VareKode);

    // Håndtering af multipack
    if (vare.ErMultipack && antalAfSammeVare >= vare.MultipackAntal)
    {
        pris = vare.MultipackPris / vare.MultipackAntal;
    }

    // Håndtering af kampagnepriser
    if (vare.HarKampagnepris && antalAfSammeVare >= vare.KampagneAntal)
    {
        pris = vare.KampagnePris / vare.KampagneAntal;
    }

    // Tilføj pant, hvis relevant
    if (vare.HarPant)
    {
        pris += vare.PantBeløb;
    }

    return pris;
}


private void VisDetaljeretKvittering(double totalPris)
{
    Console.Clear();
    var unikkeVarer = scannedeVarer.GroupBy(v => v.VareKode).Select(gruppe => gruppe.First());

    foreach (var gruppe in unikkeVarer.GroupBy(v => v.VareGruppe))
    {
        Console.WriteLine($"Varegruppe: {gruppe.Key}");
        foreach (var v in gruppe)
        {
            int antalAfSammeVare = scannedeVarer.Count(v2 => v2.VareKode == v.VareKode);
            double samletPrisForVare = BeregnAktuelPris(v) * antalAfSammeVare;

            Console.WriteLine($" - {v.VareNavn} ({antalAfSammeVare} stk): {samletPrisForVare} kr (Stk. pris: {BeregnAktuelPris(v)} kr)");
        }
    }

    Console.WriteLine($"Samlet pris: {totalPris} kr");
}

}


