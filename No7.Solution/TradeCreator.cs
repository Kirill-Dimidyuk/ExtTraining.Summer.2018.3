using System;
using System.Collections.Generic;

namespace No7.Solution
{
    //Класс для создания трейдов
    public class TradeCreator
    {
        public static List<TradeRecord> CreateTradeList(List<string> lines)
        {
            var trades = new List<TradeRecord>();

            int lineNumber = 0;

            foreach (var line in lines)
            {
                lineNumber++;
                var field = line.Split(new char[] { ',' });

                //проверка на заполнение полей
                if(!CheckFields(field, lineNumber))
                {
                    continue;
                }

                //Не придумал как это обыграть
                if (!int.TryParse(field[1], out var tradeAmount))
                {
                    Console.WriteLine($"WARN: Trade amount on line {lineNumber} not a valid integer: '{field[1]}'");
                }

                if (!decimal.TryParse(field[2], out var tradePrice))
                {
                    Console.WriteLine($"WARN: Trade price on line {lineNumber} not a valid decimal: '{field[2]}'");
                }

                TradeRecord trade = new TradeRecord
                {
                    SourceCurrency = field[0].Substring(0, 3),
                    DestinationCurrency = field[0].Substring(3, 3),
                    Lots = tradeAmount / 100000f,
                    Price = tradePrice
                };

                trades.Add(trade);
            }
            return trades;
        }

        //Проверка полей. Объяснение ошибок
        private static bool CheckFields(string[] fields, int lineNumber)
        {
            if (fields.Length != 3)
            {
                Console.WriteLine($"WARN: Line {lineNumber} malformed. Only {fields.Length} field(s) found.");
                return false;
            }

            if (fields[0].Length != 6)
            {
                Console.WriteLine($"WARN: Trade currencies on line {lineNumber} malformed: {fields[0]} is not suitable.");
                return false;
            }

            return true;
        }
    }
}
