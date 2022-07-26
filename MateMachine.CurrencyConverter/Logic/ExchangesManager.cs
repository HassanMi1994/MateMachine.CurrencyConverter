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

            var target = currency.CurrenciesInSteps.OrderBy(x => x.totalStep).FirstOrDefault(x => x.currency == to);
            if (target.totalStep == default(int))
                throw new Exception($"The From currency is not valid. {to}");


            Console.WriteLine(target.path);
            return currency.CalculateBasedOnSteps(amount, target.stepToCalculate);
        }

        public void CalculatePossibleExchagneRates(IEnumerable<Tuple<string, string, double>> userList)
        {
            this.userList = userList;
            HashSet<string> uniqueCurrencies = new();
            foreach (var item in userList)
            {
                uniqueCurrencies.Add(item.Item1);
                uniqueCurrencies.Add(item.Item2);
            }

            foreach (var item in uniqueCurrencies)
            {
                Exchange exchange = new Exchange(item);
                exchange.Steps = GetAllNodeFor(item, exchange);
                Exchanges.Add(exchange);
            }
        }

        private List<Step> GetAllNodeFor(string currency, Exchange exchange, Step previousStep = null)
        {
            List<Step> steps = new List<Step>();

            FindRouteFromLeftHand(currency, exchange, previousStep, steps);
            FindRouteFromRightHand(currency, exchange, previousStep, steps);

            return steps;
        }

        private void FindRouteFromLeftHand(string currency, Exchange exchange, Step previousStep, List<Step> steps)
        {
            foreach (var item in userList.Where(x => x.Item1 == currency && x.Item2 != exchange.Currency))
            {
                bool isThereAnyBetterPerformance, isOldRecordAvailable;
                UpdateConfig(exchange, previousStep, item.Item2, out isThereAnyBetterPerformance, out isOldRecordAvailable);

                if (!isOldRecordAvailable || (isOldRecordAvailable && !isThereAnyBetterPerformance))
                {
                    var step = new Step(previousStep, item.Item1, item.Item2, item.Item3, ExchageRateOperation.Multiply, item.Item3);
                    exchange.CurrenciesInSteps.Add((item.Item2, step.ToString(), step.StepNumber, step.GetAllSteps()));
                    step.NextSteps = GetAllNodeFor(item.Item2, exchange, step);
                    steps.Add(step);
                }
            }
        }

        private static void UpdateConfig(Exchange exchange, Step previousStep, string symbol, out bool isThereAnyBetterPerformance, out bool isOldRecordAvailable)
        {
            var isThereAny = exchange.CurrenciesInSteps.OrderByDescending(x => x.totalStep).FirstOrDefault(x => x.currency == symbol);
            isThereAnyBetterPerformance = false;
            isOldRecordAvailable = isThereAny.totalStep != default(int);
            if (isOldRecordAvailable)
            {
                isThereAnyBetterPerformance = isThereAny.totalStep <= previousStep?.StepNumber;
            }
        }

        private void FindRouteFromRightHand(string currency, Exchange exchange, Step previousStep, List<Step> steps)
        {
            foreach (var item in userList.Where(x => x.Item2 == currency && x.Item1 != exchange.Currency))
            {
                bool isThereAnyBetterPerformance, isOldRecordAvailable;
                UpdateConfig(exchange, previousStep, item.Item1, out isThereAnyBetterPerformance, out isOldRecordAvailable);

                if (!isOldRecordAvailable || (isOldRecordAvailable && !isThereAnyBetterPerformance))
                {
                    var step = new Step(previousStep, item.Item2, item.Item1, item.Item3, ExchageRateOperation.Divide, item.Item3);
                    exchange.CurrenciesInSteps.Add((item.Item1, step.ToString(), step.StepNumber, step.GetAllSteps()));
                    step.NextSteps = GetAllNodeFor(item.Item1, exchange, step);
                    steps.Add(step);
                }
            }
        }
    }
}
