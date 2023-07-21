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
        private static ForexData Instance = null;
        private readonly UserInput UserInputObj = null;
        private Dictionary<string, double> ExchangeRatesUSD;

        private ForexData(UserInput userInputObj, Dictionary<string, double> exchangeRatesUSD)
        {
            UserInputObj = userInputObj;
            ExchangeRatesUSD = exchangeRatesUSD;
        }

        private ForexData(UserInput userInputObj)
        {
            UserInputObj = userInputObj;
            ExchangeRatesUSD = null;
        }

        private ForexData()
        {
            UserInputObj = new UserInput();
            ExchangeRatesUSD = null;
        }

        public static ForexData GetInstance(UserInput userInputObj)
        {
            if (Instance == null) 
                Instance = new ForexData(userInputObj);
            
            if (userInputObj != Instance.UserInputObj)
                Instance = new ForexData(userInputObj, Instance.ExchangeRatesUSD);
            
            return Instance;
        }

        public static ForexData GetInstance()
        {
            if (Instance == null)
                Instance = new ForexData();
            
            return Instance;
        }

        public double GetExchangeRate()
        {
            GetExchangeRatesFromAPI();

            if (Instance.ExchangeRatesUSD == null ||
                Instance.ExchangeRatesUSD.Count <= 0 ||
                !Instance.ExchangeRatesUSD.ContainsKey(Instance.UserInputObj.Currency.Key))
            {
                if (!Instance.UserInputObj.Currency.Key.Equals("USD"))
                    Instance.UserInputObj.SetCurrency(new KeyValuePair<string, string>("USD", "United States – Dollar ($) USD"));

                return 1.0;
            }

            return Instance.ExchangeRatesUSD[Instance.UserInputObj.Currency.Key];
        }

        private void GetExchangeRatesFromFile()
        {
            Instance.UserInputObj.LoggerObj.LogInformation("Obtaining forex data from cached file");

            if (!File.Exists(ForexConstants.ForexDataFileName))
            {
                Instance.UserInputObj.LoggerObj.LogWarning($"Cached {ForexConstants.ForexDataFileName} file not found");
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
                Instance.UserInputObj.LoggerObj.LogWarning("Cached exchange rates are more than 30 days old");
                return;
            }

            Instance.ExchangeRatesUSD = forexJSONObj.Rates;
        }

        private void GetExchangeRatesFromAPI()
        {
            if (Instance.ExchangeRatesUSD != null)
                return;

            Instance.UserInputObj.LoggerObj.LogInformation($"Obtaining forex data from API");

            string jsonResponse = "";
            try
            {
                jsonResponse = new HttpClientHelper().GetExchangeRateJsonStringResponse(Instance.UserInputObj).Result;
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
                Instance.UserInputObj.LoggerObj.LogWarning($"Fetching latest prices from API caused an exception: {errorMessage}");
                GetExchangeRatesFromFile();
                return;
            }
            catch (Exception exJsonResponse)
            {
                Instance.UserInputObj.LoggerObj.LogWarning($"Fetching latest prices from API caused an exception: {exJsonResponse.Message}");
                GetExchangeRatesFromFile();
                return;
            }

            ForexJSON forexJSONObj = JsonConvert.DeserializeObject<ForexJSON>(jsonResponse);
            string indentedJsonString = JsonConvert.SerializeObject(forexJSONObj, Formatting.Indented);
            Instance.ExchangeRatesUSD = forexJSONObj.Rates;

            try
            {
                File.WriteAllText(ForexConstants.ForexDataFileName, indentedJsonString);
            }
            catch (Exception exUpdateJsonFile)
            { 
                Instance.UserInputObj.LoggerObj.LogWarning($"Failed to update latest prices to {ForexConstants.ForexDataFileName}: {exUpdateJsonFile.Message}");
            }
        }
    }
}
