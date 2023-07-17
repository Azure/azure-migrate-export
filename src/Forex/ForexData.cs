using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.HttpRequestHelper;
using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Forex
{
    public static class ForexData
    {
        private static Dictionary<string, double> ExchangeRatesUSD;
        private static double ExchangeRate = 1.0;
        private static ExchangeRateStates ExchangeRateState = ExchangeRateStates.Unfetched;

        public static Dictionary<string, double> GetExchangeRatesUSD()
        {
            return ExchangeRatesUSD;
        }

        public static ExchangeRateStates GetExchangeRateState()
        {
            return ExchangeRateState;
        }

        public static double GetExchangeRate()
        {
            return ExchangeRate;
        }

        public static void UpdateExchangeRatesWithUSDFallback()
        {
            List<KeyValuePair<string, string>> currencies = InitializationData.GetSupportedCurrenciesInitializationData();
            ExchangeRatesUSD = new Dictionary<string, double>();

            foreach(var kvp in currencies)
                if (!ExchangeRatesUSD.ContainsKey(kvp.Key))
                    ExchangeRatesUSD.Add(kvp.Key, 1.0);

            ExchangeRateState = ExchangeRateStates.USD;
        }

        private static void GetExchangeRatesFromFile(UserInput userInputObj)
        {
            if (!File.Exists(ForexConstants.ForexDataFileName))
            {
                userInputObj.LoggerObj.LogWarning($"Cached {ForexConstants.ForexDataFileName} not found");
                UpdateExchangeRatesWithUSDFallback();
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
                UpdateExchangeRatesWithUSDFallback();
                return;
            }

            ExchangeRatesUSD = forexJSONObj.Rates;
            ExchangeRateState = ExchangeRateStates.Cached;
        }

        public static void GetExchangeRatesFromAPI(UserInput userInputObj)
        {
            if (ExchangeRateState == ExchangeRateStates.Latest || ExchangeRateState == ExchangeRateStates.InMemory)
            {
                ExchangeRateState = ExchangeRateStates.InMemory;
                return;
            }

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
            ExchangeRateState = ExchangeRateStates.Latest;

            try
            {
                File.WriteAllText(ForexConstants.ForexDataFileName, indentedJsonString);
            }
            catch (Exception exUpdateJsonFile)
            { 
                userInputObj.LoggerObj.LogWarning($"Failed to update latest prices to {ForexConstants.ForexDataFileName}: {exUpdateJsonFile.Message}");
            }
        }

        public static void UpdateExchangeRate(string currencySymbol)
        {
            if (ExchangeRatesUSD.Count <= 0)
            {
                ExchangeRate = 1.0;
                ExchangeRateState = ExchangeRateStates.USD;
                return;
            }

            if (!ExchangeRatesUSD.ContainsKey(currencySymbol))
            {
                ExchangeRate = 1.0;
                ExchangeRateState = ExchangeRateStates.USD;
                return;
            }

            ExchangeRate = ExchangeRatesUSD[currencySymbol];
        }
    }
}
