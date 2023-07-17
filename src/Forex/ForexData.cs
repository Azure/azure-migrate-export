using System;
using System.IO;
using Newtonsoft.Json;

using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.HttpRequestHelper;
using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Forex
{
    public class ForexData
    {
        public double GetExchangeRate(UserInput userInputObj)
        {
            userInputObj.LoggerObj.LogInformation($"Obtaining exchange rate for currency {userInputObj.Currency.Value}");

            if (File.Exists(ForexConstants.ForexDataFileName))
                return GetExchangeRateFromFile(userInputObj);

            return GetExchangeRateFromAPI(userInputObj);
        }

        private double GetExchangeRateFromFile(UserInput userInputObj)
        {
            string forexDataFileText = File.ReadAllText(ForexConstants.ForexDataFileName);
            ForexJSON forexJSONObj = JsonConvert.DeserializeObject<ForexJSON>(forexDataFileText);

            // Check for age of file
            DateTime forexDataDate = DateTime.ParseExact(forexJSONObj.Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime currentDate = DateTime.UtcNow;
            double difference = (currentDate - forexDataDate).TotalDays;

            if (difference >= 1.0)
            {
                userInputObj.LoggerObj.LogInformation($"Cached {ForexConstants.ForexDataFileName} is more than a day old");
                return GetExchangeRateFromAPI(userInputObj, forexJSONObj.Rates[userInputObj.Currency.Key], difference);
            }

            userInputObj.LoggerObj.LogInformation($"Obtaining exchange rate from cached {ForexConstants.ForexDataFileName}");
            return forexJSONObj.Rates[userInputObj.Currency.Key];
        }

        private double GetExchangeRateFromAPI(UserInput userInputObj, double fallbackExchangeRateFromFile = 1.0, double dayDifference = 30.0)
        {
            userInputObj.LoggerObj.LogInformation($"Trying {Routes.ForexApi} API for obtaining latest forex data");
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
                string message = dayDifference >= 30.0 ? "Using USD for some prices" : "Using cached exchange rate";
                userInputObj.LoggerObj.LogError($"{message} as an error occurred trying to obtain exchange rates' data: {errorMessage}");
                return dayDifference >= 30.0 ? 1.0 : fallbackExchangeRateFromFile;
            }
            catch (Exception exJsonResponse)
            {
                string message = dayDifference >= 30.0 ? "Using USD for some prices" : "Using cached exchange rate";
                userInputObj.LoggerObj.LogError($"{message} as an error occurred trying to obtain exchange rates' data: {exJsonResponse.Message}");
                return dayDifference >= 30.0 ? 1.0 : fallbackExchangeRateFromFile; ;
            }

            ForexJSON forexJSONObj = JsonConvert.DeserializeObject<ForexJSON>(jsonResponse);
            string indentedJsonString = JsonConvert.SerializeObject(forexJSONObj, Formatting.Indented);
            File.WriteAllText(ForexConstants.ForexDataFileName, indentedJsonString);
            userInputObj.LoggerObj.LogInformation($"Updated cache {ForexConstants.ForexDataFileName} with latest forex data");

            return forexJSONObj.Rates[userInputObj.Currency.Key];
        }
    }
}
