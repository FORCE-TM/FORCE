using System.Reflection;
using FORCE.Core.Models;
using FORCE.Core.Plugin;

namespace FORCE.Core.Builders;

internal class CommandParameterBuilder : ISummaryAttribute, IRemainderAttribute
{
    private readonly ParameterInfo _parameter;
    private readonly string _parameterPath;
    private readonly int _implParameterCount;

    public string? Summary { get; private set; }
    public bool IsRemainder { get; private set; }

    public CommandParameterBuilder(ParameterInfo parameter)
    {
        _parameterPath = $"Parameter: {_parameter!.Name} in {_parameter.Member.DeclaringType!.Name}.{_parameter.Member.Name}";

        if (_parameter.ParameterType != typeof(string))
            throw new InvalidOperationException($"At the moment, only parameters of type {typeof(string)} are supported in commands. {_parameterPath}");

        _parameter = parameter;
        _implParameterCount = ((MethodInfo)_parameter.Member).GetParameters().Length;
    }

    public CommandParameterBuilder WithSummary(ISummaryAttribute summary)
    {
        Summary = summary.Summary;
        return this;
    }

    public CommandParameterBuilder WithRemainder(IRemainderAttribute remainder)
    {
        if (remainder.IsRemainder)
        {
            if (_parameter.Position == _implParameterCount - 1)
                throw new InvalidOperationException($"{nameof(RemainderAttribute)} can only be used on the last parameter of a command. {_parameterPath}");

            if (_parameter.ParameterType != typeof(string))
                throw new InvalidOperationException($"{nameof(RemainderAttribute)} can only be used on parameters of type {typeof(string)}. {_parameterPath}");
        }

        IsRemainder = remainder.IsRemainder;
        return this;
    }

    public CommandParameterInfo Build() => new()
    {
        Type = _parameter.ParameterType,
        Name = _parameter.Name!,
        DefaultValue = _parameter.DefaultValue,
        HasDefaultValue = _parameter.HasDefaultValue,
        Summary = Summary,
        IsRemainder = IsRemainder
    };
}
