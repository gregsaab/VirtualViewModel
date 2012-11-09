using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualViewModel;

namespace DynamicViewModelTests
{
    [TestClass]
    public class Behavior
    {
        private ViewModel<Model> _viewModel;
        const String ExpectedName = "sam";
        const int ExpectedAge = 10;
        private readonly List<string> _expectedListItems = new List<string> { "a", "b", "c" };

        [TestInitialize]
        public void init()
        {
            _viewModel = new ViewModel<Model>(new { Name = ExpectedName, Age = ExpectedAge, List = _expectedListItems });
        }

        [TestMethod]
        public void can_setup_when_on_string_to_update_int()
        {
            var expectedAge = 5;
            _viewModel.When(x=>x.Name,"string").Set(x=>x.Age,expectedAge);

            ((dynamic)_viewModel).Name = "string";

            Assert.AreEqual(expectedAge, ((dynamic)_viewModel).Age);
        }
    }
}
