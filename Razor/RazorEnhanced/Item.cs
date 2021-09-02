using Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace RazorEnhanced
{
    /// <summary>
    /// The Item class represent a single in-game Item object. Examples of Item are: Swords, bags, bandages, reagents, clothing.
    /// While the Item.Serial is unique for each Item, Item.ItemID is the unique for the Item apparence, or image. Sometimes is also called ID or Graphics ID.
    /// Item can also be house foriture as well as decorative items on the ground, like lamp post and banches.
    /// However, for Item on the ground that cannot be picked up, they might be part of the world map, see Statics class.
    /// </summary>
	public class Item : EnhancedEntity
    {
        private readonly Assistant.Item m_AssistantItem;

        internal Item(Assistant.Item item)
            : base(item)
        {
            m_AssistantItem = item;
        }

        /// <summary>
        /// Check if the Item already have been updated with all the properties. (need better documentation) 
        /// </summary>
		public bool Updated { get { return m_AssistantItem.Updated; } }

        /// <summary>
        /// Represents the type of Item, usually unique for the Item image.  Sometime called ID or Graphics ID.
        /// </summary>
		public int ItemID
        {
            get
            {
                if (m_AssistantItem != null)
                    return m_AssistantItem.ItemID.Value;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Read amount from item type object.
        /// </summary>
        public int Amount { get { return m_AssistantItem.Amount; } }

        /// <summary>
        /// Item direction. 
        /// </summary>
		public string Direction { get { return m_AssistantItem.Direction.ToString(); } }

        /// <summary>
        /// Item is Visible
        /// </summary>
		public bool Visible { get { return m_AssistantItem.Visible; } }

        /// <summary>
        /// Item is movable
        /// </summary>
		public bool Movable { get { return m_AssistantItem.Movable; } }

        /// <summary>
        /// Item name
        /// </summary>
		public string Name { get { return m_AssistantItem.Name; } }

        /// <summary>
        /// Gets the Layer, for werable items only. (need better documentation) 
        /// </summary>
		public string Layer { get { return m_AssistantItem.Layer.ToString(); } }

        /// <summary>
        /// Serial of the container which contains the object.
        /// </summary>
		public int Container
        {
            get
            {
                if (m_AssistantItem.Container is Assistant.Item)
                    return (m_AssistantItem.Container as Assistant.Item).Serial;
                else if (m_AssistantItem.Container is Assistant.Mobile)
                    return (m_AssistantItem.Container as Assistant.Mobile).Serial;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Get serial of root container of item.
        /// </summary>
		public int RootContainer
        {
            get
            {
                if (m_AssistantItem.RootContainer is Assistant.Item)
                    return (m_AssistantItem.RootContainer as Assistant.Item).Serial;
                else if (m_AssistantItem.RootContainer is Assistant.Mobile)
                    return (m_AssistantItem.RootContainer as Assistant.Mobile).Serial;
                else
                    return 0;
            }
        }

        /// <summary>
        /// True: if Properties are updated - False: otherwise.
        /// </summary>
		public bool PropsUpdated { get { return m_AssistantItem.PropsUpdated; } }

        /// <summary>
        /// Check if an Item is contained in a container. Can be a Item or a Mobile (wear by).
        /// </summary>
        /// <param name="container">Item as container.</param>
        /// <returns>True: if is contained - False: otherwise.</returns>
		public bool IsChildOf(Item container)
        {
            return m_AssistantItem.IsChildOf(container);
        }

        /// <param name="container">Mobile as container.</param>
		public bool IsChildOf(Mobile container)
        {
            return m_AssistantItem.IsChildOf(container);
        }

        /// <summary>
        /// Return the distance in number of tiles, from Item to Mobile.
        /// </summary>
        /// <param name="mob">Target as Mobile</param>
        /// <returns>Distance in number of tiles.</returns>
        public int DistanceTo(Mobile mob)
        {
            return Utility.Distance(Position.X, Position.Y, mob.Position.X, mob.Position.Y);
        }
        /// <param name="itm">Target as Item</param>
        public int DistanceTo(Item itm)
        {
            return Utility.Distance(Position.X, Position.Y, itm.Position.X, itm.Position.Y);
        }

        /// <summary>
        /// Contains the list of Item inside a container.
        /// </summary>
        public List<Item> Contains
        {
            get
            {
                List<Item> items = new List<Item>();
                for (int i = 0; i < m_AssistantItem.Contains.Count; i++)
                {
                    RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(m_AssistantItem.Contains[i]);
                    items.Add(enhancedItem);
                }
                return items;
            }
        }

        // possibly 4 bit x/y - 16x16?
        /// <summary>
        /// Returns the GridNum of the item. (need better documentation) 
        /// </summary>
        public byte GridNum { get { return m_AssistantItem.GridNum; } }

        /// <summary>
        /// True: if the item is on the ground - False: otherwise.
        /// </summary>
		public bool OnGround { get { return m_AssistantItem.OnGround; } }

        /// <summary>
        /// True: if the item is a container - False: otherwise.
        /// </summary>
		public bool IsContainer { get { return m_AssistantItem.IsContainer; } }

        /// <summary>
        /// True: if the item is a bag of sending - False: otherwise.
        /// </summary>
		public bool IsBagOfSending { get { return m_AssistantItem.IsBagOfSending; } }

        /// <summary>
        /// True: if the item is in the Player's backpack - False: otherwise.
        /// </summary>
        internal bool IsInBackpack
        { get { return m_AssistantItem.IsInBackpack; } }

        /// <summary>
        /// True: if the item is lootable - False: otherwise.
        /// </summary>
        internal bool IsLootableTarget
        { get { return m_AssistantItem.IsLootableTarget; } }

        /// <summary>
        /// True: if the item is in the Player's bank - False: otherwise.
        /// </summary>
        public bool IsInBank { get { return m_AssistantItem.IsInBank; } }

        /// <summary>
        /// True: if the item is a pouch - False: otherwise.
        /// </summary>

        public bool IsSearchable { get { return m_AssistantItem.IsSearchable; } }

        /// <summary>
        /// True: if the item is a corpse - False: otherwise.
        /// </summary>
		public bool IsCorpse { get { return m_AssistantItem.IsCorpse; } }

        /// <summary>
        /// True: if the item is a door - False: otherwise.
        /// </summary>
        public bool IsDoor { get { return m_AssistantItem.IsDoor; } }

        /// <summary>
        /// True: For regualar items - False: for hair, beards, etc.
        /// </summary>
        public bool IsLootable { get { return m_AssistantItem.IsLootable; } }

        /// <summary>
        /// True: if the item is a resource (ore, sand, wood, stone, fish) - False: otherwise
        /// </summary>
        public bool IsResource { get { return m_AssistantItem.IsResource; } }

        /// <summary>
        /// True: if the item is a potion - False: otherwise.
        /// </summary>
		public bool IsPotion { get { return m_AssistantItem.IsPotion; } }


        /// <summary>
        /// True: if the item is a virtue shield - False: otherwise.
        /// </summary>
		public bool IsVirtueShield { get { return m_AssistantItem.IsVirtueShield; } }

        /// <summary>
        /// True: if the item is a 2-handed weapon - False: otherwise.
        /// </summary>
		public bool IsTwoHanded { get { return m_AssistantItem.IsTwoHanded; } }

        public override string ToString()
        {
            return m_AssistantItem.ToString();
        }

        /// <summary>
        /// @nodoc
        /// @experimental
        /// Price of a recently purchased item. (see Vendor class )
        /// </summary>
		public int Price { get { return m_AssistantItem.Price; } }

        /// <summary>
        /// @nodoc
        /// @experimental
        /// Descrition of a recently purchased item. (see Vendor class )
        /// </summary>
		public string BuyDesc { get { return m_AssistantItem.BuyDesc; } }

        public Point3D GetWorldPosition()
        {
            Assistant.Point3D assistantPoint = m_AssistantItem.GetWorldPosition();
            RazorEnhanced.Point3D enhancedPoint = new RazorEnhanced.Point3D(assistantPoint);
            return enhancedPoint;
        }

        internal Assistant.Layer AssistantLayer { get { return m_AssistantItem.Layer; } }

        /// <summary>
        /// Get the list of Properties of an Item.
        /// </summary>
		public List<Property> Properties
        {
            get
            {
                List<Property> properties = new List<Property>();
                foreach (Assistant.ObjectPropertyList.OPLEntry entry in m_AssistantItem.ObjPropList.Content)
                {
                    Property property = new Property(entry);
                    properties.Add(property);
                }
                return properties;
            }
        }

        /// <summary>
        /// Get the weight of a item. (0: no weight)
        /// </summary>
		public int Weight
        {
            get
            {
                List<Property> properties = Properties;
                foreach (Property property in properties)
                {
                    int number = property.Number;
                    string args = property.Args;
                    switch (number)
                    {
                        case 1072788:
                            return 1;       // Peso 1 se cliloc � 1072788
                        case 1072789:
                            try
                            {
                                return Convert.ToInt32(args);  // Ritorna valore peso
                            }
                            catch
                            {
                                return 1;  // errore di conversione torna peso  1
                            }
                    }
                }
                return 0;  // item senza peso
            }
        }

        /// <summary>
        /// Get the current durability of an Item. (0: no durability)
        /// </summary>
		public int Durability
        {
            get
            {
                List<Property> properties = Properties;
                foreach (Property property in properties)
                {
                    int number = property.Number;
                    if (number != 1060639)
                        continue;

                    string Text = property.Args;
                    int step = 0;
                    string Durability = String.Empty;

                    for (int i = 0; i <= Text.Length - 1; i++)
                    {
                        if (step == 0)
                            if (Char.IsNumber(Text[i]))
                            {
                                Durability += Text[i];
                                step = 1;
                                i++;
                            }
                        if (step == 1)
                            if (Char.IsNumber(Text[i]))
                            {
                                Durability += Text[i];
                            }
                            else
                                step = 2;
                    }

                    try
                    {
                        return Convert.ToInt32(Durability);  // Ritorna valore Dur
                    }
                    catch
                    {
                        return 0;  // errore di conversione torna 0
                    }
                }
                return 0; // item senza Dur
            }
        }

        /// <summary>
        /// Get the maximum durability of an Item. (0: no durability)
        /// </summary>
		public int MaxDurability
        {
            get
            {
                List<Property> properties = Properties;
                foreach (Property property in properties)
                {
                    int number = property.Number;
                    if (number != 1060639)
                        continue;

                    string Text = property.Args;
                    string TempMaxDurability = String.Empty;
                    int step = 0;
                    string MaxDurability = String.Empty;
                    for (int y = Text.Length - 1; y != 0; y--)
                    {
                        if (step == 0)
                            if (Char.IsNumber(Text[y]))
                            {
                                TempMaxDurability += Text[y];
                                step = 1;
                                y--;
                            }
                        if (step == 1)
                            if (Char.IsNumber(Text[y]))
                            {
                                TempMaxDurability += Text[y];
                            }
                            else
                                step = 2;
                    }
                    for (int i = TempMaxDurability.Length - 1; i > -1; i--)
                    {
                        MaxDurability += TempMaxDurability[i];
                    }
                    try
                    {
                        return Convert.ToInt32(MaxDurability);  // Ritorna valore maxdur
                    }
                    catch
                    {
                        return 0;  // errore di conversione torna 0
                    }
                }
                return 0; // item senza maxdur
            }
        }

        /// <summary>
        /// Get the in-game image on an Item as Bitmap object.
        /// See MSDN: https://docs.microsoft.com/dotnet/api/system.drawing.bitmap
        /// </summary>
		public System.Drawing.Bitmap Image
        {
            get
            {
                return Items.GetImage(m_AssistantItem.ItemID, m_AssistantItem.Hue);
            }
        }
    }

    /// <summary>
    /// The Items class provides a wide range of functions to search and interact with Items.
    /// </summary>
	public class Items
    {

        /// <summary>
        /// Open a container an wait for the Items to load, for a maximum amount of time.
        /// </summary>
        /// <param name="bag">Container as Item object.</param>
        /// <param name="delay">Maximum wait, in milliseconds.</param>
        public static void WaitForContents(Item bag, int delay) // Delay in MS
        {
            if (!bag.IsCorpse && !bag.IsContainer)
                return;

            RazorEnhanced.Items.UseItem(bag);

            if (bag.Updated)
                return;

            int subdelay = delay;
            while (!bag.Updated)
            {
                Thread.Sleep(2);
                subdelay -= 2;
                if (subdelay <= 0)
                    break;
            }
        }

        /// <param name="bag_serial">Container as Item serial.</param>
        /// <param name="delay">max time to wait for contents</param>
        public static void WaitForContents(int bag_serial, int delay) // Delay in MS
        {
            Item bag = FindBySerial(bag_serial);
            if (bag != null)
                WaitForContents(bag, delay);
        }

        private static readonly Dictionary<uint, int> m_HuedItems = new Dictionary<uint, int>();

        internal static int Hued(uint serial)
        {
            if (m_HuedItems.ContainsKey(serial))
                return m_HuedItems[serial];
            return -1;
        }


        /// <summary>
        /// Change/override the Color of an Item, the change affects only Player client. The change is not persistent.
        /// If the color is -1 or unspecified, the color of the item is restored.
        /// </summary>
        /// <param name="serial">Serial of the Item.</param>
        /// <param name="color">Color as number. (default: -1, reset original color)</param>
        ///
        public static void SetColor(int serial, int color = -1)
        {
            //Reset original color
            if (color == -1)
            {
                try
                {
                    m_HuedItems.Remove((uint)serial);
                }
                catch (Exception) { }

                return;
            }

            // store the setting even if item is not exist yet
            m_HuedItems[(uint)serial] = color;
            RazorEnhanced.Item i = RazorEnhanced.Items.FindBySerial(serial);
            Assistant.Item assistantItem = Assistant.World.FindItem((Assistant.Serial)((uint)serial));
            if (assistantItem == null)
                return;

            if (i.Container == World.Player.Serial)
            {
                Assistant.Client.Instance.SendToClient(new EquipmentItem(assistantItem, (ushort)color, World.Player.Serial));
                return;
            }

            // if not worn Apply color for valid flag
            assistantItem.Hue = (ushort)color;
            if (i.Container == 0)
            {
                if ((assistantItem.ItemID & 0x4000) == 0x4000)
                    Assistant.Client.Instance.SendToClient(new SAWorldItem(assistantItem));
                else
                    Assistant.Client.Instance.SendToClient(new WorldItem(assistantItem));
            }
            else
                Assistant.Client.Instance.SendToClient(new ContainerItem(assistantItem, true));
        }

        /// <summary>
        /// @nodoc: Method ranamed to SetColor, to be removed.
        /// </summary>
        public static void Color(int serial, int color = -1) {
            SetColor(serial, color);
        }

        /// <summary>
        /// Use the Dyes on a Dyeing Tub and select the color via color picker, using dedicated packets. 
        /// Need to specify the dyes, the dye tube and the color to use.
        /// </summary>
        /// <param name="dyes">Dyes as Item object.</param>
        /// <param name="dyeingTub">Dyeing Tub as Item object.</param>
        /// <param name="color">Color to choose.</param>
        public void ChangeDyeingTubColor(Item dyes, Item dyeingTub, int color)
        {
            Items.UseItem(dyes);
            if (Target.WaitForTarget(1000))
            {
                var cb = new HueEntry.HueEntryCallback(
                    (serial, iid, hue) =>
                    {
                        HueEntry.Callback = null;
                        Assistant.Client.Instance.SendToServer(new HuePicker(serial, iid, (ushort)color));
                    }
                );
                HueEntry.Callback = cb;
                Target.TargetExecute(dyeingTub);

                // Dalamar: Callback AutoCleanup
                // If the selected object doesn't prompt for a Color Picker (any other object, including some cusom dyeing tub) the callback would remain pending, waiting for a packet which will never arrive.
                // Eventually, the first time a player try to dye something, the callback finally fire and apply the last color tried.
                // As the packet response usually arrives very fast, we will safely assume the following:
                //    if after X seconds the original callback is still waiting for a packet, probably something went wrong and the callback gets removed ( probably the packet will never arrive )
                var cleanup = new Thread( () => {
                    Thread.Sleep(2000);
                    if (HueEntry.Callback == cb) { HueEntry.Callback = null; } 
                });
                cleanup.Start();

            }
        }

        /// <summary>
        /// The Items.Filter class is used to store options to filter the global Item list.
        /// Often used in combination with Items.ApplyFilter.
        /// </summary>
        public class Filter
        {
            /// <summary>
            /// True: The filter is used - False: Return all Item. ( default: True, active )
            /// </summary>
			public bool Enabled = true;

            /// <summary>
            /// Limit the search to a list of Serials of Item to find. (ex: 0x0406EFCA )
            /// Supports .Add() and .AddRange()
            /// </summary>
			public List<int> Serials = new List<int>();

            /// <summary>
            /// Limit the search to a list of Grapichs ID (see: Item.ItemID ) 
            /// Supports .Add() and .AddRange()
            /// </summary>
			public List<int> Graphics = new List<int>();

            /// <summary>
            /// Limit the search by name of the Item.
            /// </summary>
			public string Name = String.Empty;

            /// <summary>
            /// Limit the search to a list of Colors.
            /// Supports .Add() and .AddRange()
            /// </summary>
			public List<int> Hues = new List<int>();

            /// <summary>
            /// Limit the search by distance, to Items on the ground which are at least RangeMin tiles away from the Player. ( default: -1, any Item )
            /// </summary>
			public double RangeMin = -1;
            /// <summary>
            /// Limit the search by distance, to Items on the ground which are at most RangeMax tiles away from the Player. ( default: -1, any Item )
            /// </summary>
			public double RangeMax = -1;
            /// <summary>
            /// Limit the search to only Movable Items. ( default: -1, any Item )
            /// </summary>
			public int Movable = -1;
            /// <summary>
            /// Exclude from the search Items which are currently on the global Ignore List. ( default: False, any Item )
            /// </summary>
			public bool CheckIgnoreObject = false;
            /// <summary>
            /// Limit the search to the wearable Items by Layer.
            /// Supports .Add() and .AddRange()
            /// 
            /// Layers:
            ///     RightHand
            ///     LeftHand
            ///     Shoes
            ///     Pants
            ///     Shirt
            ///     Head
            ///     Gloves
            ///     Ring
            ///     Neck
            ///     Waist
            ///     InnerTorso
            ///     Bracelet
            ///     MiddleTorso
            ///     Earrings
            ///     Arms
            ///     Cloak
            ///     OuterTorso
            ///     OuterLegs
            ///     InnerLegs
            ///     Talisman
            /// </summary>
			public List<string> Layers = new List<string>();
            /// <summary>
            /// Limit the search to the Items on the ground. (default: -1, any Item)
            /// </summary>
			public int OnGround = -1;
            /// <summary>
            /// Limit the search to the corpses on the ground. (default: -1, any Item)
            /// </summary>
			public int IsCorpse = -1;
            /// <summary>
            /// Limit the search to the Items which are also containers. (default: -1: any Item)
            /// </summary>
			public int IsContainer = -1;
            /// <summary>
            /// Limit the search to the doors. (default: -1: any Item)
            /// </summary>
            public int IsDoor = -1;

            public Filter()
            {
            }
        }

        /// <summary>
        /// Filter the global list of Items according to the options specified by the filter ( see: Items.Filter ).
        /// </summary>
        /// <param name="filter">A filter object.</param>
        /// <returns>the list of Items respectinf the filter criteria.</returns>
		public static List<Item> ApplyFilter(Filter filter)
        {
            List<Item> result = new List<Item>();
            List<Assistant.Item> assistantItems = new List<Assistant.Item>(World.Items.Values.ToList());

            try
            {
                if (filter.Enabled)
                {
                    if (filter.Serials.Count > 0)
                    {
                        assistantItems = assistantItems.Where((i) => filter.Serials.Contains((int)i.Serial.Value)).ToList();
                    }
                    else
                    {
                        if (filter.Name != String.Empty)
                        {
                            Regex rgx = new Regex(filter.Name, RegexOptions.IgnoreCase);
                            List<Assistant.Item> list = new List<Assistant.Item>();
                            foreach (Assistant.Item i in assistantItems)
                            {
                                if (rgx.IsMatch(i.Name))
                                {
                                    list.Add(i);
                                }
                            }
                            assistantItems = list;
                        }

                        if (filter.Graphics.Count > 0)
                        {
                            assistantItems = assistantItems.Where((i) => filter.Graphics.Contains(i.ItemID.Value)).ToList();
                        }

                        if (filter.Hues.Count > 0)
                        {
                            assistantItems = assistantItems.Where((i) => filter.Hues.Contains(i.Hue)).ToList();
                        }

                        if (filter.RangeMin != -1)
                        {
                            assistantItems = assistantItems.Where((i) =>
                                Utility.Distance(World.Player.Position.X, World.Player.Position.Y, i.Position.X, i.Position.Y) >= filter.RangeMin
                            ).ToList();
                        }

                        if (filter.RangeMax != -1)
                        {
                            assistantItems = assistantItems.Where((i) =>
                                Utility.Distance(World.Player.Position.X, World.Player.Position.Y, i.Position.X, i.Position.Y) <= filter.RangeMax
                            ).ToList();
                        }

                        if (filter.Movable >= 0)
                        {
                            assistantItems = assistantItems.Where((i) => i.Movable == (filter.Movable > 0)).ToList();
                        }

                        if (filter.Layers.Count > 0)
                        {
                            List<Assistant.Layer> list = new List<Assistant.Layer>();

                            foreach (string text in filter.Layers)
                            {
                                Enum.TryParse<Layer>(text, out Layer l);
                                if (l != Assistant.Layer.Invalid)
                                {
                                    list.Add(l);
                                }
                            }

                            assistantItems = assistantItems.Where((i) => list.Contains(i.Layer)).ToList();
                        }

                        if (filter.OnGround != -1)
                        {
                            assistantItems = assistantItems.Where((i) => i.OnGround == Convert.ToBoolean(filter.OnGround)).ToList();
                        }

                        if (filter.IsContainer != -1)
                        {
                            assistantItems = assistantItems.Where((i) => i.IsContainer == Convert.ToBoolean(filter.IsContainer)).ToList();
                        }

                        if (filter.IsCorpse != -1)
                        {
                            assistantItems = assistantItems.Where((i) => i.IsCorpse == Convert.ToBoolean(filter.IsCorpse)).ToList();
                        }

                        if (filter.IsDoor != -1)
                        {
                            assistantItems = assistantItems.Where((i) => i.IsDoor == Convert.ToBoolean(filter.IsDoor)).ToList();
                        }

                        if (filter.CheckIgnoreObject)
                        {
                            assistantItems = assistantItems.Where((i) => Misc.CheckIgnoreObject(i.Serial) != true).ToList();
                        }
                    }
                }

            }
            catch { }

            foreach (Assistant.Item assistantItem in assistantItems)
            {
                RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(assistantItem);
                result.Add(enhancedItem);
            }

            return result;
        }

        /// <summary>
        /// Select a single Item from a list by some criteria: Distance, Amount, Durability or Randomly
        /// </summary>
        /// <param name="items">List of Item.</param>
        /// <param name="selector">
        ///     Nearest 
        ///     Farthest 
        ///     Less 
        ///     Most 
        ///     Weakest 
        ///     Strongest
        ///     Random
        /// </param>
        /// <returns>The selected item.</returns>
        public static Item Select(List<Item> items, string selector)
        {
            Item result = null;

            if (items.Count <= 0)
                return null;

            switch (selector)
            {
                case "Random":
                    result = items[Utility.Random(items.Count)] as Item;
                    break;

                case "Nearest":
                    Item nearest = items[0];
                    if (nearest != null)
                    {
                        double minDist = Misc.DistanceSqrt(Player.Position, nearest.Position);
                        foreach (Item t in items)
                        {
                            if (t == null)
                                continue;

                            double dist = Misc.DistanceSqrt(Player.Position, t.Position);

                            if (!(dist < minDist))
                                continue;

                            nearest = t;
                            minDist = dist;
                        }
                        result = nearest;
                    }
                    break;

                case "Farthest":
                    Item farthest = items[0];
                    if (farthest != null)
                    {
                        double maxDist = Misc.DistanceSqrt(Player.Position, farthest.Position);
                        foreach (Item t in items)
                        {
                            if (t == null)
                                continue;

                            double dist = Misc.DistanceSqrt(Player.Position, t.Position);
                            if (dist > maxDist)
                            {
                                farthest = t;
                                maxDist = dist;
                            }
                        }
                        result = farthest;
                    }
                    break;

                case "Less":
                    Item least = items[0];
                    if (least != null)
                    {
                        int minAmount = least.Amount;
                        foreach (Item t in items)
                        {
                            if (t == null)
                                continue;

                            int amount = t.Amount;
                            if (amount < minAmount)
                            {
                                least = t;
                                minAmount = amount;
                            }
                        }
                        result = least;
                    }
                    break;

                case "Most":
                    Item most = items[0];
                    if (most != null)
                    {
                        int maxAmount = most.Amount;
                        foreach (Item t in items)
                        {
                            if (t == null)
                                continue;

                            int amount = t.Amount;

                            if (amount <= maxAmount)
                                continue;

                            most = t;
                            maxAmount = amount;
                        }
                        result = most;
                    }
                    break;

                case "Weakest":
                    Item weakest = items[0];
                    if (weakest != null)
                    {
                        int minDur = weakest.Durability;
                        foreach (Item t in items)
                        {
                            if (t == null)
                                continue;

                            int dur = t.Durability;

                            if (dur >= minDur)
                                continue;

                            weakest = t;
                            minDur = dur;
                        }
                        result = weakest;
                    }
                    break;

                case "Strongest":
                    Item strongest = items[0];
                    if (strongest != null)
                    {
                        int maxDur = strongest.Durability;
                        foreach (Item t in items)
                        {
                            if (t == null)
                                continue;

                            int dur = t.Durability;

                            if (dur <= maxDur)
                                continue;
                            strongest = t;
                            maxDur = dur;
                        }
                        result = strongest;
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// Search for a specific Item by using it Serial
        /// </summary>
        /// <param name="serial">Serial of the Item.</param>
        /// <returns>Item object if found, or null if not found.</returns>
		public static Item FindBySerial(int serial)
        {
            Assistant.Item assistantItem = Assistant.World.FindItem((Assistant.Serial)((uint)serial));
            if (assistantItem == null)
                return null;
            else
            {
                RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(assistantItem);
                return enhancedItem;
            }
        }

        /// <summary>
        /// Lift an Item and hold it in-hand. ( see: Items.DropFromHand )
        /// </summary>
        /// <param name="item">Item object to Lift.</param>
        /// <param name="amount">Amount to lift. (0: the whole stack)</param>
		public static void Lift(Item item, int amount)
        {
            if (item == null)
            {
                Scripts.SendMessageScriptError("Script Error: Move: Source Item  not found");
                return;
            }
            if (amount == 0)
            {
                Assistant.Client.Instance.SendToServerWait(new LiftRequest(item.Serial, item.Amount));
            }
            else
            {
                if (item.Amount < amount)
                {
                    amount = item.Amount;
                }
                Assistant.Client.Instance.SendToServerWait(new LiftRequest(item.Serial, amount));
            }
        }

        /// <summary>
        /// Drop into a bag an Item currently held in-hand. ( see: Items.Lift )
        /// </summary>
        /// <param name="item">Item object to drop.</param>
        /// <param name="container">Target container.</param>
		public static void DropFromHand(Item item, Item container)
        {
            if (item == null)
            {
                Scripts.SendMessageScriptError("Script Error: Move: Source Item  not found");
                return;
            }
            if (container == null)
            {
                Scripts.SendMessageScriptError("Script Error: Move: Destination Item not found");
                return;
            }
            if (!container.IsContainer)
            {
                Scripts.SendMessageScriptError("Script Error: Move: Destination Item is not a container");
                return;
            }
            Assistant.Client.Instance.SendToServerWait(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, container.Serial));
        }

        /// <summary>
        /// Move an Item to a destination, which can be an Item or a Mobile.
        /// </summary>
        /// <param name="source">Serial or Item of the Item to move.</param>
        /// <param name="destination">Serial, Mobile or Item as destination.</param>
        /// <param name="amount">Amount to move (-1: the whole stack)</param>
        /// <param name="x">Optional: X coordinate inside the container.</param>
        /// <param name="y">Optional: Y coordinate inside the container.</param>
        public static void Move(int source, int destination, int amount, int x, int y)
        {
            Assistant.Item bag = Assistant.World.FindItem(destination);
            Assistant.Item item = Assistant.World.FindItem(source);
            Assistant.Mobile mbag = null;

            int serialdestination = 0;
            bool isMobile = false;
            bool onLocation = false;
            if (item == null)
            {
                Scripts.SendMessageScriptError("Script Error: Move: Source Item  not found");
                return;
            }

            if (bag != null)
            {
                serialdestination = bag.Serial;
            }
            else
            {
                mbag = Assistant.World.FindMobile(destination);
                if (mbag != null)
                {
                    isMobile = true;
                    serialdestination = mbag.Serial;
                }
            }

            if (serialdestination == 0)
            {
                Scripts.SendMessageScriptError("Script Error: Move: Destination not found");
                return;
            }

            Assistant.Point3D loc = Assistant.Point3D.MinusOne;
            if (x != -1 && y != -1)
            {
                onLocation = true;
                loc = new Assistant.Point3D(x, y, 0);
            }

            int newamount;
            // calcolo amount
            if (amount == 0)
            {
                newamount = item.Amount;
            }
            else
            {
                if (item.Amount < amount)
                    newamount = item.Amount;
                else
                    newamount = amount;
            }

            if (isMobile)
                Assistant.DragDropManager.DragDrop(item, newamount, mbag.Serial);
            else if (onLocation)
                Assistant.DragDropManager.DragDrop(item, newamount, bag, loc);
            else
                Assistant.DragDropManager.DragDrop(item, newamount, bag);
        }

        /*public static void Move(int source, int destination, int amount, int x, int y)
		{
			Assistant.Item bag = Assistant.World.FindItem(destination);
			Assistant.Item item = Assistant.World.FindItem(source);
			int serialdestination = 0;

			if (item == null)
			{
				Scripts.SendMessageScriptError("Script Error: Move: Source Item  not found");
				return;
			}

			if (bag != null)
			{
				serialdestination = bag.Serial;
			}
			else
			{
				Assistant.Mobile mbag = Assistant.World.FindMobile(destination);
				if (mbag != null)
				{
					serialdestination = mbag.Serial;
				}
				else
				{
					Scripts.SendMessageScriptError("Script Error: Move: Destination not found");
					return;
				}
			}

			Assistant.Point3D loc = Assistant.Point3D.MinusOne;
			if (x != -1 && y != -1)
				loc = new Assistant.Point3D(x, y, 0);

			if (amount == 0)
			{
				Assistant.Client.Instance.SendToServerWait(new LiftRequest(item.Serial, item.Amount));
				Assistant.Client.Instance.SendToServerWait(new DropRequest(item.Serial, loc, serialdestination));
			}
			else
			{
				if (item.Amount < amount)
				{
					amount = item.Amount;
				}
				Assistant.Client.Instance.SendToServerWait(new LiftRequest(item.Serial, amount));
				Assistant.Client.Instance.SendToServerWait(new DropRequest(item.Serial, loc, serialdestination));
			}
		}*/

        public static void Move(Item source, Mobile destination, int amount)
        {
            Move(source.Serial, destination.Serial, amount, -1, -1);
        }

        public static void Move(int source, Mobile destination, int amount)
        {
            Move(source, destination.Serial, amount, -1, -1);
        }

        public static void Move(Item source, int destination, int amount)
        {
            Move(source.Serial, destination, amount, -1, -1);
        }

        public static void Move(int source, Item destination, int amount)
        {
            Move(source, destination.Serial, amount, -1, -1);
        }

        public static void Move(Item source, Item destination, int amount)
        {
            Move(source.Serial, destination.Serial, amount, -1, -1);
        }

        public static void Move(int source, int destination, int amount)
        {
            Move(source, destination, amount, -1, -1);
        }

        public static void Move(Item source, Mobile destination, int amount, int x, int y)
        {
            Move(source.Serial, destination.Serial, amount, x, y);
        }

        public static void Move(int source, Mobile destination, int amount, int x, int y)
        {
            Move(source, destination.Serial, amount, x, y);
        }

        public static void Move(Item source, int destination, int amount, int x, int y)
        {
            Move(source.Serial, destination, amount, x, y);
        }

        public static void Move(int source, Item destination, int amount, int x, int y)
        {
            Move(source, destination.Serial, amount, x, y);
        }

        public static void Move(Item source, Item destination, int amount, int x, int y)
        {
            Move(source.Serial, destination.Serial, amount, x, y);
        }




        /// <summary>
        /// Move an Item on the ground to a specific location.
        /// </summary>
        /// <param name="source">Serial or Item to move.</param>
        /// <param name="amount">Amount of Items to move (0: the whole stack )</param>
        /// <param name="x">X world coordinates.</param>
        /// <param name="y">Y world coordinates.</param>
        /// <param name="z">Z world coordinates.</param>
        public static void MoveOnGround(int source, int amount, int x, int y, int z)
        {
            Assistant.Item item = Assistant.World.FindItem(source);

            if (item == null)
            {
                Scripts.SendMessageScriptError("Script Error: MoveOnGround: Source Item  not found");
                return;
            }

            Assistant.Point3D loc = new Assistant.Point3D(x, y, z);

            int amounttodrop = amount;
            if ((item.Amount < amount) || (amount == 0))
                amounttodrop = item.Amount;

            Assistant.DragDropManager.DragDrop(item, loc, amounttodrop);
            //Assistant.Client.Instance.SendToServerWait(new LiftRequest(item.Serial, amounttodrop));
            //Assistant.Client.Instance.SendToServerWait(new DropRequest(item.Serial, loc, Assistant.Serial.MinusOne));
        }


        public static void MoveOnGround(Item source, int amount, int x, int y, int z)
        {
            MoveOnGround(source.Serial, amount, x, y, z);
        }

        /// <summary>
        /// Drop an Item on the ground, at the current Player position.
        /// NOTE: On some server is not allowed to drop Items on tiles occupied by Mobiles and the Player.
        /// </summary>
        /// <param name="item">Item object to drop.</param>
        /// <param name="amount">Amount to move. (default: 0, the whole stack)</param>
		public static void DropItemGroundSelf(Item item, int amount = 0)
        {
            if (item == null)
            {
                Scripts.SendMessageScriptError("Script Error: DropItemGroundSelf: Item not found");
                return;
            }

            int amounttodrop = amount;
            if ((item.Amount < amount) || (amount == 0))
                amounttodrop = item.Amount;

            MoveOnGround(item.Serial, amounttodrop, Player.Position.X, Player.Position.Y, Player.Position.Z);
        }

        public static void DropItemGroundSelf(int serialitem, int amount = 0)
        {
            Item i = FindBySerial(serialitem);
            DropItemGroundSelf(i, amount);
        }

        /// <summary>
        /// Use an Item, optionally is possible to specify a Item or Mobile target.
        /// NOTE: The optional target may not work on some free shards. Use Target.Execute instead.
        /// </summary>
        /// <param name="itemSerial">Serial or Item to use.</param>
        /// <param name="targetSerial">Optional: Serial of the Item or Mobile target.</param>
        /// <param name="wait">Optional: Wait for confirmation by the server. (default: True)</param>
        public static void UseItem(int itemSerial, int targetSerial, bool wait)
        {
            Assistant.Item item = Assistant.World.FindItem(itemSerial);
            if (item == null)
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: Invalid Use Serial");
                return;
            }

            Assistant.Item itemTarget = Assistant.World.FindItem(targetSerial);
            Assistant.Mobile mobileTarget = Assistant.World.FindMobile(targetSerial);
            if (itemTarget == null && mobileTarget == null)
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: Invalid Target Serial");
                return;
            }

            if (!item.Serial.IsItem)
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: (" + item.Serial.ToString() + ") is not an item");
                return;
            }
            if (itemTarget == null && !mobileTarget.Serial.IsMobile)
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: (" + targetSerial.ToString() + ") is not a mobile");
                return;
            }
            if (mobileTarget == null && !itemTarget.Serial.IsItem)
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: (" + targetSerial.ToString() + ") is not an item");
                return;
            }

            if (wait)
                Assistant.Client.Instance.SendToServerWait(new UseTargetedItem((uint)itemSerial, (uint)targetSerial));
            else
                Assistant.Client.Instance.SendToServer(new UseTargetedItem((uint)itemSerial, (uint)targetSerial));
        }

        public static void UseItem(int itemserial)
        {
            Assistant.Item item = Assistant.World.FindItem(itemserial);
            if (item == null)
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: Invalid Serial");
                return;
            }

            if (item.Serial.IsItem)
            {
                Assistant.Client.Instance.SendToServerWait(new DoubleClick(item.Serial));
            }
            else
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: (" + item.Serial.ToString() + ") is not a item");
            }
        }

        public static void UseItem(Item item)
        {
            if (item != null)
                UseItem(item.Serial);
        }

        public static void UseItem(Item item, EnhancedEntity target)
        {
            if (item == null || target == null)
                return;

            UseItem(item.Serial, target.Serial, true);
        }
        public static void UseItem(int item, EnhancedEntity target)
        {
            if (target == null)
                return;

            UseItem(item, target.Serial, true);
        }
        public static void UseItem(Item item, int target)
        {
            if (item == null)
                return;

            UseItem(item.Serial, target, true);
        }

        public static void UseItem(int itemSerial, int targetSerial)
        {
            UseItem(itemSerial, targetSerial, true);
        }


        /// <summary>
        /// Use any item of a specific type, matching Item.ItemID. Optionally also of a specific color, matching Item.Hue.
        /// </summary>
        /// <param name="itemid">ItemID to be used.</param>
        /// <param name="color">Color to be used. (default: -1, any)</param>
        /// <returns></returns>
        public static bool UseItemByID(int itemid, int color = -1)
        {
            // Genero filtro item
            Items.Filter itemFilter = new Items.Filter
            {
                Enabled = true
            };
            itemFilter.Graphics.Add(itemid);

            if (color != -1)
                itemFilter.Hues.Add(color);

            List<Item> containeritem = RazorEnhanced.Items.ApplyFilter(itemFilter);

            foreach (Item found in containeritem)
            {
                if (found.IsInBackpack)
                {
                    RazorEnhanced.Items.UseItem(found);
                    return true;
                }
            }

            return false;
        }

        // Find item by id
        /// <summary>
        /// Find a single Item matching specific ItemID, Color and Container. 
        /// Optionally can search in all subcontaners or to a maximum depth in subcontainers.
        /// Can use -1 on color for no chose color, can use -1 on container for search in all item in memory. The depth defaults to only the top but can search for # of sub containers.
        /// </summary>
        /// <param name="itemid">ItemID filter.</param>
        /// <param name="color">Color filter. (-1: any, 0: natural )</param>
        /// <param name="container">Serial of the container to search. (-1: any Item)</param>
        /// <param name="recursive">
        /// Search subcontainers. 
        ///     True: all subcontainers
        ///     False: only main
        ///     1,2,n: Maximum subcontainer depth
        /// </param>
        /// <param name="considerIgnoreList">True: Ignore Items are excluded - False: any Item.</param>
        /// <returns>The Item matching the criteria.</returns>
        public static Item FindByID(int itemid, int color, int container, bool recursive = false, bool considerIgnoreList = true)
        {
            if (container != -1)  // search in specific container
            {
                Item cont = FindBySerial(container);
                if (cont == null) // not valid serial or container not found
                {
                    Scripts.SendMessageScriptError("Script Error: FindByID: Container serial not found");
                    return null;
                }
                foreach (Item i in cont.Contains)
                {
                    if (considerIgnoreList && Misc.CheckIgnoreObject(i.Serial))
                        continue;

                    if (i.ItemID == itemid) // check item id
                    {
                        if (color != -1) // color -1 ALL
                        {
                            if (i.Hue == color)
                                return i;
                        }
                        else
                        {
                            return i;
                        }
                    }
                    else if (recursive && i.IsContainer)
                    {
                        Item recursItem = FindByID(itemid, color, i.Serial, recursive, considerIgnoreList); // recall for sub container
                        if (recursItem != null)
                            return recursItem;
                    }
                }
                return null; // Return null if no item found
            }
            else  // Search in world
            {
                Items.Filter itemFilter = new Items.Filter
                {
                    Enabled = true
                };
                itemFilter.Graphics.Add(itemid);
                itemFilter.CheckIgnoreObject = considerIgnoreList;

                if (color != -1)
                    itemFilter.Hues.Add(color);

                List<Item> containeritem = RazorEnhanced.Items.ApplyFilter(itemFilter);

                foreach (Item found in containeritem)  // Return frist one found
                    return found;

                return null;
            }
        }

        public static Item FindByID(int itemid, int color, int container, int range, bool considerIgnoreList = true)
        {
            if (container != -1)  // search in specific container
            {
                // range should be # of packs deep to search .. but not implemented
                Item cont = FindBySerial(container);
                if (cont == null) // not valid serial or container not found
                {
                    Scripts.SendMessageScriptError("Script Error: FindByID: Container serial not found");
                    return null;
                }
                foreach (Item i in cont.Contains)
                {
                    if (considerIgnoreList && Misc.CheckIgnoreObject(i.Serial))
                        continue;

                    if (i.ItemID == itemid) // check item id
                    {
                        if (color != -1) // color -1 ALL
                        {
                            if (i.Hue == color)
                                return i;
                        }
                        else
                        {
                            return i;
                        }
                    }
                    else if (i.IsContainer && range != 0)
                    {
                        Item recursItem = FindByID(itemid, color, i.Serial, range - 1, considerIgnoreList); // recall for sub container
                        if (recursItem != null)
                            return recursItem;
                    }
                }
                return null; // Return null if no item found
            }
            else  // Search in world
            {
                Items.Filter itemFilter = new Items.Filter
                {
                    Enabled = true
                };
                itemFilter.Graphics.Add(itemid);
                itemFilter.RangeMax = range;
                itemFilter.CheckIgnoreObject = considerIgnoreList;

                if (color != -1)
                    itemFilter.Hues.Add(color);

                List<Item> containeritem = RazorEnhanced.Items.ApplyFilter(itemFilter);

                foreach (Item found in containeritem)  // Return frist one found
                    return found;

                return null;
            }
        }

        // Single Click
        /// <summary>
        /// Send a single click network event to the server.
        /// </summary>
        /// <param name="item">Serial or Item to click</param>
        public static void SingleClick(Item item)
        {
            Assistant.Client.Instance.SendToServerWait(new SingleClick(item));
        }

        public static void SingleClick(int itemserial)
        {
            Assistant.Item item = Assistant.World.FindItem(itemserial);
            if (item == null)
            {
                Scripts.SendMessageScriptError("Script Error: SingleClick: Invalid Serial");
                return;
            }
            Assistant.Client.Instance.SendToServerWait(new SingleClick(item));
        }

        // Props

        /// <summary>
        /// If not updated, request to the Properties of an Item, and wait for a maximum amount of time. 
        /// </summary>
        /// <param name="itemserial">Serial or Item read.</param>
        /// <param name="delay">Maximum waiting time, in milliseconds.</param>
        public static void WaitForProps(int itemserial, int delay) // Delay in MS
        {
            if (World.Player != null && World.Player.Expansion <= 3) //  Expansion <= 3. Non esistono le props
                return;

            Assistant.Item i = Assistant.World.FindItem(itemserial);

            if (i == null)
                return;

            if (i.PropsUpdated)
                return;

            Assistant.Client.Instance.SendToServerWait(new QueryProperties(i.Serial));
            int subdelay = delay;

            while (!i.PropsUpdated)
            {
                Thread.Sleep(2);
                subdelay -= 2;
                if (subdelay <= 0)
                    break;
            }
        }

        public static void WaitForProps(Item i, int delay)
        {
            WaitForProps(i.Serial, delay);
        }



        /// <summary>
        /// Get string list of all Properties of an item, if item no props list is empty.
        /// </summary>
        /// <param name="serial">Serial or Item to read.</param>
        /// <returns>List of strings.</returns>
        public static List<string> GetPropStringList(int serial)
        {
            List<string> propstringlist = new List<string>();
            Assistant.Item assistantItem = Assistant.World.FindItem((uint)serial);

            if (assistantItem == null)
                return propstringlist;

            List<Assistant.ObjectPropertyList.OPLEntry> props = assistantItem.ObjPropList.Content;
            foreach (Assistant.ObjectPropertyList.OPLEntry prop in props)
            {
                propstringlist.Add(prop.ToString());
            }
            return propstringlist;
        }

        public static List<string> GetPropStringList(Item item)
        {
            return GetPropStringList(item.Serial);
        }



        /// <summary>
        /// Get a Property line, by index. if not found returns and empty string.
        /// </summary>
        /// <param name="serial">Serial or Item to read.</param>
        /// <param name="index">Number of the Property line.</param>
        /// <returns>A property line as a string.</returns>
		public static string GetPropStringByIndex(int serial, int index)
        {
            string propstring = String.Empty;
            Assistant.Item assistantItem = Assistant.World.FindItem((uint)serial);

            if (assistantItem == null)
                return propstring;

            List<Assistant.ObjectPropertyList.OPLEntry> props = assistantItem.ObjPropList.Content;
            if (props.Count > index)
                propstring = props[index].ToString();
            return propstring;
        }

        public static string GetPropStringByIndex(Item item, int index)
        {
            return GetPropStringByIndex(item.Serial, index);
        }


        /// <summary>
        /// Read the value of a Property.
        /// </summary>
        /// <param name="serial">Serial or Item to read.</param>
        /// <param name="name">Name of the Propery.</param>
        /// <returns></returns>

        public static float GetPropValue(int serial, string name)
        {
            if (name.ToLower().Contains("total") && name.ToLower().Contains("resist"))
                return GetTotalResistProp(serial);

            Assistant.Item assistantItem = World.FindItem((uint)serial);
            try
            {
                if (assistantItem != null && assistantItem.ObjPropList != null && assistantItem.ObjPropList.Content != null)
                {
                    var content = assistantItem.ObjPropList.Content;
                    if (content != null)
                    {
                        for (int i = 0; i < content.Count; i++)
                        {
                            if (!content[i].ToString().ToLower().Contains(name.ToLower())) // Props Name not match
                                continue;

                            if (content[i].Args == null)  // Props exist but not have value
                                return 1;

                            try
                            {
                                return Convert.ToSingle(Language.ParsePropsCliloc(content[i].Args), CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                return 1;  // Conversion error
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                // do nothing because sometimes the content[i] no longer existed so fall through
                // and return 0
            }
            return 0;  // Item not exist or props not exist
        }

        public static float GetPropValue(Item item, string name)
        {
            if (item == null)
                return 0;

            return GetPropValue(item.Serial, name);
        }

        // GetPropValue: Special case "Total Resist" so that items can be collected based on total resist
        /// <summary>
        /// Get the total resistence of an Item by reading the Propery.
        /// </summary>
        /// <param name="serial">Serial or Item to read.</param>
        /// <returns>The value of the property as float a number.</returns>
        static float GetTotalResistProp(int serial)
        {
            Assistant.Item assistantItem = World.FindItem((uint)serial);

            float totalResist = 0;
            if (assistantItem != null)
            {
                try
                {
                    if (assistantItem != null && assistantItem.ObjPropList != null && assistantItem.ObjPropList.Content != null)
                    {
                        for (int i = 0; i < assistantItem.ObjPropList.Content.Count; i++)
                        {
                            if (assistantItem.ObjPropList.Content[i].ToString().ToLower().Contains("resist"))
                            {
                                if (assistantItem.ObjPropList.Content[i].Args != null)
                                {
                                    float addIt = 0;
                                    try
                                    {
                                        addIt = Convert.ToSingle(Language.ParsePropsCliloc(assistantItem.ObjPropList.Content[i].Args), CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        addIt = 1;  // Conversion error
                                    }
                                    totalResist += addIt;
                                }
                            }
                        }
                    }
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    // Do nothing. This occurs when looting or claiming a corpse while processing is still going on
                }
            }
            return totalResist;
        }

        // Message
        /// <summary>
        /// Display an in-game message on top of an Item, visibile only for the Player.
        /// </summary>
        /// <param name="item">Serial or Item to display text on.</param>
        /// <param name="hue">Color of the message.</param>
        /// <param name="message">Message as </param>
        public static void Message(Item item, int hue, string message)
        {
            // Prevent spamm message on left bottom screen
            if (World.Player == null || Utility.Distance(World.Player.Position.X, World.Player.Position.Y, item.Position.X, item.Position.Y) > 11)
                return;

            Assistant.Client.Instance.SendToClientWait(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, hue, 3, Language.CliLocName, item.Name, message));
        }

        public static void Message(int serial, int hue, string message)
        {
            Item item = FindBySerial(serial);

            if (item == null) //Intem
                return;

            // Prevent spamm message on left bottom screen
            if (World.Player == null || Utility.Distance(World.Player.Position.X, World.Player.Position.Y, item.Position.X, item.Position.Y) > 11)
                return;

            Assistant.Client.Instance.SendToClientWait(new UnicodeMessage(item.Serial, item.ItemID, MessageType.Regular, hue, 3, Language.CliLocName, item.Name, message));
        }

        // Count

        /// <summary>
        /// Count items inside a container, summing also the amount in stacks.
        /// </summary>
        /// <param name="container">Serial or Item to search into.</param>
        /// <param name="itemid">ItemID of the item to search.</param>
        /// <param name="color">Color to match. (default: -1, any color)</param>
        /// <param name="recursive">Search also in already open subcontainers.</param>
        /// <returns></returns>
        public static int ContainerCount(Item container, int itemid, int color = -1, bool recursive = false)
        {
            int count = 0;
            if (container != null && container.IsContainer)
            {
                foreach (RazorEnhanced.Item itemToCount in container.Contains)
                {
                    if (color == -1)
                    {
                        if (itemToCount.ItemID == itemid)
                            count += itemToCount.Amount;
                        if (recursive && itemToCount.IsContainer)
                        {
                            int recurseCount = ContainerCount(itemToCount, itemid, color); // recall for sub container
                            count += recurseCount;
                        }
                    }
                    else
                    {
                        if (itemToCount.ItemID == itemid && itemToCount.Hue == color)
                            count += itemToCount.Amount;
                        if (recursive && itemToCount.IsContainer)
                        {
                            int recurseCount = ContainerCount(itemToCount, itemid, color); // recall for sub container
                            count += recurseCount;
                        }

                    }
                }
            }
            else
            {
                Scripts.SendMessageScriptError("Script Error: ContainerCount: Invalid container");
            }
            return count;
        }

        public static int ContainerCount(int serial, int itemid, int color = -1, bool recursive = false)
        {
            Item container = FindBySerial(serial);
            if (container != null)
                return ContainerCount(container, itemid, color, recursive);
            else
            {
                Scripts.SendMessageScriptError("Script Error: ContainerCount: Invalid container");
                return 0;
            }
        }


        /// <summary>
        /// Hied an Item, affects only the player.
        /// </summary>
        /// <param name="serial">Serial or Item to hide.</param>
        /// 
        public static void Hide(int serial)
        {
            Assistant.Item item = World.FindItem(serial);
            if (item != null)
            {
                item.Visible = false;
                Assistant.Client.Instance.SendToClientWait(new RemoveObject(serial));
            }
        }
        public static void Hide(Item item)
        {
            Hide(item.Serial);
        }



        /// <summary>
        /// Count items in Player Backpack.
        /// </summary>
        /// <param name="itemid">ItemID to search.</param>
        /// <param name="color">Color to search. (default -1: any color)</param>
        /// <returns></returns>
        public static int BackpackCount(int itemid, int color = -1)
        {
            List<Assistant.Item> items = new List<Assistant.Item>(World.Items.Values.ToList());
            if (color == -1)
                items = items.Where((i) => i.IsInBackpack && i.ItemID == itemid).ToList();
            else
                items = items.Where((i) => i.IsInBackpack && i.ItemID == itemid && i.Hue == color).ToList();

            int amount = 0;
            foreach (Assistant.Item i in items)
                amount += i.Amount;

            return amount;
        }

        // Context

        /// <summary>
        /// Check if Context Menu entry exists for an Item.
        /// </summary>
        /// <param name="serial">Serial or Item to check.</param>
        /// <param name="name">Name of the Context Manu entry</param>
        /// <returns></returns>
        public static int ContextExist(int serial, string name)
        {
            Assistant.Item item = World.FindItem(serial);
            if (item == null) // Se item non valido
                return -1;

            Misc.WaitForContext(serial, 1500);

            foreach (KeyValuePair<ushort, int> entry in item.ContextMenu)
            {
                string menuname = Language.GetCliloc(entry.Value);
                if (menuname.ToLower() == name.ToLower())
                {
                    return entry.Key;
                }
            }

            return -1; // Se non trovata
        }

        public static int ContextExist(Item i, string name)
        {
            return ContextExist(i.Serial, name);
        }



        /// <summary>
        /// Get the Image on an Item by specifing the ItemID. Optinally is possible to apply a color.
        /// </summary>
        /// <param name="itemID">ItemID to use.</param>
        /// <param name="hue">Optional: Color to apply. (Default 0, natural)</param>
        /// <returns></returns>
        public static System.Drawing.Bitmap GetImage(int itemID, int hue = 0)
        {
            System.Drawing.Bitmap bitmapImage = null;

            try
            {
                // Get original cached Bitmap Static
                System.Drawing.Bitmap bitmapOriginal = Ultima.Art.GetStatic(itemID);
                {
                    if (bitmapOriginal != null)
                    {
                        bitmapImage = bitmapOriginal;

                        if (hue > 0)
                        {
                            // Create clone to preserve original cached bitmap!
                            System.Drawing.Bitmap bitmapDeepCopy = new System.Drawing.Bitmap(bitmapOriginal);

                            bool onlyHueGrayPixels = (hue & 0x8000) != 0;
                            hue = (hue & 0x3FFF) - 1;
                            Ultima.Hue m_hue = Ultima.Hues.GetHue(hue);
                            m_hue.ApplyTo(bitmapDeepCopy, onlyHueGrayPixels);

                            bitmapImage = bitmapDeepCopy;
                        }
                    }
                }
            }
            catch { Misc.SendMessage("Items.GetBitmapImage() exception => itemID: " + itemID + ", hue: " + hue); }

            return bitmapImage;
        }
    }
}
