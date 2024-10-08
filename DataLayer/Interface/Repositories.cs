using Domain;

namespace DataLayer.Interface;

public interface IBeerRepository : IAbstractRepository<Beer>;

public interface IBreweryRepository : IAbstractRepository<Brewery>;

public interface IWholesalerStockRepository : IAbstractRepository<WholesalerStock>;

public interface IWholesalerRepository : IAbstractRepository<Wholesaler>;
