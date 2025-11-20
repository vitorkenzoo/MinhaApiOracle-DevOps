using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaApiOracle.Data;
using MinhaApiOracle.Models;

namespace MinhaApiOracle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogAuditoriasController : ControllerBase
    {
        private readonly AppDb _context;

        public LogAuditoriasController(AppDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _context.LogAuditorias.ToListAsync();
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var log = await _context.LogAuditorias.FindAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarPorTabela([FromQuery] string tabela)
        {
            if (string.IsNullOrWhiteSpace(tabela))
                return BadRequest("O parâmetro 'tabela' é obrigatório.");

            var logs = await _context.LogAuditorias
                .Where(l => l.NomeTabela != null && l.NomeTabela.Contains(tabela))
                .ToListAsync();

            if (logs.Count == 0) return NotFound();
            return Ok(logs);
        }

        /// <summary>
        /// Cria um novo log de auditoria
        /// </summary>
        /// <param name="dto">Dados do log de auditoria</param>
        /// <returns>Log criado com ID gerado</returns>
        [HttpPost]
        public async Task<IActionResult> Create(LogAuditoriaCreateDto dto)
        {
            try
            {
                var novoLog = new LogAuditoria
                {
                    NomeTabela = dto.NomeTabela,
                    DsOperacao = dto.DsOperacao,
                    DataOperacao = string.IsNullOrEmpty(dto.DataOperacao) 
                        ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") 
                        : dto.DataOperacao,
                    NmUsuarioDb = dto.NmUsuarioDb
                };

                _context.LogAuditorias.Add(novoLog);
                await _context.SaveChangesAsync();

                var logCriado = await _context.LogAuditorias.FindAsync(novoLog.IdLog);
                return CreatedAtAction(nameof(GetById), new { id = novoLog.IdLog }, logCriado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao criar log de auditoria", message = ex.Message, innerException = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LogAuditoria logAuditoria)
        {
            if (id != logAuditoria.IdLog) return BadRequest();

            var existingLog = await _context.LogAuditorias.FindAsync(id);
            if (existingLog == null) return NotFound();

            existingLog.NomeTabela = logAuditoria.NomeTabela;
            existingLog.DsOperacao = logAuditoria.DsOperacao;
            existingLog.DataOperacao = logAuditoria.DataOperacao;
            existingLog.NmUsuarioDb = logAuditoria.NmUsuarioDb;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var log = await _context.LogAuditorias.FindAsync(id);
            if (log == null) return NotFound();

            _context.LogAuditorias.Remove(log);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

