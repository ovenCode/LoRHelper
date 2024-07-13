using System;

namespace desktop.misc
{
    /// <summary>
    /// Class describing an attribute designed to inform users whether a program element has not been implemented yet, and the degree of it
    /// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class NotImplementedAttribute : Attribute
	{
		string? Reason;
        ElementSeverity? Severity;

        public NotImplementedAttribute(string? reasonInfo = null, string? elementSeverity = null)
        {
            Reason = reasonInfo;
            Severity = elementSeverity == "Low" ? ElementSeverity.Low : elementSeverity == "Medium" ? ElementSeverity.Medium : ElementSeverity.High;
        }

        public string? GetReason() => Reason;
        public ElementSeverity? GetSeverity() => Severity;
    }

    public enum ElementSeverity
    {
        Low,
        Medium,
        High
    }
}