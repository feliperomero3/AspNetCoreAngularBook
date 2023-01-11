namespace WorldCitiesAPI.Data;

public class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    internal void Initialize()
    {
        _context.Database.EnsureCreated();
    }
}
