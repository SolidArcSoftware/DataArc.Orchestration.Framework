namespace DataArc.Demo.Domain.SharedKernel
{
    public interface IPolicy<TContext>
    {
        PolicyResult Apply(TContext context);
    }
}