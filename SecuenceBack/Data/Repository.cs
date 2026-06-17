
using Microsoft.EntityFrameworkCore;

namespace SecuenceBack.Data
{
	public class Repository<TDbContext> : IRepository where TDbContext : DbContext
	{
		private TDbContext _dbContext;
		public Repository(TDbContext context)
		{
			this._dbContext = context;
		}
		public async Task CreateAsync<T>(T entity) where T : class
		{
			this._dbContext.Set<T>().Add(entity);
			_ = await this._dbContext.SaveChangesAsync();
		}

		public async Task<long> CreateAsyncLong<T>(T entity) where T : class
		{
			this._dbContext.Set<T>().Add(entity);
			await this._dbContext.SaveChangesAsync();
			var IdProperty = entity.GetType().GetProperty("id").GetValue(entity, null);
			return (long)IdProperty;
		}
		public async Task<int> CreateAsyncInt<T>(T entity) where T : class
		{
			this._dbContext.Set<T>().Add(entity);
			await this._dbContext.SaveChangesAsync();
			var IdProperty = entity.GetType().GetProperty("id").GetValue(entity, null);
			return (int)IdProperty;
		}
		public async Task DeleteAsync<T>(T entity) where T : class
		{
			this._dbContext.Set<T>().Remove(entity);
			_ = await this._dbContext.SaveChangesAsync();
		}

		public async Task<List<T>> SelectAll<T>() where T : class
		{
			return await this._dbContext
					.Set<T>()
					.ToListAsync();
		}

		public async Task<T> SelectById<T>(int Id) where T : class
		{
			return await this._dbContext.Set<T>().FindAsync(Id);
		}
        public async Task<T> SelectById<T>(long Id) where T : class
        {
            return await this._dbContext.Set<T>().FindAsync(Id);
        }

        public async Task UpdateAsync<T>(T entity) where T : class
		{
			this._dbContext.Set<T>().Update(entity);
			_ = await this._dbContext.SaveChangesAsync();
		}

		public async Task LogAsync<T>(string user, long idModule, string moduleName, string action, string message) where T : class
        {
			//var log = new
			//{
			//	userName = user,
			//	idModule = idModule,
			//	moduleName = moduleName,
			//	actionDate = DateTime.Now,
			//	action = action,
			//	message = message
			//};
			//this._dbContext.Set<T>().Add(log);
			await this._dbContext.SaveChangesAsync();
		}
	
	}
}
