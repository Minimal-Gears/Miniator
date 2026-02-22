using MinimalGears.Miniator.Contracts;

namespace MinimalGears.Miniator.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        AddCommand request = new AddCommand
                                 {
                                     A = 1,
                                     B = 2
                                 };

        ISender sender = new Sender();
        ((Sender)sender)._handlers = new Dictionary<Type, IRequestHandler>();
        ((Sender)sender)._handlers.Add(typeof(AddCommand), new AddCommandHandler());
        var result = await sender.Send<AddCommand, AddCommandHandler, int>(request);
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
