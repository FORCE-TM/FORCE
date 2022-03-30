﻿namespace FORCE.Core.Plugin.Models;

internal class PluginInfo : PluginDisplayInfo
{
    public List<CommandInfo> Commands { get; set; }
    public ClassInfo Class { get; set; } = null!;
}
