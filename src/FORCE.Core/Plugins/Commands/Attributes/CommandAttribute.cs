namespace FORCE.Core.Plugins.Commands.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CommandAttribute : Attribute
{
    public string Name { get; }
    public bool Admin { get; }

    public CommandAttribute(string name, bool admin = false)
    {
        Name = name;
        Admin = admin;
    }
}
