This project will take care of all of the WPF binding notification wizardry that you normally have to deal with.
All you have to do is instantiate a VirtualViewModel object like so:

	var vm = new VirtualViewModel<Model>(); 
	
Another added bonus is that you can set up behavior on properties. 

For example, if you have a Person class with Name and Age properties, you can do something like this:
	vm.When(x => x.Name, "Henry").Set(x => x.Age, 33);
	
The VirtualViewModel class extends from DynamicObject, thus if you treat it as a dynamic, you can modify and read properties that are defined in the underlying model.
	dynamic vm = new VirtualViewModel<Person>();
	vm.Age = 22;