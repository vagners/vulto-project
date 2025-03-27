using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MemoryCore.Services
{
    public class EmbeddingService
    {
        private readonly ILogger<EmbeddingService> _logger;
        private readonly Dictionary<string, int> _vocabulary = new Dictionary<string, int>();
        private int _vocabSize = 0;
        private const int EmbeddingDimension = 100;

        public EmbeddingService(ILogger<EmbeddingService> logger)
        {
            _logger = logger;
        }

        public Task<float[]> GenerateEmbeddingAsync(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return Task.FromResult(new float[EmbeddingDimension]);
                }

                var tokens = text.ToLower().Split(new[] { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\r', '\t' }, 
                    StringSplitOptions.RemoveEmptyEntries);
                
                var embedding = new float[EmbeddingDimension];
                
                foreach (var token in tokens)
                {
                    if (!_vocabulary.ContainsKey(token))
                    {
                        _vocabulary[token] = _vocabSize++;
                    }
                    
                    int dimension = _vocabulary[token] % EmbeddingDimension;
                    embedding[dimension] += 1.0f;
                }
                
                float magnitude = (float)Math.Sqrt(embedding.Sum(x => x * x));
                
                if (magnitude > 0)
                {
                    for (int i = 0; i < embedding.Length; i++)
                    {
                        embedding[i] /= magnitude;
                    }
                }
                
                return Task.FromResult(embedding);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating embedding");
                throw;
            }
        }

        public float CalculateCosineSimilarity(float[] embedding1, float[] embedding2)
        {
            if (embedding1 == null || embedding2 == null || 
                embedding1.Length != embedding2.Length)
                return 0;

            float dotProduct = 0, norm1 = 0, norm2 = 0;
            for (int i = 0; i < embedding1.Length; i++)
            {
                dotProduct += embedding1[i] * embedding2[i];
                norm1 += embedding1[i] * embedding1[i];
                norm2 += embedding2[i] * embedding2[i];
            }

            return dotProduct / ((float)Math.Sqrt(norm1) * (float)Math.Sqrt(norm2));
        }
    }
}