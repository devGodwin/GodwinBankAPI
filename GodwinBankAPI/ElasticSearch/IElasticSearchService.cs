namespace GodwinBankAPI.ElasticSearch;

public interface IElasticSearchService
{
    Task<bool> AddAsync<T>(T doc) where T : class;
    Task<T> GetByIdAsync<T>(string id) where T : class;
    Task<bool> UpdateAsync<T>(string id, T doc) where T : class;
    Task<bool> DeleteAsync<T>(string id) where T : class;
}