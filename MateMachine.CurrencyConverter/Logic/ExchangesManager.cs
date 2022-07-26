namespace MateMachine.CurrencyConverter.Logic
{
    public class ExchangesRateManager
    {
        public List<Exchange> Exchanges { get; set; }

        private IEnumerable<Tuple<string, string, double>> userList;
        public ExchangesRateManager(IEnumerable<Tuple<string, string, double>> userList)
        {
            Exchanges = new List<Exchange>();
            this.userList = userList;
        }

        public double Convert(string from, string to, double amount)
        {
            var currency = Exchanges.FirstOrDefault(x => x.Currency == from);
            if (currency == null)
                throw new Exception($"The From currency is not valid. {from}");

            var target = currency.CurrenciesInSteps.FirstOrDefault(x => x.currency == to);
            if (target.totalStep == default(int))
                throw new Exception($"The From currency is not valid. {to}");


            Console.WriteLine(target.path);
            return currency.CalculateBasedOnSteps(amount, target.stepToCalculate);
        }

        public void CalculatePossibleExchagneRates()
        {
            HashSet<string> uniqueCurrencies = new();
            foreach (var item in userList)
            {
                uniqueCurrencies.Add(item.Item1);
                uniqueCurrencies.Add(item.Item2);
            }

            foreach (var item in uniqueCurrencies)
            {
                Exchange exchange = new Exchange(item);
                exchange.Steps = GetAllNodeFor(item, exchange, 1);
                Exchanges.Add(exchange);
            }
        }

        private List<Step> GetAllNodeFor(string currency, Exchange exchange, double previousStepRate, Step previousStep = null)
        {
            List<Step> steps = new List<Step>();

            FindRouteFromLeftHand(currency, exchange, previousStepRate, previousStep, steps);
            FindRouteFromRightHand(currency, exchange, previousStepRate, previousStep, steps);

            return steps;
        }

        private void FindRouteFromLeftHand(string currency, Exchange exchange, double previousStepRate, Step previousStep, List<Step> steps)
        {
            foreach (var item in userList.Where(x => x.Item1 == currency && x.Item2 != exchange.Currency))
            {
                if (exchange.CurrenciesInSteps.Count(x => x.currency == item.Item2) == 0)
                {
                    var operatoin = previousStep?.GetFirstStepOperatoin() ?? ExchageRateOperation.Multiply;
                    var step = new Step(previousStep, item.Item1, item.Item2, item.Item3, ExchageRateOperation.Multiply, item.Item3);
                    exchange.CurrenciesInSteps.Add((item.Item2, step.ToString(), step.StepNumber, step.GetAllSteps()));
                    step.NextSteps = GetAllNodeFor(item.Item2, exchange, item.Item3 * previousStepRate, step);
                    steps.Add(step);
                }
            }
        }

        private void FindRouteFromRightHand(string currency, Exchange exchange, double previousStepRate, Step previousStep, List<Step> steps)
        {
            foreach (var item in userList.Where(x => x.Item2 == currency && x.Item1 != exchange.Currency))
            {
                if (exchange.CurrenciesInSteps.Count(x => x.currency == item.Item1) == 0)
                {
                    var operatoin = previousStep?.GetFirstStepOperatoin() ?? ExchageRateOperation.Divide;

                    var step = new Step(previousStep, item.Item2, item.Item1, item.Item3, ExchageRateOperation.Divide, item.Item3);
                    exchange.CurrenciesInSteps.Add((item.Item1, step.ToString(), step.StepNumber, step.GetAllSteps()));
                    step.NextSteps = GetAllNodeFor(item.Item1, exchange, item.Item3 / previousStepRate, step);
                    steps.Add(step);
                }
            }
        }
    }
}
