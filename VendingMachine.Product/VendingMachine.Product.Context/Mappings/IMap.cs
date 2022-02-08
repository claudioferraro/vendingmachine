namespace VendingMachine.ProductCtx.Mappings
{
    public interface IMap<TDTO, TEntity>
    {
        TEntity Map(TDTO dto);
        TDTO InverseMap(TEntity entity);
    }
}
