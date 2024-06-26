using System.ComponentModel.DataAnnotations;

namespace ebooks_dotnet8_api.DTOs;

/// <summary>
/// Represents the data transfer object for creating an eBook.
/// </summary>
public class CreateEBookDto
{
    /// <summary>
    /// Title of the eBook.
    /// </summary>
    [StringLength(256, MinimumLength = 3)]
    public required string Title { get; set; }

    /// <summary>
    /// Author of the eBook.
    /// </summary>
    [StringLength(256, MinimumLength = 3)]
    public required string Author { get; set; }

    /// <summary>
    /// Genre of the eBook.
    /// </summary>
    [StringLength(256, MinimumLength = 3)]
    public required string Genre { get; set; }

    /// <summary>
    /// Format of the eBook.
    /// </summary>
    [StringLength(256, MinimumLength = 3)]
    public required string Format { get; set; }

    /// <summary>
    /// Price of the eBook.
    /// </summary>
    [Range(1, int.MaxValue)]
    public required int Price { get; set; }
}
