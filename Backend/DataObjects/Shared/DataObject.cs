using System.Collections.Generic;

namespace SocialNet.Backend.DataObjects
{
	public abstract class DataObject
	{
		protected DataObject()
		{
			Context = new Dictionary<string, object>();
		}
        
		private Dictionary<string, object> _context;

		public Dictionary<string, object> Context
		{
			get
			{
				if (_context == null) _context = new Dictionary<string, object>();
				return _context;
			}
			set { _context = value; }
		}

        public virtual void CopyTo(DataObject target)
        {
            target.Context.Clear();

            foreach (var pair in Context)
            {
                target.Context.Add(pair.Key, pair.Value);
            }
        }
	}
}

