namespace Application.Shared.Models;

public record OrderModel(Guid Id, string CustomerName, int NumberOfProducts);
public record OrderModelDto(string CustomerName, int NumberOfProducts);