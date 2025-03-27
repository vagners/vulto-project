using MemoryCore.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace MemoryCore.Services
{
    public class ZeyraService
    {
        private readonly IMongoCollection<Zeyra> _zeyras;
        private readonly EmbeddingService _embeddingService;

        // Update constructor
        private const string ZeyrasCollectionName = "Zeyras";

        public ZeyraService(IDatabaseSettings settings, EmbeddingService embeddingService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _zeyras = database.GetCollection<Zeyra>(ZeyrasCollectionName);
            _embeddingService = embeddingService;
        }

        public async Task<List<Zeyra>> GetAsync() =>
            await _zeyras.Find(zeyra => true).ToListAsync();

        public async Task<Zeyra> GetAsync(string id) =>
            await _zeyras.Find(zeyra => zeyra.Id == id).FirstOrDefaultAsync();

        public async Task<Zeyra> CreateAsync(Zeyra zeyra)
        {
            // Generate embedding for the content
            zeyra.Embedding = await _embeddingService.GenerateEmbeddingAsync(zeyra.Conteudo);
            
            await _zeyras.InsertOneAsync(zeyra);
            return zeyra;
        }

        public async Task UpdateAsync(string id, Zeyra zeyraIn)
        {
            // Update embedding if content changed
            if (zeyraIn.Embedding == null || zeyraIn.Embedding.Length == 0)
            {
                zeyraIn.Embedding = await _embeddingService.GenerateEmbeddingAsync(zeyraIn.Conteudo);
            }
            
            await _zeyras.ReplaceOneAsync(zeyra => zeyra.Id == id, zeyraIn);
        }

        public async Task RemoveAsync(string id) =>
            await _zeyras.DeleteOneAsync(zeyra => zeyra.Id == id);
            
        // In the SearchAsync method, change:
        public async Task<List<Zeyra>> SearchAsync(string query, string? emocao = null, int? minImportancia = null)
        {
            // Generate embedding for the search query
            var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(query);
            
            // Get all Zeyras that match the filter criteria
            var filter = Builders<Zeyra>.Filter.Empty;
            
            if (!string.IsNullOrEmpty(emocao))
            {
                filter = Builders<Zeyra>.Filter.Eq(z => z.Emocao, emocao);
            }
            
            if (minImportancia.HasValue)
            {
                var importanciaFilter = Builders<Zeyra>.Filter.Gte(z => z.Importancia, minImportancia.Value);
                filter = filter == Builders<Zeyra>.Filter.Empty 
                    ? importanciaFilter 
                    : Builders<Zeyra>.Filter.And(filter, importanciaFilter);
            }
            
            var zeyras = await _zeyras.Find(filter).ToListAsync();
            
            // Calculate similarity scores
            var scoredZeyras = zeyras
                .Select(z => new 
                {
                    Zeyra = z,
                    Score = z.Embedding != null 
                        ? _embeddingService.CalculateCosineSimilarity(queryEmbedding, z.Embedding)
                        : 0
                })
                .Where(item => item.Score > 0.5) // Threshold for relevance
                .OrderByDescending(item => item.Score)
                .Select(item => item.Zeyra)
                .ToList();
                
            return scoredZeyras;
        }
    }
}