using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SiteMap;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.DatabaseFromSiteMapUpdate
{
	public class SamplePostsDatabase
	{
		/// <summary>
		/// Тайтлы (соотв массиву PostURLs)
		/// </summary>
		public static readonly string[] PostTitles =
		{
			"Speed Grapher",				//0
			"Princess Connect! Re:Dive",	//1
			"KissXsis",						//2
			"Isuca",						//3
			"Uchuu no Stellvia"				//4
		};


		/// <summary>
		/// Тест-урлы сайта
		/// </summary>
		public static readonly URLInfo[] PostURLs =
		{
			//0
			new URLInfo()
			{
				 LastModified=new DateTime(2020,08,02,10,11,47),
				 //Title="Speed Grapher",
				 URL="https://kawaii-mobile.com/2020/08/speed-grapher/"
			},

			//1
			new URLInfo()
			{
				 LastModified=new DateTime(2020,09,05,17,29,20),
				 //Title="Princess Connect! Re:Dive",
				 URL="https://kawaii-mobile.com/2020/08/princess-connect-redive/"
			},

			//2
			new URLInfo()
			{
				 LastModified=new DateTime(2020,09,05,17,29,20),
				 //Title="KissXsis",
				 URL="https://kawaii-mobile.com/2020/09/kissxsis/"
			},

			//3
			new URLInfo()
			{
				 LastModified=new DateTime(2020,09,13,10,49,03),
				 //Title="Isuca",
				 URL="https://kawaii-mobile.com/2020/09/isuca/"
			},

			//4
			new URLInfo()
			{
				 LastModified=new DateTime(2020,10,03,22,15,0),
				 //Title="Uchuu no Stellvia",
				 URL="https://kawaii-mobile.com/2020/10/uchuu-no-stellvia/"
			},
		};

		public static readonly AnimatedImage[] Images =
		{
			new AnimatedImage()	//0
			{
				BlobName="uchuu-no-stellvia:image1.gif"
			},

			new AnimatedImage()	//1
			{
				BlobName="uchuu-no-stellvia:katase.gif"
			},

			new AnimatedImage()	//2
			{
				BlobName="uchuu-no-stellvia:Kouta Otoyama.gif"
			},


			new AnimatedImage()	//3
			{
				BlobName="isuca:Sakuya.gif"
			},

			new AnimatedImage()	//4
			{
				BlobName="kissxsis:ako.gif"
			},

			new AnimatedImage()	//5
			{
				BlobName="kissxsis:riko.gif"
			}

		};


	}
}
