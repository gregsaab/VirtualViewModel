using System;
using System.Linq.Expressions;

namespace VirtualViewModel
{
    public class WhenExpression<TModel,TValue> where TModel : class
    {
        public string PropertyName { get; private set; }
        public TValue Value { get; private set; }
        public ViewModel<TModel> Model { get; private set; }

        public WhenExpression(string propertyName,ViewModel<TModel> model, TValue value)
        {
            PropertyName = propertyName;
            Value = value;
            Model = model;
        }

        public void Set<TProperty>(Expression<Func<TModel, TProperty>> func, TProperty value)
        {
            var memberExpression = func.Body as MemberExpression;
            var propertyName = memberExpression.Member.Name;

            Model.AddUpdateAction(PropertyName, Value, propertyName, value);
        }
    }
}
