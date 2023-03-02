using System;
using System.ComponentModel;
using System.Reflection;

namespace Azure.Migrate.Export.Common
{
    public enum GroupPollResponse
    {
        NotCompleted,
        Completed,
        Invalid,
        Error
    }

    public enum AssessmentPollResponse
    {
        [Description("Not Created")]
        NotCreated,

        [Description("Created")]
        Created,

        [Description("In Progress")]
        InProgress,

        [Description("Not Completed")]
        NotCompleted,

        [Description("Completed")]
        Completed,

        [Description("Out of Date")]
        OutDated,

        [Description("Invalid")]
        Invalid,

        [Description("Error")]
        Error
    }

    public enum EnvironmentType
    {
        Dev,
        Prod
    }

    public enum DiscoverySites
    {
        VMWare,
        Physical,
        HyperV
    }

    public enum AssessmentType
    {
        [Description("assessments")]
        MachineAssessment,

        [Description("avsAssessments")]
        AVSAssessment,

        [Description("sqlAssessments")]
        SQLAssessment,

        [Description("webAppAssessments")]
        WebAppAssessment
    }

    public enum AssessmentSizingCriterion
    {
        AsOnPremises,
        PerformanceBased
    }

    public enum AssessmentTag
    {
        [Description("As-is")]
        AsOnPremises,

        [Description("Pay-as-you-go")]
        PerformanceBased,

        [Description("Pay-as-you-go + RI1Year")]
        PerformanceBased_RI1year,

        [Description("Pay-as-you-go + RI3Year")]
        PerformanceBased_RI3year,

        [Description("Pay-as-you-go + AHUB")]
        PerformanceBased_AHUB,

        [Description("Pay-as-you-go + AHUB + RI3Year")]
        PerformanceBased_AHUB_RI3year,

        [Description("Pay-as-you-go + ASP3Year")]
        PerformanceBased_ASP3year
    }

    public enum RecommendedDiskTypes
    {
        Unknown,
        Standard,
        StandardSSD,
        Premium,
        StandardOrPremium,
        Ultra
    }

    public enum Suitabilities
    {
        [Description("Unknown")]
        Unknown,

        [Description("Ready")]
        Suitable,

        [Description("Ready With Conditions")]
        ConditionallySuitable,

        [Description("Not Ready")]
        NotSuitable,

        [Description("Readiness Unknown")]
        ReadinessUnknown
    }

    public enum AzureSQLTargetType
    {
        Unknown,
        Recommended,
        AzureSqlManagedInstance,
        AzureSqlVirtualMachine,
        AzureSqlDatabase,
        AzureVirtualMachine
    }

    public enum IssueCategories
    {
        Unknown,
        Info,
        Warning,
        Issue,
        Internal
    }

    public class EnumDescriptionHelper
    {
        public string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));

            return attribute != null
                ? attribute.Description
                : value.ToString();
        }
    }
}