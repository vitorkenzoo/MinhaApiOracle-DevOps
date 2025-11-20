using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaApiOracle.Data;
using MinhaApiOracle.Models;
using MinhaApiOracle.DTOs;

namespace MinhaApiOracle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificadosController : ControllerBase
    {
        private readonly AppDb _context;

        public CertificadosController(AppDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var certificados = await _context.Certificados
                .Include(c => c.Usuario)
                .Include(c => c.Curso)
                .ToListAsync();
            return Ok(certificados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var certificado = await _context.Certificados
                .Include(c => c.Usuario)
                .Include(c => c.Curso)
                .FirstOrDefaultAsync(c => c.IdCertificado == id);
            
            if (certificado == null) return NotFound();
            return Ok(certificado);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> GetByUsuario(int idUsuario)
        {
            var certificados = await _context.Certificados
                .Include(c => c.Usuario)
                .Include(c => c.Curso)
                .Where(c => c.IdUsuario == idUsuario)
                .ToListAsync();

            if (certificados.Count == 0) return NotFound();
            return Ok(certificados);
        }

        /// <summary>
        /// Cria um novo certificado
        /// </summary>
        /// <param name="dto">Dados do certificado (apenas IDs, sem objetos de navegação)</param>
        /// <returns>Certificado criado</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CertificadoCreateDto dto)
        {
            try
            {
                // Validação básica
                if (string.IsNullOrWhiteSpace(dto.IdCertificado))
                    return BadRequest("O campo 'idCertificado' é obrigatório.");

                // Verifica se o usuário existe
                var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.IdUsuario == dto.IdUsuario);
                if (!usuarioExiste)
                    return BadRequest($"Usuário com ID {dto.IdUsuario} não encontrado.");

                // Verifica se o curso existe
                var cursoExiste = await _context.Cursos.AnyAsync(c => c.IdCurso == dto.IdCurso);
                if (!cursoExiste)
                    return BadRequest($"Curso com ID {dto.IdCurso} não encontrado.");

                // Cria o certificado apenas com os dados básicos
                var novoCertificado = new Certificado
                {
                    IdCertificado = dto.IdCertificado,
                    DtEmissao = dto.DtEmissao ?? DateTime.Now,
                    Descricao = dto.Descricao,
                    CodigoValidacao = dto.CodigoValidacao,
                    IdUsuario = dto.IdUsuario,
                    IdCurso = dto.IdCurso
                };

                _context.Certificados.Add(novoCertificado);
                await _context.SaveChangesAsync();

                // Carrega o certificado criado com os relacionamentos para retornar
                var certificadoCriado = await _context.Certificados
                    .Include(c => c.Usuario)
                    .Include(c => c.Curso)
                    .FirstOrDefaultAsync(c => c.IdCertificado == novoCertificado.IdCertificado);

                return CreatedAtAction(nameof(GetById), new { id = novoCertificado.IdCertificado }, certificadoCriado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao criar certificado", message = ex.Message, innerException = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Certificado certificado)
        {
            if (id != certificado.IdCertificado) return BadRequest();

            var existingCertificado = await _context.Certificados.FindAsync(id);
            if (existingCertificado == null) return NotFound();

            existingCertificado.DtEmissao = certificado.DtEmissao;
            existingCertificado.Descricao = certificado.Descricao;
            existingCertificado.CodigoValidacao = certificado.CodigoValidacao;
            existingCertificado.IdUsuario = certificado.IdUsuario;
            existingCertificado.IdCurso = certificado.IdCurso;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var certificado = await _context.Certificados.FindAsync(id);
            if (certificado == null) return NotFound();

            _context.Certificados.Remove(certificado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

