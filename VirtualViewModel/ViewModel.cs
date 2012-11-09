using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace VirtualViewModel
{
    public class ViewModel<TModel> : DynamicObject, INotifyPropertyChanged where TModel:class 
    {
        private readonly Dictionary<string, object> _properties;
        private readonly Dictionary<string, List<object>> _propertyUpdateBindings;
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var name = binder.Name;

            this[name] = value;
            return true;
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            if (_properties.ContainsKey(binder.Name))
                result = _properties[binder.Name];

            return (result != null);
        }
      
        public object this[string name]
        {
            get { return _properties[name]; }
            set
            {
                _properties[name] = value; 
                UpdateProperty(name);

                if (!_propertyUpdateBindings.ContainsKey(name))
                    return;

                var bindingsDependingOn = _propertyUpdateBindings[name];
                if (bindingsDependingOn == null)
                    return;

                foreach(var binding in bindingsDependingOn)
                {
                    var action = binding as Action<object>;
                    action(value);
                }
            }
        }
        
        public ViewModel(object defaultObject = null)
        {
            _properties = new Dictionary<string, object>();
            _propertyUpdateBindings = new Dictionary<string, List<object>>();

            var type = typeof (TModel);


            // keep demeter somewhat happy
            var properties = type.GetProperties();
            var defaultProperties = (defaultObject != null) ?
                defaultObject.GetType().GetProperties() : null;

            // iterate through properties of the type
            foreach(var property in properties)
            {
                var propertyName = property.Name;
                var propertyType = property.PropertyType;

                // variable to hold the value that is applied to the viewmodel's property
                object typeInstance = null;

                var defaultConstructorExists = DoesDefaultConstructorExist(propertyType);

                // if we have a default value for this property
                if (defaultProperties != null && defaultProperties.Any(x=>x.Name.Equals(propertyName) && x.PropertyType.Equals(propertyType)))
                {
                    // grab the value of that property in the defaults
                    typeInstance = defaultProperties.First(x=>x.Name.Equals(propertyName)).GetValue(defaultObject,null);
                }

                else if (typeof(IList).IsAssignableFrom(propertyType))
                {
                    var itemType = GetItemTypeOfList(propertyType);

                    var observableCollectionType = typeof(ObservableCollection<>).MakeGenericType(new[] { itemType });

                    typeInstance = Activator.CreateInstance(observableCollectionType);
                }
        
                // when we dont have a default value or there is a () ctor
                else if (propertyType.IsValueType || defaultConstructorExists)
                {
                    typeInstance = Activator.CreateInstance(propertyType);
                }
                else if(propertyType == typeof(String))
                {
                    typeInstance = string.Empty;
                }

                
                // set the viewmodels value for the property
                this[propertyName] = typeInstance;
                
            }
        }

        public WhenExpression<TModel, TProperty> When<TProperty>(Expression<Func<TModel, TProperty>> func, TProperty value)
        {

            var memberExpression = func.Body as MemberExpression;
            var propertyName = memberExpression.Member.Name;

            var ret = new WhenExpression<TModel, TProperty>(propertyName, this, value);

            return ret;

        }

        public void AddUpdateAction<TWhenProperty, TSetProperty>(string whenProperty, TWhenProperty whenValue, string setProperty, TSetProperty setValue)
        {
            Action<object> action = (value) =>
            {
                if (!value.Equals(whenValue))
                    return;

                this[setProperty] = setValue;
            };

            if (!_propertyUpdateBindings.ContainsKey(whenProperty))
                _propertyUpdateBindings[whenProperty] = new List<object>();


            _propertyUpdateBindings[whenProperty].Add(action);
        }

        public TModel GetModel()
        {
            var ret = Activator.CreateInstance<TModel>();
            foreach (var property in ret.GetType().GetProperties())
            {
                var propertyName = property.Name;
                if (!_properties.ContainsKey(propertyName))
                    continue;

                property.SetValue(ret, _properties[propertyName], null);
            }

            return ret;
        }

        private Type GetItemTypeOfList(Type type )
        {
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition()
                    == typeof(IList<>))
                {
                    Type itemType = type.GetGenericArguments()[0];
                    return itemType;
                    break;
                }
            }
            throw new ArgumentException();
        } 

        private bool DoesDefaultConstructorExist(Type propertyType)
        {
            return propertyType.GetConstructors().Any(x => x.GetParameters().Count() == 0);   
        }

        private void UpdateProperty(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
