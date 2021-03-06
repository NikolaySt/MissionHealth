using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialNet.Backend.DataObjects
{
    public class AlbumCategory: IdDataObject
    {
        public string CategoryName { get; set; }

        public string CategoryValue { get; set; }

        public string AreaName { get; set; }

        public string AreaValue { get; set; }

		public string AlbumValue { get; set; }

		public bool New { get; set; }
    }
}
