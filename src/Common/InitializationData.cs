using System.Collections.Generic;

namespace Azure.Migrate.Export.Common
{
    public static class InitializationData
    {
        public static List<KeyValuePair<string, string>> GetSupportedCurrenciesInitializationData()
        {
            List<KeyValuePair<string, string>> currency = new List<KeyValuePair<string, string>>();
            currency.Add(new KeyValuePair<string, string>("USD", "United States – Dollar ($) USD"));
            currency.Add(new KeyValuePair<string, string>("AUD", "Australia – Dollar ($) AUD"));
            currency.Add(new KeyValuePair<string, string>("BRL", "Brazil – Real (R$) BRL"));
            currency.Add(new KeyValuePair<string, string>("CAD", "Canada – Dollar ($) CAD"));
            currency.Add(new KeyValuePair<string, string>("DKK", "Denmark – Krone (kr) DKK"));
            currency.Add(new KeyValuePair<string, string>("EUR", "Euro Zone – Euro (€) EUR"));
            currency.Add(new KeyValuePair<string, string>("INR", "India – Rupee (₹) INR"));
            currency.Add(new KeyValuePair<string, string>("JPY", "Japan – Yen (¥) JPY"));
            currency.Add(new KeyValuePair<string, string>("KRW", "Korea – Won (₩) KRW"));
            currency.Add(new KeyValuePair<string, string>("NZD", "New Zealand – Dollar ($) NZD"));
            currency.Add(new KeyValuePair<string, string>("NOK", "Norway – Krone (kr) NOK"));
            currency.Add(new KeyValuePair<string, string>("RUB", "Russia – Ruble (руб) RUB"));
            currency.Add(new KeyValuePair<string, string>("SEK", "Sweden – Krona (kr) SEK"));
            currency.Add(new KeyValuePair<string, string>("CHF", "Switzerland – Franc (chf) CHF"));
            currency.Add(new KeyValuePair<string, string>("TWD", "Taiwan – Dollar (NT$) TWD"));
            currency.Add(new KeyValuePair<string, string>("GBP", "United Kingdom – Pound (£) GBP"));

            return currency;
        }

        public static List<KeyValuePair<string, string>> GetSupportedRegionsInitializationData()
        {
            List<KeyValuePair<string, string>> location = new List<KeyValuePair<string, string>>();
            location.Add(new KeyValuePair<string, string>("eastus", "East US"));
            location.Add(new KeyValuePair<string, string>("eastus2", "East US 2"));
            location.Add(new KeyValuePair<string, string>("southcentralus", "South Central US"));
            location.Add(new KeyValuePair<string, string>("westus2", "West US 2"));
            location.Add(new KeyValuePair<string, string>("australiaeast", "Australia East"));
            location.Add(new KeyValuePair<string, string>("southeastasia", "Southeast Asia"));
            location.Add(new KeyValuePair<string, string>("northeurope", "North Europe"));
            location.Add(new KeyValuePair<string, string>("swedencentral", "Sweden Central"));
            location.Add(new KeyValuePair<string, string>("uksouth", "UK South"));
            location.Add(new KeyValuePair<string, string>("westeurope", "West Europe"));
            location.Add(new KeyValuePair<string, string>("centralus", "Central US"));
            location.Add(new KeyValuePair<string, string>("southafricanorth", "South Africa North"));
            location.Add(new KeyValuePair<string, string>("centralindia", "Central India"));
            location.Add(new KeyValuePair<string, string>("eastasia", "East Asia"));
            location.Add(new KeyValuePair<string, string>("japaneast", "Japan East"));
            location.Add(new KeyValuePair<string, string>("koreacentral", "Korea Central"));
            location.Add(new KeyValuePair<string, string>("canadacentral", "Canada Central"));
            location.Add(new KeyValuePair<string, string>("francecentral", "France Central"));
            location.Add(new KeyValuePair<string, string>("germanywestcentral", "Germany West Central"));
            location.Add(new KeyValuePair<string, string>("norwayeast", "Norway East"));
            location.Add(new KeyValuePair<string, string>("switzerlandnorth", "Switzerland North"));
            location.Add(new KeyValuePair<string, string>("uaenorth", "UAE North"));
            location.Add(new KeyValuePair<string, string>("brazilsouth", "Brazil South"));
            location.Add(new KeyValuePair<string, string>("eastus2euap", "East US 2 EUAP"));
            location.Add(new KeyValuePair<string, string>("jioindiawest", "Jio India West"));
            location.Add(new KeyValuePair<string, string>("centraluseuap", "Central US EUAP"));
            location.Add(new KeyValuePair<string, string>("westcentralus", "West Central US"));
            location.Add(new KeyValuePair<string, string>("australiasoutheast", "Australia Southeast"));
            location.Add(new KeyValuePair<string, string>("japanwest", "Japan West"));
            location.Add(new KeyValuePair<string, string>("koreasouth", "Korea South"));
            location.Add(new KeyValuePair<string, string>("southindia", "South India"));
            location.Add(new KeyValuePair<string, string>("ukwest", "UK West"));

            return location;
        }

        public static List<KeyValuePair<string, string>> GetSupportedAssessmentDurationData()
        {
            List<KeyValuePair<string, string>> assessmentDurations = new List<KeyValuePair<string, string>>();
            assessmentDurations.Add(new KeyValuePair<string, string>("day", "Day"));
            assessmentDurations.Add(new KeyValuePair<string, string>("week", "Week"));
            assessmentDurations.Add(new KeyValuePair<string, string>("month", "Month"));

            return assessmentDurations;
        }
    }
}
