using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;

namespace No7.Solution
{
    //класс для вызова основных методов
    public class TradeHandler
    {
        public void HandleTrades(Stream stream)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

            var lines = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            var trades = TradeCreator.CreateTradeList(lines); //массив объектов trade

            SaveInDataBase(trades);
        }

        private void SaveInDataBase(List<TradeRecord> trades)
        {
            // save into database
            string connectionString = ConfigurationManager.ConnectionStrings["TradeData"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var trade in trades)
                    {
                        var command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "dbo.Insert_Trade";
                        command.Parameters.AddWithValue("@sourceCurrency", trade.SourceCurrency);
                        command.Parameters.AddWithValue("@destinationCurrency", trade.DestinationCurrency);
                        command.Parameters.AddWithValue("@lots", trade.Lots);
                        command.Parameters.AddWithValue("@price", trade.Price);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                connection.Close();
            }
        }
    }
}
