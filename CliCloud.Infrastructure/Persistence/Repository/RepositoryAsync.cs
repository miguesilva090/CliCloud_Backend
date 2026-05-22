#nullable enable
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

// Repository class
// -- this class should be used by all application services
// -- it provides abstraction from _context, it can return DTO-mapped lists with pagination
// -- use ISpecification (Ardalis Specification) to pass query criteria, include statements, and sort expressions.
// -- returning mapped DTOs is handled by Automapper's projectTo() method for better performance when handling related entities (https://dev.to/cloudx/entity-framework-core-simplify-your-queries-with-automapper-3m8k)

namespace CliCloud.Infrastructure.Persistence.Repository
{
  public class RepositoryAsync(ApplicationDbContext context, IMapper mapper) : IRepositoryAsync
  {
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    #region [-- GET --]

    // get all, return non-paginated list of domain entities

    public async Task<IEnumerable<T>> GetListAsync<T, TId>(
      ISpecification<T>? specification = null,
      CancellationToken cancellationToken = default
    )
      where T : BaseEntity<TId>
    {
      IQueryable<T> query = specification is null
        ? _context.Set<T>().AsQueryable()
        : SpecificationEvaluator.Default.GetQuery(
          query: _context.Set<T>().AsQueryable(),
          specification: specification
        );
      List<T> result = await query.ToListAsync(cancellationToken);
      return result;
    }

    // get all, return non-paginated list of mapped dtos
    public async Task<IEnumerable<TDto>> GetListAsync<T, TDto, TId>(
      ISpecification<T>? specification = null,
      CancellationToken cancellationToken = default
    )
      where T : BaseEntity<TId>
      where TDto : IDto
    {
      IQueryable<T> query = specification is null
        ? _context.Set<T>().AsQueryable()
        : SpecificationEvaluator.Default.GetQuery(
          query: _context.Set<T>().AsQueryable(),
          specification: specification
        );

      List<TDto> result = await query
        .ProjectTo<TDto>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);

      return result;
    }

    public async Task<IEnumerable<TResult>> GetListAsync<T, TResult, TId>(
      ISpecification<T, TResult>? specification,
      CancellationToken cancellationToken = default
    )
      where T : BaseEntity<TId>
    {
      ArgumentNullException.ThrowIfNull(specification);

      IQueryable<TResult> query = SpecificationEvaluator.Default.GetQuery(
        query: _context.Set<T>().AsNoTracking(),
        specification: specification
      );

      List<TResult> result = await query.ToListAsync(cancellationToken);

      return result;
    }

    // get by Id, return domain entity
    // Quando não há specification, usa FindAsync (chave primária) para evitar problemas com query filters ou tradução SQL
    public async Task<T> GetByIdAsync<T, TId>(
      TId id,
      ISpecification<T>? specification = null,
      CancellationToken cancellationToken = default
    )
      where T : BaseEntity<TId>
    {
      if (specification is null)
      {
        T? entity = await _context.Set<T>().FindAsync([id], cancellationToken);
        return entity ?? throw new InvalidOperationException("Não encontrado");
      }

      IQueryable<T> query = SpecificationEvaluator.Default.GetQuery(
        query: _context.Set<T>().AsQueryable(),
        specification: specification
      );

      T? entityFromQuery = await query
        .Where(x => x.Id!.Equals(id))
        .FirstOrDefaultAsync(cancellationToken);

      return entityFromQuery ?? throw new InvalidOperationException("Não encontrado");
    }

    // get by Id, return mapped dtos
    public async Task<TDto> GetByIdAsync<T, TDto, TId>(
      TId id,
      ISpecification<T>? specification = null,
      CancellationToken cancellationToken = default
    )
      where T : BaseEntity<TId>
      where TDto : IDto
    {
      IQueryable<T> query = specification is null
        ? _context.Set<T>().AsQueryable()
        : SpecificationEvaluator.Default.GetQuery(
          query: _context.Set<T>().AsQueryable(),
          specification: specification
        );

      TDto? result = await query
        .Where(x => x.Id!.Equals(id))
        .AsSingleQuery()
        .ProjectTo<TDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(cancellationToken);

      return result ?? throw new InvalidOperationException("Não encontrado");
    }

    // check if exists, return true/false
    public async Task<bool> ExistsAsync<T, TId>(
      ISpecification<T>? specification = null,
      CancellationToken cancellationToken = default
    )
      where T : BaseEntity<TId>
    {
      IQueryable<T> query = specification is null
        ? _context.Set<T>().AsQueryable()
        : SpecificationEvaluator.Default.GetQuery(
          query: _context.Set<T>().AsQueryable(),
          specification: specification
        );

      bool result = await query.AnyAsync(cancellationToken);
      return result;
    }

    #endregion [-- GET --]

    #region [-- CREATE --]

    // create
    public async Task<T> CreateAsync<T, TId>(T entity)
      where T : BaseEntity<TId>
    {
      _ = await _context.Set<T>().AddAsync(entity);
      return entity;
    }

    // create range, retun list of guid
    public async Task<IList<TId>> CreateRangeAsync<T, TId>(IEnumerable<T> entityList)
      where T : BaseEntity<TId>
    {
      await _context.Set<T>().AddRangeAsync(entityList);
      return entityList.Select(x => x.Id).ToList();
    }
    #endregion [-- CREATE --]

