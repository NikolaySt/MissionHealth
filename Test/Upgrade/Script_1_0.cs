using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Upgrade
{
	public class Script_1_0
	{
		public static async Task Run()
		{
			await Script_Articles_Review();
		}

		private static string TruncateAtWord(string input, int length)
		{
			if (input == null || input.Length < length)
				return input;
			int iNextSpace = input.LastIndexOf(" ", length);
			return string.Format("{0}", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
		}

		private static List<string> Keywords(string text, int weight = 1, int count = 20)
		{
			var list = Regex
				.Split(text.ToLowerInvariant().Trim(), @"\W+")
				.Where(it => !string.IsNullOrWhiteSpace(it) && it.Length >= 5)
				.OrderBy(it => it);

			var keyword = list.GroupBy(x => x)
				.Select(g => new { Value = g.Key, Count = g.Count() })
				.OrderByDescending(x => x.Count).Where(it => it.Count > weight).Select(it => it.Value).Take(count).ToList();

			if (keyword.Count() < count)
			{
				var more = list.GroupBy(x => x)
				.Select(g => new { Value = g.Key, Count = g.Count() })
				.OrderByDescending(x => x.Count)
				.Where(it => it.Count <= weight).Select(it => it.Value).Take(count - keyword.Count()).ToList();
				keyword.AddRange(more);
			}

			return keyword;
		}

		private static async Task Script_Articles_Review()
		{
			Console.Write("Process Script_Articles_Review...");
			var catalog = new ArticleCatalog();
			var filter = new ArticleListFilter() { };
			var page = 0;
			var pageSize = 20;
			var result = await catalog.GetPageAsync(filter, page, pageSize);
			while (result.Items.Any())
			{
				foreach (var item in result.Items)
				{
					if (string.IsNullOrEmpty(item.Review))
					{
						var review = TruncateAtWord(item.Content.Text, 200);
						var update = new Dictionary<string, object>() { { "Review", review } };
						await catalog.UpdateAsync(item.Id, update);
					}
					if (item.Keywords == null || !item.Keywords.Any())
					{
						var keywords = Keywords(item.Content.Text);
						var update = new Dictionary<string, object>() { { "Keywords", keywords } };
						await catalog.UpdateAsync(item.Id, update);
					}
					if (string.IsNullOrWhiteSpace(item.TitleId))
					{
						var title = Regex.Replace(item.Title.Text, @"\W+", "-").ToLower();
						var update = new Dictionary<string, object>() { { "TitleId", title } };
						await catalog.UpdateAsync(item.Id, update);
					}
				}

				page++;

				result = await catalog.GetPageAsync(filter, page, pageSize);
			}
			Console.WriteLine("[success]");
		}
	}
}
