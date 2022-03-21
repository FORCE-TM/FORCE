namespace FORCE.Core.Plugins.Commands.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CommandGroupAttribute : Attribute
{
    public string Prefix { get; }
    public bool Admin { get; }

    public CommandGroupAttribute(string prefix, bool admin)
    {
        Prefix = prefix;
        Admin = admin;
    }
}
