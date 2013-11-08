using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CorporateBrowser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            GenerateData();
        }

        static void GenerateData()
        {
            Observable.Start(() =>
            {
                var searchRoot = new DirectoryEntry("LDAP://DC=fabrikam,DC=com");
                var searcher = new DirectorySearcher(searchRoot)
                {
                    Asynchronous = false,
                    Filter = "(&(objectClass=user)(objectCategory=person))",
                };
                var users = searcher.FindAll();
                // Dispose of users!
            },
            RxApp.TaskpoolScheduler);
        }
    }
}
