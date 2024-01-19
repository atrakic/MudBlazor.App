namespace app.Services;

public class SumService
{
    public int CalculateSum(int A, int B)
    {
        Console.WriteLine("calling: SumService.CalculateSum");
        return A + B;
    }
}
