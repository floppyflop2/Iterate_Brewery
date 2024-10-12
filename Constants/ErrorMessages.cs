namespace Constants;

public static class ErrorMessages
{
    public const string WholesalerMustExist = "The wholesaler must exist";
    public const string BeerMustExist = "The beer must exist";
    public const string QuantityMustBeGreaterThanZero = "The quantity must be greater than 0";
    public const string BeerMustBeSoldByWholesaler = "The beer must be sold by the wholesaler";
    public const string QuantityMustBeLessThanOrEqualToStock = "The quantity must be less than or equal to the stock";
    public const string ThereCantBeAnyDuplicateInTheOrder = "There can't be any duplicate in the order";
    public const string BeerNameIsMandatory = "Beer Name is mandatory";
    public const string ThisNameIsAlreadyInUse = "This name is already in use";
    public const string WholesalerCannotBeEmpty = "The Wholesaler cannot be empty";
}