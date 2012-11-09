using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualViewModel;

namespace DynamicViewModelTests
{
    [TestClass]
    public class DefaultValuesTests
    {
        private ViewModel<Model> _viewModel;
        const String ExpectedName = "sam";
        const int ExpectedAge = 10;
        private readonly List<string> _expectedListItems = new List<string>{"a","b","c"};

        [TestInitialize]
        public void Init()
        {
            _viewModel = new ViewModel<Model>(new { Name = ExpectedName, Age = ExpectedAge, List = _expectedListItems });
        }

        [TestMethod]
        public void should_have_default_string_value()
        {
            Assert.AreEqual(ExpectedName, _viewModel["Name"]);
        }

        [TestMethod]
        public void should_have_default_int_value()
        {
            Assert.AreEqual(ExpectedAge, _viewModel["Age"]);
        }

        [TestMethod]
        public void should_have_default_list_values()
        {
            var actualList = _viewModel["List"] as IList;

            Assert.AreEqual(_expectedListItems.Count, actualList.Count);

            foreach(var listItem in _expectedListItems)
            {
                Assert.IsTrue(actualList.Contains(listItem));
            }
        }

    }
}
