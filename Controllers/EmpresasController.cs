using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaApiOracle.Data;
using MinhaApiOracle.Models;

namespace MinhaApiOracle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpresasController : ControllerBase
    {
        private readonly AppDb _context;

        public EmpresasController(AppDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var empresas = await _context.Empresas
                .Include(e => e.Vagas)
                .ToListAsync();
            return Ok(empresas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var empresa = await _context.Empresas
                .Include(e => e.Vagas)
                .FirstOrDefaultAsync(e => e.IdEmpresa == id);
            
            if (empresa == null) return NotFound();
            return Ok(empresa);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarPorRazaoSocial([FromQuery] string razaoSocial)
        {
            if (string.IsNullOrWhiteSpace(razaoSocial))
                return BadRequest("O parâmetro 'razaoSocial' é obrigatório.");

            var empresas = await _context.Empresas
                .Include(e => e.Vagas)
                .Where(e => e.RazaoSocial.Contains(razaoSocial))
                .ToListAsync();

            if (empresas.Count == 0) return NotFound();
            return Ok(empresas);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Empresa empresa)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(empresa.RazaoSocial))
                return BadRequest("O campo 'razaoSocial' é obrigatório.");
            
            if (string.IsNullOrWhiteSpace(empresa.Cnpj))
                return BadRequest("O campo 'cnpj' é obrigatório.");

            // Cria uma nova empresa apenas com os dados básicos, ignorando relacionamentos
            var novaEmpresa = new Empresa
            {
                RazaoSocial = empresa.RazaoSocial,
                Cnpj = empresa.Cnpj,
                EmailEmpresa = empresa.EmailEmpresa,
                // IdEmpresa será gerado automaticamente pelo banco (auto-incremento)
                // Vagas será null (será populado quando necessário)
            };

            _context.Empresas.Add(novaEmpresa);
            await _context.SaveChangesAsync();
            
            // Carrega a empresa criada com os relacionamentos para retornar
            var empresaCriada = await _context.Empresas
                .Include(e => e.Vagas)
                .FirstOrDefaultAsync(e => e.IdEmpresa == novaEmpresa.IdEmpresa);
            
            return CreatedAtAction(nameof(GetById), new { id = novaEmpresa.IdEmpresa }, empresaCriada);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Empresa empresa)
        {
            if (id != empresa.IdEmpresa) return BadRequest();

            var existingEmpresa = await _context.Empresas.FindAsync(id);
            if (existingEmpresa == null) return NotFound();

            existingEmpresa.RazaoSocial = empresa.RazaoSocial;
            existingEmpresa.Cnpj = empresa.Cnpj;
            existingEmpresa.EmailEmpresa = empresa.EmailEmpresa;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null) return NotFound();

            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

