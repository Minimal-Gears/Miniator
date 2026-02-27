using MinimalGears.Miniator.Contracts;

namespace MinimalGears.Miniator.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        Sender.RegisterHandler(new AddCommandHandler());

        AddCommand request = new AddCommand
                                 {
                                     A = 1,
                                     B = 2
                                 };

        ISender sender = new Sender();
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