    #region [-- UPDATE --]

    // update
    public async Task<T> UpdateAsync<T, TId>(T entity)
      where T : BaseEntity<TId>
    {
      Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> entry = _context.Entry(entity);

      // Se a entidade já está tracked (ex: veio de GetListAsync no mesmo request), atualizar in-place
      if (entry.State != EntityState.Detached)
      {
        entry.CurrentValues.SetValues(entity);
        entry.State = EntityState.Modified;
        MarkEntidadeMoradaModified<T, TId>(entry);
        return entity;
      }

      // Caso contrário, carregar do DB (evita problema quando entidade foi carregada noutro contexto)
      T? entityInDb =
        await _context.Set<T>().FindAsync(entity.Id)
        ?? throw new InvalidOperationException("Não encontrado");

      entry = _context.Entry(entityInDb);
      entry.CurrentValues.SetValues(entity);
      entry.State = EntityState.Modified;
      MarkEntidadeMoradaModified<T, TId>(entry);
      return entityInDb;
    }

    /// <summary>
    /// Em herança TPT, garante que as colunas de morada na tabela base Utility.Entidade
    /// sejam consideradas modificadas para o UPDATE ser gerado corretamente.
    /// </summary>
    private static void MarkEntidadeMoradaModified<T, TId>(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> entry)
      where T : BaseEntity<TId>
    {
      if (entry.Entity is not Entidade)
        return;
      string[] moradaProps = { "RuaId", "CodigoPostalId", "FreguesiaId", "ConcelhoId", "DistritoId", "PaisId" };
      foreach (var name in moradaProps)
      {
        if (entry.Metadata.FindProperty(name) != null)
          entry.Property(name).IsModified = true;
      }
    }
    #endregion [-- UPDATE --]

    #region [-- REMOVE --]

    // remove by entity
    public Task RemoveAsync<T, TId>(T entity)
      where T : BaseEntity<TId>
    {
      if (entity is ISoftDelete softDeleteEntity)
      {
        softDeleteEntity.DeletedOn = DateTime.UtcNow;
        // DeletedBy será preenchido a partir do contexto de utilizador (se existir) noutro ponto do pipeline
        _ = _context.Set<T>().Update(entity);
      }
      else
      {
        _ = _context.Set<T>().Remove(entity);
      }

      return Task.CompletedTask;
    }

    // remove by Id
    public async Task<T> RemoveByIdAsync<T, TId>(TId entityId)
      where T : BaseEntity<TId>
    {
      T? entity =
        await _context.Set<T>().FindAsync(entityId)
        ?? throw new InvalidOperationException("Não encontrado");

      if (entity is ISoftDelete softDeleteEntity)
      {
        softDeleteEntity.DeletedOn = DateTime.UtcNow;
        _ = _context.Set<T>().Update(entity);
      }
      else
      {
        _ = _context.Set<T>().Remove(entity);
      }

      return entity;
    }

    // remove multiple by Ids
    public async Task<IEnumerable<TId>> RemoveRangeAsync<T, TId>(IEnumerable<TId> ids)
      where T : BaseEntity<TId>
    {
      List<T> entities = await _context.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();

      if (entities.Count == 0)
      {
        return [];
      }

      foreach (T entity in entities)
      {
        if (entity is ISoftDelete softDeleteEntity)
        {
          softDeleteEntity.DeletedOn = DateTime.UtcNow;
          _ = _context.Set<T>().Update(entity);
        }
        else
        {
          _context.Set<T>().Remove(entity);
        }
      }

      return entities.Select(x => x.Id).ToList();
    }
    #endregion [-- REMOVE --]

    #region [-- PAGINATION --]
    // return paginated list of mapped dtos -- format specific to Tanstack Table v8 (React, Vue)
    public async Task<PaginatedResponse<TDto>> GetPaginatedResultsAsync<T, TDto, TId>(
      int pageNumber,
      int pageSize,
      ISpecification<T>? specification = null,
      CancellationToken cancellationToken = default
    )
      where T : BaseEntity<TId>
      where TDto : IDto
    {
      IQueryable<T> query = specification is null
        ? _context.Set<T>().AsQueryable()
        : SpecificationEvaluator.Default.GetQuery(
          query: _context.Set<T>().AsQueryable(),
          specification: specification
        );

      List<TDto> pagedResult;
      int recordsTotal;
      try
      {
        recordsTotal = await query.CountAsync(cancellationToken);
        pagedResult = await query
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .ProjectTo<TDto>(_mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException(ex.Message, ex);
      }

      return new PaginatedResponse<TDto>(pagedResult, recordsTotal, pageNumber, pageSize);
    }
    #endregion [-- PAGINATION --]

    #region [-- SAVE --]

    // save the changes to database
    public async Task<int> SaveChangesAsync()
    {
      return await _context.SaveChangesAsync();
    }

    // clear the change tracker to reset context state
    public void ClearChangeTracker()
    {
      _context.ChangeTracker.Clear();
    }

    #endregion [-- SAVE --]
  }
}
