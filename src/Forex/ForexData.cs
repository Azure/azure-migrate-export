using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.HttpRequestHelper;
using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Forex
{
    public class ForexData
    {
        private static Dictionary<string, double> ExchangeRatesUSD = null;

        public static double GetExchangeRate(UserInput userInputObj)
        {
            GetExchangeRatesFromAPI(userInputObj);

            if (ExchangeRatesUSD == null ||
                ExchangeRatesUSD.Count <= 0 ||
                !ExchangeRatesUSD.ContainsKey(userInputObj.Currency.Key))
            {
                userInputObj.SetCurrency(new KeyValuePair<string, string>("USD", "United States – Dollar ($) USD"));
                return 1.0;
            }

            return ExchangeRatesUSD[userInputObj.Currency.Key];
        }

        private static void GetExchangeRatesFromFile(UserInput userInputObj)
        {
            userInputObj.LoggerObj.LogInformation("Obtaining forex data from cached file");

            if (!File.Exists(ForexConstants.ForexDataFileName))
            {
                userInputObj.LoggerObj.LogWarning($"Cached {ForexConstants.ForexDataFileName} file not found");
                return;
            }

            string forexDataFileText = File.ReadAllText(ForexConstants.ForexDataFileName);
            ForexJSON forexJSONObj = JsonConvert.DeserializeObject<ForexJSON>(forexDataFileText);

            // Check for age of file
            DateTime forexDataDate = DateTime.ParseExact(forexJSONObj.Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime currentDate = DateTime.UtcNow;
            double difference = (currentDate - forexDataDate).TotalDays;

            if (difference >= 30.0)
            {
                userInputObj.LoggerObj.LogWarning("Cached exchange rates are more than 30 days old");
                return;
            }

            ExchangeRatesUSD = forexJSONObj.Rates;
        }

        private static void GetExchangeRatesFromAPI(UserInput userInputObj)
        {
            if (ExchangeRatesUSD != null)
                return;

            userInputObj.LoggerObj.LogInformation($"Obtaining forex data from API");

            string jsonResponse = "";
            try
            {
                jsonResponse = new HttpClientHelper().GetExchangeRateJsonStringResponse(userInputObj).Result;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (AggregateException aeForexData)
            {
                string errorMessage = "";
                foreach (var e in aeForexData.Flatten().InnerExceptions)
                {
                    if (e is OperationCanceledException)
                        throw e;
                    else
                    {
                        errorMessage = errorMessage + e.Message + " ";
                    }
                }
                userInputObj.LoggerObj.LogWarning($"Fetching latest prices from API caused an exception: {errorMessage}");
                GetExchangeRatesFromFile(userInputObj);
                return;
            }
            catch (Exception exJsonResponse)
            {
                userInputObj.LoggerObj.LogWarning($"Fetching latest prices from API caused an exception: {exJsonResponse.Message}");
                GetExchangeRatesFromFile(userInputObj);
                return;
            }

            ForexJSON forexJSONObj = JsonConvert.DeserializeObject<ForexJSON>(jsonResponse);
            string indentedJsonString = JsonConvert.SerializeObject(forexJSONObj, Formatting.Indented);
            ExchangeRatesUSD = forexJSONObj.Rates;

            try
            {
                File.WriteAllText(ForexConstants.ForexDataFileName, indentedJsonString);
            }
            catch (Exception exUpdateJsonFile)
            { 
                userInputObj.LoggerObj.LogWarning($"Failed to update latest prices to {ForexConstants.ForexDataFileName}: {exUpdateJsonFile.Message}");
            }
        }
    }
}
