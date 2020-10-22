using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.SelectLogic.Page
{
	class SamplePages
	{
		public static readonly SitePage PagePrincessReDive = new SitePage
		{
			LastModified = new DateTime(2020, 08, 09),
			Title = "Princess Connect! Re:Dive",
			TweetDate = new DateTime(2020, 08, 16, 0, 0, 0),
			URL = "https://kawaii-mobile.com/2020/08/princess-connect-redive/"
		};

		public static readonly SitePage PageSpeedGrapher = new SitePage
		{
			LastModified = new DateTime(2020, 08, 02),
			Title = "Speed Grapher",
			TweetDate = new DateTime(2020, 08, 15, 0, 0, 0),
			URL = "https://kawaii-mobile.com/2020/08/speed-grapher/"
		};

		public static readonly SitePage PageGleipnir = new SitePage
		{
			LastModified = new DateTime(2020, 07, 19),
			Title = "Gleipnir",
			TweetDate = new DateTime(2020, 08, 14, 0, 0, 0),
			URL = "https://kawaii-mobile.com/2020/07/gleipnir/"
		};

		public static readonly SitePage PageHameFura = new SitePage
		{
			LastModified = new DateTime(2020, 07, 05),
			Title = "Otome Game no Hametsu Flag shika Nai Akuyaku Reijou ni Tensei Shiteshimatta",
			TweetDate = new DateTime(2020, 08, 13, 0, 0, 0),
			URL = "https://kawaii-mobile.com/2020/07/otome-game-no-hametsu-flag-shika-nai-akuyaku-reijou-ni-tensei-shiteshimatta/"
		};


		//Ниже блок страниц для специальных дней - Хеллоуин
		//

		public static readonly SitePage PageHalloween2019 = new SitePage
		{
			LastModified = new DateTime(2019, 10, 31),
			Title = "Halloween 2019",
			TweetDate = new DateTime(2019, 10, 31),
			URL = "https://kawaii-mobile.com/2019/10/halloween-2019/",
			SpecialDay = kawaii.twitter.db.SpecialDays.HALLOWEEN
		};

		public static readonly SitePage PageHalloween2013 = new SitePage
		{
			LastModified = new DateTime(2013, 10, 30),
			Title = "Halloween 2013",
			TweetDate = new DateTime(2013,10,30),
			URL = "https://kawaii-mobile.com/2013/10/halloween-2013/",
			SpecialDay = kawaii.twitter.db.SpecialDays.HALLOWEEN
		};

		public static readonly SitePage PageHalloween2014 = new SitePage
		{
			LastModified = new DateTime(2014, 10, 29),
			Title = "Halloween 2014",
			TweetDate = new DateTime(2014,10,29),
			URL = "https://kawaii-mobile.com/2014/11/halloween-2014/",
			SpecialDay = kawaii.twitter.db.SpecialDays.HALLOWEEN
		};


		public static readonly SitePage PageHalloween2015 = new SitePage
		{
			LastModified = new DateTime(2015, 10, 28),
			Title = "Halloween 2015",
			TweetDate = new DateTime(2015,10,28),
			URL = "https://kawaii-mobile.com/2015/11/halloween-2015/",
			SpecialDay = kawaii.twitter.db.SpecialDays.HALLOWEEN
		};



	}
}
