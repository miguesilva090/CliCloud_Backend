using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Filters;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;

// After creating this service:
// -- 1. Create a EntidadeContacto domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<EntidadeContacto> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a EntidadeContacto api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Utility.EntidadeContactoService
{
    public class EntidadeContactoService(IRepositoryAsync repository, IMapper mapper) : IEntidadeContactoService
    {
        private readonly IRepositoryAsync _repository = repository;
        private readonly IMapper _mapper = mapper; 

        // get full List
        public async Task<Response<IEnumerable<EntidadeContactoDTO>>> GetEntidadeContactoAsync(string keyword = "")
        {
            EntidadeContactoSearchList specification = new(keyword); // ardalis specification
            IEnumerable<EntidadeContactoDTO> list = await _repository.GetListAsync<EntidadeContacto, EntidadeContactoDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<EntidadeContactoDTO>>(list);
        }

        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<EntidadeContactoDTO>> GetEntidadeContactoPaginatedAsync(EntidadeContactoTableFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword)) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            EntidadeContactoSearchTable specification = new(filter.Keyword, dynamicOrder); // ardalis specification
            PaginatedResponse<EntidadeContactoDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<EntidadeContacto, EntidadeContactoDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }


        // get single EntidadeContacto by Id 
        public async Task<Response<EntidadeContactoDTO>> GetEntidadeContactoAsync(Guid id)
        {
            try
            {
                EntidadeContactoDTO dto = await _repository.GetByIdAsync<EntidadeContacto, EntidadeContactoDTO, Guid>(id);
                return ResponseFactory.Success<EntidadeContactoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<EntidadeContactoDTO>(ex.Message);
            }
        }

        // create new EntidadeContacto
        public async Task<Response<Guid>> CreateEntidadeContactoAsync(CreateEntidadeContactoRequest request)
        {
            Guid entidadeId = Guid.Parse(request.EntidadeId);
            EntidadeContactoMatchTipo specification = new(entidadeId, request.EntidadeContactoTipoId); // ardalis specification 
            bool EntidadeContactoExists = await _repository.ExistsAsync<EntidadeContacto, Guid>(specification);
            if (EntidadeContactoExists)
            {
                return ResponseFactory.Fail<Guid>("O tipo de contacto já existe para esta entidade");
            }

            EntidadeContacto newEntidadeContacto = _mapper.Map(request, new EntidadeContacto()); // map dto to domain entity

            try
            {
                EntidadeContacto response = await _repository.CreateAsync<EntidadeContacto, Guid>(newEntidadeContacto); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // create new EntidadeContactoBulk
        public async Task<Response<IEnumerable<Guid>>> CreateEntidadeContactoBulkAsync(CreateEntidadeContactoBulkRequest request)
        {
          List<Guid> createdIds = [];
          List<string> errors = [];
          Guid entidadeId = Guid.Parse(request.EntidadeId);

          foreach(CreateEntidadeContactoItemRequest contacto in request.Contactos)
          {
            EntidadeContactoMatchTipo specification = new(entidadeId, contacto.EntidadeContactoTipoId);
            bool contactoTypeExists = await _repository.ExistsAsync<EntidadeContacto, Guid>(specification);

            if(contactoTypeExists)
            {
              errors.Add($"O tipo de contacto {contacto.EntidadeContactoTipoId} já existe para esta entidade");
              continue;
            }

            EntidadeContacto newContacto = new()
            {
              EntidadeId = entidadeId, 
              EntidadeContactoTipoId = contacto.EntidadeContactoTipoId,
              Valor = contacto.Valor, 
              Principal = contacto.Principal,
            };

            try
            {
              EntidadeContacto response = await _repository.CreateAsync<EntidadeContacto, Guid>(newContacto);
              createdIds.Add(response.Id);
            }
            catch(Exception ex)
            {
              errors.Add($"Falha ao criar o contacto '{contacto.Valor}' : {ex.Message}");
            }
          }

          _ = await _repository.SaveChangesAsync();

          return errors.Count != 0
            ? ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", errors))
            : ResponseFactory.Success<IEnumerable<Guid>>(createdIds);

        }

        // update EntidadeContacto
        public async Task<Response<Guid>> UpdateEntidadeContactoAsync(UpdateEntidadeContactoRequest request, Guid id)
        {
            EntidadeContacto EntidadeContactoInDb = await _repository.GetByIdAsync<EntidadeContacto, Guid>(id); // get existing entity
            if (EntidadeContactoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("O contacto não foi encontrado");
            }

            EntidadeContacto updatedEntidadeContacto = _mapper.Map(request, EntidadeContactoInDb); // map dto to domain entity

            try
            {
                EntidadeContacto response = await _repository.UpdateAsync<EntidadeContacto, Guid>(updatedEntidadeContacto);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete EntidadeContacto
        public async Task<Response<Guid>> DeleteEntidadeContactoAsync(Guid id)
        {
            try
            {
                EntidadeContacto? EntidadeContacto = await _repository.RemoveByIdAsync<EntidadeContacto, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(EntidadeContacto.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>($"Falha ao excluir contacto '{id}' : {ex.Message}");
            }
        }

        // update multiple EntidadeContacto
        public async Task<Response<IEnumerable<Guid>>> UpdateEntidadeContactoBulkAsync(UpdateEntidadeContactoBulkRequest request)
        {
          List<Guid> updatedIds = [];
          List<string> errors = [];
          Guid entidadeId = Guid.Parse(request.EntidadeId);

          foreach(UpdateEntidadeContactoItemRequest contacto in request.Contactos)
          {
            EntidadeContacto contactoInDb = await _repository.GetByIdAsync<EntidadeContacto, Guid>(contacto.Id);

            if(contactoInDb == null)
            {
              errors.Add($"O contacto com ID {contacto.Id} não foi encontrado");
              continue;
            }

            if(contactoInDb.EntidadeId != entidadeId)
            {
              errors.Add($"O contacto com ID {contacto.Id} não pertence à entidade {entidadeId}");
              continue;
            }

            contactoInDb.Valor = contacto.Valor;
            contactoInDb.Principal = contacto.Principal;

            try
            {
              EntidadeContacto response;
              try
              {
                response = await _repository.UpdateAsync<EntidadeContacto, Guid>(contactoInDb);
              }
              catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
              {
                response = contactoInDb;
              }
              updatedIds.Add(response.Id);
            }
            catch(Exception ex)
            {
              errors.Add($"Falha ao atualizar contacto '{contacto.Id}' : {ex.Message}");
            }
          }

          _ = await _repository.SaveChangesAsync();

          return errors.Count != 0
            ? ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", errors))
            : ResponseFactory.Success<IEnumerable<Guid>>(updatedIds);
        }

        // insert multiple EntidadeContacto
        public async Task<Response<IEnumerable<Guid>>> UpsertEntidadeContactoBulkAsync(UpsertEntidadeContactoBulkRequest request)
        {
          List<Guid> processedIds = [];
          List<string> errors = [];
          Guid entidadeId = Guid.Parse(request.EntidadeId);

          EntidadeContactoSearchByEntidade searchSpec = new(entidadeId);
          IEnumerable<EntidadeContacto> existingContactos = await _repository.GetListAsync<EntidadeContacto, Guid>(searchSpec);

          HashSet<int> requestedTipoIds = request.Contactos.Select(c => c.EntidadeContactoTipoId).ToHashSet();

          foreach(UpsertEntidadeContactoItemRequest contacto in request.Contactos)
          {
            EntidadeContactoMatchTipo specification = new(entidadeId, contacto.EntidadeContactoTipoId);
            bool contactoTypeExists = await _repository.ExistsAsync<EntidadeContacto, Guid>(specification);

            try
            {
              if(contactoTypeExists)
              {
                IEnumerable<EntidadeContacto> matchingContactos = await _repository.GetListAsync<EntidadeContacto, Guid>(specification);
                EntidadeContacto existingContacto = matchingContactos.First();
                existingContacto.Valor = contacto.Valor;
                existingContacto.Principal = contacto.Principal;

                EntidadeContacto response;
                try
                {
                  response = await _repository.UpdateAsync<EntidadeContacto, Guid>(existingContacto);
                }
                catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                  response = existingContacto;
                }
                processedIds.Add(response.Id);
              }
              else
              {
                EntidadeContacto newContacto = new()
                {
                  EntidadeId = entidadeId,
                  EntidadeContactoTipoId = contacto.EntidadeContactoTipoId,
                  Valor = contacto.Valor,
                  Principal = contacto.Principal,
                };

                EntidadeContacto response = await _repository.CreateAsync<EntidadeContacto, Guid>(newContacto);
                processedIds.Add(response.Id);
              }
            }
            catch(Exception ex)
            {
              errors.Add($"Falha ao processar tipo de contacto '{contacto.EntidadeContactoTipoId}' : {ex.Message}");
            }
          }

          foreach( EntidadeContacto existingContacto in existingContactos)
          {
            if(!requestedTipoIds.Contains(existingContacto.EntidadeContactoTipoId))
            {
              try
              {
                await _repository.RemoveByIdAsync<EntidadeContacto, Guid>(existingContacto.Id);
              }
              catch(Exception ex)
              {
                errors.Add($"Falha ao excluir contacto '{existingContacto.EntidadeContactoTipoId}' : {ex.Message}");
              }
            }
          }

          _ = await _repository.SaveChangesAsync();

          return errors.Count != 0
            ? ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", errors))
            : ResponseFactory.Success<IEnumerable<Guid>>(processedIds);
        }
    }
}

