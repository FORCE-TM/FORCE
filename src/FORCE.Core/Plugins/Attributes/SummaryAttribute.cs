namespace FORCE.Core.Plugins.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
public class SummaryAttribute : Attribute
{
    public string Text { get; }

    public SummaryAttribute(string text)
    {
        Text = text;
    }
}
