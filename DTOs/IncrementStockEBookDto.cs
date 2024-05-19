using System.ComponentModel.DataAnnotations;

namespace ebooks_dotnet8_api.DTOs;

/// <summary>
/// Data transfer object for incrementing the stock of an eBook.
/// </summary>
public class IncrementStockEBookDto
{
    /// <summary>
    /// Quantity to increment the stock by.
    /// </summary>
    [Range(1, int.MaxValue)]
    public required int Quantity { get; set; }
}
