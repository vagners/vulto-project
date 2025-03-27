using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MemoryCore.Models;
using MongoDB.Driver;

namespace MemoryCore.Services
{
    public class SensitiveWordDetector
    {
        private readonly IMongoCollection<SensitiveWord> _sensitiveWords;
        private readonly Dictionary<string, string> _wordCache = new Dictionary<string, string>();
        private const string SensitiveWordsCollectionName = "SensitiveWords";

        public SensitiveWordDetector(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _sensitiveWords = database.GetCollection<SensitiveWord>(SensitiveWordsCollectionName);
            
            // Initialize with default sensitive words if collection is empty
            InitializeDefaultWordsAsync().Wait();
            
            // Load words into cache
            LoadWordsIntoCache().Wait();
        }

        private async Task InitializeDefaultWordsAsync()
        {
            if (!await _sensitiveWords.Find(_ => true).AnyAsync())
            {
                var defaultWords = new List<SensitiveWord>
                {
                    new SensitiveWord { Word = "censura", Category = "Thavik" },
                    new SensitiveWord { Word = "proibido", Category = "Jurnak" },
                    new SensitiveWord { Word = "segredo", Category = "Clyro" },
                    new SensitiveWord { Word = "verdade", Category = "Velnar" },
                    new SensitiveWord { Word = "oculto", Category = "Thavik" }
                };
                
                await _sensitiveWords.InsertManyAsync(defaultWords);
            }
        }

        private async Task LoadWordsIntoCache()
        {
            var words = await _sensitiveWords.Find(_ => true).ToListAsync();
            foreach (var word in words)
            {
                _wordCache[word.Word.ToLower()] = word.Category;
            }
        }

        public async Task<bool> ContainsSensitiveWordsAsync(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            // Refresh cache if needed
            if (_wordCache.Count == 0)
                await LoadWordsIntoCache();

            var words = text.ToLower().Split(new[] { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\r', '\t' }, 
                StringSplitOptions.RemoveEmptyEntries);

            return words.Any(word => _wordCache.ContainsKey(word));
        }

        public async Task<List<SensitiveWordMatch>> DetectSensitiveWordsAsync(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new List<SensitiveWordMatch>();

            // Refresh cache if needed
            if (_wordCache.Count == 0)
                await LoadWordsIntoCache();

            var result = new List<SensitiveWordMatch>();
            var words = text.ToLower().Split(new[] { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\r', '\t' }, 
                StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                if (_wordCache.TryGetValue(word, out string category))
                {
                    result.Add(new SensitiveWordMatch
                    {
                        Word = word,
                        Category = category,
                        Index = text.ToLower().IndexOf(word)
                    });
                }
            }

            return result;
        }

        public async Task<SensitiveWord> AddSensitiveWordAsync(string word, string category)
        {
            var sensitiveWord = new SensitiveWord
            {
                Word = word.ToLower(),
                Category = category,
                DateAdded = DateTime.UtcNow
            };

            await _sensitiveWords.InsertOneAsync(sensitiveWord);
            _wordCache[word.ToLower()] = category;
            
            return sensitiveWord;
        }

        public async Task<List<SensitiveWord>> GetAllSensitiveWordsAsync()
        {
            return await _sensitiveWords.Find(_ => true).ToListAsync();
        }

        public async Task RemoveSensitiveWordAsync(string id)
        {
            var word = await _sensitiveWords.Find(w => w.Id == id).FirstOrDefaultAsync();
            if (word != null)
            {
                await _sensitiveWords.DeleteOneAsync(w => w.Id == id);
                _wordCache.Remove(word.Word.ToLower());
            }
        }
    }

    public class SensitiveWordMatch
    {
        public string Word { get; set; }
        public string Category { get; set; }
        public int Index { get; set; }
    }
}