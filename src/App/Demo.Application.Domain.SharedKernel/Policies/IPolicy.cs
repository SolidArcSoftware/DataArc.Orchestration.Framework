namespace Demo.Application.Domain.SharedKernel
{
    public interface IPolicy<TContext>
    {
        PolicyResult Apply(TContext context);
    }
}