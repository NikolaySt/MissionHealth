using System.Collections.Generic;

namespace SocialNet.Backend.DataFilters
{
	public partial class ArticleListFilter : SuperListFilter
	{
        public string CategoryValue { get; set; }

        public string SubCategoryValue { get; set; }

        public string AreaValue { get; set; }

        public bool? Published { get; set; }

        public bool? Top { get; set; }

        public bool? Interesting { get; set; }

        public IEnumerable<string> ExcludeAreaValue { get; set; }

        public string Criteria { get; set; }

        public CriteriaType? CriteriaType { get; set; }
    }
}
