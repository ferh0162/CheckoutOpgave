class Program
{
    static void Main(string[] args)
    {
        Scanner scanner = new Scanner();
        //BilligPrisberegner billigBeregner = new BilligPrisberegner();
        DyrPrisberegner dyrBeregner = new DyrPrisberegner();

        //scanner.VareScannet += billigBeregner.BeregnPris;
        scanner.VareScannet += dyrBeregner.BeregnPris; // Tilknyttet en begivenhedshåndterer (event handler) til VareScannet-begivenheden i scanner-objektet.


        scanner.Scan();
    }
}
