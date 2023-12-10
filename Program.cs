class Program
{
    static void Main(string[] args)
    {
        Scanner scanner = new Scanner();
//        BilligPrisberegner billigBeregner = new BilligPrisberegner();
        DyrPrisberegner dyrBeregner = new DyrPrisberegner();

        //scanner.VareScannet += billigBeregner.BeregnPris;
        scanner.VareScannet += dyrBeregner.BeregnPris;

        scanner.Scan();
    }
}
