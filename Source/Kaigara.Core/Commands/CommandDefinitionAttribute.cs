﻿namespace Kaigara.Commands;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class CommandDefinitionAttribute : Attribute
{
    public CommandDefinitionAttribute(string label)
    {
        Label = label;
    }

    public string Label { get; private set; }
    public string? IconName { get; set; }
    public string? Name { get; set; }
    public string? DefaultInputGesture { get; set; }
}
