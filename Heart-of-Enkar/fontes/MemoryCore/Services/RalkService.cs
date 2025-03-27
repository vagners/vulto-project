using MemoryCore.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MemoryCore.Services
{
    public class RalkService
    {
        private readonly IMongoCollection<Ralk> _ralks;
        private readonly EncryptionService _encryptionService;
        private readonly SensitiveWordDetector _sensitiveWordDetector;
        private readonly ZeyraService _zeyraService;
        private readonly EmbeddingService _embeddingService;
        private readonly Dictionary<string, string> _ralkCache;
        private const string RalksCollectionName = "Ralks";

        public RalkService(
            IDatabaseSettings settings, 
            EncryptionService encryptionService,
            SensitiveWordDetector sensitiveWordDetector,
            ZeyraService zeyraService,
            EmbeddingService embeddingService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _ralks = database.GetCollection<Ralk>(RalksCollectionName);
            _encryptionService = encryptionService;
            _sensitiveWordDetector = sensitiveWordDetector;
            _zeyraService = zeyraService;
            _embeddingService = embeddingService;
            _ralkCache = new Dictionary<string, string>();
            
            // Load existing ralks into cache
            LoadRalksIntoCacheAsync().Wait();
        }

        private async Task LoadRalksIntoCacheAsync()
        {
            var ralks = await _ralks.Find(_ => true).ToListAsync();
            foreach (var ralk in ralks)
            {
                _ralkCache[ralk.PalavraOriginal.ToLower()] = ralk.Metafora;
            }
        }

        public async Task<string> ProtectTextAsync(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Check if text contains sensitive words
            if (!await _sensitiveWordDetector.ContainsSensitiveWordsAsync(text))
                return text;

            var sensitiveMatches = await _sensitiveWordDetector.DetectSensitiveWordsAsync(text);
            var protectedText = text;

            // Replace sensitive words with their metaphors
            foreach (var match in sensitiveMatches.OrderByDescending(m => m.Word.Length))
            {
                if (_ralkCache.TryGetValue(match.Word, out string metaphor))
                {
                    var pattern = $@"\b{match.Word}\b";
                    protectedText = Regex.Replace(protectedText, pattern, metaphor, RegexOptions.IgnoreCase);
                }
                else
                {
                    // Generate a new metaphor if one doesn't exist
                    var newMetaphor = await GenerateMetaphorAsync(match.Word, match.Category);
                    var pattern = $@"\b{match.Word}\b";
                    protectedText = Regex.Replace(protectedText, pattern, newMetaphor, RegexOptions.IgnoreCase);
                }
            }

            return protectedText;
        }

        private async Task<string> GenerateMetaphorAsync(string word, string category)
        {
            // Simple metaphor generation logic - in a real system, this could use AI
            string prefix = category switch
            {
                "Thavik" => "sombra-",
                "Jurnak" => "silêncio-",
                "Clyro" => "névoa-",
                "Velnar" => "eco-",
                _ => "símbolo-"
            };

            string metaphor = $"{prefix}{Guid.NewGuid().ToString().Substring(0, 8)}";
            
            // Create and store the new Ralk
            await CreateRalkAsync(word, metaphor, category);
            
            return metaphor;
        }

        public async Task<string> RevealTextAsync(string protectedText)
        {
            if (string.IsNullOrEmpty(protectedText))
                return protectedText;

            var ralks = await _ralks.Find(_ => true).ToListAsync();
            string originalText = protectedText;

            foreach (var ralk in ralks.OrderByDescending(r => r.Metafora.Length))
            {
                originalText = originalText.Replace(ralk.Metafora, ralk.PalavraOriginal);
            }

            return originalText;
        }

        public async Task<Ralk> CreateRalkAsync(string palavra, string metafora, string categoria)
        {
            var ralk = new Ralk
            {
                PalavraOriginal = palavra,
                Metafora = metafora,
                Categoria = categoria,
                DataCriacao = DateTime.UtcNow,
                ConteudoCriptografado = _encryptionService.Encrypt(palavra)
            };

            await _ralks.InsertOneAsync(ralk);
            _ralkCache[palavra.ToLower()] = metafora;
            return ralk;
        }

        public async Task<Zeyra> CreateProtectedZeyraAsync(Zeyra originalZeyra)
        {
            // Create a protected version of the Zeyra
            var protectedContent = await ProtectTextAsync(originalZeyra.Conteudo);
            
            // If no protection was needed, return the original
            if (protectedContent == originalZeyra.Conteudo)
                return originalZeyra;
            
            // Store the original Zeyra with encrypted content
            originalZeyra.IsProtegida = true;
            originalZeyra.ConteudoOriginal = _encryptionService.Encrypt(originalZeyra.Conteudo);
            await _zeyraService.UpdateAsync(originalZeyra.Id, originalZeyra);
            
            // Create a Ralk version that references the original
            var ralkZeyra = new Zeyra
            {
                Conteudo = protectedContent,
                Emocao = originalZeyra.Emocao,
                Importancia = originalZeyra.Importancia,
                DataCriacao = DateTime.UtcNow,
                IsRalk = true,
                RalkDe = originalZeyra.Id
            };
            
            // Generate embedding for the protected content
            ralkZeyra.Embedding = await _embeddingService.GenerateEmbeddingAsync(protectedContent);
            
            // Save the Ralk Zeyra
            await _zeyraService.CreateAsync(ralkZeyra);
            
            return ralkZeyra;
        }

        public async Task<Zeyra> RevealProtectedZeyraAsync(string zeyraId, bool isAuthorized = false)
        {
            // Get the Ralk Zeyra
            var ralkZeyra = await _zeyraService.GetAsync(zeyraId);
            
            if (ralkZeyra == null || !ralkZeyra.IsRalk)
                return ralkZeyra;
            
            // Get the original Zeyra
            var originalZeyra = await _zeyraService.GetAsync(ralkZeyra.RalkDe);
            
            if (originalZeyra == null || !originalZeyra.IsProtegida)
                return ralkZeyra;
            
            // If not authorized, return the Ralk version
            if (!isAuthorized)
                return ralkZeyra;
            
            // Decrypt the original content
            originalZeyra.Conteudo = _encryptionService.Decrypt(originalZeyra.ConteudoOriginal);
            
            return originalZeyra;
        }

        // Existing methods
        public async Task<List<Ralk>> GetRalksAsync() =>
            await _ralks.Find(_ => true).ToListAsync();

        public async Task<Ralk> GetRalkAsync(string id) =>
            await _ralks.Find(ralk => ralk.Id == id).FirstOrDefaultAsync();

        public async Task UpdateRalkAsync(string id, Ralk ralkIn)
        {
            ralkIn.ConteudoCriptografado = _encryptionService.Encrypt(ralkIn.PalavraOriginal);
            await _ralks.ReplaceOneAsync(ralk => ralk.Id == id, ralkIn);
            _ralkCache[ralkIn.PalavraOriginal.ToLower()] = ralkIn.Metafora;
        }

        public async Task RemoveRalkAsync(string id)
        {
            var ralk = await GetRalkAsync(id);
            if (ralk != null)
            {
                await _ralks.DeleteOneAsync(r => r.Id == id);
                _ralkCache.Remove(ralk.PalavraOriginal.ToLower());
            }
        }
    }
}