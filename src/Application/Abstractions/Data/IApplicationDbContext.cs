using Domain.Products;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext : IUnitOfWork
{
    DbSet<User> Users { get; }
    DbSet<Product> Products { get; }
}
