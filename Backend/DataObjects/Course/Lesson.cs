using System;

namespace SocialNet.Backend.DataObjects
{
	public class Lesson : IdDataObject
	{
		public string Course { get; set; }
		public string Module { get; set; }
		public int LessonNumber { get; set; }
		public Content Video { get; set; }
		public string Description { get; set; }
		public string Test { get; set; }
		public string Documents { get; set; }
		public string Books { get; set; }
	}
}
