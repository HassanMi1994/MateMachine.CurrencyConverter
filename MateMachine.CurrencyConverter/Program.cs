// See https://aka.ms/new-console-template for more information
using MateMachine.CurrencyConverter.Logic;
using System;

/*
 * - (USD => CAD) 1.34
- (CAD => GBP) 0.58
- (USD => EUR) 0.86
 * 
 */
List<Tuple<string, string, double>> rates = new List<Tuple<string, string, double>>
             {
              Tuple.Create("USD","CAD",1.34),
              Tuple.Create("CAD","GBP",0.58),
              Tuple.Create("USD","EUR",0.86),
             };

CurrencyConverter converter = new CurrencyConverter(rates);
converter.UpdateConfiguration(rates);

Console.WriteLine(converter.Convert("GBP", "EUR", 2.0));

Console.Read();