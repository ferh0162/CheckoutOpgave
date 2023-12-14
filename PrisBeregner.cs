public abstract class Prisberegner
{
    // Dette er en abstrakt klasse, der definerer en metode "BeregnPris",
    // som skal implementeres af underklasser.
    public abstract void BeregnPris(Vare vare);
}

public class BilligPrisberegner : Prisberegner
{
    private List<Vare> scannedeVarer = new List<Vare>();

    public override void BeregnPris(Vare vare)
    {
        // Tilføjer den givne vare til listen over scannede varer.
        scannedeVarer.Add(vare);
        // Kalder metoden "OpdaterTotal" for at opdatere den totale pris og kvittering.
        OpdaterTotal();
    }

    private void OpdaterTotal()
    {
        // Beregner den samlede pris for alle scannede varer.
        double totalPris = scannedeVarer.Sum(v => v.Pris);
        // Beregner kampagnerabat for de scannede varer.
        double kampagneRabat = BeregnKampagneRabat();
        // Beregner den samlede pantbeløb for varer med pant.
        double totalPant = scannedeVarer.Where(v => v.HarPant).Sum(v => v.PantBeløb);
        // Viser detaljeret kvittering med de beregnede værdier.
        VisDetaljeretKvittering(totalPris, kampagneRabat, totalPant);
    }

 private double BeregnKampagneRabat()
{
    // Initialiserer en variabel til at holde styr på den samlede kampagnerabat.
    // Rabatten beregnes ved at gruppere varer efter varekode og antal, og derefter sammenligne med kampagneantal.
    return scannedeVarer
        .Where(v => v.HarKampagnepris) // Filtrer kun varer med kampagnepriser.
        .GroupBy(v => v.VareKode) // Grupper varerne efter deres varekode.
        .Sum(g =>
        {
            var vare = g.First(); // Hent den første vare i hver gruppe (alle har samme varekode).
            int antalKampagner = g.Count() / vare.KampagneAntal; // Beregn antallet af kampagner.
            // Beregn rabatten for hver kampagne ved at sammenligne normalprisen og kampagneprisen.
            return antalKampagner * (vare.Pris * vare.KampagneAntal - vare.KampagnePris);
        });
}


    private void VisDetaljeretKvittering(double totalPris, double kampagneRabat, double totalPant)
    {
        // Rydder konsolvinduet.
        Console.Clear();
        // Udskriver den samlede pris efter rabat og pantbeløb.
        Console.WriteLine($"Total pris: {totalPris + totalPant - kampagneRabat} kr");
    }
}

public class DyrPrisberegner : Prisberegner
{
    private List<Vare> scannedeVarer = new List<Vare>();

    public override void BeregnPris(Vare vare)
    {
        // Tilføjer den givne vare til listen over scannede varer.
        scannedeVarer.Add(vare);
        // Kalder metoden "OpdaterTotal" for at opdatere den totale pris og kvittering.
        OpdaterTotal();
    }

    private void OpdaterTotal()
    {
        // Beregner den samlede pris for alle scannede varer.
        double totalPris = scannedeVarer.Sum(v => v.Pris);
        // Beregner kampagnerabat for de scannede varer.
        double kampagneRabat = BeregnKampagneRabat();
        // Beregner den samlede pantbeløb for varer med pant.
        double totalPant = BeregnTotalPant();
        // Viser detaljeret kvittering med de beregnede værdier.
        VisDetaljeretKvittering(totalPris, kampagneRabat, totalPant);
    }

    private double BeregnKampagneRabat()
    {
        double rabat = 0; // Initialiserer en variabel til at holde styr på den samlede kampagnerabat.

        // Grupperer varer med kampagnepriser efter deres varekode.
        var grupperedeVarer = scannedeVarer
            .Where(v => v.HarKampagnepris) // Filtrer kun varer med kampagnepriser.
            .GroupBy(v => v.VareKode); // Grupper varerne efter deres varekode.

        foreach (var gruppe in grupperedeVarer)
        {
            var vare = gruppe.First(); // Hent den første vare i gruppen (alle har samme varekode).
            int antal = gruppe.Count(); // Tæl antallet af varer i gruppen.

            // Tjek om antallet af varer i gruppen er større eller lig med kampagneantallet.
            if (antal >= vare.KampagneAntal)
            {
                int antalKampagner = antal / vare.KampagneAntal; // Beregn antallet af kampagner.
                double normalPrisForKampagne = vare.Pris * vare.KampagneAntal; // Beregn normalprisen for en kampagne.
                rabat += (normalPrisForKampagne - vare.KampagnePris) * antalKampagner; // Beregn og tilføj rabatten.
            }
        }

        return rabat; // Returner den samlede kampagnerabat.
    }

