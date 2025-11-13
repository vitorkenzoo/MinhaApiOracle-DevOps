using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaApiOracle.Data;
using MinhaApiOracle.Models;

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

        [HttpPost]
        public async Task<IActionResult> Create(Certificado certificado)
        {
            if (certificado.DtEmissao == default(DateTime))
                certificado.DtEmissao = DateTime.Now;

            _context.Certificados.Add(certificado);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = certificado.IdCertificado }, certificado);
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

