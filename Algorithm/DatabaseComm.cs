using DiscordBot.Database;
using DiscordBot.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Google.Common.Geometry;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DiscordBot.Algorithm.CheckData;

namespace DiscordBot.Algorithm
{
    class DatabaseComm
    {
        //chanel ID, kde se bude výpis z aktualizace DB (bot-output).
        private static readonly ulong id_out = 842805476973608961;
        private static readonly ulong id_comm = 843455748401790996;
        private static readonly ulong id_info = 973703930712309800;
        Task<DiscordChannel> chnl_out;
        Task<DiscordChannel> chnl_comm;
        private readonly CommandContext ctx;

        public DatabaseComm(CommandContext ctx)
        {
            this.ctx = ctx;
            chnl_out = ctx.Client.GetChannelAsync(id_out);
            chnl_comm = ctx.Client.GetChannelAsync(id_comm);
        }
        //TODO: Používat i ChangePositionOrAdd
        /// <summary>
        /// Stav vstupního bodu
        /// </summary>
        enum State
        {
            AddToDB,
            AddToDBCheck,
            ChangeName,
            ChangeNameCheck,
            ChangePosition,
            ChangePositionCheck,
            ChangePositionOrAdd,
            Duplicate,
            Unreachable
        }
        /// <summary>
        /// Zpracuje list bodů.
        /// </summary>
        /// <param name="items">List vstupních bodů</param>
        public async void ProcessInputPoints(List<Item> items)
        {
            int[] count = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            using var context = new PogoContext();

            for (int i = 0; i < items.Count; i++)
            {
                State state = CheckIfExists(context, items[i]);
                Point point;
                StringBuilder sb = new StringBuilder();
                Point result = new Point();
                Task<bool> successful;
                Task<(bool, Enums.PointType)> resultData;

                switch (state)
                {
                    // vytvoření bodu
                    case State.AddToDB:
                        point = new Point
                        {
                            Name = items[i].Name,
                            Type = Enums.PointType.Pokestop.ToString(),
                            ExGym = false,
                            Latitude = items[i].Latitude,
                            Longitude = items[i].Longitude,
                            IdCell17 = items[i].GetParent(17),
                            NeedCheck = false,
                            LastUpdate = DateTime.Now
                        };

                        sb.Append(Emoji.GetEmoji(ctx, Emoji.Pokestop));
                        sb.AppendLine(" " + items[i].Name);
                        
                        context.Points.Add(point);
                        Methods.SendBoxMessage(chnl_out, "Vytvoření nového bodu.", sb.ToString(), DiscordColor.Green, items[i].Latitude, items[i].Longitude);
                         
                        count[0] += 1;
                        break;
                    // vytvoření bodu pro kontrolu
                    case State.AddToDBCheck:
                        sb.AppendLine(" " + items[i].Name);
                        sb.AppendLine();
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.Location));
                        sb.AppendLine(" Latitude: " + items[i].Latitude);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.Location));
                        sb.AppendLine(" Longitude: " + items[i].Longitude);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.Id));
                        sb.AppendLine(" Id Cell 17: " + items[i].GetParent(17));

                        resultData = CheckData.AddCheckPoint(ctx, chnl_comm, items[i], "Nutná ruční kontrola a přidání.", sb.ToString(), DiscordColor.Orange);

                        /*if (resultData.Result.Item2 == PointType.Pokestop){sb.Insert(0,Emoji.GetEmoji(ctx, Emoji.Pokestop));}
                        else{sb.Insert(0,Emoji.GetEmoji(ctx, Emoji.Ingress));}*/

                        sb.Insert(0, Emoji.GetEmoji(ctx, resultData.Result.Item2));

                        if (resultData.Result.Item1){
                            point = new Point
                            {
                                Name = items[i].Name,
                                Type = resultData.Result.Item2.ToString(),
                                ExGym = false,
                                Latitude = items[i].Latitude,
                                Longitude = items[i].Longitude,
                                IdCell17 = items[i].GetParent(17),
                                NeedCheck = false,
                                LastUpdate = DateTime.Now
                            };

                            context.Points.Add(point);

                            Methods.SendBoxMessage(chnl_out, "Vytvoření nového bodu.", sb.ToString(), DiscordColor.Green, items[i].Latitude, items[i].Longitude);
                            if (resultData.Result.Item2 == Enums.PointType.Pokestop){count[0] += 1;}
                            else{count[1] += 1;}
                        }
                        else
                        {
                            Methods.SendBoxMessage(chnl_out, "Vytvoření nového bodu ZAMÍTNUTO.", sb.ToString(), DiscordColor.Red, items[i].Latitude, items[i].Longitude);
                            count[2] += 1;
                        }
                        break;
                    // změna jména
                    case State.ChangeName:
                        result = context.Points.FirstOrDefault(
                            x => x.Latitude == items[i].Latitude && x.Longitude == items[i].Longitude && x.IdCell17 == items[i].GetParent(17));

                        /*if (result.Type == PointType.Pokestop.ToString()) { sb.Append(Emoji.GetEmoji(ctx, Emoji.Pokestop)); }
                        else if (result.Type == PointType.Gym.ToString()) { sb.Append(Emoji.GetEmoji(ctx, Emoji.Gym)); }
                        else { sb.Append(Emoji.GetEmoji(ctx, Emoji.Ingress)); }*/

                        sb.Insert(0, Emoji.GetEmoji(ctx, Enum.Parse<Enums.PointType>(result.Type)));

                        sb.AppendLine(" " + items[i].Name);
                        sb.AppendLine();
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
                        sb.AppendLine(" Name: " + result.Name);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
                        sb.AppendLine(" Name: " + items[i].Name);

                        result.Name = items[i].Name;
                        result.LastUpdate = DateTime.Now;

                        Methods.SendBoxMessage(chnl_out, "Změna názvu bodu.", sb.ToString(), DiscordColor.Yellow, items[i].Latitude, items[i].Longitude);
                        count[3] += 1;
                        break;
                    // změna souřadnic
                    case State.ChangePosition:
                        result = context.Points.FirstOrDefault(
                           x => x.Name == items[i].Name && x.IdCell17 == items[i].GetParent(17));

                        /*if (result.Type == PointType.Pokestop.ToString()) { sb.Append(Emoji.GetEmoji(ctx, Emoji.Pokestop)); }
                        else if (result.Type == PointType.Gym.ToString()) { sb.Append(Emoji.GetEmoji(ctx, Emoji.Gym)); }
                        else { sb.Append(Emoji.GetEmoji(ctx, Emoji.Ingress)); }*/

                        sb.Insert(0, Emoji.GetEmoji(ctx, Enum.Parse<Enums.PointType>(result.Type)));

                        sb.AppendLine(" " + result.Name);
                        sb.AppendLine();
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
                        sb.AppendLine(" Latitude: " + result.Latitude);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
                        sb.AppendLine(" Latitude: " + items[i].Latitude);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
                        sb.AppendLine(" Longitude: " + result.Longitude);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
                        sb.AppendLine(" Longitude: " + items[i].Longitude);

                        result.Latitude = items[i].Latitude;
                        result.Longitude = items[i].Longitude;
                        result.LastUpdate = DateTime.Now;

                        Methods.SendBoxMessage(chnl_out, "Změna souřadnic.", sb.ToString(), DiscordColor.Yellow, items[i].Latitude, items[i].Longitude);
                        count[4] += 1;
                        break;
                    // změna souřadnic pro kontrolu
                    case State.ChangePositionCheck:
                        // TODO: zkontrolovat, zda se opravdu najde správný bod
                        result = context.Points.FirstOrDefault(
                           x => x.Name == items[i].Name && (x.Latitude == items[i].Latitude) || x.Longitude == items[i].Longitude);

                        string newIdCell17 = Item.GetParentFromCoor(items[i].Latitude, items[i].Longitude, 17);
                        sb.AppendLine(" " + result.Name);
                        sb.AppendLine();
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
                        sb.AppendLine(" Latitude: " + result.Latitude);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
                        sb.AppendLine(" Latitude: " + items[i].Latitude);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
                        sb.AppendLine(" Longitude: " + result.Longitude);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
                        sb.AppendLine(" Longitude: " + items[i].Longitude);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
                        sb.AppendLine(" Id Cell 17: " + result.IdCell17);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
                        sb.AppendLine(" Id Cell 17: " + newIdCell17);

                        successful = CheckData.CheckPoint(ctx, chnl_comm, result, "Nutná ruční kontrola polohy.", sb.ToString(), DiscordColor.Orange);

                        /*if (result.Type == Models.PointType.Pokestop.ToString()) { sb.Insert(0, Emoji.GetEmoji(ctx, Emoji.Pokestop)); }
                        else if (result.Type == Models.PointType.Gym.ToString()) { sb.Insert(0, Emoji.GetEmoji(ctx, Emoji.Gym)); }
                        else { sb.Insert(0, Emoji.GetEmoji(ctx, Emoji.Ingress)); }*/
                        // TODO: Proč se někde používá sb.Insert a jinde sb.Append
                        sb.Insert(0, Emoji.GetEmoji(ctx, Enum.Parse<Enums.PointType>(result.Type)));

                        if (successful.Result)
                        {
                            result.Latitude = items[i].Latitude;
                            result.Longitude = items[i].Longitude;
                            result.IdCell17 = newIdCell17;
                            result.LastUpdate = DateTime.Now;

                            Methods.SendBoxMessage(chnl_out, "Změna souřadnic bodu.", sb.ToString(), DiscordColor.Yellow, items[i].Latitude, items[i].Longitude);
                            count[4] += 1;
                        }
                        else
                        {
                            Methods.SendBoxMessage(chnl_out, "Změna souřadnic ZAMÍTNUTA.", sb.ToString(), DiscordColor.Red, items[i].Latitude, items[i].Longitude);
                            count[5] += 1;
                        }                        
                        break;
                    // již v databázi
                    case State.Duplicate:
                        count[6] += 1;
                        break;
                    // nemožná kombinace - pozor
                    case State.Unreachable:
                        Console.WriteLine(items[i].Name);
                        Console.WriteLine("Unreachable!");
                        result.NeedCheck = true;

                        sb.Append(Emoji.GetEmoji(ctx, Emoji.Warning));
                        sb.AppendLine(" " + items[i].Name);

                        Methods.SendBoxMessage(chnl_out, "CHYBA - NUTNÁ KONTROLA V DATABÁZI. (Unreachable)", sb.ToString(), DiscordColor.Red, items[i].Latitude, items[i].Longitude);
                        count[7] += 1;
                        break;
                    // pokud nenastane ani jedna předchozí možnost
                    default:
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.Warning));
                        sb.AppendLine(" " + items[i].Name);
                        result.NeedCheck = true;

                        Methods.SendBoxMessage(chnl_out, "CHYBA - NUTNÁ KONTROLA V DATABÁZI. (Default)", sb.ToString(), DiscordColor.Red, items[i].Latitude, items[i].Longitude);
                        count[8] += 1;
                        break;
                }
                await context.SaveChangesAsync();
            }

            await Task.Delay(5000);
            await context.SaveChangesAsync();

            StringBuilder sbr = new StringBuilder();
            sbr.Append(Emoji.GetEmoji(ctx, Emoji.Pokestop));
            sbr.AppendLine(" Přidáno pokestopů: " + count[0]);
            sbr.Append(Emoji.GetEmoji(ctx, Emoji.Ingress));
            sbr.AppendLine(" Přidáno portálů: " + count[1]);
            sbr.Append(Emoji.GetEmoji(ctx, Emoji.Warning));
            sbr.AppendLine(" Přidání bodu ZAMÍTNUTO: " + count[2]);
            sbr.Append(Emoji.GetEmoji(ctx, Emoji.GreenCheck));
            sbr.AppendLine(" Název změnilo: " + count[3]);
            sbr.Append(Emoji.GetEmoji(ctx, Emoji.GreenCheck));
            sbr.AppendLine(" Souřadnice změnilo: " + count[4]);
            sbr.Append(Emoji.GetEmoji(ctx, Emoji.Warning));
            sbr.AppendLine(" Změna polohy ZAMÍTNUTA: " + count[5]);
            sbr.Append(Emoji.GetEmoji(ctx, Emoji.GreenCheck));
            sbr.AppendLine(" Již v databázi: " + count[6]);
            sbr.Append(Emoji.GetEmoji(ctx, Emoji.Warning));
            sbr.AppendLine(" Chyba v databázi (Unreachable): " + count[7]);
            sbr.Append(Emoji.GetEmoji(ctx, Emoji.Warning));
            sbr.AppendLine(" Chyba v databázi (Default): " + count[8]);

            Methods.SendBoxMessage(ctx, chnl_out, "Aktualizace dat proběhla úspěšně.", sbr.ToString(), DiscordColor.Green);
        }
        /// <summary>
        /// Zpracuje změnu typu bodu u zadaného jména.
        /// </summary>
        /// <param name="name">Název bodu (může být i částečný)</param>
        public async void ChangePointType(string name)
        {
            // TODO: Používat sjednocený název contextu: ctx nebo context a sjednocenou syntax. POZOR na DB context a Command context
            using var context = new PogoContext();
            StringBuilder sb;
            string title;
            Task<(bool, Enums.PointType)> resultData;
            // TODO: Omezit výpis pokud bude počet nalezených stopů větší než 20 (30?), tak vybídnout k podrobějšímu vyhledávání.
            List<Point> points = context.Points.Where(i => i.Name.ToLower().Contains(name.ToLower())).ToList();

            for (int i = 0; i < points.Count; i++)
            //foreach (Point p in points)
            {
                sb = new StringBuilder();
                sb.Append("Vyber nový typ bodu nebo pokračuj na další ");
                sb.Append(Emoji.GetEmoji(ctx, Emoji.ArrowRight));    

                title = (i+1) + "/" + points.Count + " " + Emoji.GetEmoji(ctx, Enum.Parse<Enums.PointType>(points[i].Type)) + " " + points[i].Type + " " + points[i].Name;

                resultData = CheckData.ChangeType(ctx, chnl_out, points[i], title, sb.ToString(), DiscordColor.Orange);
                if (resultData.Result.Item1)
                {
                    sb = new StringBuilder();
                    sb.AppendLine(points[i].Name);
                    sb.AppendLine();
                    sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
                    sb.AppendLine(" Původní typ: " + Emoji.GetEmoji(ctx, Enum.Parse<Enums.PointType>(points[i].Type)) + " " + points[i].Type);
                    sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
                    sb.AppendLine(" Nový typ: " + Emoji.GetEmoji(ctx, resultData.Result.Item2) + " " + resultData.Result.Item2.ToString());

                    points[i].Type = resultData.Result.Item2.ToString();
                    points[i].LastUpdate = DateTime.Now;
                    
                    await context.SaveChangesAsync();
                    Methods.SendBoxMessage(chnl_out, "Změna proběhla úspěšně.", sb.ToString(), DiscordColor.Green, points[i]);
                }
                
            }
            //await context.SaveChangesAsync();

        }
        /// <summary>
        /// Zpracuje změnu názvu bodu u zadaného jména.
        /// </summary>
        /// <param name="name">Název bodu (může být i částečný)</param>
        public async void ChangePointName(string name)
        {
            using var context = new PogoContext();
            StringBuilder sb;
            string title;
            Task<(bool, string)> resultData;
            // TODO: Oddělit metodu na hledání bodů v databázi
            // TODO: Omezit výpis pokud bude počet nalezených stopů větší než 20 (30?), tak vybídnout k podrobějšímu vyhledávání.
            List<Point> points = context.Points.Where(i => i.Name.ToLower().Contains(name.ToLower())).ToList();

            for (int i = 0; i < points.Count; i++)
            //foreach (Point p in points)
            {
                sb = new StringBuilder();
                sb.Append("Napiš nový název bodu nebo pokračuj na další ");
                sb.Append(Emoji.GetEmoji(ctx, Emoji.ArrowRight));

                title = (i+1) + "/" + points.Count + " " + Emoji.GetEmoji(ctx, Enum.Parse<Enums.PointType>(points[i].Type)) + " " + points[i].Name;

                resultData = CheckData.ChangeName(ctx, chnl_out, points[i], title, sb.ToString(), DiscordColor.Orange);
                if (resultData.Result.Item1)
                {
                    sb = new StringBuilder();
                    sb.AppendLine(Emoji.GetEmoji(ctx, Enum.Parse<Enums.PointType>(points[i].Type)) + " " + resultData.Result.Item2);
                    sb.AppendLine();
                    sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
                    sb.AppendLine(" Původní název: " + points[i].Name);
                    sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
                    sb.AppendLine(" Nový název: " + resultData.Result.Item2);

                    points[i].Name = resultData.Result.Item2;
                    points[i].LastUpdate = DateTime.Now;
                    
                    await context.SaveChangesAsync();
                    Methods.SendBoxMessage(chnl_out, "Změna proběhla úspěšně.", sb.ToString(), DiscordColor.Green, points[i]);
                }

            }
            //await context.SaveChangesAsync();

        }
        /// <summary>
        /// Zahájí ruční kontrolu databáze.
        /// </summary>       
        public async void CheckPoints()
        {
            using var context = new PogoContext();
            StringBuilder sb;
            string title;
            Item originalPoint;
            Task<(ManualChange,string)> resultData;
            // TODO: Oddělit metodu na hledání bodů v databázi
            // TODO: Změnit NeedCheck z bool na number (jedno číslo = určitá chyba, nula = v pořádku)
            List<Point> points = context.Points.Where(i => i.NeedCheck.Equals(true)).ToList();

            if (points.Count == 0)
            {
                StringBuilder sbu = new StringBuilder();
                sbu.Append("Žádné body ke kontrole ");
                sbu.Append(Emoji.GetEmoji(ctx,Emoji.GreenCheck));
                Methods.SendBoxMessage(chnl_out, sbu.ToString(), "", DiscordColor.Green);
            }

            for (int i = 0; i < points.Count; i++)
            {
                originalPoint = new Item(points[i].Latitude,points[i].Longitude,points[i].Name);
                sb = new StringBuilder();
                sb.Append("Potvrď nebo pokračuj na další ");
                sb.Append(Emoji.GetEmoji(ctx, Emoji.ArrowRight));

                title = (i + 1) + "/" + points.Count + " " + Emoji.GetEmoji(ctx, Enum.Parse<Enums.PointType>(points[i].Type)) + " " + originalPoint.Name;

                resultData = CheckData.ManualCheck(ctx, chnl_out, points[i], title, sb.ToString(), DiscordColor.Orange);
                // TODO: Zde odchytávat nastalé události a vypisovat zde do chatu co se stalo.
                // TODO: Vypisovat správný textbox podle změny
                switch (resultData.Result.Item1)
                {
                    case ManualChange.Name:
                        points[i].Name = resultData.Result.Item2;
                        points[i].LastUpdate = DateTime.Now;
                        points[i].NeedCheck = false;
                        await context.SaveChangesAsync();
                        Methods.SendBoxMessage(ctx, chnl_out, "Změna názvu proběhla úspěšně.", originalPoint.Name, points[i].Name, points[i]);
                        break;
                    case ManualChange.Type:
                        switch (resultData.Result.Item2)
                        {
                            case "Pokestop":
                                points[i].Type = Enums.PointType.Pokestop.ToString();
                                points[i].LastUpdate = DateTime.Now;
                                points[i].NeedCheck = false;
                                await context.SaveChangesAsync();
                                Methods.SendBoxMessage(ctx, chnl_out, "Změna typu proběhla úspěšně.", points[i].Latitude, points[i].Longitude, points[i]);
                                break;
                            case "Gym":
                                points[i].Type = Enums.PointType.Gym.ToString();
                                points[i].LastUpdate = DateTime.Now;
                                points[i].NeedCheck = false;
                                await context.SaveChangesAsync();
                                Methods.SendBoxMessage(ctx, chnl_out, "Změna typu proběhla úspěšně.", points[i].Latitude, points[i].Longitude, points[i]);
                                break;
                            case "ExGym":
                                points[i].Type = Enums.PointType.ExGym.ToString();
                                points[i].LastUpdate = DateTime.Now;
                                points[i].NeedCheck = false;
                                await context.SaveChangesAsync();
                                Methods.SendBoxMessage(ctx, chnl_out, "Změna typu proběhla úspěšně.", points[i].Latitude, points[i].Longitude, points[i]);
                                break;
                            case "Portal":
                                points[i].Type = Enums.PointType.Portal.ToString();
                                points[i].LastUpdate = DateTime.Now;
                                points[i].NeedCheck = false;
                                await context.SaveChangesAsync();
                                Methods.SendBoxMessage(ctx, chnl_out, "Změna typu proběhla úspěšně.", points[i].Latitude, points[i].Longitude, points[i]);
                                break;
                        }
                        
                        break;
                    case ManualChange.Duplicate:
                        Methods.SendBoxMessage(ctx, chnl_out, "Název je stejný jako předchozí.", points[i].Latitude, points[i].Longitude, points[i]);
                        break;
                    case ManualChange.Skip:
                        Methods.SendBoxMessage(ctx, chnl_out, "Změna zamítnuita.", points[i].Latitude, points[i].Longitude, points[i]);
                        break;
                    case ManualChange.Cancel:
                        Methods.SendBoxMessage(chnl_out, "Kontrola ukončena.", "", DiscordColor.Orange);
                        i = points.Count;
                        break;
                    case ManualChange.Coordinates:
                        points[i].Latitude = Double.Parse(resultData.Result.Item2.Split(" ")[0]);
                        points[i].Longitude = Double.Parse(resultData.Result.Item2.Split(" ")[1]);
                        points[i].LastUpdate = DateTime.Now;
                        points[i].NeedCheck = false;
                        await context.SaveChangesAsync();
                        Methods.SendBoxMessage(ctx, chnl_out, "Změna typu proběhla úspěšně.", points[i].Latitude, points[i].Longitude, points[i]);
                        break;


                }
                //TODO: await context.SaveChangesAsync();

            }


        }

        public List<Point> GetPoints(string searchString = "")
        {
            using var context = new PogoContext();            
            List<Point> points = context.Points.Where(i => i.Name.ToLower().Contains(searchString.ToLower())).ToList();
            return points;
        }

        public List<Pokemon> GetPokemons()
        {
            using var context = new PogoContext();
            List<Pokemon> pokemons = context.Pokemons.ToList();
            //List<Pokemon> pokemons = context.Pokemons.Where(i => i.Name.ToLower().Contains(searchString.ToLower())).ToList();
            return pokemons;
        }
        /// <summary>
        /// Kontrola vstupního bodu s databází.
        /// </summary>
        /// <param name="context">DB context</param>
        /// <param name="item">Bod, který chceme kontrolovat</param>
        /// <returns>Vrací jaký má bod vztah k bodům vytvořených v databázi.</returns>
        private State CheckIfExists(PogoContext context, Item item)
        {
            var p_name = context.Points.FirstOrDefault(i => i.Name == item.Name);
            var p_lat = context.Points.FirstOrDefault(i => i.Latitude == item.Latitude);
            var p_lng = context.Points.FirstOrDefault(i => i.Longitude == item.Longitude);
            var p_cell = context.Points.FirstOrDefault(i => i.IdCell17 == item.GetParent(17));

            // Název bodu v DB není
            if (p_name == null)
            {
                // Souřadnice bodu nejsou v DB -> přidání do DB
                if (p_lat == null && p_lng == null && p_cell == null)
                {
                    return (State.AddToDB);
                }
                // Jedna souřadnice je v DB (nový bod nebo posun starého) -> přidání do DB - nutná kontrola
                if (p_lat == null || p_lng == null)
                {
                    return (State.AddToDBCheck);
                }
                // Souřadnice jsou v DB -> změna názvu nebo chyba
                if (p_lat.IdPoint == p_lng.IdPoint)
                {
                    if (p_cell == null) { return (State.Unreachable); }
                    else { return (State.ChangeName); }
                }
            }
            else
            {
                // Souřadnice bodu nejsou v DB (nový bod nebo posun starého) -> přidání do DB - nutná kontrola
                if (p_lat == null && p_lng == null && p_cell == null)
                {
                    return (State.AddToDBCheck);
                }
                // Jedna souřadnice je v DB -> změna souřadnic (nutná kontrola)
                if ((p_lat == null || p_lng == null))
                {
                    if (p_cell == null)
                    {
                        return (State.ChangePositionCheck);
                    }
                    else
                    {
                        if (p_name.IdPoint != p_cell.IdPoint) { return (State.ChangePositionCheck); }
                        return (State.ChangePosition);
                    }
                }
                // Souřadnice jsou v DB -> duplikát nebo chyba
                if (p_lat.IdPoint == p_lng.IdPoint)
                {
                    if (p_cell == null) { return (State.Unreachable); }
                    else { return (State.Duplicate); }
                }
            }
            return (State.Unreachable);

        }
    }
}
