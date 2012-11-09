using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualViewModel;

namespace DynamicViewModelTests
{
    [TestClass]
    public class Extraction
    {
        private ViewModel<Model> _viewModel;
        const String ExpectedName = "sam";
        const int ExpectedAge = 10;
        private dynamic _extractedModel;
        private readonly List<string> _expectedListItems = new List<string> { "a", "b", "c" };

        [TestInitialize]
        public void init()
        {
            _viewModel = new ViewModel<Model>(new { Name = ExpectedName, Age = ExpectedAge, List = _expectedListItems });
            _extractedModel = _viewModel.GetModel();
        }

        [TestMethod]
        public void should_return_a_model_of_correct_type()
        {
            Assert.IsTrue(_extractedModel is Model);
        }

        [TestMethod]
        public void should_return_string_value()
        {
            Assert.AreEqual(ExpectedName, _extractedModel.Name);
        }

        [TestMethod]
        public void should_return_int_value()
        {
            Assert.AreEqual(ExpectedAge, _extractedModel.Age);
        }

        [TestMethod]
        public void should_return_list_values()
        {
            List<String> actualList = _extractedModel.List;
            Assert.AreEqual(_expectedListItems.Count, actualList.Count);

            foreach (var listItem in _expectedListItems)
            {
                Assert.IsTrue(actualList.Contains(listItem));
            }
        }


    }
}
