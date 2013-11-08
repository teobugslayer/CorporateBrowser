using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CorporateBrowser
{
	public class MainWindowPresenter: ReactiveObject
	{
		private readonly ReactiveList<User> users;
		public ListCollectionView Users { get; set; }

		public MainWindowPresenter()
		{
			this.users = new ReactiveList<User>();
			this.Users = new ListCollectionView(users);
			this.Users.GroupDescriptions.Add(new PropertyGroupDescription("Team"));

			GenerateData().ObserveOn(RxApp.MainThreadScheduler).Subscribe(data => this.users.Add(data));
		}

		static IObservable<User> GenerateData()
		{
			var usersSubject = new Subject<User>();

			Observable.Start(() =>
			{
				try
				{
					var searchRoot = new DirectoryEntry("LDAP://DC=fabrikam,DC=com");
					var searcher = new DirectorySearcher(searchRoot)
					{
						Asynchronous = false,
						Filter = "(&(&(objectClass=user)(objectCategory=person))(!(memberof=InvalidAccounts)))",
					};

					searcher.PropertiesToLoad.AddRange(PROPERTIES_TO_LOAD);

					using (var users = searcher.FindAll())
					{
						foreach (SearchResult ldapUser in users)
						{
							var userName = ldapUser.SafeReadString("name");
							var mail = ldapUser.SafeReadString("mail");
							var phone = ldapUser.SafeReadString("mobile");
							var title = ldapUser.SafeReadString("title");
							var team = ldapUser.SafeReadString("department");

							var picture = ldapUser.SafeReadPicture();

							var newUser = new User(userName, mail, phone, title, picture, team);
							usersSubject.OnNext(newUser);
						}
					}

					usersSubject.OnCompleted();
				}
				catch (Exception ex)
				{
					usersSubject.OnError(ex);
				}
			},
			RxApp.TaskpoolScheduler);

			return usersSubject;
		}

		private static readonly string[] PROPERTIES_TO_LOAD = new string[] { "name", "mail", "mobile", "title", "department", "jpegPhoto" };
	}
}
