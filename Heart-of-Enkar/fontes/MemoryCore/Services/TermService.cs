using MemoryCore.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryCore.Services
{
    public class TermService
    {
        private readonly IMongoCollection<Term> _terms;

        public TermService(ITermDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _terms = database.GetCollection<Term>(settings.TermsCollectionName);
        }

        public async Task<List<Term>> GetAsync() =>
            await _terms.Find(term => true).ToListAsync();

        public async Task<Term> GetAsync(string id) =>
            await _terms.Find(term => term.Id == id).FirstOrDefaultAsync();

        public async Task<Term> GetByNameAsync(string nome) =>
            await _terms.Find(term => term.Nome.ToLower() == nome.ToLower()).FirstOrDefaultAsync();

        public async Task<Term> CreateAsync(Term term)
        {
            term.DataCriacao = DateTime.UtcNow;
            term.Versoes.Add(new TermVersion
            {
                Definicao = term.Definicao,
                DataModificacao = term.DataCriacao,
                Autor = "Sistema" // You might want to get this from authentication
            });

            await _terms.InsertOneAsync(term);
            return term;
        }

        public async Task UpdateAsync(string id, Term termIn)
        {
            var existingTerm = await GetAsync(id);
            if (existingTerm != null && existingTerm.Definicao != termIn.Definicao)
            {
                termIn.Versoes = existingTerm.Versoes;
                termIn.Versoes.Add(new TermVersion
                {
                    Definicao = termIn.Definicao,
                    DataModificacao = DateTime.UtcNow,
                    Autor = "Sistema" // You might want to get this from authentication
                });
            }

            await _terms.ReplaceOneAsync(term => term.Id == id, termIn);
        }

        public async Task RemoveAsync(string id) =>
            await _terms.DeleteOneAsync(term => term.Id == id);
    }
}