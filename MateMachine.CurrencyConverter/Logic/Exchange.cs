namespace MateMachine.CurrencyConverter.Logic
{
    public class Exchange
    {
        public string Currency { get; set; }
        public List<Step> Steps { get; set; }

        public HashSet<(string currency, string path, double totalStep, string stepToCalculate)> CurrenciesInSteps { get; set; }

        public Exchange(string currencyName)
        {
            Currency = currencyName;
            Steps = new List<Step>();
            CurrenciesInSteps = new HashSet<(string, string, double, string)>();
        }

        public void AddSteps(List<Step> steps)
        {
            Steps = steps;
        }

        public double CalculateBasedOnSteps(double amount, string stepToCalculate)
        {
            var steps = stepToCalculate.Split(",").Where(x => !string.IsNullOrEmpty(x)).ToList();
            for (int i = 0; i < steps.Count; i += 2)
            {
                if (steps[i] == "*")
                    amount *= double.Parse(steps[i + 1]);

                if (steps[i] == "/")
                    amount /= double.Parse(steps[i + 1]);
            }
            return amount;
        }

        public bool HasCurrency(string currency) => CurrenciesInSteps.Any(x => x.currency == currency);
    }
}
