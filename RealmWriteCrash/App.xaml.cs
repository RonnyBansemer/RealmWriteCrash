using Prism.DryIoc;
using Prism.Ioc;
using RealmWriteCrash.Services;
using RealmWriteCrash.ViewModels;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace RealmWriteCrash
{
    public partial class App : PrismApplication
    {
        public App() : base()
        {
            InitializeComponent();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            containerRegistry.RegisterSingleton<PersistenceService1>();
            containerRegistry.RegisterSingleton<PersistenceService2>();
        }

        protected override void OnInitialized()
        {
            NavigationService.NavigateAsync(nameof(MainPage));
        }
    }
}