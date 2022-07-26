using MateMachine.CurrencyConverter.Abstraction;

namespace MateMachine.CurrencyConverter.Logic
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private List<Tuple<string, string, double>> _userConversionRates = new List<Tuple<string, string, double>>();
        private ExchangesRateManager _exchangeManager;

        public CurrencyConverter(List<Tuple<string, string, double>> userConversionRates)
        {
            _userConversionRates = userConversionRates;
            _exchangeManager = new ExchangesRateManager(userConversionRates);
        }

        public void ClearConfiguration()
        {
            _userConversionRates.Clear();
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            return _exchangeManager.Convert(fromCurrency, toCurrency, amount);
        }

        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            foreach (var item in conversionRates)
            {
                var isAvailable = _userConversionRates.FirstOrDefault(x => x.Item1 == item.Item1 && x.Item2 == item.Item2);

                if (isAvailable is not null)

                    isAvailable = Tuple.Create(isAvailable.Item1, isAvailable.Item2, item.Item3);
                else
                    _userConversionRates.Add(Tuple.Create(item.Item1, item.Item2, item.Item3));
            }
            _exchangeManager.CalculatePossibleExchagneRates(_userConversionRates);
        }
    }
}
