using System;
using System.Threading.Tasks;
using Prism.Navigation;
using RealmWriteCrash.Models;
using RealmWriteCrash.Services;

namespace RealmWriteCrash.ViewModels
{
    sealed class MainPageViewModel : INavigatedAware
    {
        private PersistenceService1 PersistenceService1 { get; }

        private PersistenceService2 PersistenceService2 { get; }
        
        public MainPageViewModel(PersistenceService1 persistenceService1, PersistenceService2 persistenceService2)
        {
            PersistenceService1 = persistenceService1;
            PersistenceService2 = persistenceService2;
        }
        
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            Task.Run(ExecuteInsertTransactionCrash);
            // Task.Run(ExecuteInsertTransactionFix1);
            // Task.Run(ExecuteInsertTransactionFix2);
        }

        void ExecuteInsertTransactionCrash()
        {
            WriteTransactionBatch1();
            
            System.Diagnostics.Debug.WriteLine("Batch#1 finished");

            WriteTransactionBatch2();
            
            // this method normally crashes after executing first transaction of batch 2
            
            System.Diagnostics.Debug.WriteLine("Batch#2 finished");
        }

        async Task ExecuteInsertTransactionFix1()
        {
            // this fix works well
            
            WriteTransactionBatch1();
            
            System.Diagnostics.Debug.WriteLine("Batch#1 finished");

            await Task.Run(WriteTransactionBatch2)
                .ContinueWith(_ =>  System.Diagnostics.Debug.WriteLine("Batch#2 finished"));
        }
        
        async Task ExecuteInsertTransactionFix2()
        {
            // this fix works well
            
            WriteTransactionBatch1();
            
            System.Diagnostics.Debug.WriteLine("Batch#1 finished");

            await Task.Delay(TimeSpan.FromMilliseconds(500));
            
            WriteTransactionBatch2();
            
            System.Diagnostics.Debug.WriteLine("Batch#2 finished");
        }
        
        void WriteTransactionBatch1()
        {
            for (var i = 0; i < 100; i++)
            {
                PersistenceService1.Exec(realm =>
                {
                    realm.Write(() =>
                    {
                        System.Diagnostics.Debug.WriteLine($"Thread-Id: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                        
                        realm.Add(CreateModel(), true);
                        realm.Add(CreateModel(), true);
                        realm.Add(CreateModel(), true);
                        realm.Add(CreateModel(), true);
                        realm.Add(CreateModel(), true);
                        realm.Add(CreateModel(), true);
                        realm.Add(CreateModel(), true);
                        realm.Add(CreateModel(), true);
                        realm.Add(CreateModel(), true);
                        realm.Add(CreateModel(), true);
                        realm.Add(CreateModel(), true);
                        realm.Add(CreateModel(), true);
                    });
                });
            }
        }
        
        void WriteTransactionBatch2()
        {
            for (var i = 0; i < 100; i++)
            {
                PersistenceService2.Exec(realm =>
                {
                    realm.Write(() =>
                    {
                        System.Diagnostics.Debug.WriteLine($"Thread-Id: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                        
                        realm.Add(CreateModel(), true);
                        realm.Add(CreateModel(), true);
                        realm.Add(CreateModel(), true);
                    });
                });
            }
        }

        TestModel1 CreateModel()
        {
            return new TestModel1()
            {
                Id = Guid.NewGuid().ToString(),
                Property1 = "dsdsdfdgfsdgkfdgksasdasfsdsdafsdfsdaf dsaf sdafdsf sdaf sd fds",
                Property2 = "dsdsdfdgfsdgkfdgksasdasfsdsdafsdfsdaf dsaf sdafdsf sdaf sd fds",
                Property3 = "dsdsdfdgfsdgkfdgksasdasfsdsdafsdfsdaf dsaf sdafdsf sdaf sd fds"
            };
        }
    }
}