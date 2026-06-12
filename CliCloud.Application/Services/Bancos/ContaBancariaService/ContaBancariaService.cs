using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Bancos.ContaBancariaService.DTOs;
using CliCloud.Application.Services.Bancos.ContaBancariaService.Filters;
using CliCloud.Application.Services.Bancos.ContaBancariaService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Bancos;

namespace CliCloud.Application.Services.Bancos.ContaBancariaService
{
    public class ContaBancariaService : IContaBancariaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public ContaBancariaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ContaBancariaDTO>>> GetContaBancariaAsync(string keyword = "")
        {
            ContaBancariaSearchList spec = new(keyword);
            IEnumerable<ContaBancariaDTO> list =
                await _repository.GetListAsync<ContaBancaria, ContaBancariaDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<IEnumerable<ContaBancariaLightDTO>>> GetContaBancariaLightAsync(string keyword = "")
        {
            ContaBancariaSearchList spec = new(keyword);
            IEnumerable<ContaBancariaLightDTO> list =
                await _repository.GetListAsync<ContaBancaria, ContaBancariaLightDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<ContaBancariaTableDTO>> GetContaBancariaPaginatedAsync(
            ContaBancariaTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            ContaBancariaSearchTable spec = new(filter.Filters ?? [], dynamicOrder);
            return await _repository.GetPaginatedResultsAsync<ContaBancaria, ContaBancariaTableDTO, Guid>(
                filter.PageNumber, filter.PageSize, spec);
        }

        public async Task<Response<IEnumerable<ContaBancariaTableDTO>>> GetAllContaBancariaAsync(
            ContaBancariaAllFilter filter)
        {
            try
            {
                filter ??= new ContaBancariaAllFilter();
                ContaBancariaSearchTable spec = new(filter.Filters ?? [], filter.GetOrderByString());
                IEnumerable<ContaBancariaTableDTO> list =
                    await _repository.GetListAsync<ContaBancaria, ContaBancariaTableDTO, Guid>(spec);
                return ResponseFactory.Success(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<ContaBancariaTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<ContaBancariaDTO>> GetContaBancariaAsync(Guid id)
        {
            try
            {
                ContaBancariaByIdWithBancoSpec spec = new(id);
                ContaBancaria? entity =
                    (await _repository.GetListAsync<ContaBancaria, Guid>(spec)).FirstOrDefault();
                if (entity == null)
                    return ResponseFactory.Fail<ContaBancariaDTO>("Conta bancária não encontrada");

                ContaBancariaDTO dto = _mapper.Map<ContaBancariaDTO>(entity);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<ContaBancariaDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> CreateContaBancariaAsync(CreateContaBancariaRequest request)
        {
            string numero = request.Numero.Trim();
            ContaBancariaMatchNumero numeroSpec = new(numero);
            if (await _repository.ExistsAsync<ContaBancaria, Guid>(numeroSpec))
                return ResponseFactory.Fail<Guid>("Já existe uma conta bancária com este número.");

            Response<Guid>? bancoError = await ValidateBancoAsync(request.BancoId);
            if (bancoError != null)
                return bancoError;

            ContaBancaria entity = _mapper.Map<ContaBancaria>(request);
            entity.Numero = numero;
            entity.TipoConta = request.TipoConta.Trim();

            try
            {
                ContaBancaria created = await _repository.CreateAsync<ContaBancaria, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateContaBancariaAsync(UpdateContaBancariaRequest request, Guid id)
        {
            ContaBancaria entity = await _repository.GetByIdAsync<ContaBancaria, Guid>(id);
            string numero = request.Numero.Trim();

            if (!string.Equals(entity.Numero, numero, StringComparison.Ordinal))
            {
                ContaBancariaMatchNumero numeroSpec = new(numero);
                if (await _repository.ExistsAsync<ContaBancaria, Guid>(numeroSpec))
                    return ResponseFactory.Fail<Guid>("Já existe uma conta bancária com este número.");
            }

            Response<Guid>? bancoError = await ValidateBancoAsync(request.BancoId);
            if (bancoError != null)
                return bancoError;

            _ = _mapper.Map(request, entity);
            entity.Numero = numero;
            entity.TipoConta = request.TipoConta.Trim();

            try
            {
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> DeleteContaBancariaAsync(Guid id)
        {
            try
            {
                ContaBancaria? entity = await _repository.RemoveByIdAsync<ContaBancaria, Guid>(id);
                if (entity == null)
                    return ResponseFactory.Fail<Guid>("Conta bancária não encontrada");

                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleContaBancariaAsync(IEnumerable<Guid> ids)
        {
            try
            {
                List<Guid> idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = [];
                List<string> failedDeletions = [];

                foreach (Guid id in idsList)
                {
                    try
                    {
                        ContaBancaria? entity = await _repository.GetByIdAsync<ContaBancaria, Guid>(id);
                        if (entity == null)
                        {
                            failedDeletions.Add($"Conta bancária com ID {id} não encontrada.");
                            continue;
                        }

                        _ = await _repository.RemoveByIdAsync<ContaBancaria, Guid>(id);
                        _ = await _repository.SaveChangesAsync();
                        successfullyDeletedIds.Add(id);
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"Conta bancária com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                if (successfullyDeletedIds.Count > 0)
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(
                        successfullyDeletedIds,
                        $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} contas.");
                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }

        private async Task<Response<Guid>?> ValidateBancoAsync(Guid? bancoId)
        {
            if (!bancoId.HasValue)
                return null;

            try
            {
                _ = await _repository.GetByIdAsync<Banco, Guid>(bancoId.Value);
                return null;
            }
            catch
            {
                return ResponseFactory.Fail<Guid>("Instituição financeira não encontrada.");
            }
        }
    }
}