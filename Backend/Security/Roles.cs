using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNet.Backend.Security
{
	public class Roles
	{
		public static string Guest
		{
			get { return "Guest"; }
		}

		public static string CloudUser
		{
			get { return "CloudUser"; }
		}

		public static string EndUser
		{
			get { return "EndUser"; }
		}

		public static string CourseAcne
		{
			get { return "CourseAcne"; }
		}

		public static string Viewer
		{
			get { return "Viewer"; }
		}

		public static string Developer
		{
			get { return "Developer"; }
		}

		public static string Manager
		{
			get { return "Manager"; }
		}

		public static string Admin
		{
			get { return "Admin"; }
		}

		public static string Editor
		{
			get { return "Editor"; }
		}
	}
}
