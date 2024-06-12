using ebooks_dotnet8_api;
using ebooks_dotnet8_api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("ebooks"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowLocalhost",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});
var app = builder.Build();
app.UseCors("AllowLocalhost");

var ebooks = app.MapGroup("api/ebook");

ebooks.MapPost("/", CreateEBookAsync);
ebooks.MapGet("/", GetAllEBooksAsync);
ebooks.MapPut("/{id}", UpdateEBookAsync);
ebooks.MapPut("/{id}/change-availability", ChangeAvailabilityEBookAsync);
ebooks.MapPut("/{id}/increment-stock", IncrementStockEBookAsync);
ebooks.MapPost("/purchase", PurchaseEBookAsync);
ebooks.MapDelete("/{id}", DeleteEBookAsync);

app.Run();

async Task<IResult> CreateEBookAsync(DataContext context, [FromBody] CreateEBookDto createEBook)
{
    var existingEBook = await context.EBooks.FirstOrDefaultAsync(e =>
        e.Title == createEBook.Title && e.Author == createEBook.Author
    );

    if (existingEBook != null)
    {
        return Results.Conflict("An eBook with the same title and author already exists.");
    }

    var eBook = new EBook
    {
        Title = createEBook.Title,
        Author = createEBook.Author,
        Genre = createEBook.Genre,
        Format = createEBook.Format,
        IsAvailable = true,
        Price = createEBook.Price,
        Stock = 0
    };

    context.EBooks.Add(eBook);
    await context.SaveChangesAsync();

    return Results.Created($"/api/ebook/{eBook.Id}", eBook);
}

async Task<IResult> GetAllEBooksAsync(
    DataContext context,
    [FromQuery] string? genre,
    [FromQuery] string? author,
    [FromQuery] string? format
)
{
    IQueryable<EBook> query = context.EBooks.Where(e => e.IsAvailable).OrderBy(e => e.Title);

    query = !string.IsNullOrEmpty(genre) ? query.Where(e => e.Genre == genre) : query;
    query = !string.IsNullOrEmpty(author) ? query.Where(e => e.Author == author) : query;
    query = !string.IsNullOrEmpty(format) ? query.Where(e => e.Format == format) : query;

    var eBooks = await query.ToListAsync();
    return Results.Ok(eBooks);
}

async Task<IResult> UpdateEBookAsync(
    DataContext context,
    int id,
    [FromBody] UpdateEBookDto updateEBook
)
{
    var eBook = await context.EBooks.FindAsync(id);

    if (eBook == null)
    {
        return Results.NotFound("EBook not found.");
    }

    eBook.Title = updateEBook.Title ?? eBook.Title;
    eBook.Author = updateEBook.Author ?? eBook.Author;
    eBook.Genre = updateEBook.Genre ?? eBook.Genre;
    eBook.Format = updateEBook.Format ?? eBook.Format;
    eBook.Price = updateEBook.Price ?? eBook.Price;

    await context.SaveChangesAsync();
    return Results.Ok(eBook);
}

async Task<IResult> ChangeAvailabilityEBookAsync(DataContext context, int id)
{
    var eBook = await context.EBooks.FindAsync(id);

    if (eBook == null)
    {
        return Results.NotFound("EBook not found.");
    }

    eBook.IsAvailable = !eBook.IsAvailable;
    await context.SaveChangesAsync();

    return Results.Ok(eBook);
}

async Task<IResult> IncrementStockEBookAsync(
    DataContext context,
    int id,
    [FromBody] IncrementStockEBookDto incrementStockEBook
)
{
    var eBook = await context.EBooks.FindAsync(id);

    if (eBook == null)
    {
        return Results.NotFound("EBook not found.");
    }

    eBook.Stock += incrementStockEBook.Quantity;
    await context.SaveChangesAsync();

    return Results.Ok(eBook);
}

async Task<IResult> PurchaseEBookAsync(
    DataContext context,
    [FromBody] PurchaseEBookDto purchaseEBook
)
{
    var eBook = await context.EBooks.FindAsync(purchaseEBook.EBookId);

    if (eBook == null)
    {
        return Results.NotFound("EBook not found.");
    }
    else if (!eBook.IsAvailable)
    {
        return Results.BadRequest("EBook is not available.");
    }
    else if (eBook.Stock < purchaseEBook.Quantity)
    {
        return Results.BadRequest("Not enough stock.");
    }

    var totalPrice = eBook.Price * purchaseEBook.Quantity;

    if (totalPrice != purchaseEBook.TotalPrice)
    {
        return Results.BadRequest("Total price mismatch.");
    }

    eBook.Stock -= purchaseEBook.Quantity;
    await context.SaveChangesAsync();

    return Results.Ok("Purchase successful.");
}

async Task<IResult> DeleteEBookAsync(DataContext context, int id)
{
    var eBook = await context.EBooks.FindAsync(id);

    if (eBook == null)
    {
        return Results.NotFound("EBook not found.");
    }

    context.EBooks.Remove(eBook);
    await context.SaveChangesAsync();

    return Results.NoContent();
}
