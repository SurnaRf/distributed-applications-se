using Amigurumemi.Data;
using Amigurumemi.Data.Entities;
using Amigurumemi.Repositories.Interfaces;

namespace Amigurumemi.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
	private readonly IDbContext _context;

	private IUserRepository? _users;
	private IPatternRepository? _patterns;
	private IProjectRepository? _projects;
	private IYarnRepository? _yarns;
	private IProjectYarnRepository _projectYarns;

	public UnitOfWork(IDbContext context)
	{
		_context = context;
	}

	public IUserRepository Users
		=> _users ??= new UserRepository(_context);

	public IPatternRepository Patterns
		=> _patterns ??= new PatternRepository(_context);

	public IProjectRepository Projects
		=> _projects ??= new ProjectRepository(_context);

	public IYarnRepository Yarns
		=> _yarns ??= new YarnRepository(_context);

	public IProjectYarnRepository ProjectYarns
		=> _projectYarns ??= new ProjectYarnRepository(_context);

	public Task<int> SaveChangesAsync(
		CancellationToken cancellationToken = default)
		=> _context.SaveChangesAsync(cancellationToken);

	public void Dispose()
	{
		if (_context is IDisposable disposable)
			disposable.Dispose();
	}
}