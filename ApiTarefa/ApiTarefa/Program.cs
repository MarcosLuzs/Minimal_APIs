using Microsoft.EntityFrameworkCore;

// Inicialização Da Aplicação Com WebApplication.
var builder = WebApplication.CreateBuilder(args);

// As Definiçoes dos Serviços com Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Do Registro do Contexto Como um Serviço.
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("TarefasDB"));

// Build da Aplicação.
var app = builder.Build();

// Ambiente de Desenvolvimento.
// incluo o Swagger.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Definindo os Mapeamentos.

// Retorna uma Lista De Tarefas.
app.MapGet("/tarefas", async (AppDbContext db) => await db.Tarefas.ToListAsync());

// Retorna uma Lista De Tarefas Pelo ID.
app.MapGet("/tarefas/{id}", async (int id, AppDbContext db) =>
    await db.Tarefas.FindAsync(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound());

// Retorna a Lista De Tarefas Concluida.
app.MapGet("/tarefas/concluida", async (AppDbContext db) =>
                                 await db.Tarefas.Where(t => t.IsConcluida).ToListAsync());

// Inclui Uma Tarefa.
app.MapPost("/tarefas", async (Tarefa tarefa, AppDbContext db) =>
{
    db.Tarefas.Add(tarefa);
    await db.SaveChangesAsync();
    return Results.Created($"/tarefas/{tarefa.Id}", tarefa);
});

// Atualiza Tarefas Pelo Seu Id.
app.MapPut("/tarefas/{id}", async (int id, Tarefa inputTarefa, AppDbContext db) =>
{
    var tarefa = await db.Tarefas.FindAsync(id);

    if (tarefa is null) return Results.NotFound();

    tarefa.Nome = inputTarefa.Nome;
    tarefa.IsConcluida = inputTarefa.IsConcluida;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Deleta Tarefas Pelo Id
app.MapDelete("/tarefas/{id}", async (int id, AppDbContext db) =>
{
    if(await db.Tarefas.FindAsync(id) is Tarefa tarefa)
    {
        db.Tarefas.Remove(tarefa);
        await db.SaveChangesAsync();
        return Results.Ok(tarefa);
    }
    return Results.NotFound();
});

app.Run();

class Tarefa
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public bool IsConcluida { get; set; }
}

class AppDbContext : DbContext
{ 
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}