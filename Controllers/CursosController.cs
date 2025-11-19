using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaApiOracle.Data;
using MinhaApiOracle.Models;

namespace MinhaApiOracle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly AppDb _context;

        public CursosController(AppDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cursos = await _context.Cursos
                .Include(c => c.Certificados)
                .ToListAsync();
            return Ok(cursos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Certificados)
                .FirstOrDefaultAsync(c => c.IdCurso == id);
            
            if (curso == null) return NotFound();
            return Ok(curso);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarPorNome([FromQuery] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("O parâmetro 'nome' é obrigatório.");

            var cursos = await _context.Cursos
                .Include(c => c.Certificados)
                .Where(c => c.NomeCurso.Contains(nome))
                .ToListAsync();

            if (cursos.Count == 0) return NotFound();
            return Ok(cursos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Curso curso)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(curso.NomeCurso))
                return BadRequest("O campo 'nomeCurso' é obrigatório.");

            // Cria um novo curso apenas com os dados básicos, ignorando relacionamentos
            var novoCurso = new Curso
            {
                NomeCurso = curso.NomeCurso,
                Descricao = curso.Descricao,
                QtHoras = curso.QtHoras,
                // IdCurso será gerado automaticamente pelo banco (auto-incremento)
                // Certificados será null (será populado quando necessário)
            };

            _context.Cursos.Add(novoCurso);
            await _context.SaveChangesAsync();
            
            // Carrega o curso criado com os relacionamentos para retornar
            var cursoCriado = await _context.Cursos
                .Include(c => c.Certificados)
                .FirstOrDefaultAsync(c => c.IdCurso == novoCurso.IdCurso);
            
            return CreatedAtAction(nameof(GetById), new { id = novoCurso.IdCurso }, cursoCriado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Curso curso)
        {
            if (id != curso.IdCurso) return BadRequest();

            var existingCurso = await _context.Cursos.FindAsync(id);
            if (existingCurso == null) return NotFound();

            existingCurso.NomeCurso = curso.NomeCurso;
            existingCurso.Descricao = curso.Descricao;
            existingCurso.QtHoras = curso.QtHoras;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return NotFound();

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

