namespace SocialNet.Backend.DataFilters
{
	public partial class PostCategoryListFilter : SuperListFilter
	{
        public string SubCategoryValue { get; set; }

        public string CategoryValue { get; set; }

        public string AreaValue { get; set; }
    }
}
