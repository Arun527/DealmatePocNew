using DealmateApi.Domain.Aggregates;
using DealmateApi.Infrastructure.Interfaces;
using DealmateApi.Service.Common;

namespace DealmateApi.Infrastructure.Repositories;

public class DealerRepository : IDealerRepository
{
    private readonly IRepository<Dealer> repository;
    public DealerRepository(IRepository<Dealer> repository)
    {
        this.repository = repository;
    }

    public async Task<Dealer> Create(Dealer dealer)
    {
        var existDealer = await repository.FirstOrDefaultAsync(x => x.Name == dealer.Name);
        if (existDealer != null)
        {
            throw new Exception($"The Dealer {dealer.Name} was already exist");
        }
        dealer=await repository.AddAsync(dealer);
        return dealer;
    }

    public async Task<Dealer> Update(Dealer dealer)
    {
        var existDealer = await repository.GetByIdAsync(dealer.Id);
        if (existDealer == null)
        {
            throw new Exception($"The DealerID {dealer.Id} not exist");
        }
        existDealer.Name = dealer.Name;
        existDealer.Address = dealer.Address;
        existDealer = await repository.Update(existDealer);
        return existDealer;
    }

    public async Task<Dealer> Delete(int id)
    {
        var dealer = await repository.GetByIdAsync(id);
        if (dealer == null)
        {
            throw new Exception($"The Dealer Id:{id} was not found");
        }
        dealer = await repository.Remove(dealer);
        return dealer;
    }
}