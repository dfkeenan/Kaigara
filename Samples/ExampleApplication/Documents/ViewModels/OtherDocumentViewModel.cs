using System.Collections.ObjectModel;
using System.ComponentModel;
using Dock.Model.ReactiveUI.Controls;

namespace ExampleApplication.Documents.ViewModels;

public class OtherDocumentViewModel : Document
{
    public ObservableCollection<InspectorTestObject> Items { get; }
    public OtherDocumentViewModel()
    {
        Id = Guid.NewGuid().ToString();
        Title = "Other Document";

        Items = new ObservableCollection<InspectorTestObject>()
        {
            new InspectorTestObject()
            {
                TestObjects = new List<InspectorTestObject>()
                {
                    new InspectorTestObject(),
                }
            }
        };
    }

    public class InspectorTestObject
    {
        public string TestProperty { get; set; }

        //[NumericRange(0, 10.0, 0.5d)]
        public double DoubleTrouble { get; set; }

        public int Integer { get; set; }

        [Category("Enums")]
        public TestEnum EnumProperty { get; set; }

        [Category("Enums")]

        [DisplayName("Flags Yo!")]
        public TestFlagsEnum FlagsEnumProperty { get; set; }
        [Category("Lists")]

        public List<InspectorTestObject> TestObjects { get; set; }
        [Category("Lists")]

        public List<int> TestNumbers { get; set; }  
    }





    public enum TestEnum
    {
        First,
        Second,
        Third
    }

    [Flags]
    public enum TestFlagsEnum
    {
        FirstFlag = 1,
        SecondFlag = 2,
        ThirdFlag = 4
    }
}
