using MateMachine.CurrencyConverter.Logic;

namespace MateMachine.Tests
{
    public class CurrencyConverterTests
    {
        private CurrencyConverter.Logic.CurrencyConverter converter;

        public CurrencyConverterTests()
        {

            List<Tuple<string, string, double>> rates = new List<Tuple<string, string, double>>
             {
              Tuple.Create("USD","CAD",1.34),
              Tuple.Create("CAD","GBP",0.58),
              Tuple.Create("USD","EUR",0.86),
              Tuple.Create("RIAL","GBP",380000.0),
             };

            converter = new CurrencyConverter.Logic.CurrencyConverter(rates);
            converter.UpdateConfiguration(rates);
        }

        [Fact]
        public void Convert_7_USD_To_CAD_Should_Return_9_38()
        {
            var result = converter.Convert("USD", "CAD", 7);
            Assert.Equal(9.38, result);
        }

        [Fact]
        public void Convert_14_CAD_To_USD_Should_Return_10_44776119402985()
        {
            var result = converter.Convert("CAD", "USD", 14);
            Assert.Equal(10.44776119402985, result);
        }

        [Fact]
        public void Convert_2_GBP_To_EUR_Should_Return_2_2130725681935153()
        {
            var result = converter.Convert("GBP", "EUR", 2);
            Assert.Equal(2.2130725681935153, result);
        }
    }
}