using System;

namespace SocialNet.Backend.DataObjects
{
    public abstract class VisibilityDataObject : IdDataObject
	{
        public VisibilityStatus Visibility { get; set; }
	}
}
