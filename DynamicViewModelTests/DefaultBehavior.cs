using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualViewModel;

namespace DynamicViewModelTests
{
    
    [TestClass]
    public class DefaultBehavior
    {
        private dynamic _viewModel;
        private HashSet<string> _updatedValues;

        [TestInitialize]
        public void init()
        {
            _updatedValues = new HashSet<string>();
            _viewModel = new ViewModel<Model>();
            ((ViewModel<Model>) _viewModel).PropertyChanged += (o, e) => _updatedValues.Add(e.PropertyName);
        }

        [TestMethod]
        public void should_have_string_property()
        {
            Assert.IsNotNull(_viewModel.Name);
        }

        [TestMethod]
        public void should_have_int_property()
        {
            Assert.IsNotNull(_viewModel.Age);
        }

        [TestMethod]
        public void should_have_list_property()
        {
            Assert.IsNotNull(_viewModel.List);
        }

        [TestMethod]
        public void the_list_should_be_made_into_an_observable_collection()
        {

            var listProperty = _viewModel.List;
            Assert.IsTrue(typeof(ObservableCollection<string>) == listProperty.GetType());
        }

        [TestMethod]
        public void should_notify_when_a_property_has_changed_in_view_model()
        {

            _viewModel.Name = "theodore";
            Assert.IsTrue(_updatedValues.Contains("Name"));
        }
    }
}
