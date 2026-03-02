using Microsoft.Extensions.DependencyInjection;
using MinimalGears.Miniator.Contracts;
using MinimalGears.Miniator.DependencyInjection;

namespace MinimalGears.Miniator.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var services = new ServiceCollection();
        //Sender.RegisterHandler<AddCommandHandler, AddCommand, int>();

        services.AddMiniator(cfg => { cfg.RegisterServicesFromAssembly(typeof(AddCommand).Assembly); });
        var serviceProvider = services.BuildServiceProvider();
        ISender sender = serviceProvider.GetRequiredService<ISender>();

        AddCommand request = new AddCommand
                                 {
                                     A = 1,
                                     B = 2
                                 };

        var result = await sender.Send<AddCommand, int>(request);
        Assert.Equal(3, result);
    }
}

public class AddCommand : IRequest<int>
{
    public int A { get; set; }
    public int B { get; set; }
};

public class AddCommandHandler : IRequestHandler<AddCommand, int>
{
    public Task<int> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.A + request.B);
    }
}
