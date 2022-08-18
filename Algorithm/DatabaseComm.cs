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
using SQLiteNetExtensionsAsync.Extensions;
using static DiscordBot.Algorithm.CheckData;
using static DiscordBot.Models.Enums;

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
                StringBuilder sb = new();
                Point result = new();
                Task<bool> successful;
                Task<(bool, PointType)> resultData;
                // TODO: U změny souřadnic kontrolovat i změnu buňky CELL14 !!
                switch (state)
                {
                    // vytvoření bodu
                    case State.AddToDB:
                        point = new Point
                        {
                            Name = items[i].Name,
                            Type = PointType.Pokestop,
                            Latitude = items[i].Latitude,
                            Longitude = items[i].Longitude,
                            IdCell14 = items[i].GetParent(14),
                            IdCell17 = items[i].GetParent(17),
                            NeedCheck = NeedCheck.No,
                            CheckedInfo = "N/A"
                        };

                        sb.Append(Emoji.GetEmoji(ctx, Emoji.Pokestop));
                        sb.AppendLine(" " + items[i].Name);                        
                        
                        AddToGymCellsAsync(point);

                        Methods.SendBoxMessage(chnl_out, "Vytvoření nového bodu.", sb.ToString(), DiscordColor.Green, items[i].Latitude, items[i].Longitude);
                         
                        count[0] += 1;
                        break;
                    // vytvoření bodu pro kontrolu
                    case State.AddToDBCheck:
                        // TODO: Oddělit StringBuilder do jedné přetížené metody v jiné třídě
                        sb.AppendLine(" " + items[i].Name);
                        sb.AppendLine();
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.Location));
                        sb.AppendLine(" Latitude: " + items[i].Latitude);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.Location));
                        sb.AppendLine(" Longitude: " + items[i].Longitude);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.Id));
                        sb.AppendLine(" Id Cell 17: " + items[i].GetParent(17));

                        resultData = AddCheckPoint(ctx, chnl_comm, items[i], "Nutná ruční kontrola a přidání.", sb.ToString(), DiscordColor.Orange);

                        sb.Insert(0, Emoji.GetEmoji(ctx, resultData.Result.Item2));

                        if (resultData.Result.Item1){
                            point = new Point
                            {
                                Name = items[i].Name,
                                Type = resultData.Result.Item2,
                                Latitude = items[i].Latitude,
                                Longitude = items[i].Longitude,
                                IdCell14 = items[i].GetParent(14),
                                IdCell17 = items[i].GetParent(17),
                                NeedCheck = NeedCheck.No,
                                CheckedInfo = "N/A"
                            };
                                                        
                            AddToGymCellsAsync(point);

                            Methods.SendBoxMessage(chnl_out, "Vytvoření nového bodu.", sb.ToString(), DiscordColor.Green, items[i].Latitude, items[i].Longitude);

                            if (resultData.Result.Item2 == PointType.Pokestop){count[0] += 1;}
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

                        sb.Insert(0, Emoji.GetEmoji(ctx, result.Type));

                        sb.AppendLine(" " + items[i].Name);
                        sb.AppendLine();
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
                        sb.AppendLine(" Name: " + result.Name);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
                        sb.AppendLine(" Name: " + items[i].Name);

                        result.Name = items[i].Name;

                        Methods.SendBoxMessage(chnl_out, "Změna názvu bodu.", sb.ToString(), DiscordColor.Yellow, items[i].Latitude, items[i].Longitude);

                        count[3] += 1;
                        break;
                    // změna souřadnic
                    case State.ChangePosition:
                        result = context.Points.FirstOrDefault(
                           x => x.Name == items[i].Name && x.IdCell17 == items[i].GetParent(17));                        

                        sb.Insert(0, Emoji.GetEmoji(ctx, result.Type));

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

                        Methods.SendBoxMessage(chnl_out, "Změna souřadnic.", sb.ToString(), DiscordColor.Yellow, items[i].Latitude, items[i].Longitude);

                        count[4] += 1;
                        break;
                    // změna souřadnic pro kontrolu
                    case State.ChangePositionCheck:
                        // TODO: zkontrolovat, zda se opravdu najde správný bod
                        result = context.Points.FirstOrDefault(
                           x => x.Name == items[i].Name && (x.Latitude == items[i].Latitude) || x.Longitude == items[i].Longitude);

                        string newIdCell14 = Item.GetParentFromCoor(items[i].Latitude, items[i].Longitude, 14);
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
                        sb.AppendLine(" Id Cell 14: " + result.IdCell14);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
                        sb.AppendLine(" Id Cell 14: " + newIdCell14);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
                        sb.AppendLine(" Id Cell 17: " + result.IdCell17);
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
                        sb.AppendLine(" Id Cell 17: " + newIdCell17);

                        successful = CheckPoint(ctx, chnl_comm, result, "Nutná ruční kontrola polohy.", sb.ToString(), DiscordColor.Orange);                        

                        // TODO: Proč se někde používá sb.Insert a jinde sb.Append
                        sb.Insert(0, Emoji.GetEmoji(ctx, result.Type));

                        if (successful.Result)
                        {
                            result.Latitude = items[i].Latitude;
                            result.Longitude = items[i].Longitude;
                            result.IdCell14 = newIdCell14;
                            result.IdCell17 = newIdCell17;

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
                        result.NeedCheck = NeedCheck.Yes;

                        sb.Append(Emoji.GetEmoji(ctx, Emoji.Warning));
                        sb.AppendLine(" " + items[i].Name);

                        Methods.SendBoxMessage(chnl_out, "CHYBA - NUTNÁ KONTROLA V DATABÁZI. (Unreachable)", sb.ToString(), DiscordColor.Red, items[i].Latitude, items[i].Longitude);
                        count[7] += 1;
                        break;
                    // pokud nenastane ani jedna předchozí možnost
                    default:
                        sb.Append(Emoji.GetEmoji(ctx, Emoji.Warning));
                        sb.AppendLine(" " + items[i].Name);
                        result.NeedCheck = NeedCheck.Yes;

                        Methods.SendBoxMessage(chnl_out, "CHYBA - NUTNÁ KONTROLA V DATABÁZI. (Default)", sb.ToString(), DiscordColor.Red, items[i].Latitude, items[i].Longitude);
                        count[8] += 1;
                        break;
                }
                await context.SaveChangesAsync();
            }

            await Task.Delay(5000);
            await context.SaveChangesAsync();

            StringBuilder sbr = new();
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

                title = (i+1) + "/" + points.Count + " " + Emoji.GetEmoji(ctx, points[i].Type) + " " + points[i].Type + " " + points[i].Name;

                resultData = CheckData.ChangeType(ctx, chnl_out, points[i], title, sb.ToString(), DiscordColor.Orange);
                if (resultData.Result.Item1)
                {
                    sb = new StringBuilder();
                    sb.AppendLine(points[i].Name);
                    sb.AppendLine();
                    sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
                    sb.AppendLine(" Původní typ: " + Emoji.GetEmoji(ctx,points[i].Type) + " " + points[i].Type);
                    sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
                    sb.AppendLine(" Nový typ: " + Emoji.GetEmoji(ctx, resultData.Result.Item2) + " " + resultData.Result.Item2.ToString());

                    points[i].Type = resultData.Result.Item2;
                    points[i].UpdatedAt = DateTime.Now;
                    
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

                title = (i+1) + "/" + points.Count + " " + Emoji.GetEmoji(ctx, points[i].Type) + " " + points[i].Name;

                resultData = CheckData.ChangeName(ctx, chnl_out, points[i], title, sb.ToString(), DiscordColor.Orange);
                if (resultData.Result.Item1)
                {
                    sb = new StringBuilder();
                    sb.AppendLine(Emoji.GetEmoji(ctx, points[i].Type) + " " + resultData.Result.Item2);
                    sb.AppendLine();
                    sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
                    sb.AppendLine(" Původní název: " + points[i].Name);
                    sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
                    sb.AppendLine(" Nový název: " + resultData.Result.Item2);

                    points[i].Name = resultData.Result.Item2;
                    points[i].UpdatedAt = DateTime.Now;
                    
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
            List<Point> points = context.Points.Where(i => i.NeedCheck == NeedCheck.Yes).ToList();

            if (points.Count == 0)
            {
                StringBuilder sbu = new();
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

                title = (i + 1) + "/" + points.Count + " " + Emoji.GetEmoji(ctx, points[i].Type) + " " + originalPoint.Name;

                resultData = ManualCheck(ctx, chnl_out, points[i], title, sb.ToString(), DiscordColor.Orange);
                // TODO: Zde odchytávat nastalé události a vypisovat zde do chatu co se stalo.
                // TODO: Vypisovat správný textbox podle změny
                switch (resultData.Result.Item1)
                {
                    case ManualChange.Name:
                        points[i].Name = resultData.Result.Item2;
                        points[i].NeedCheck = NeedCheck.Checked;
                        points[i].CheckedInfo = "Změna názvu.";
                        await context.SaveChangesAsync();
                        Methods.SendBoxMessage(ctx, chnl_out, "Změna názvu proběhla úspěšně.", originalPoint.Name, points[i].Name, points[i]);
                        break;
                    case ManualChange.Type:
                        switch (resultData.Result.Item2)
                        {
                            case "Pokestop":
                                points[i].Type = PointType.Pokestop;
                                points[i].NeedCheck = NeedCheck.Checked;
                                points[i].CheckedInfo = "Změna na Pokestop.";
                                await context.SaveChangesAsync();
                                Methods.SendBoxMessage(ctx, chnl_out, "Změna typu proběhla úspěšně.", points[i].Latitude, points[i].Longitude, points[i]);
                                break;
                            case "Gym":
                                points[i].Type = PointType.Gym;
                                points[i].NeedCheck = NeedCheck.Checked;
                                points[i].CheckedInfo = "Změna na Gym.";
                                await context.SaveChangesAsync();
                                Methods.SendBoxMessage(ctx, chnl_out, "Změna typu proběhla úspěšně.", points[i].Latitude, points[i].Longitude, points[i]);
                                break;
                            case "ExGym":
                                points[i].Type = PointType.ExGym;
                                points[i].NeedCheck = NeedCheck.Checked;
                                points[i].CheckedInfo = "Změna na ExGym.";
                                await context.SaveChangesAsync();
                                Methods.SendBoxMessage(ctx, chnl_out, "Změna typu proběhla úspěšně.", points[i].Latitude, points[i].Longitude, points[i]);
                                break;
                            case "Portal":
                                points[i].Type = PointType.Portal;
                                points[i].NeedCheck = NeedCheck.Checked;
                                points[i].CheckedInfo = "Změna na Portal.";
                                await context.SaveChangesAsync();
                                Methods.SendBoxMessage(ctx, chnl_out, "Změna typu proběhla úspěšně.", points[i].Latitude, points[i].Longitude, points[i]);
                                break;
                        }
                        
                        break;
                    case ManualChange.Duplicate:
                        Methods.SendBoxMessage(ctx, chnl_out, "Název je stejný jako předchozí.", points[i].Latitude, points[i].Longitude, points[i]);
                        break;
                    case ManualChange.Skip:
                        Methods.SendBoxMessage(ctx, chnl_out, "Změna zamítnuta.", points[i].Latitude, points[i].Longitude, points[i]);
                        break;
                    case ManualChange.Cancel:
                        Methods.SendBoxMessage(chnl_out, "Kontrola ukončena.", "", DiscordColor.Orange);
                        i = points.Count;
                        break;
                    case ManualChange.Coordinates:
                        points[i].Latitude = Double.Parse(resultData.Result.Item2.Split(" ")[0]);
                        points[i].Longitude = Double.Parse(resultData.Result.Item2.Split(" ")[1]);
                        points[i].NeedCheck = NeedCheck.Checked;
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
            //List<PokemonStat> pokemons = context.Pokemons.Where(i => i.Name.ToLower().Contains(searchString.ToLower())).ToList();
            return pokemons;
        }
        
        private async void AddToGymCellsAsync(Point point)
        {
            // TODO: Lépe využívat context / new PogoContext
            // TODO: V metodě vracet bool a kontrolovat, zda proběhlo zapsání v pořádku
            // TODO: Přidat try / catch - odchytávání výjimek
            using var context = new PogoContext();
            GymLocationCell gymCell = context.GymLocationCells.FirstOrDefault(i => i.IdCell14 == point.IdCell14);

            if (gymCell == null)
            {
                GymLocationCell newGymCell = new()
                {
                    Name = point.Name + " a okolí",
                    IdCell14 = point.IdCell14,
                    GymCount = 0,
                    PokestopCount = 0,
                    PortalCount = 0,
                    NeedCheck = NeedCheck.No,
                    CheckedInfo = "N/A"
                };

                context.GymLocationCells.Add(newGymCell);               
                await context.SaveChangesAsync();

                gymCell = context.GymLocationCells.FirstOrDefault(i => i.IdCell14 == point.IdCell14);
            }
            
            gymCell.Points.Add(point);            

            switch (point.Type)
            {
                case PointType.Pokestop:
                    gymCell.PokestopCount += 1;
                    break;
                case PointType.Gym:
                case PointType.ExGym:
                    gymCell.GymCount += 1;
                    break;
                case PointType.Portal:
                    gymCell.PortalCount += 1;
                    break;
            }            
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
                if (p_lat.PointId == p_lng.PointId)
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
                        if (p_name.PointId != p_cell.PointId) { return (State.ChangePositionCheck); }
                        return (State.ChangePosition);
                    }
                }
                // Souřadnice jsou v DB -> duplikát nebo chyba
                if (p_lat.PointId == p_lng.PointId)
                {
                    if (p_cell == null) { return (State.Unreachable); }
                    else { return (State.Duplicate); }
                }
            }
            return (State.Unreachable);

        }
    }
}
