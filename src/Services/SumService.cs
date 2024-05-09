namespace app.Services;

interface ISumService
{
    int CalculateSum(int A, int B);
}

public class SumService : ISumService
{
    public int CalculateSum(int A, int B)
    {
        Console.WriteLine("calling: SumService.CalculateSum");
        return A + B;
    }
}
