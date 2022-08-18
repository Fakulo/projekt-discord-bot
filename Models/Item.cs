using Google.Common.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Models
{
    class Item
    {
        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public string Name { get; private set; }

        //public S2Cell Cell { get; private set; }   
        public Item(double lat, double lgn, string name)
        {
            this.Latitude = lat;
            this.Longitude = lgn;
            this.Name = name;
        }
        public Item(double lat, double lgn)
        {
            this.Latitude = lat;
            this.Longitude = lgn;
            this.Name = "-bez názvu-";
        }
        public S2Cell GetCell()
        {
            return new S2Cell(S2LatLng.FromDegrees(Latitude, Longitude));
        }

        public string GetParent(int level)
        {
            return GetCell().Id.ParentForLevel(level).Id.ToString();
        }
        /// <summary>
        /// Vrací S2Cell ID zadaných souřadnic.
        /// </summary>
        /// <param name="lat">Zeměpisná šířka</param>
        /// <param name="lgn">Zeměpisná délka</param>
        /// <param name="level">Úroveň</param>
        /// <returns>ID zadané úrovně</returns>
        public static string GetParentFromCoor(double lat, double lgn, int level)
        {
            S2Cell tempcell = new S2Cell(S2LatLng.FromDegrees(lat, lgn));
            return tempcell.Id.ParentForLevel(level).Id.ToString();
        }

    }
}
