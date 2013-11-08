using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CorporateBrowser
{
	public class User
	{
		public User(string name, string mail, string phone, string title, ImageSource photo, string team)
		{
			this.Name = name;
			this.Mail = mail;
			this.Phone = phone;
			this.Title = title;
			this.Team = team;
			this.Photo = photo;
		}

		public string Name { get; private set; }
		public string Mail { get; private set; }
		public string Phone { get; private set; }
		public string Team { get; private set; }
		public string Title { get; private set; }
		public ImageSource Photo { get; private set; }
	}
}