    private double BeregnTotalPant()
    {
        // Beregner den samlede pantbeløb for varer med pant.
        return scannedeVarer.Where(v => v.HarPant).Sum(v => v.PantBeløb);
    }

   private void VisDetaljeretKvittering(double totalPris, double kampagneRabat, double totalPant)
{
    // Rydder konsolvinduet for at klargøre det til kvitteringsudskrivning.
    Console.Clear();

    // Grupperer og opsummerer varer efter varekode og udskriver en detaljeret kvittering.
    var grupperedeOgOpsummeredeVarer = scannedeVarer
        .GroupBy(v => v.VareKode) // Grupperer varerne efter deres varekode.
        .Select(g => new
        {
            Vare = g.First(), // Hent den første vare i hver gruppe (alle har samme varekode).
            Antal = g.Count(), // Tæl antallet af varer i hver gruppe.
            SamletPris = g.Sum(v => v.Pris) // Beregn den samlede pris for varerne i hver gruppe.
        });

    // Udskriver overskriften for kvitteringen.
    Console.WriteLine("Kvittering");

    // Udskriver varerne grupperet efter varegruppe.
    foreach (var gruppe in grupperedeOgOpsummeredeVarer.GroupBy(v => v.Vare.VareGruppe))
    {
        // Udskriver varegruppen som en overskrift.
        Console.WriteLine($"-- Varegruppe: {gruppe.Key} ---------------------");
        foreach (var v in gruppe)
        {
            // Udskriver hver vare i gruppen med detaljerede oplysninger.
            Console.WriteLine($"{v.Antal}x {v.Vare.VareNavn,-15} {v.Vare.Pris.ToString("F2")} kr.{new string(' ', 5)}pris: {v.SamletPris.ToString("F2"),7} kr.");

            // Hvis varen har kampagnepris og antallet er tilstrækkeligt stort, udskriv kampagnerabat.
            if (v.Vare.HarKampagnepris && v.Antal >= v.Vare.KampagneAntal)
            {
                int antalKampagner = v.Antal / v.Vare.KampagneAntal;
                double samletKampagneRabat = (v.Vare.Pris * v.Vare.KampagneAntal - v.Vare.KampagnePris) * antalKampagner;

                Console.ForegroundColor = ConsoleColor.Red; // Sæt farven til rød
                Console.WriteLine($"   Kampagne-rabat: {samletKampagneRabat} kr");
                Console.ResetColor(); // Nulstil farven
            }

            // Hvis varen har pant, udskriv pantbeløb.
            if (v.Vare.HarPant)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow; // Sæt farven til mørkegul
                double samletPant = v.Antal * v.Vare.PantBeløb;
                Console.WriteLine($"   Pant: {samletPant} kr");
                Console.ResetColor(); // Nulstil farven
            }
        }
    }

    Console.WriteLine();
    Console.WriteLine($"Samlet pris før Rabat og Pant: {totalPris} kr"); // Udskriv samlet pris før rabat og pant.
    Console.WriteLine($"Multipack-rabat: {kampagneRabat} kr"); // Udskriv kampagnerabat.
    Console.WriteLine($"Total Pant: {totalPant} kr"); // Udskriv samlet pantbeløb.
    Console.WriteLine();

    // Opdater totalPris til at inkludere pantbeløb og trække kampagnerabat fra.
    totalPris = totalPris + totalPant - kampagneRabat;

    Console.WriteLine($"Total pris {totalPris} kr"); // Udskriv den endelige totalpris inklusive rabat og pant.
    Console.WriteLine(); // Skriv en tom linje for at afslutte kvitteringen.
}
}