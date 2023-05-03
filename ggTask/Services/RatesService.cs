using Currency.Exchange.Rate.Scraper.Web.Helpers.Constants;
using Currency.Exchange.Rate.Scraper.Web.Models;
using HtmlAgilityPack;

namespace Currency.Exchange.Rate.Scraper.Web.Services
{
    public interface IRatesService
    {
        Task<List<Entity>> GetExchangeRates();
    }

    public class RatesService : IRatesService
    {
        private readonly HtmlWeb _webClient;
        private readonly IConfiguration _config;

        public RatesService(HtmlWeb webClient, IConfiguration config)
        {
            _webClient = webClient;
            _config = config;
        }

        /// <summary>
        /// Retrieves the current exchange rates from the configured source.
        /// </summary>
        /// <returns>A list of Entity objects representing the current exchange rates.</returns>
        public async Task<List<Entity>> GetExchangeRates()
        {
            var document = await _webClient.LoadFromWebAsync(_config.GetValue<string>("RATE_AM_URL"));

            if (document != null)
            {
                var table = document.DocumentNode.SelectSingleNode("//table[@class='rb']");
                var rows = table.SelectNodes(".//tr");

                var exchangeRates = new List<Entity>();

                foreach (var row in rows)
                {
                    var cells = row.SelectNodes(".//td");
                    if (cells != null && cells.Count == 13)
                    {
                        exchangeRates.Add(new Entity()
                        {
                            Id = int.Parse(cells[0].InnerText.TrimEnd('.')),
                            Bank = cells[1].InnerText.Trim(),
                            SetDateTime = cells[4].InnerText.Trim(),
                            CurList = new List<CurrencyEntity>()
                            {
                                new CurrencyEntity()
                                {
                                    Currency = CurrencyConstant.USD.ToString(),
                                    BuyPrice = cells[5].InnerText.Trim(),
                                    SellPrice = cells[6].InnerText.Trim(),
                                },

                                new CurrencyEntity()
                                {
                                    Currency = CurrencyConstant.EUR.ToString(),
                                    BuyPrice = cells[7].InnerText.Trim(),
                                    SellPrice = cells[8].InnerText.Trim()
                                },

                                new CurrencyEntity()
                                {
                                    Currency = CurrencyConstant.RUB.ToString(),
                                    BuyPrice = cells[9].InnerText.Trim(),
                                    SellPrice = cells[10].InnerText.Trim()
                                },

                                new CurrencyEntity()
                                {
                                    Currency = CurrencyConstant.GBP.ToString(),
                                    BuyPrice = cells[11].InnerText.Trim(),
                                    SellPrice = cells[12].InnerText.Trim()
                                }
                            }
                        });
                    }
                }
                return exchangeRates;
            }
            return new List<Entity>();
        }
    }
}
