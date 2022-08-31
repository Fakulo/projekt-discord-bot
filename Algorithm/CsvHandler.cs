using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using System.Threading.Tasks;
using System.Globalization;
using DiscordBot.Models;

namespace DiscordBot.Algorithm
{
    class CsvHandler
    {
        /// <summary>
        /// Z ručně nastaveného souboru .csv vytáhne list bodů
        /// </summary>
        /// <returns>Vrací list bodů</returns>
        public static List<Item> ReadFile()
        {
            List<Item> items = new List<Item>();
            Item item;

            using (TextFieldParser csvParser = new TextFieldParser(@"D:\Tomáš\Documents\Visual Studio 2019\Projekty\DiscordBot\gyms+stops_2021_02_02_15_28_02.csv"))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    string Name = fields[0];
                    string Lat = fields[1].Replace(".", ",");
                    string Lng = fields[2].Replace(".", ",");
                    item = new Item(Double.Parse(Lat), Double.Parse(Lng), Name);
                    items.Add(item);
                }
            }
            return items;
        }
    }
}
