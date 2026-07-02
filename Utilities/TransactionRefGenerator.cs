namespace SimpleBankingAPI.Utilities;

public static class TransactionRefGenerator
{
        public static string GenerateRef(string prefix = "TRX")
        {
            var timestamp = DateTime.Now.ToString("yyMMddHHmmssfff");
            return $"{prefix}{timestamp}";
        }
}