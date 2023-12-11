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
        OpdaterTotal();
    }

    private void OpdaterTotal()
    {
        double totalPris = scannedeVarer.Sum(v => v.Pris);
        double multipackRabat = BeregnMultipackRabat();

        VisDetaljeretKvittering(totalPris, multipackRabat);
    }

    private double BeregnMultipackRabat()
    {
        double rabat = 0;

        var grupperedeVarer = scannedeVarer
            .Where(v => v.ErMultipack)
            .GroupBy(v => v.VareKode);

        foreach (var gruppe in grupperedeVarer)
        {
            var vare = gruppe.First();
            int antal = gruppe.Count();
            if (antal >= vare.MultipackAntal)
            {
                int antalMultipacks = antal / vare.MultipackAntal;
                double normalPrisForMultipack = vare.Pris * vare.MultipackAntal;
                rabat += (normalPrisForMultipack - vare.MultipackPris) * antalMultipacks;
            }
        }

        return rabat;
    }

private void VisDetaljeretKvittering(double totalPris, double multipackRabat)
{
    Console.Clear();

    // Grupper og opsummer varer efter varekode
    var grupperedeOgOpsummeredeVarer = scannedeVarer
        .GroupBy(v => v.VareKode)
        .Select(g => new 
        {
            Vare = g.First(),
            Antal = g.Count(),
            SamletPris = g.Sum(v => v.Pris) // Brug den basale pris fra Vare objektet
        });

    // Udskriv varerne grupperet efter varegruppe
    foreach (var gruppe in grupperedeOgOpsummeredeVarer.GroupBy(v => v.Vare.VareGruppe))
    {
        Console.WriteLine($"Varegruppe: {gruppe.Key}");
        foreach (var v in gruppe)
        {
            Console.WriteLine($" - {v.Vare.VareNavn} ({v.Antal} stk): {v.SamletPris} kr (Stk. pris: {v.Vare.Pris} kr)");

            if (v.Vare.ErMultipack && v.Antal >= v.Vare.MultipackAntal)
            {
                int antalMultipacks = v.Antal / v.Vare.MultipackAntal;
                double samletMultipackRabat = (v.Vare.Pris * v.Vare.MultipackAntal - v.Vare.MultipackPris) * antalMultipacks;
                Console.WriteLine($"   Multipack-rabat: {samletMultipackRabat} kr");
            }
        }
    }

    Console.WriteLine();
    Console.WriteLine($"Samlet pris før Rabat: {totalPris + multipackRabat} kr");
    Console.WriteLine($"Multipack-rabat: {multipackRabat} kr");
    Console.WriteLine();
    Console.WriteLine($"Total pris: {totalPris} kr");
    Console.WriteLine();

}


}
