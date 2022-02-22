using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using SongAPI;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Songs") ?? "Data Source=Songs.db";
builder.Services.AddSqlite<SongsDB>(connectionString);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo {
         Title = "SongAPI",
         Description = "CRUD for songs",
         Version = "v1" });
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "SongAPI");
});
//app.UseHttpsRedirection();

// Create
app.MapPost("/song", async (SongsDB db, Song song) =>
{
    await db.Songs.AddAsync(song);
    await db.SaveChangesAsync();
    return Results.Created($"/{song.Id}", song);
});

// Read
app.MapGet("/song", async (SongsDB db) => await db.Songs.ToListAsync());

// Update
app.MapPut("/song/{id}", async (SongsDB db, Song updatesong, int id) =>
{
    var song = await db.Songs.FindAsync(id);
    if (song is null) return Results.NotFound();

    song.artist = updatesong.artist;
    song.title = updatesong.title;
    song.length = updatesong.length;
    song.category = song.category;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Delete
app.MapDelete("/song/{id}", async (SongsDB db, int id) =>
{
  var song = await db.Songs.FindAsync(id);
  if (song is null)
  {
    return Results.NotFound();
  }
  db.Songs.Remove(song);
  await db.SaveChangesAsync();
  return Results.Ok();
});

app.Run();