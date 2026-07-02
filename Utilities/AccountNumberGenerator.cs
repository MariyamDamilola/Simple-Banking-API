namespace SimpleBankingAPI.Utilities;

public static class AccountNumberGenerator
{
    private static readonly Random _random = new Random();

    public static string GenerateAcc()
    {
        long Digits = _random.Next(1000000000, 1111111111);
        return Digits.ToString();
    }
}