using SocialNet.Backend.Catalogs;
using SocialNet.Backend.Configuration;
using SocialNet.Backend.Database;
using SocialNet.Backend.Managers;
using SocialNet.Backend.Security;
using SocialNet.Backend.Tokens.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BackendTest
{
	class Program
	{
		static void Main(string[] args)
		{
            Init();

			//RenewSecureToken().Wait();

			Send();


			Console.ReadLine();
        }

        static async Task RenewSecureToken()
        {
            var issuer = await TokenIssuerFactory.GetInstance();
            var parse = TokenParserFactory.GetInstance();          

            var list = parse.GetClaims(SystemSettings.SecureToken).ToList();

            list.RemoveAll(it => it.Type == Claims.Issuer || it.Type == Claims.Audience || it.Type == Claims.ExpirationTime);
            var token = issuer.GetToken(list);

            Console.WriteLine(token);
        }

        private static void MailSend()
        {
            //var manager = new MailManager();

            //Task.Factory.StartNew(async () =>
            //{
            try
            {
                //manager.SendBookAsync("n_stoychev@yahoo.com", "Nikolay", "Test", "Test", "Хормони").Wait();
                //Send();
                Console.Write("[Success]");
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
            }
        }

        private static void Send()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = 
                new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
            using (var client = new System.Net.Mail.SmtpClient("smtp.gmail.com"))
            {
                client.EnableSsl = true;
                client.Port = 587;
				client.Credentials = new NetworkCredential("ati@misiazdrave.bg", "Shveik77");
                using (System.Net.Mail.MailMessage M = 
					new System.Net.Mail.MailMessage("Ati<ati@misazdrave.bg>", "Nikolay<fostarfx@gmail.com>", 
					"Test", "Test tova e testvo sob6tenie ot admin."))
                {
                    try
                    {
						client.Send(M);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        return;
                    }
                }
            }
        }

        private static bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine(certificate);

            return true;
        }

        static void Init()
        {

            SystemSettings.Load(AppDomain.CurrentDomain.BaseDirectory);

            var settings = new MongoProviderSettings
            {
                ConnectionString = SystemSettings.DatabaseConnectionString,
                DatabaseName = SystemSettings.DatabaseName,
                CollectionNamespace = SystemSettings.DatabaseCollectionNamespace,
                ConnectionTimeout = SystemSettings.ConnectionTimeout,
                ConnectionPoolSize = SystemSettings.ConnectionPoolSize,
                WaitQueueSize = SystemSettings.WaitQueueSize
            };

            MongoProvider.InitDefaultInstance(settings);

            DatabaseInitializer.Init();
        }

        static void Test()
        {

            var cultureInfo = new CultureInfo("bg-BG");

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            //var text = "Kakva e tajnata na socikolkopio, za edno ne6to e sigurno 1990 e klu4ova; za germani. i * vsiqki ostanali";
            //var result = Regex.Replace(text, @"\W+", "-");
            var text = @"Бавно идва сезонът на разходките и почивките сред природата, пикниците, излетите в планината, но също и на комарите, а никой не иска да бъде мишена за тях. Ухапванията от тези насекоми могат да бъдат не само неприятни и сърбящи, но и при по-чувствителни хора да доведат до изразени алергични реакции на кожата.За жалост често репелентите, които използваме нанасят по-големи вреди за здравето, отколкото самите комари, защото могат да съдържат токсични пестициди. Затова е трябва да четем хубаво етикетите. Едно от най-често използваните вещества в препаратите против насекоми е DEET (диетил-мета-толуамид) . За DEET има много крайни мнения, тъй като е известно, че в по-големи концентрации можа да има токсични ефекти върху нервната система, но такива не са доказани при приложение върху кожата. Въпреки това производителите съветват да не се използва в закрити помещения и под дрехите, както и при малки деца. Все пак винаги трябва да преценяваме съотношението полза-риск и ако ни се налага да пътуваме в страни, където маларията и други преносими от насекомите болести са силно разпространени, този тип репеленти (съдържащи DEET) са дори задължителни. Препарати в концентрация 30-40% осигуряват защита в продължение на 6 часа. По-ниските концентрации на DEET имат по-краткотрайна защита и трябва да се нанасят по-често. Гледайте репелентите, които съдържат DEET, да не са в пластмасова опаковка, защото DEET има свойството да разтваря пластмасата. Друг инсектицид използван в репелентите е т.нар икаридин. Засега проучванията са показали достатъчна ефикасност в защитата срещу комари, мухи и дори кърлежи, но не бива да се използва при деца под 2 годишна възраст. Вариантите на пазара са продукти с концентрация 10% икаридин и защита до 4 часа и такива с 20% , които осигуряват до 8 часа защита срещу насекоми. Същестувават и репеленти на натурална основа с етерични масла - най-често от цитронела в препаратите предназначени за деца. На природните репеленти може да се разчита, ако се намираме в регион, където комарите ухаванията от комари са свързани едиствено със сърбеж и дискомфорт. В противен случай трябва да се запасим с по-сериозен спрей, съдържащ DEET.  Въпреки че производителите и търговците умело ни убеждават, че подобен тип продукти са безопасни за здравето, винаги е по-добре да използваме други природни и естествени варианти, отколкото синтетични пестициди, защото дори, че ги слагаме само върху кожата – те проникват и в кръвобрaщението. Най-лесният вариант е да си направим сами репелент, които се приготвя изключително лесно и е не по-малко ефикасен.Очаквайте съвсем скоро и рецепта как да си направим сами репелент. Няколко съвета при използване на репелент:Използвайте само върху откритите части на тялото (не под дрехите)Не нанасяйте върху рани и раздразнена кожаНе нанасяйте върху ръцете на детето, тъй като може да си пипне лицетоИзбирайте гел за децата, тъй като при аерозола има по-голяма вероятност от вдишванеСлед като се приберете измийте участъците, върху които е нанасян препарата";

            var list = Regex
                .Split(text.ToLowerInvariant().Trim(), @"\W+")
                .Where(it => !string.IsNullOrWhiteSpace(it) && it.Length >= 5)
                .OrderBy(it => it);

            var weight = 1;

            var keyword = list.GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count).Where(it => it.Count > weight).Select(it => it.Value).Take(20).ToList();

            if (keyword.Count() < 20)
            {
                var more = list.GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Where(it => it.Count <= weight).Select(it => it.Value).Take(20 - keyword.Count()).ToList();
                keyword.AddRange(more);
            }

            var result = string.Join(",", keyword);
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(result);
            Console.ReadLine();
            /*
				.Split(text.ToLowerInvariant().Trim(), @"\W+")
				.Where(it => !string.IsNullOrWhiteSpace(it) && it.Length >= minSymbolsForWord)
				.OrderBy(it => it);
			*/
        }


    }
}
