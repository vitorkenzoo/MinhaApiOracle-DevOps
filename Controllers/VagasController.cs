using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaApiOracle.Data;
using MinhaApiOracle.Models;

namespace MinhaApiOracle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VagasController : ControllerBase
    {
        private readonly AppDb _context;

        public VagasController(AppDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vagas = await _context.Vagas
                .Include(v => v.Empresa)
                .ToListAsync();
            return Ok(vagas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vaga = await _context.Vagas
                .Include(v => v.Empresa)
                .FirstOrDefaultAsync(v => v.IdVaga == id);
            
            if (vaga == null) return NotFound();
            return Ok(vaga);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarPorNome([FromQuery] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("O parâmetro 'nome' é obrigatório.");

            var vagas = await _context.Vagas
                .Include(v => v.Empresa)
                .Where(v => v.NomeVaga.Contains(nome))
                .ToListAsync();

            if (vagas.Count == 0) return NotFound();
            return Ok(vagas);
        }

        [HttpGet("empresa/{idEmpresa}")]
        public async Task<IActionResult> GetByEmpresa(int idEmpresa)
        {
            var vagas = await _context.Vagas
                .Include(v => v.Empresa)
                .Where(v => v.IdEmpresa == idEmpresa)
                .ToListAsync();

            if (vagas.Count == 0) return NotFound();
            return Ok(vagas);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Vaga vaga)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(vaga.NomeVaga))
                return BadRequest("O campo 'nomeVaga' é obrigatório.");
            
            if (vaga.IdEmpresa <= 0)
                return BadRequest("O campo 'idEmpresa' é obrigatório e deve ser maior que zero.");

            // Verifica se a empresa existe
            var empresaExiste = await _context.Empresas.AnyAsync(e => e.IdEmpresa == vaga.IdEmpresa);
            if (!empresaExiste)
                return BadRequest($"A empresa com ID {vaga.IdEmpresa} não existe.");

            // Cria uma nova vaga apenas com os dados básicos, ignorando relacionamentos
            var novaVaga = new Vaga
            {
                NomeVaga = vaga.NomeVaga,
                DescricaoVaga = vaga.DescricaoVaga,
                Salario = vaga.Salario,
                DtPublicacao = vaga.DtPublicacao ?? DateTime.Now,
                IdEmpresa = vaga.IdEmpresa,
                // IdVaga será gerado automaticamente pelo banco (auto-incremento)
            };

            _context.Vagas.Add(novaVaga);
            await _context.SaveChangesAsync();
            
            // Carrega a vaga criada com os relacionamentos para retornar
            var vagaCriada = await _context.Vagas
                .Include(v => v.Empresa)
                .FirstOrDefaultAsync(v => v.IdVaga == novaVaga.IdVaga);
            
            return CreatedAtAction(nameof(GetById), new { id = novaVaga.IdVaga }, vagaCriada);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Vaga vaga)
        {
            if (id != vaga.IdVaga) return BadRequest();

            var existingVaga = await _context.Vagas.FindAsync(id);
            if (existingVaga == null) return NotFound();

            existingVaga.NomeVaga = vaga.NomeVaga;
            existingVaga.DescricaoVaga = vaga.DescricaoVaga;
            existingVaga.Salario = vaga.Salario;
            existingVaga.DtPublicacao = vaga.DtPublicacao;
            existingVaga.IdEmpresa = vaga.IdEmpresa;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vaga = await _context.Vagas.FindAsync(id);
            if (vaga == null) return NotFound();

            _context.Vagas.Remove(vaga);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
