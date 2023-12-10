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
    new Vare("Brød", "B", 20.0, 2),
    new Vare("Æg", "C", 5.0, 1),
    new Vare("Smør", "D", 15.0, 1),
    new Vare("Pasta", "E", 25.0, 2),
    new Vare("Kaffe", "F", 30.0, 3),
    new Vare("Te", "G", 20.0, 3),
    new Vare("Kiks", "H", 12.0, 4),
    new Vare("Sukker", "I", 8.0, 5)
};

    }

    public void Scan()
    {
        while (true)
        {
            Console.WriteLine("Indtast varekode (eller 'exit' for at afslutte): ");
            string input = Console.ReadLine();

            if (input.ToLower() == "exit")
                break;

            var vare = varer.FirstOrDefault(v => v.VareKode == input.ToUpper());

            if (vare != null)
            {
                VareScannet?.Invoke(vare);
                Thread.Sleep(500); // Simulerer en delay for scanning
            }
            else
            {
                Console.WriteLine("Vare ikke fundet.");
            }
        }
    }
}
