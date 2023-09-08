using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.HttpRequestHelper;
using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Assessment.Parser
{
    public class BusinessCaseParser
    {
        private readonly KeyValuePair<BusinessCaseInformation, AssessmentPollResponse> BizCaseCompletionResult;

        public BusinessCaseParser(KeyValuePair<BusinessCaseInformation, AssessmentPollResponse> bizCaseCompletionResult)
        {
            BizCaseCompletionResult = bizCaseCompletionResult;
        }

        public void ParseBusinessCase(UserInput userInputObj, BusinessCaseDataset BusinessCaseData)
        {
            if (userInputObj == null)
                throw new Exception("Received null user input object");

            string commonUrl = Routes.ProtocolScheme + Routes.AzureManagementApiHostname + Routes.ForwardSlash +
                               Routes.SubscriptionPath + Routes.ForwardSlash + userInputObj.Subscription.Key + Routes.ForwardSlash +
                               Routes.ResourceGroupPath + Routes.ForwardSlash + userInputObj.ResourceGroupName.Value + Routes.ForwardSlash +
                               Routes.ProvidersPath + Routes.ForwardSlash + Routes.MigrateProvidersPath + Routes.ForwardSlash +
                               Routes.AssessmentProjectsPath + Routes.ForwardSlash + userInputObj.AssessmentProjectName + Routes.ForwardSlash +
                               Routes.BusinessCasesPath + Routes.ForwardSlash + BizCaseCompletionResult.Key.BusinessCaseName + Routes.ForwardSlash +
                               "{BusinessCaseSummariesPath}" + Routes.QueryStringQuestionMark +
                               Routes.QueryParameterApiVersion + Routes.QueryStringEquals + Routes.BusinessCaseApiVersion;

            string compareSummaryUrl = commonUrl.Replace("{BusinessCaseSummariesPath}", Routes.BusinessCaseCompareSummariesPath);
            BusinessCaseCompareSummaryJSON bizCaseCompareSummaryJsonObj = ParseBusinessCaseCompareSummary(compareSummaryUrl, userInputObj);

            string overviewSummariesUrl = commonUrl.Replace("{BusinessCaseSummariesPath}", Routes.BusinessCaseOverviewSummariesPath + Routes.ForwardSlash + Routes.DefaultPath);
            BusinessCaseOverviewSummaryJSON bizCaseOverviewSummariesJsonObj = ParseBusinessCaseOverviewSummaries(overviewSummariesUrl, userInputObj);

            string iaasSummariesUrl = commonUrl.Replace("{BusinessCaseSummariesPath}", Routes.BusinessCaseIaaSSummariesPath + Routes.ForwardSlash + Routes.DefaultPath);
            BusinessCaseIaaSSummaryJSON bizCaseIaaSSummariesJsonObj = ParseBusinessCaseIaaSSummaries(iaasSummariesUrl, userInputObj);

            string paasSummariesUrl = commonUrl.Replace("{BusinessCaseSummariesPath}", Routes.BusinessCasePaaSSummariesPath + Routes.ForwardSlash + Routes.DefaultPath);
            BusinessCasePaaSSummaryJSON bizCasePaaSSummariesJsonObj = ParseBusinessCasePaaSSummaries(paasSummariesUrl, userInputObj);

            UpdateBusinessCaseDataset(bizCaseCompareSummaryJsonObj,
                                      bizCaseOverviewSummariesJsonObj,
                                      bizCaseIaaSSummariesJsonObj,
                                      bizCasePaaSSummariesJsonObj,
                                      BusinessCaseData,
                                      userInputObj);
        }

        private void UpdateBusinessCaseDataset(BusinessCaseCompareSummaryJSON bizCaseCompareSummaryJsonObj,
                                               BusinessCaseOverviewSummaryJSON bizCaseOverviewSummariesJsonObj,
                                               BusinessCaseIaaSSummaryJSON bizCaseIaaSSummariesJsonObj,
                                               BusinessCasePaaSSummaryJSON bizCasePaaSSummariesJsonObj,
                                               BusinessCaseDataset BusinessCaseData,
                                               UserInput userInputObj)
        {
            if (bizCaseCompareSummaryJsonObj == null ||
                bizCaseOverviewSummariesJsonObj == null ||
                bizCaseIaaSSummariesJsonObj == null ||
                bizCasePaaSSummariesJsonObj == null)
            {
                userInputObj.LoggerObj.LogWarning("Business case information not parsed successfully, dataset may not be complete");
                return;
            }

            BusinessCaseData.OnPremIaaSCostDetails.ComputeLicenseCost = bizCaseCompareSummaryJsonObj.OnPremisesIaaSCostDetails.ComputeCost;
            BusinessCaseData.OnPremIaaSCostDetails.EsuLicenseCost = bizCaseCompareSummaryJsonObj.OnPremisesIaaSCostDetails.ESUSavings;
            BusinessCaseData.OnPremIaaSCostDetails.StorageCost = bizCaseCompareSummaryJsonObj.OnPremisesIaaSCostDetails.StorageCost;
            BusinessCaseData.OnPremIaaSCostDetails.NetworkCost = bizCaseCompareSummaryJsonObj.OnPremisesIaaSCostDetails.NetworkCost;
            BusinessCaseData.OnPremIaaSCostDetails.SecurityCost = bizCaseCompareSummaryJsonObj.OnPremisesIaaSCostDetails.SecurityCost;
            BusinessCaseData.OnPremIaaSCostDetails.ITStaffCost = bizCaseCompareSummaryJsonObj.OnPremisesIaaSCostDetails.ITLaborCost;
            BusinessCaseData.OnPremIaaSCostDetails.FacilitiesCost = 0.0;

            BusinessCaseData.OnPremPaaSCostDetails.ComputeLicenseCost = bizCaseCompareSummaryJsonObj.OnPremisesPaaSCostDetails.ComputeCost;
            BusinessCaseData.OnPremPaaSCostDetails.EsuLicenseCost = bizCaseCompareSummaryJsonObj.OnPremisesPaaSCostDetails.ESUSavings;
            BusinessCaseData.OnPremPaaSCostDetails.StorageCost = bizCaseCompareSummaryJsonObj.OnPremisesPaaSCostDetails.StorageCost;
            BusinessCaseData.OnPremPaaSCostDetails.NetworkCost = bizCaseCompareSummaryJsonObj.OnPremisesPaaSCostDetails.NetworkCost;
            BusinessCaseData.OnPremPaaSCostDetails.SecurityCost = bizCaseCompareSummaryJsonObj.OnPremisesPaaSCostDetails.SecurityCost;
            BusinessCaseData.OnPremPaaSCostDetails.ITStaffCost = bizCaseCompareSummaryJsonObj.OnPremisesPaaSCostDetails.ITLaborCost;
            BusinessCaseData.OnPremPaaSCostDetails.FacilitiesCost = 0.0;

            BusinessCaseData.AzureIaaSCostDetails.ITStaffCost = bizCaseCompareSummaryJsonObj.AzureIaaSCostDetails.ITLaborCost;
            BusinessCaseData.AzurePaaSCostDetails.ITStaffCost = bizCaseCompareSummaryJsonObj.AzurePaaSCostDetails.ITLaborCost;

            BusinessCaseData.TotalYOYCashFlows = bizCaseOverviewSummariesJsonObj.YearOnYearEstimates;
            BusinessCaseData.IaaSYOYCashFlows = bizCaseIaaSSummariesJsonObj.AzureIaaSSummary.YearOnYearEstimates;
        }

        private BusinessCasePaaSSummaryJSON ParseBusinessCasePaaSSummaries(string url, UserInput userInputObj)
        {
            string response = GetJsonResponse(url, userInputObj);

            if (string.IsNullOrEmpty(response))
            {
                userInputObj.LoggerObj.LogWarning($"Received empty response for business case url: {url}");
                return null;
            }

            return JsonConvert.DeserializeObject<BusinessCasePaaSSummaryJSON>(response);
        }

        private BusinessCaseIaaSSummaryJSON ParseBusinessCaseIaaSSummaries(string url, UserInput userInputObj)
        {
            string response = GetJsonResponse(url, userInputObj);

            if (string.IsNullOrEmpty(response))
            {
                userInputObj.LoggerObj.LogWarning($"Received empty response for business case url: {url}");
                return null;
            }

            return JsonConvert.DeserializeObject<BusinessCaseIaaSSummaryJSON>(response);
        }

        private BusinessCaseOverviewSummaryJSON ParseBusinessCaseOverviewSummaries(string url, UserInput userInputObj)
        {
            string response = GetJsonResponse(url, userInputObj);

            if (string.IsNullOrEmpty(response))
            {
                userInputObj.LoggerObj.LogWarning($"Received empty response for business case url: {url}");
                return null;
            }

            return JsonConvert.DeserializeObject<BusinessCaseOverviewSummaryJSON>(response);
        }

        private BusinessCaseCompareSummaryJSON ParseBusinessCaseCompareSummary(string url, UserInput userInputObj)
        {
            string response = GetJsonResponse(url, userInputObj, true);

            if (string.IsNullOrEmpty(response))
            {
                userInputObj.LoggerObj.LogWarning($"Received empty response for business case url: {url}");
                return null;
            }

            return JsonConvert.DeserializeObject<BusinessCaseCompareSummaryJSON>(response);
        }   

        private string GetJsonResponse(string url, UserInput userInputObj, bool isPost = false)
        {
            string response = "";
            try
            {
                response = new HttpClientHelper().GetHttpRequestJsonStringResponse(url, userInputObj, isPost).Result;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (AggregateException aeBusinessCase)
            {
                string errorMessage = "";
                foreach (var e in aeBusinessCase.Flatten().InnerExceptions)
                {
                    if (e is OperationCanceledException)
                        throw e;
                    else
                    {
                        errorMessage = errorMessage + e.Message + " ";
                    }
                }
                userInputObj.LoggerObj.LogWarning($"Failed parsing business case url {url}: {errorMessage}");
            }
            catch (Exception exBusinessCase)
            {
                userInputObj.LoggerObj.LogWarning($"Failed parsing business case url {url}: {exBusinessCase.Message}");
            }

            return response;
        }
    }
}