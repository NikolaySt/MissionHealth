using System;

namespace SocialNet.Backend.DataFilters
{
	public abstract class SuperListFilter
	{
		public DateTime? CreatedAfter { get; set; }

		public DateTime? UpdatedAfter { get; set; }

	    public virtual bool IsEmpty()
	    {
	        return (CreatedAfter == null && UpdatedAfter == null);
	    }
	}
}
