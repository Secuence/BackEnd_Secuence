namespace SecuenceBack.Data
{
	public interface IRepository
	{
		Task<List<T>> SelectAll<T>() where T : class;
		Task<T> SelectById<T>(int Id) where T : class;
        Task<T> SelectById<T>(long Id) where T : class;
        Task CreateAsync<T>(T entity) where T : class;
		Task<long> CreateAsyncLong<T>(T entity) where T : class;
		Task<int> CreateAsyncInt<T>(T entity) where T : class;
		Task UpdateAsync<T>(T entity) where T : class;
		Task DeleteAsync<T>(T entity) where T : class;
		Task LogAsync<T>(string user, long idModule, string moduleName, string action, string message) where T : class;

    }
}
