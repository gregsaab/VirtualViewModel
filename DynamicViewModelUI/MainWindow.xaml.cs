using System.Windows;
using DynamicViewModelTests;
using VirtualViewModel;

namespace DynamicViewModelUI
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            var dataModel = new ViewModel<Model>(new {Name = "sam"});
            dataModel.When(x=>x.Name,"Thelonious").Set(x=>x.Age,51);
            DataContext = dataModel;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var model = (DataContext as dynamic);
            model.Age = _newAge.Text;
            model.Name = _newName.Text;
        }
    }
}
