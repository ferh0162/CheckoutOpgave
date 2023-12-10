public delegate void VareScannetEventHandler(Vare vare);

public class Scanner
{
    public event VareScannetEventHandler VareScannet;

    public void Scan()
    {
        // Simulerer scanning af varer
        var scannedeVarer = new List<Vare>
        {
            new Vare("Mælk", "A", 10.0, 1),
            new Vare("Brød", "B", 20.0, 2),
        };

        foreach (var vare in scannedeVarer)
        {
            VareScannet?.Invoke(vare);
            Thread.Sleep(500);
        }
    }
}
