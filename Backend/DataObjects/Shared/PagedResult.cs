using System.Collections.Generic;

namespace SocialNet.Backend.DataObjects
{
	public class PagedResult<T>
	{
		public int PageNumber { get; set; }

		public int PageSize { get; set; }

		public List<T> Items { get; set; }

	    public PagedResult()
	    {
	        Items = new List<T>();
	    }
	}
}
