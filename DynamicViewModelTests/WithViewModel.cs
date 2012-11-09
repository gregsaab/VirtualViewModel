using Machine.Specifications;
using VirtualViewModel;

namespace DynamicViewModelTests
{
    public abstract class WithViewModel<T> where T: class 
    {
        protected static ViewModel<T> Subject;
        protected static dynamic DynamicSubject { get { return Subject; } }
        Establish context = () => { Subject = new ViewModel<T>();};
    }
}
