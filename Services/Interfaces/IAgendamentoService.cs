using MotoTrackAPI.DTO.Agendamento.Request;
using MotoTrackAPI.DTO.Agendamento.Response;
using MotoTrackAPI.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MotoTrackAPI.Services.Interfaces
{
    /// <summary>
    /// Define o contrato de operações de negócio para a entidade <see cref="Agendamento"/>.
    /// Segue o padrão assíncrono e permite cancelamento de tarefas.
    /// </summary>
    public interface IAgendamentoService
    {
        /// <summary>Retorna todos os agendamentos cadastrados.</summary>
        Task<IEnumerable<AgendamentoResponse>> GetAllAsync(CancellationToken ct = default);

        /// <summary>Busca um agendamento específico pelo seu ID.</summary>
        Task<AgendamentoResponse> GetByIdAsync(long id, CancellationToken ct = default);

        /// <summary>Cria um novo agendamento.</summary>
        Task<AgendamentoResponse> CreateAsync(AgendamentoCreateRequest dto, CancellationToken ct = default);

        /// <summary>Atualiza um agendamento existente.</summary>
        Task<AgendamentoResponse> UpdateAsync(long id, AgendamentoUpdateRequest dto, CancellationToken ct = default);

        /// <summary>Exclui um agendamento pelo ID.</summary>
        Task DeleteAsync(long id, CancellationToken ct = default);
    }
}
