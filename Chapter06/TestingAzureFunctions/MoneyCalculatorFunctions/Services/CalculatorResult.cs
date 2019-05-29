namespace MoneyCalculatorFunctions.Services
{
    public class CalculatorResult
    {
        public decimal? Result { get; set; }

        public bool Succeed { get; set; } = true;

        public CalculatorError Error { get; set; }
    }

    public class CalculatorError
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}