using MotoTrackAPI.DTO.Moto.Request;
using MotoTrackAPI.DTO.Moto.Response;
using MotoTrackAPI.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MotoTrackAPI.Services.Interfaces
{
    /// <summary>
    /// Define o contrato de operações de negócio para a entidade <see cref="Moto"/>.
    /// Segue o padrão assíncrono e suporta cancelamento de tarefas.
    /// </summary>
    public interface IMotoService
    {
        /// <summary>Retorna todas as motos cadastradas.</summary>
        Task<IEnumerable<MotoResponse>> GetAllAsync(CancellationToken ct = default);

        /// <summary>Busca uma moto específica pelo seu ID.</summary>
        Task<MotoResponse> GetByIdAsync(long id, CancellationToken ct = default);

        /// <summary>Cria uma nova moto a partir do DTO informado.</summary>
        Task<MotoResponse> CreateAsync(MotoCreateRequest dto, CancellationToken ct = default);

        /// <summary>Atualiza uma moto existente.</summary>
        Task<MotoResponse> UpdateAsync(long id, MotoUpdateRequest dto, CancellationToken ct = default);

        /// <summary>Exclui uma moto, validando se não há dependências relacionadas.</summary>
        Task DeleteAsync(long id, CancellationToken ct = default);
    }
}
