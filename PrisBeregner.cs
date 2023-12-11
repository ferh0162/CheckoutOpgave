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
        double kampagneRabat = BeregnKampagneRabat();
        double totalPant = BeregnTotalPant();

        VisDetaljeretKvittering(totalPris, kampagneRabat, totalPant);
    }

    private double BeregnKampagneRabat()
    {
        double rabat = 0;

        var grupperedeVarer = scannedeVarer
            .Where(v => v.HarKampagnepris)
            .GroupBy(v => v.VareKode);

        foreach (var gruppe in grupperedeVarer)
        {
            var vare = gruppe.First();
            int antal = gruppe.Count();
            if (antal >= vare.KampagneAntal)
            {
                int antalKampagner = antal / vare.KampagneAntal;
                double normalPrisForKampagne = vare.Pris * vare.KampagneAntal;
                rabat += (normalPrisForKampagne - vare.KampagnePris) * antalKampagner;
            }
        }

        return rabat;
    }

    private double BeregnTotalPant()
    {
        return scannedeVarer.Where(v => v.HarPant).Sum(v => v.PantBeløb);
    }

    private void VisDetaljeretKvittering(double totalPris, double kampagneRabat, double totalPant)
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


        Console.WriteLine("Kvittering");

    // Udskriv varerne grupperet efter varegruppe
    foreach (var gruppe in grupperedeOgOpsummeredeVarer.GroupBy(v => v.Vare.VareGruppe))
    {
        Console.WriteLine($"-- Varegruppe: {gruppe.Key} ---------------------");
        foreach (var v in gruppe)
        {
        Console.WriteLine($"{v.Antal, -3}x {v.Vare.VareNavn, -15} {v.Vare.Pris.ToString("F2")} kr.{new string(' ', 5)}pris: {v.SamletPris.ToString("F2"), 7} kr.");

            if (v.Vare.HarKampagnepris && v.Antal >= v.Vare.KampagneAntal)
            {
                int antalKampagner = v.Antal / v.Vare.KampagneAntal;
                double samletKampagneRabat = (v.Vare.Pris * v.Vare.KampagneAntal - v.Vare.KampagnePris) * antalKampagner;

                Console.ForegroundColor = ConsoleColor.Red; // Sæt farven til rød
                Console.WriteLine($"   Kampagne-rabat: {samletKampagneRabat} kr");
                Console.ResetColor(); // Reset farven

            }
            if (v.Vare.HarPant)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow; // Sæt farven til rød                
                double samletPant = v.Antal * v.Vare.PantBeløb;
                Console.WriteLine($"   Pant: {samletPant} kr");
                Console.ResetColor(); // Reset farven
            }
     
        }

    }

        Console.WriteLine();
        Console.WriteLine($"Samlet pris før Rabat og Pant: {totalPris} kr");
        Console.WriteLine($"Multipack-rabat: {kampagneRabat} kr");
        Console.WriteLine($"Total Pant: {totalPant} kr");
        Console.WriteLine();
        // set totalPris til at være totalPris + totalPant - multipackRabat
        Console.WriteLine($"Total pris {totalPris + totalPant - kampagneRabat} kr");
        Console.WriteLine();

}


}
