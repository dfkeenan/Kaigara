namespace Kaigara.ToolBars
{
    public class ToolBarItemDefinition : UIComponentItemDefinition<ToolBarItemDefinition>, IUIComponentDefinition
    {
        public ToolBarItemDefinition(string name, string? label = null, string? iconName = null) 
            : base(name, label, iconName)
        {
        }

        IEnumerable<IUIComponentDefinition> IUIComponentDefinition.Items => Enumerable.Empty<IUIComponentDefinition>();

        void IUIComponentDefinition.OnParentDefined(IUIComponentDefinition parent)
            => ((ToolBarDefinition)parent).Add(this);

        internal IToolBarItemViewModel Build()
            => new DefinedToolBarItemViewModel(this);
    }
}
