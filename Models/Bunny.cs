using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Caching.Memory;

namespace BunniesAPI
{
    public class Bunny
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [RegularExpressionAttribute(@"^[A-Za-z]{1}[a-zA-Z\s-]*")]
        public string Name { get; set; }
        [RangeAttribute(0, 100)]
        public int Age { get; set; }
        [Range(0.0, 1.0)]
        public decimal Cuteness { get; set; }
        [Range(0.0, 1.0)]
        public decimal Hunger { get; set; }
        [Range(0.0, 1.0)]
        public decimal Aggressiveness { get; set; }
        [Range(0.0, 1.0)]
        public decimal Hoppyness { get; set; }
        [EnumDataType(typeof(BunnyColor))]
        public string Color { get; set; }
    }

    public enum BunnyColor
    {
        White,
        Black,
        Red,
        Brown,
        Grey,
        BlackWithWhiteSpots,
        WhiteWithBlackSpots,
        CandyCorn,
        RainbowSprinkles
    }

    public class BunniesCollection : IList<Bunny>
    {
        private List<Bunny> bunnies;
        private IMemoryCache _cache;
        const string CacheKey = "BunniesAPI.BunniesController_key_BunniesCollection";
        private MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddYears(20));

        public BunniesCollection(IMemoryCache memoryCache)
        {
            _cache = memoryCache;

            if (!_cache.TryGetValue(CacheKey, out bunnies))
            {
                Reset();
                _cache.Set(CacheKey, bunnies, cacheEntryOptions);
            }
        }

        public Bunny this[int index]
        {
            get => bunnies[index];
            set
            {
                bunnies[index] = value;
                _cache.Set(CacheKey, bunnies, cacheEntryOptions);
            }
        }

        public int Count => bunnies.Count;

        public bool IsReadOnly => false;

        public void Add(Bunny item)
        {
            bunnies.Add(item);
            _cache.Set(CacheKey, bunnies, cacheEntryOptions);
        }

        public void Clear()
        {
            bunnies.Clear();
            _cache.Set(CacheKey, bunnies, cacheEntryOptions);
        }

        public bool Contains(Bunny item) => bunnies.Contains(item);

        public void CopyTo(Bunny[] array, int arrayIndex) => bunnies.CopyTo(array, arrayIndex);

        public IEnumerator<Bunny> GetEnumerator() => bunnies.GetEnumerator();

        public int IndexOf(Bunny item) => bunnies.IndexOf(item);

        public void Insert(int index, Bunny item)
        {
            bunnies.Insert(index, item);
            _cache.Set(CacheKey, bunnies, cacheEntryOptions);
        }

        public bool Remove(Bunny item)
        {
            var retVal = bunnies.Remove(item);
            _cache.Set(CacheKey, bunnies, cacheEntryOptions);
            return retVal;
        }

        public void RemoveAt(int index)
        {
            bunnies.RemoveAt(index);
            _cache.Set(CacheKey, bunnies, cacheEntryOptions);
        }

        IEnumerator IEnumerable.GetEnumerator() => bunnies.GetEnumerator();

        private void Reset()
        {
            bunnies = new List<Bunny>();

            bunnies.Add(new Bunny()
            {
                Id = Guid.NewGuid().ToString(),
                Age = 1,
                Aggressiveness = 0.8M,
                Name = "Thumper",
                Cuteness = 0.9M,
                Hunger = 0.0M,
                Hoppyness = 0.5M,
                Color = BunnyColor.Brown.ToString()
            });
            bunnies.Add(new Bunny()
            {
                Id = Guid.NewGuid().ToString(),
                Age = 12,
                Aggressiveness = 0.2M,
                Name = "Granny",
                Cuteness = 0.1M,
                Hunger = 0.2M,
                Hoppyness = 0.1M,
                Color = BunnyColor.Grey.ToString()
            });
            bunnies.Add(new Bunny()
            {
                Id = Guid.NewGuid().ToString(),
                Age = 99,
                Aggressiveness = 0.99M,
                Name = "Party Pete",
                Cuteness = 0.99M,
                Hunger = 0.99M,
                Hoppyness = 0.99M,
                Color = BunnyColor.RainbowSprinkles.ToString()
            });
        }
    }

}
