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

        var grupperedeVarer = scannedeVarer
            .GroupBy(v => v.VareGruppe)
            .Select(group => new 
            {
                VareGruppe = group.Key,
                Varer = group.GroupBy(v => v.VareKode).Select(g => new
                {
                    VareNavn = g.First().VareNavn,
                    Antal = g.Count(),
                    SamletPris = g.Sum(v => v.Pris)
                }).ToList()
            });

        Console.Clear(); // Renser konsollen for hver scanning for overskuelighed
        double totalPris = 0;

        foreach (var gruppe in grupperedeVarer)
        {
            Console.WriteLine($"Varegruppe: {gruppe.VareGruppe}");
            foreach (var v in gruppe.Varer)
            {
                Console.WriteLine($" - {v.VareNavn}: {v.Antal} stk. til {v.SamletPris} kr (Stk. pris: {v.SamletPris / v.Antal})");
                totalPris += v.SamletPris;
            }
        }

        Console.WriteLine($"Samlet pris for alle varer: {totalPris} kr");
    }
}
