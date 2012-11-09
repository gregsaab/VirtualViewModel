using System.Collections.Generic;
using Machine.Specifications;
using VirtualViewModel;

namespace DynamicViewModelTests
{
    [Subject(typeof(ViewModel<>))]
    public class When_property_setting_behavior_is_added_to_a_string_property : WithViewModel<Model>
    {
        Establish context = () =>
        {
            Subject.PropertyChanged += (e, s) => _updatedProperties.Add(s.PropertyName);
            Subject.When(x => x.Name, "Cliff").Set(x => x.Age, 55);
        };

        Because of = () => DynamicSubject.Name = "Cliff";

        It should_update_the_property_in_behavior = () => ((int)DynamicSubject.Age).ShouldEqual(55);
        It should_notify_that_original_property_updated = () => _updatedProperties.ShouldContain("Name");
        It should_notify_that_behavior_property_updated = () => _updatedProperties.ShouldContain("Age");

        static List<string> _updatedProperties = new List<string>();
    }
}
