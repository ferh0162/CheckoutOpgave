public delegate void VareScannetEventHandler(Vare vare);

public class Scanner
{
    public event VareScannetEventHandler VareScannet;
    private List<Vare> varer;

    public Scanner()
    {
        // Initialiser listen af varer
        varer = new List<Vare>
{
             //Object Initializers
            new Vare("Mælk", "A", 10.0, 1),
            new Vare("Brød", "B", 20.0, 1),
            new Vare("Æg", "C", 5.0, 1),
            new Vare("Smør", "D", 15.0, 1),
            new Vare("Pasta", "E", 25.0, 1),
            new Vare("Shampoo", "F", 30.0, 2),
            new Vare("Tandpasta", "G", 20.0, 2),
            new Vare("Sokker", "H", 12.0, 3),
            new Vare("Hue", "I", 8.0, 3),

            // Tilføjelse af nye varetyper med multipack, kampagnepriser og pant
            new Vare("Bananer", "J", 10.0, 1, erMultipack: true, multipackAntal: 6, multiPackReference: "K"),
            new Vare("Banan", "K", 10.0, 1),
            new Vare("Sæbe", "L", 15.0, 2, harKampagnepris: true, kampagneAntal: 3, kampagnePris: 30.0),
            new Vare("Conditioner", "P", 15.0, 2, harKampagnepris: true, kampagneAntal: 2, kampagnePris: 20.0),
            new Vare("Sodavand", "O", 8.0, 1, harPant: true, pantBeløb: 1.0),
            new Vare("Øl", "N", 20.0, 4, erMultipack: true, multipackAntal: 6, multiPackReference: "O"),
            new Vare("Øl", "M", 4.0, 4, harPant: true, pantBeløb: 3.0),
};

    }

     public void Scan()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Indtast varekode (eller 'exit' for at afslutte): ");
            Console.ResetColor();
            string input = Console.ReadLine();

            if (input.ToLower() == "exit")
                break;

            // Søg efter varen i listen baseret på den indtastede varekode.
            var vare = varer.FirstOrDefault(v => v.VareKode == input.ToUpper());

            if (vare != null)
            {
                if (vare.ErMultipack)
                {
                    var multipackVare = varer.FirstOrDefault(v => v.VareKode == vare.MultiPackReference);
                    if (multipackVare != null)
                    {
                        // Hvis varen er en multipack, scan den tilsvarende multipack-referencevare flere gange.
                        for (int i = 0; i < vare.MultipackAntal; i++)
                        {
                            VareScannet?.Invoke(multipackVare);
                            Thread.Sleep(200); // Simuler en forsinkelse for scanning
                        }
                    }
                    else
                    {
                        Console.WriteLine("Multipack referencevare ikke fundet.");
                    }
                }
                else
                {
                    // Udløs VareScannet-begivenheden for den scannede vare.
                    VareScannet?.Invoke(vare);
                    Thread.Sleep(500); // Simuler en forsinkelse for scanning
                }
            }
            else
            {
                Console.WriteLine("Vare ikke fundet.");
            }
        }
    }
}