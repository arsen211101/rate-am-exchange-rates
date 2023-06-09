﻿namespace Currency.Exchange.Rate.Scraper.Web.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public string Bank { get; set; }
        public string SetDateTime { get; set; }

        public List<CurrencyEntity> CurList { get; set; }

        public Entity()
        {
            CurList = new List<CurrencyEntity>();
        }
    }
}
