using System.Reflection;
using FORCE.Core.Extensions;
using FORCE.Core.Plugins.Attributes;
using FORCE.Core.Plugins.Commands.Attributes;

namespace FORCE.Core.Plugins.Commands.Models;

internal class CommandParameterInfo
{
    public Type Type { get; }
    public string Name { get; }
    public object DefaultValue { get; }
    public bool HasDefaultValue { get; }

    public string Summary { get; }
    public bool IsRemainder { get; }

    public CommandParameterInfo(ParameterInfo parameterInfo)
    {
        Type = parameterInfo.ParameterType;
        Name = parameterInfo.Name;
        DefaultValue = parameterInfo.DefaultValue;
        HasDefaultValue = parameterInfo.HasDefaultValue;

        if (parameterInfo.TryGetCustomAttribute<SummaryAttribute>(out var summaryAttribute))
            Summary = summaryAttribute.Text;

        if (parameterInfo.TryGetCustomAttribute<RemainderAttribute>(out _))
            IsRemainder = true;

        if (Type != typeof(string))
        {
            throw new($"At the moment, only parameters of type {typeof(string)} are supported in command methods.");
        }
    }
}
