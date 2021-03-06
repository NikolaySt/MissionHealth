namespace SocialNet.Backend.DataObjects
{
    public class ArticleCategory: IdDataObject
    {
        public string CategoryName { get; set; }

        public string CategoryValue { get; set; }

        public string SubCategoryName { get; set; }

        public string SubCategoryValue { get; set; }

        public string AreaName { get; set; }

        public string AreaValue { get; set; }

        public bool New { get; set; }
    }
}
